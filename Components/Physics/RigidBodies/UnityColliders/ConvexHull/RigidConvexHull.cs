using BaseX;

namespace FrooxEngine
{
    [Category(new string[] { "Physics/RigidBodies" })]
    public class RigidConvexHull : RigidBody
    {
        public readonly Sync<bool> IsTrigger;
        public readonly AssetRef<Mesh> Mesh;

        protected override void OnAttach()
        {
            base.OnAttach();
            IsTrigger.Value = false;
        }

        protected override void OnInit()
        {
            base.OnInit();
            var hull = Slot.GetComponent<ConvexHullCollider>();
            if (hull != null)
            {
                IsTrigger.Value = hull.Type.Value == ColliderType.Trigger;
            }

            var mesh = Slot.GetComponent<MeshRenderer>().Mesh ?? Slot.GetComponent<SkinnedMeshRenderer>().Mesh;
            if (mesh != null)
            {
                Mesh.Target = mesh.Target;
            }
        }
    }
}
