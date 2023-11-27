include(${CMAKE_CURRENT_LIST_DIR}/clrfeatures.cmake)

add_compile_definitions($<$<BOOL:$<TARGET_PROPERTY:DAC_COMPONENT>>:DACCESS_COMPILE>)

if (CLR_CMAKE_TARGET_ARCH_ARM64)
  if (CLR_CMAKE_TARGET_UNIX)
    add_definitions(-DFEATURE_EMULATE_SINGLESTEP)
  endif()
  add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:FEATURE_MULTIREG_RETURN>)
elseif (CLR_CMAKE_TARGET_ARCH_ARM)
  if (CLR_CMAKE_HOST_WIN32 AND NOT DEFINED CLR_CROSS_COMPONENTS_BUILD)
    # Set this to ensure we can use Arm SDK for Desktop binary linkage when doing native (Arm32) build
    add_definitions(-D_ARM_WINAPI_PARTITION_DESKTOP_SDK_AVAILABLE)
    add_definitions(-D_ARM_WORKAROUND_)
  endif (CLR_CMAKE_HOST_WIN32 AND NOT DEFINED CLR_CROSS_COMPONENTS_BUILD)
  add_definitions(-DFEATURE_EMULATE_SINGLESTEP)
endif (CLR_CMAKE_TARGET_ARCH_ARM64)

if (CLR_CMAKE_TARGET_UNIX)

  if (CLR_CMAKE_TARGET_ARCH_AMD64)
    add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:UNIX_AMD64_ABI>)
    add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:FEATURE_MULTIREG_RETURN>)
  elseif (CLR_CMAKE_TARGET_ARCH_ARM)
    add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:UNIX_ARM_ABI>)
  elseif (CLR_CMAKE_TARGET_ARCH_I386)
    add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:UNIX_X86_ABI>)
  endif()

endif(CLR_CMAKE_TARGET_UNIX)

if (CLR_CMAKE_TARGET_APPLE AND CLR_CMAKE_TARGET_ARCH_ARM64)
  add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:OSX_ARM64_ABI>)
endif(CLR_CMAKE_TARGET_APPLE AND CLR_CMAKE_TARGET_ARCH_ARM64)

if(CLR_CMAKE_TARGET_LINUX_MUSL)
  # musl-libc doesn't have fixed stack limit, this define disables some stack pointer
  # sanity checks in debug / checked build that rely on a fixed stack limit
  add_definitions(-DNO_FIXED_STACK_LIMIT)
endif(CLR_CMAKE_TARGET_LINUX_MUSL)

add_definitions(-D_BLD_CLR)
add_definitions(-DDEBUGGING_SUPPORTED)
add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:DAC_COMPONENT>>>:PROFILING_SUPPORTED>)
add_compile_definitions($<$<BOOL:$<TARGET_PROPERTY:DAC_COMPONENT>>:PROFILING_SUPPORTED_DATA>)

if(CLR_CMAKE_HOST_WIN32)
  add_definitions(-DWIN32)
  add_definitions(-D_WIN32)
  add_definitions(-DWINVER=0x0602)
  add_definitions(-D_WIN32_WINNT=0x0602)
  add_definitions(-DWIN32_LEAN_AND_MEAN)
  add_definitions(-D_CRT_SECURE_NO_WARNINGS)
endif(CLR_CMAKE_HOST_WIN32)

if (NOT (CLR_CMAKE_TARGET_ARCH_I386 AND CLR_CMAKE_TARGET_UNIX))
  add_compile_definitions(FEATURE_METADATA_UPDATER)
endif()
if(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR (CLR_CMAKE_TARGET_ARCH_I386 AND CLR_CMAKE_TARGET_WIN32))
  add_compile_definitions(FEATURE_REMAP_FUNCTION)
endif(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR (CLR_CMAKE_TARGET_ARCH_I386 AND CLR_CMAKE_TARGET_WIN32))

if(CLR_CMAKE_TARGET_WIN32 AND CLR_CMAKE_TARGET_ARCH_AMD64)
add_compile_definitions(OUT_OF_PROCESS_SETTHREADCONTEXT)
endif(CLR_CMAKE_TARGET_WIN32 AND CLR_CMAKE_TARGET_ARCH_AMD64)

# Features - please keep them alphabetically sorted
if(CLR_CMAKE_TARGET_WIN32)
  if(NOT CLR_CMAKE_TARGET_ARCH_I386)
    add_definitions(-DFEATURE_ARRAYSTUB_AS_IL)
    add_definitions(-DFEATURE_MULTICASTSTUB_AS_IL)
  endif()
else(CLR_CMAKE_TARGET_WIN32)
  add_definitions(-DFEATURE_ARRAYSTUB_AS_IL)
  add_definitions(-DFEATURE_MULTICASTSTUB_AS_IL)
endif(CLR_CMAKE_TARGET_WIN32)

if(NOT CLR_CMAKE_TARGET_ARCH_I386)
  add_definitions(-DFEATURE_PORTABLE_SHUFFLE_THUNKS)
endif()

if(CLR_CMAKE_TARGET_UNIX OR NOT CLR_CMAKE_TARGET_ARCH_I386)
  add_definitions(-DFEATURE_INSTANTIATINGSTUB_AS_IL)
endif()

add_compile_definitions(FEATURE_CODE_VERSIONING)
add_definitions(-DFEATURE_COLLECTIBLE_TYPES)

if(CLR_CMAKE_TARGET_WIN32)
    add_definitions(-DFEATURE_COMINTEROP)
    add_definitions(-DFEATURE_COMINTEROP_APARTMENT_SUPPORT)
    add_definitions(-DFEATURE_COMINTEROP_UNMANAGED_ACTIVATION)
endif(CLR_CMAKE_TARGET_WIN32)

add_definitions(-DFEATURE_BASICFREEZE)
add_definitions(-DFEATURE_CORECLR)
if(FEATURE_DBGIPC)
  add_definitions(-DFEATURE_DBGIPC_TRANSPORT_DI)
  add_definitions(-DFEATURE_DBGIPC_TRANSPORT_VM)
endif(FEATURE_DBGIPC)
add_definitions(-DFEATURE_DEFAULT_INTERFACES)
if(FEATURE_AUTO_TRACE)
    add_compile_definitions(FEATURE_AUTO_TRACE)
endif(FEATURE_AUTO_TRACE)
if(FEATURE_EVENT_TRACE)
    add_compile_definitions(FEATURE_EVENT_TRACE)
    add_definitions(-DFEATURE_PERFTRACING)
else(FEATURE_EVENT_TRACE)
    add_custom_target(eventing_headers) # add a dummy target to avoid checking for FEATURE_EVENT_TRACE in multiple places
endif(FEATURE_EVENT_TRACE)
if(FEATURE_GDBJIT)
    add_definitions(-DFEATURE_GDBJIT)
endif()
if(FEATURE_GDBJIT_FRAME)
    add_definitions(-DFEATURE_GDBJIT_FRAME)
endif(FEATURE_GDBJIT_FRAME)
if(FEATURE_GDBJIT_LANGID_CS)
    add_definitions(-DFEATURE_GDBJIT_LANGID_CS)
endif(FEATURE_GDBJIT_LANGID_CS)
if(FEATURE_GDBJIT_SYMTAB)
    add_definitions(-DFEATURE_GDBJIT_SYMTAB)
endif(FEATURE_GDBJIT_SYMTAB)
if(CLR_CMAKE_TARGET_LINUX)
    add_definitions(-DFEATURE_EVENTSOURCE_XPLAT)
endif(CLR_CMAKE_TARGET_LINUX)
# NetBSD doesn't implement this feature
if(NOT CLR_CMAKE_TARGET_NETBSD)
    add_definitions(-DFEATURE_HIJACK)
endif(NOT CLR_CMAKE_TARGET_NETBSD)
add_definitions(-DFEATURE_ICASTABLE)
if (CLR_CMAKE_TARGET_WIN32 AND (CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_I386 OR CLR_CMAKE_TARGET_ARCH_ARM64))
    add_definitions(-DFEATURE_INTEROP_DEBUGGING)
endif (CLR_CMAKE_TARGET_WIN32 AND (CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_I386 OR CLR_CMAKE_TARGET_ARCH_ARM64))
if(FEATURE_INTERPRETER)
    add_compile_definitions(FEATURE_INTERPRETER)
endif(FEATURE_INTERPRETER)

if (CLR_CMAKE_TARGET_WIN32)
    add_definitions(-DFEATURE_ISYM_READER)
endif(CLR_CMAKE_TARGET_WIN32)

if(FEATURE_MERGE_JIT_AND_ENGINE)
  add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_FEATURE_MERGE_JIT_AND_ENGINE>>>:FEATURE_MERGE_JIT_AND_ENGINE>)
endif(FEATURE_MERGE_JIT_AND_ENGINE)
add_compile_definitions(FEATURE_MULTICOREJIT)
if(CLR_CMAKE_TARGET_UNIX)
  add_definitions(-DFEATURE_PAL_ANSI)
endif(CLR_CMAKE_TARGET_UNIX)
if(CLR_CMAKE_TARGET_LINUX AND CLR_CMAKE_HOST_LINUX)
    add_definitions(-DFEATURE_PERFMAP)
endif(CLR_CMAKE_TARGET_LINUX AND CLR_CMAKE_HOST_LINUX)
if(CLR_CMAKE_TARGET_FREEBSD)
    add_compile_definitions(FEATURE_PERFMAP)
endif(CLR_CMAKE_TARGET_FREEBSD)

if(FEATURE_COMWRAPPERS)
    add_compile_definitions(FEATURE_COMWRAPPERS)
endif(FEATURE_COMWRAPPERS)

if(FEATURE_OBJCMARSHAL)
  add_compile_definitions(FEATURE_OBJCMARSHAL)
endif()

add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:DAC_COMPONENT>>>:FEATURE_PROFAPI_ATTACH_DETACH>)

add_definitions(-DFEATURE_READYTORUN)

set(FEATURE_READYTORUN 1)

add_compile_definitions(FEATURE_REJIT)

if (CLR_CMAKE_HOST_UNIX AND CLR_CMAKE_TARGET_UNIX)
  add_definitions(-DFEATURE_REMOTE_PROC_MEM)
endif (CLR_CMAKE_HOST_UNIX AND CLR_CMAKE_TARGET_UNIX)

if (CLR_CMAKE_TARGET_UNIX OR CLR_CMAKE_TARGET_ARCH_ARM64)
    add_definitions(-DFEATURE_STUBS_AS_IL)
endif ()
if (FEATURE_ENABLE_NO_ADDRESS_SPACE_RANDOMIZATION)
  add_definitions(-DFEATURE_ENABLE_NO_ADDRESS_SPACE_RANDOMIZATION)
endif(FEATURE_ENABLE_NO_ADDRESS_SPACE_RANDOMIZATION)
add_definitions(-DFEATURE_SVR_GC)
add_definitions(-DFEATURE_SYMDIFF)
add_compile_definitions(FEATURE_TIERED_COMPILATION)
if (CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64)
   add_compile_definitions(FEATURE_ON_STACK_REPLACEMENT)
endif (CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64)
add_compile_definitions(FEATURE_PGO)
if (CLR_CMAKE_TARGET_WIN32)
    add_definitions(-DFEATURE_TYPEEQUIVALENCE)
endif(CLR_CMAKE_TARGET_WIN32)
if (CLR_CMAKE_TARGET_ARCH_AMD64)
  # Enable the AMD64 Unix struct passing JIT-EE interface for all AMD64 platforms, to enable altjit.
  add_definitions(-DUNIX_AMD64_ABI_ITF)
endif (CLR_CMAKE_TARGET_ARCH_AMD64)
add_definitions(-DFEATURE_USE_ASM_GC_WRITE_BARRIERS)
if(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64 OR CLR_CMAKE_TARGET_ARCH_RISCV64)
  add_definitions(-DFEATURE_USE_SOFTWARE_WRITE_WATCH_FOR_GC_HEAP)
endif(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64 OR CLR_CMAKE_TARGET_ARCH_RISCV64)
if(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64 OR CLR_CMAKE_TARGET_ARCH_RISCV64)
  add_definitions(-DFEATURE_MANUALLY_MANAGED_CARD_BUNDLES)
endif(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_ARM64 OR CLR_CMAKE_TARGET_ARCH_LOONGARCH64 OR CLR_CMAKE_TARGET_ARCH_RISCV64)

if(NOT CLR_CMAKE_TARGET_UNIX)
    add_definitions(-DFEATURE_WIN32_REGISTRY)
endif(NOT CLR_CMAKE_TARGET_UNIX)
add_definitions(-D_SECURE_SCL=0)
add_definitions(-DUNICODE)
add_definitions(-D_UNICODE)

if(CLR_CMAKE_TARGET_WIN32)
  if (CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_I386)
    add_definitions(-DFEATURE_DATABREAKPOINT)
  endif(CLR_CMAKE_TARGET_ARCH_AMD64 OR CLR_CMAKE_TARGET_ARCH_I386)
endif(CLR_CMAKE_TARGET_WIN32)

if (NOT CLR_CMAKE_TARGET_ARCH_I386 OR NOT CLR_CMAKE_TARGET_WIN32)
  add_compile_definitions($<$<NOT:$<BOOL:$<TARGET_PROPERTY:IGNORE_DEFAULT_TARGET_ARCH>>>:FEATURE_EH_FUNCLETS>)
endif (NOT CLR_CMAKE_TARGET_ARCH_I386 OR NOT CLR_CMAKE_TARGET_WIN32)

if (CLR_CMAKE_TARGET_WIN32 AND CLR_CMAKE_TARGET_ARCH_AMD64)
  add_definitions(-DFEATURE_SPECIAL_USER_MODE_APC)
endif()


# Use this function to enable building with a specific target OS and architecture set of defines
# This is known to work for the set of defines used by the JIT and gcinfo, it is not likely correct for
# other components of the runtime
function(set_target_definitions_to_custom_os_and_arch)
  set(oneValueArgs TARGET OS ARCH)
  cmake_parse_arguments(TARGETDETAILS "" "${oneValueArgs}" "" ${ARGN})

  set_target_properties(${TARGETDETAILS_TARGET} PROPERTIES IGNORE_DEFAULT_TARGET_ARCH TRUE)
  set_target_properties(${TARGETDETAILS_TARGET} PROPERTIES IGNORE_DEFAULT_TARGET_OS TRUE)

  if ((TARGETDETAILS_OS MATCHES "^unix"))
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_UNIX)
    if (TARGETDETAILS_ARCH STREQUAL "x64")
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE UNIX_AMD64_ABI)
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE FEATURE_MULTIREG_RETURN)
    elseif ((TARGETDETAILS_ARCH STREQUAL "arm") OR (TARGETDETAILS_ARCH STREQUAL "armel"))
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE UNIX_ARM_ABI)
    elseif (TARGETDETAILS_ARCH STREQUAL "x86")
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE UNIX_X86_ABI)
    elseif (TARGETDETAILS_ARCH STREQUAL "arm64")
    elseif (TARGETDETAILS_ARCH STREQUAL "loongarch64")
    endif()
    if ((TARGETDETAILS_ARCH STREQUAL "arm64") AND (TARGETDETAILS_OS STREQUAL "unix_osx"))
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_APPLE)
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE OSX_ARM64_ABI)
    endif()
    if (TARGETDETAILS_OS STREQUAL "unix_osx")
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_APPLE)
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_OSX)
    endif()
    if (TARGETDETAILS_OS STREQUAL "unix_anyos")
      target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_UNIX_ANYOS)
    endif()
  elseif (TARGETDETAILS_OS STREQUAL "win")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_WINDOWS)
  endif((TARGETDETAILS_OS MATCHES "^unix"))

  if (TARGETDETAILS_ARCH STREQUAL "x86")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_X86)
  elseif(TARGETDETAILS_ARCH STREQUAL "x64")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_64BIT)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_AMD64)
  elseif(TARGETDETAILS_ARCH STREQUAL "arm64")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_64BIT)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_ARM64)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE FEATURE_MULTIREG_RETURN)
  elseif(TARGETDETAILS_ARCH STREQUAL "loongarch64")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_64BIT)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_LOONGARCH64)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE FEATURE_MULTIREG_RETURN)
  elseif((TARGETDETAILS_ARCH STREQUAL "arm") OR (TARGETDETAILS_ARCH STREQUAL "armel"))
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_ARM)
  elseif(TARGETDETAILS_ARCH STREQUAL "wasm64")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_64BIT)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_WASM64)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_WASM)
  elseif(TARGETDETAILS_ARCH STREQUAL "wasm32")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_WASM32)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_WASM)
  elseif((TARGETDETAILS_ARCH STREQUAL "riscv64"))
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_64BIT)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE TARGET_RISCV64)
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE FEATURE_MULTIREG_RETURN)
  endif()

  if (TARGETDETAILS_ARCH STREQUAL "armel")
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE ARM_SOFTFP)
  endif()

  if (NOT (TARGETDETAILS_ARCH STREQUAL "x86") OR (TARGETDETAILS_OS MATCHES "^unix"))
    target_compile_definitions(${TARGETDETAILS_TARGET} PRIVATE FEATURE_EH_FUNCLETS)
  endif (NOT (TARGETDETAILS_ARCH STREQUAL "x86") OR (TARGETDETAILS_OS MATCHES "^unix"))
endfunction()
