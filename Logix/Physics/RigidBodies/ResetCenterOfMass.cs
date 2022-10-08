using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    [NodeName("Reset Center Of Mass")]
    [Category(new string[] { "LogiX/Physics" })]
    public class ResetCenterOfMass : LogixNode
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

            body.ResetCenterOfMass();
            OnDone.Trigger();
        }
    }
}
