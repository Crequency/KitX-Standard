using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Interfaces
{
    public interface IFunction
    {
        Function Info { get; }
        PluginInfo AssociatedPlugin { get; }
        Task<object> Invoke(params string[] parameters);
        Type GetReturnType();
    }
}
