import { readFile, writeFile } from 'node:fs/promises';
import { transpile } from '@bytecodealliance/jco';

const base = import.meta.url;
const component = await readFile(new URL("./SharedLibrary.wasm", base));
const transpiled = await transpile(component, {
    name: "shared-library",
    typescript: false,
});
await writeFile(new URL("./shared-library.core.wasm", base), transpiled.files["shared-library.core.wasm"]);
await writeFile(new URL("./shared-library.core2.wasm", base), transpiled.files["shared-library.core2.wasm"]);
await writeFile(new URL("./shared-library.mjs", base), transpiled.files["shared-library.js"]);
const instance = await import(new URL("./shared-library.mjs", base));

if (instance.returnsPrimitiveInt() != 10)
    process.exit(1);

if (instance.returnsPrimitiveBool() != 1)
    process.exit(2);

if (instance.returnsPrimitiveChar() != 'a')
    process.exit(3);

// As long as no unmanaged exception is thrown managed class loaders were initialized successfully.
instance.ensureManagedClassLoaders();

if (instance.checkSimpleGcCollect() != 100)
    process.exit(4);

if (instance.checkSimpleExceptionHandling() != 100)
   process.exit(5);

process.exit(100);
