using FrooxEngine;
using UnityEngine;
using UnityNeos;

namespace NEOSPlus
{
    public class RigidSphereColliderConnector : RigidBodyConnector<FrooxEngine.RigidSphere>
    {
        private UnityEngine.SphereCollider SphereCollider;
        public override void SetupCollider()
        {
            SphereCollider = go.AddComponent<UnityEngine.SphereCollider>();
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
                SphereCollider.isTrigger = Owner.IsTrigger.Value;
            }

            if (Owner.RigidSphereCenter.WasChanged)
            {
                SphereCollider.center = Owner.RigidSphereCenter.Value.ToUnity();
            }

            if (Owner.Radius.WasChanged)
            {
                SphereCollider.radius = Owner.Radius;
            }
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool)SphereCollider)
            {
                Object.Destroy(SphereCollider);
            }
            SphereCollider = null;
            base.Destroy(destroyingWorld);
        }
    }
}
