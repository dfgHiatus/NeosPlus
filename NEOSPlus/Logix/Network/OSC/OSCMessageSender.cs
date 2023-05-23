namespace FrooxEngine.LogiX.Network;

[Category(new string[] { "LogiX/Network/OSC" })]
public class OSCMessageSender : LogixNode
{
    public readonly Input<OSCSender> Client;
    public readonly Input<string> Data;
    public readonly Impulse OnSendStart;
    public readonly Impulse OnSent;
    public readonly Impulse OnSendError;
    public readonly Output<string> SentData;

    [ImpulseTarget]
    public void Send()
    {
        OSCSender _client = Client.Evaluate();
        string _data = Data.Evaluate();
        if (_client == null)
        {
            return;
        }
        OnSendStart.Trigger();
        StartTask(async delegate
        {
            bool wasSent = await _client.Send(_data);
            SentData.Value = _data;
            if (wasSent)
            {
                OnSent.Trigger();
            }
            else
            {
                OnSendError.Trigger();
            }
            SentData.Value = null;
        });
    }
}
