using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/RigidBodies" })]
    public class RigidSphere : RigidBody
    {
        public readonly Sync<bool> IsTrigger;
        public readonly Sync<float3> RigidSphereCenter;
        public readonly Sync<float> Radius;

        protected override void OnAttach()
        {
            base.OnAttach();
            IsTrigger.Value = false;
            RigidSphereCenter.Value = float3.Zero;
            Radius.Value = 0.5f;
        }

        protected override void OnInit()
        {
            base.OnInit();
            var sphere = Slot.GetComponent<SphereCollider>();
            if (sphere != null)
            {
                IsTrigger.Value = sphere.Type.Value == ColliderType.Trigger;
                Radius.Value = sphere.Radius.Value;
            }
        }
    }
}
