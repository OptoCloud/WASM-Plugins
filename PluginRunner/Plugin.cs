using System.Text;
using Wasmtime;

namespace ConsoleApp1;

internal sealed class Plugin : IDisposable
{
    private readonly Module _module;
    private readonly Linker _linker;
    private readonly Store _store;
    private readonly Instance _instance;

    private readonly Action? _onLoad;
    private readonly Action? _onUnload;

    private Plugin(Module module, Linker linker, Store store, Instance instance)
    {
        _module = module;
        _linker = linker;
        _store = store;
        _instance = instance;

        _onLoad = instance.GetAction("OnLoad");
        _onUnload = instance.GetAction("OnUnload");
    }

    public static Plugin? PluginFromFile(Engine engine, Linker linker, Store store, string filename)
    {
        if (engine is null || linker is null || store is null) return null;

        Module? module = null;
        Instance? instance = null;

        try
        {
            module = Module.FromFile(engine, filename);
            if (module == null) return null;

            linker.DefineFunction("env", "_consoleLog", (int ptr, int len) =>
            {
                if (instance is null) throw new ArgumentNullException(nameof(instance));

                // Read the memory buffer from the WebAssembly module
                var memory = instance.GetMemory("memory");
                if (memory == null) throw new NullReferenceException(nameof(memory));

                var buffer = memory.GetSpan(ptr, len);

                // Decode the buffer to a string
                var message = Encoding.UTF8.GetString(buffer.ToArray());

                // Log the message to the console
                Console.WriteLine(message);
            });

            instance = linker.Instantiate(store, module);

            return new Plugin(module, linker, store, instance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return null;
    }

    public void OnLoad()
    {
        if (_onLoad is not null)
        {
            _onLoad();
        }
    }

    public void OnUnload()
    {
        if (_onUnload is not null)
        {
            _onUnload();
        }
    }

    private bool _disposed = false;
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
            _module.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
