using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/RigidBodies" })]
    public class RigidBox : RigidBody
    {
        public readonly Sync<bool> IsTrigger;
        public readonly Sync<float3> RigidBoxCenter;
        public readonly Sync<float3> RigidBoxSize;

        protected override void OnAttach()
        {
            base.OnAttach();
            IsTrigger.Value = false;
            RigidBoxCenter.Value = float3.Zero;
            RigidBoxSize.Value = float3.One;
        }

        protected override void OnInit()
        {
            base.OnInit();
            var box = Slot.GetComponent<BoxCollider>();
            if (box != null)
            {
                IsTrigger.Value = box.Type.Value == ColliderType.Trigger;
                RigidBoxSize.Value = box.Size.Value;
            }
        }
    }
}
