using FrooxEngine;
using UnityEngine;
using UnityNeos;

namespace NEOSPlus
{
    public class RigidBoxColliderConnector : RigidBodyConnector<FrooxEngine.RigidBox>
    {
        private UnityEngine.BoxCollider BoxCollider;
        public override void SetupCollider()
        {
            BoxCollider = go.AddComponent<UnityEngine.BoxCollider>();
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
                BoxCollider.isTrigger = Owner.IsTrigger.Value;
            }

            if (Owner.RigidBoxCenter.WasChanged)
            {
                BoxCollider.center = Owner.RigidBoxCenter.Value.ToUnity();
            }

            if (Owner.RigidBoxSize.WasChanged)
            {
                BoxCollider.size = Owner.RigidBoxSize.Value.ToUnity();
            }
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool)BoxCollider)
            {
                Object.Destroy(BoxCollider);
            }
            BoxCollider = null;
            base.Destroy(destroyingWorld);
        }
    }
}
