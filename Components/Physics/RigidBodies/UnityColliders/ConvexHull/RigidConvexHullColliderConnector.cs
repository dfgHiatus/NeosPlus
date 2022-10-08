using FrooxEngine;
using UnityEngine;
using UnityNeos;

namespace NEOSPlus
{
    public class RigidConvexHullColliderConnector : RigidBodyConnector<FrooxEngine.RigidConvexHull>
    {
        private int vertexCount; // Hacky
        private UnityEngine.MeshCollider MeshCollider;

        public override void SetupCollider()
        {
            MeshCollider = Gameobject.AddComponent<UnityEngine.MeshCollider>();
            MeshCollider.material = PhysicsMaterial;
        }

        public override void Initialize()
        {
            base.Initialize();
            MeshCollider.convex = true; // Non-convex MeshColliders with non-kinematic Rigidbodies are no longer supported on Unity 5+
            MeshCollider.sharedMesh = null;
            vertexCount = 0;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            if (Owner.IsTrigger.WasChanged)
            {
                MeshCollider.isTrigger = Owner.IsTrigger.Value;
            }

            // If the vertex count is the same, we assume no mesh change has taken place
            if (Owner.Mesh.Asset.Metadata.VertexCount != vertexCount)
            {
                MeshCollider.sharedMesh = Owner.Mesh.Asset.GetUnity();
                vertexCount = Owner.Mesh.Asset.Metadata.VertexCount;
            }
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool)MeshCollider)
            {
                Object.Destroy(MeshCollider);
            }
            MeshCollider = null;
            base.Destroy(destroyingWorld);
        }
    }
}
