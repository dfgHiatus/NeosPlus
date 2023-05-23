namespace FrooxEngine.LogiX.Network;

public abstract class OSCReceiverBaseNode : LogixNode
{
    public readonly Input<OSCSender> Client;

    private OSCSender _registered;

    protected override void OnChanges()
    {
        base.OnChanges();
        OSCSender oscClient = Client.Evaluate();
        if (oscClient != _registered)
        {
            Unregister();
            if (oscClient != null)
            {
                Register(oscClient);
            }
            _registered = oscClient;
        }
    }

    protected abstract void Register(OSCSender client);

    protected abstract void Unregister(OSCSender client);

    private void Unregister()
    {
        if (_registered != null)
        {
            Unregister(_registered);
        }
        _registered = null;
    }

    protected override void OnDispose()
    {
        Unregister();
        base.OnDispose();
    }
}
