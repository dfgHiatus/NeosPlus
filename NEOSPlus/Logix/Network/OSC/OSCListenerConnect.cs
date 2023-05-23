using System.Net;

namespace FrooxEngine.LogiX.Network;

[Category(new string[] { "LogiX/Network/OSC" })]
public class OSCListenerConnect : LogixNode
{
    public readonly Input<OSCListener> Client;
    public readonly Input<string> IP;
    public readonly Input<string> Address;
    public readonly Input<int> Port;
    public readonly Input<User> HandlingUser;
    public readonly Impulse OnConnectStart;
    public readonly Impulse OnConnectFail;

    [ImpulseTarget]
    public void Connect()
    {
        OSCListener oscClient = Client.Evaluate();
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

        User target = HandlingUser.Evaluate(LocalUser);
        oscClient.IP.Value = IP.Evaluate();
        oscClient.Address.Value = Address.Evaluate();
        oscClient.Port.Value = Port.Evaluate();
        oscClient.HandlingUser.Target = target;
        OnConnectStart.Trigger();
    }
}
