using System.Linq;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus
{
    public class StopRigidBodiesInChildren : LogixNode
    {
        public readonly Input<Slot> Instance;
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]
        public void Stop()
        {
            var bodies = Instance.Evaluate().GetComponentsInChildren<RigidBody>();
            if (bodies.Count() == 0)
            {
                OnFail.Trigger();
                return;
            }

            foreach (var body in bodies)
            {
                body.Stop();
            }

            OnDone.Trigger();
        }
    }
}
