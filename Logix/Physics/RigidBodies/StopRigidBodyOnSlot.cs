using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    [NodeName("Stop Rigid Body Simulations On Slot")]
    [Category(new string[] { "LogiX/Physics" })]
    public class StopRigidBodyOnSlot : LogixNode
    {
        public readonly Input<Slot> Instance;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Stop()
        {
            var body = Instance.Evaluate().GetComponent<RigidBody>();
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
