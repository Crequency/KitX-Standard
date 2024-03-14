using System;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.WebCommand.Infos;

namespace KitX.Shared.CSharp.WebCommand;

public class Request
{
    public RequestTypes Type { get; set; } = RequestTypes.Command;

    public RequestVersions Version { get; set; } = RequestVersions.V1;

    public DeviceLocator? Sender { get; set; }

    public DeviceLocator? Target { get; set; }

    public EncryptionInfo EncryptionInfo { get; set; } = new();

    public CompressionInfo CompressionInfo { get; set; } = new();

    public string Content { get; set; } = string.Empty;
}

public enum RequestTypes
{
    Unknown = 0,
    Command = 1,
}

public enum RequestVersions
{
    Unknown = 0,
    V1 = 1,
}

public static class RequestExtensions
{
    public static string GetContent(this Request request, Func<string, string> decryptFunc) => decryptFunc.Invoke(request.Content);

    public static void Match(this Request request, string decryptedContent, Action<string>? matchCommand = null)
    {
        switch (request.Type)
        {
            case RequestTypes.Unknown:
                break;
            case RequestTypes.Command:
                matchCommand?.Invoke(decryptedContent);
                break;
        }
    }

    public static Request Rebuild(this Request request, Func<Request, Request> rebuilder) => rebuilder(request);
}
