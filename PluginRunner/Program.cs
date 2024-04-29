
using ConsoleApp1;
using System.Diagnostics;
using Wasmtime;

using var engine = new Engine();
using var linker = new Linker(engine);
using var store = new Store(engine);

using Plugin? test = Plugin.PluginFromFile(engine, linker, store, "C:\\Source\\testing\\output.wasm");
if (test is null) throw new NullReferenceException(nameof(test));

Stopwatch sw = Stopwatch.StartNew();

test.OnLoad();
test.OnUnload();

sw.Stop();

Console.WriteLine(sw.Elapsed.ToString());