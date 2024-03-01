using System.ComponentModel.Composition;
using KitX.Shared.CSharp.Plugin;

namespace KitX.Contract.CSharp;

[InheritedExport]
public interface IIdentityInterface
{
    PluginInfo GetPluginInfo();

    IController GetController();

    IMarketPluginContract GetMarketPluginContract();
}
