using KitX.Shared.CSharp.WebCommand;
using KitX.Shared.CSharp.WebCommand.Details;
using System;

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
