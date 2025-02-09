using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Interfaces
{
    public interface IFunction
    {
        Function Info { get; }
        PluginInfo AssociatedPlugin { get; }
        Task<object> Invoke(params object[] parameters);
        bool ValidateParameters(object[] parameters);
        Type GetReturnType();
    }
}
