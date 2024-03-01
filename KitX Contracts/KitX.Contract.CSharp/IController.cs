using System;
using KitX.Shared.CSharp.WebCommand;
using KitX.Shared.CSharp.WebCommand.Details;

namespace KitX.Contract.CSharp;

public interface IController
{
    void Start();

    void Pause();

    void End();

    void Execute(Command cmd);

    void SetSendCommandAction(Action<Request> action);

    void SetWorkingDetail(PluginWorkingDetail workingDetail);
}
