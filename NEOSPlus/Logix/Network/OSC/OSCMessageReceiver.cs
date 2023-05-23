namespace FrooxEngine.LogiX.Network;

[Category(new string[] { "LogiX/Network/OSC" })]
public class OSCMessageReceiver : LogixNode
{
    public readonly Input<OSCListener> Client;
    public readonly Output<string> SentData;

    [ImpulseTarget]
    public void Receive()
    {
        OSCListener _client = Client.Evaluate();
        if (_client == null)
        {
            return;
        }
        _client.OnMessage += _client_OnMessageSent;
    }

    private void _client_OnMessageSent(OSCListener osc, string msg)
    {
        SentData.Value = msg;
    }

    protected override void OnDispose()
    {
        OSCListener _client = Client.Evaluate();
        if (_client == null)
        {
            _client.OnMessage -= _client_OnMessageSent;
        }
        base.OnDispose();
    }
}
