using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    [NodeName("Reset Intertia Tensor On Slot")]
    [Category(new string[] { "LogiX/Physics" })]
    public class ResetIntertiaTensorOnSlot : LogixNode
    {
        public readonly Input<Slot> Instance;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Reset()
        {
            var body = Instance.Evaluate().GetComponent<RigidBody>();
            if (body is null)
            {
                OnFail.Trigger();
                return;
            }

            body.ResetInteriaTensor();
            OnDone.Trigger();
        }
    }
}
