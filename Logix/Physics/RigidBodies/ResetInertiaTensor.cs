using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    [NodeName("Reset Inertia Tensor")]
    [Category(new string[] { "LogiX/Physics" })]
    public class ResetInertiaTensor : LogixNode
    {
        public readonly Input<RigidBody> Instance;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Reset()
        {
            var body = Instance.Evaluate();
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
