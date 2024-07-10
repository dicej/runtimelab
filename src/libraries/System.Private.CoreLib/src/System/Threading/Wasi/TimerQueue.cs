// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WasiPollWorld.wit.imports.wasi.clocks.v0_2_0;

namespace System.Threading
{
    internal partial class TimerQueue
    {
        private static long TickCount64 => Environment.TickCount64;
        // Ideally, we'd use a `SortedDictionary` here, which would allow us to
        // avoid an O(n) scan in `GetNextDue`, but that's not currently part of
        // `System.Private.CoreLib`.
        private static Dictionary<long, TimerState> s_schedule = new();

        private readonly int _index;
        private long _due;

        private TimerQueue(int index)
        {
            _index = index;
            _due = long.MaxValue;
        }

        private bool SetTimer(uint actualDuration)
        {
            var now = TickCount64;
            var due = now + (long)actualDuration;

            // If this queue was already scheduled, remove it from the schedule
            // before re-adding it.
            {
                if (s_schedule.TryGetValue(_due, out TimerState state))
                {
                    state.queues.Remove(_index);
                    if (state.queues.Count == 0)
                    {
                        s_schedule.Remove(_due);
                        // TODO: consider adding an `UnregisterWasiPollable`
                        // method to `WasiEventLoop` and using it here so we
                        // don't waste time polling an obsolete pollable.  This
                        // will require replacing the `registered` field in
                        // `TimerState` with one that refers to the pollable we
                        // registered earlier, if any.
                    }
                }
            }

            _due = due;

            // (Re-)add the queue to the schedule.
            {
                if (s_schedule.TryGetValue(due, out TimerState state))
                {
                    // There are already other queues scheduled to fire at the same
                    // time, so we add this one to the list and return.
                    state.queues.Add(_index, this);
                }
                else
                {
                    // This is the first queue to be scheduled to fire at this
                    // particular time.
                    //
                    // If this queue is due to fire sooner than any others, register
                    // a timer pollable with `WasiEventLoop`.  Otherwise, a timer
                    // pollable has already been registered to call `FireReady`
                    // before this queue is due, and `FireReady` will register
                    // another pollable for the next queues that will be due, and so
                    // on.
                    var next = GetNextDue();
                    var shouldRegister =
                        next == null || due < ((KeyValuePair<long, TimerState>)next!).Key;
                    if (shouldRegister)
                    {
                        Register((ulong)actualDuration, due);
                    }

                    var queues = new Dictionary<int, TimerQueue>();
                    queues.Add(_index, this);

                    s_schedule.Add(
                        due,
                        new TimerState { registered = shouldRegister, queues = queues }
                    );
                }
            }

            return true;
        }

        private static void Register(ulong durationMillis, long due)
        {
            // `SubscribeDuration` expects nanoseconds:
            var pollable = MonotonicClockInterop.SubscribeDuration(durationMillis * 1000 * 1000);
            var handle = pollable.Handle;
            pollable.Handle = 0;
            var task = WasiEventLoop.RegisterWasiPollable(handle);
            task.ContinueWith((_) => FireReady(due), TaskScheduler.Default);
        }

        private static void FireReady(long due)
        {
            var now = TickCount64;

            // The pollable for a given set of queues should only become ready
            // once the time we specified has fully elapsed, so we assert that
            // here.
            if (due > now) {
                throw new Exception("pollable became ready too early");
            }

            // First, identify which queues are due and add them to the `ready` list.
            var ready = new List<KeyValuePair<long, TimerState>>();
            foreach (var pair in s_schedule)
            {
                if (pair.Key <= now)
                {
                    ready.Add(pair);
                }
            }

            // Next, remove the ready queues from the schedule.  Note that we
            // must do this before calling `FireNextTimers` on any queues since
            // those calls might call `SetTimer` and re-add one or more queues
            // to the schedule.
            foreach (var pair in ready)
            {
                s_schedule.Remove(pair.Key);
                // See TODO comment in `SetTimer` about unregistering pollables,
                // which is also relevant here.

                foreach (var queue in pair.Value.queues)
                {
                    queue.Value._due = long.MaxValue;
                }
            }

            // Now call `FireNextTimers` on each ready queue.
            foreach (var pair in ready)
            {
                foreach (var queue in pair.Value.queues)
                {
                    queue.Value.FireNextTimers();
                }
            }

            // Finally, register a timer pollable with `WasiEventLoop` for the
            // next set of queues due if such a set exists and has not already
            // been registered.
            var next = GetNextDue();
            if (next is not null && !((KeyValuePair<long, TimerState>)next!).Value.registered)
            {
                var nextDue = ((KeyValuePair<long, TimerState>)next!).Key;
                Register((ulong)(nextDue - now), nextDue);
            }
        }

        private static KeyValuePair<long, TimerState>? GetNextDue()
        {
            KeyValuePair<long, TimerState>? soonest = null;
            foreach (var pair in s_schedule)
            {
                if (soonest is null || pair.Key < ((KeyValuePair<long, TimerState>)soonest!).Key)
                {
                    soonest = pair;
                }
            }
            return soonest;
        }

        private struct TimerState
        {
            internal bool registered;
            internal Dictionary<int, TimerQueue> queues;
        }
    }
}
