using System;
using System.Net;
using System.Threading.Tasks;
using BaseX;
using Rug.Osc;

namespace FrooxEngine
{
    [Category("Network")]
    public class OSCSender : Component
    {
        public readonly Sync<string> IP;
        public readonly Sync<int> Port;
        public readonly Sync<string> Address;
        public readonly Sync<string> Message;
        public readonly UserRef HandlingUser;
        public readonly Sync<string> AccessReason;
        public readonly Sync<float> ConnectRetryInterval;
        public readonly Sync<bool> IsConnected;

        private string _currentIP;
        private int _currentPort;
        private OscSender _sender;

        public event Action<OSCSender> Connected;
        public event Action<OSCSender> Closed;

        protected override void OnAwake()
        {
            base.OnAwake();
            ConnectRetryInterval.Value = 10f;
        }

        public Task<bool> Send(string data)
        {
            if (_sender == null || data.Length == 0)
            {
                return Task.FromResult(result: false);
            }
            TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            Task.Run(delegate
            {
                var msg = new OscMessage(Address.Value, Message.Value);
                _sender.Send(msg);
                completionSource.SetResult(true);
            });
            return completionSource.Task;
        }

        protected override void OnChanges()
        {
            string testIP = (Enabled ? IP.Value : null);
            int testPort = (Enabled ? Port.Value : -1);

            if (HandlingUser.Target != LocalUser)
            {
                testIP = null;
                testPort = -1;
            }

            if (testIP == _currentIP && testPort == _currentPort)
            {
                return;
            }

            _currentIP = testIP;
            _currentPort = testPort;
            CloseCurrent();
            IsConnected.Value = false;
            if (_currentIP != null)
            {
                StartTask(async delegate
                {
                    await ConnectTo();
                });
            }
        }

        private async Task ConnectTo()
        {
            if (!IPAddress.TryParse(IP.Value, out var parsedIP)) return;

            if (await Engine.Security.RequestAccessPermission(IP.Value, Port.Value, AccessReason.Value ?? "OSC Sender") == HostAccessPermission.Allowed && !IsRemoved)
            {
                _sender = new OscSender(parsedIP, Port.Value);
                _sender.Connect();
                Connected?.Invoke(this);
            }
        }

        public async Task RetryConnection()
        {
            if (ConnectRetryInterval.Value >= float.MaxValue)
            {
                return;
            }
            await DelaySeconds(ConnectRetryInterval);
            await default(ToBackground);
            if (!(_sender?.State == OscSocketState.Connected)) // TESTME
            {
                await default(ToWorld);
                if (_sender == null)
                {
                    MarkChangeDirty();
                }
            }
        }

        protected override void OnDispose()
        {
            CloseCurrent();
            base.OnDispose();
        }

        private void CloseCurrent()
        {
            if (_sender != null)
            {
                try
                {
                    _sender.Close();
                    _sender = null;
                    Closed?.Invoke(this);
                }
                catch (Exception ex)
                {
                    UniLog.Error("Exception in running Closed event on WebsocketClient:\n" + ex);
                }
            }
        }
    }
}
