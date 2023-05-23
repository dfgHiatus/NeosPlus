using System.Net;

namespace FrooxEngine.LogiX.Network;

[Category(new string[] { "LogiX/Network/OSC" })]
public class OSCSenderConnect : LogixNode
{
    public readonly Input<OSCSender> Client;
    public readonly Sync<string> IP;
    public readonly Sync<int> Port;
    public readonly Input<User> HandlingUser;
    public readonly Impulse OnConnectStart;
    public readonly Impulse OnConnectFail;

    [ImpulseTarget]
    public void Connect()
    {
        OSCSender oscClient = Client.Evaluate();
        if (oscClient == null)
        {
            OnConnectFail.Trigger();
            return;
        }

        if (!IPAddress.TryParse(oscClient.IP.Value, out var parsedValue))
        {
            OnConnectFail.Trigger();
            return;
        }

        User target = HandlingUser.Evaluate(base.LocalUser);
        oscClient.IP.Value = IP;
        oscClient.Port.Value = Port.Value;
        oscClient.HandlingUser.Target = target;
        OnConnectStart.Trigger();
    }
}
