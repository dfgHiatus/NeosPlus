using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    [NodeName("Stop Rigid Body Simulation")]
    [Category(new string[] { "LogiX/Physics" })]
    public class StopRigidBody : LogixNode
    {
        public readonly Input<RigidBody> Instance;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Stop()
        {
            var body = Instance.Evaluate();
            if (body is null)
            {
                OnFail.Trigger();
                return;
            }

            body.Stop();
            OnDone.Trigger();
        }
    }
}
