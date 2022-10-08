using FrooxEngine;
using UnityEngine;
using UnityNeos;

namespace NEOSPlus
{
    public class RigidCapsuleColliderConnector : RigidBodyConnector<FrooxEngine.RigidCapsule>
    {
        private UnityEngine.CapsuleCollider CapsuleCollider;
        public override void SetupCollider()
        {
            CapsuleCollider = Gameobject.AddComponent<UnityEngine.CapsuleCollider>();
            CapsuleCollider.material = PhysicsMaterial;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            if (Owner.IsTrigger.WasChanged)
            {
                CapsuleCollider.isTrigger = Owner.IsTrigger.Value;
            }

            if (Owner.RigidCapsuleCenter.WasChanged)
            {
                CapsuleCollider.center = Owner.RigidCapsuleCenter.Value.ToUnity();
            }

            if (Owner.Height.WasChanged)
            {
                CapsuleCollider.height = Owner.Height;
            }

            if (Owner.Radius.WasChanged)
            {
                CapsuleCollider.radius = Owner.Radius;
            }
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool)CapsuleCollider)
            {
                Object.Destroy(CapsuleCollider);
            }
            CapsuleCollider = null;
            base.Destroy(destroyingWorld);
        }
    }
}
