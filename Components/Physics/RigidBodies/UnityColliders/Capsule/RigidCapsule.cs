using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/RigidBodies" })]
    public class RigidCapsule : RigidBody
    {
        public readonly Sync<bool> IsTrigger;
        public readonly Sync<float3> RigidCapsuleCenter;
        public readonly Sync<float> Radius;
        public readonly Sync<float> Height;

        protected override void OnAttach()
        {
            base.OnAttach();
            IsTrigger.Value = false;
            RigidCapsuleCenter.Value = float3.Zero;
            Radius.Value = 0.5f;
            Height.Value = 2f; 
        }

        protected override void OnInit()
        {
            base.OnInit();
            var capsule = Slot.GetComponent<CapsuleCollider>();
            if (capsule != null)
            {
                IsTrigger.Value = capsule.Type.Value == ColliderType.Trigger;
                Radius.Value = capsule.Radius.Value;
                Height.Value = capsule.Height.Value;
            }
        }
    }
}
