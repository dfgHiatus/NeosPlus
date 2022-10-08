using UnityNeos;
using UnityEngine;
using FrooxEngine;
using BaseX;

namespace NEOSPlus
{
    public abstract class RigidBodyConnector<T> : ComponentConnector<T>, IRigidBodyUpdatable where T : RigidBody
    {
        protected GameObject go;
        private UnityEngine.Rigidbody RigidBody;

        public abstract void SetupCollider();

        public override void Initialize()
        {
            base.Initialize();
            go = new GameObject("");
            go.transform.SetParent((World.Connector as WorldConnector).WorldRoot.transform);
            RigidBody = go.AddComponent<UnityEngine.Rigidbody>();
            OnPositionChange(Owner.PositionDrive.Target.Value);
            OnRotationChange(Owner.RotationDrive.Target.Value);
            OnScaleChange(Owner.ScaleDrive.Target.Value);

            Owner.OnApplyPos += OnPositionChange;
            Owner.OnApplyRot += OnRotationChange;
            Owner.OnApplyScale += OnScaleChange;
            Owner.OnStop += OnStop;
            SetupCollider();

            // RigidBody = attachedGameObject.AddComponent<UnityEngine.Rigidbody>();
        }

        public override void ApplyChanges()
        {
            if (Owner.Mass.WasChanged)
            {
                RigidBody.mass = Owner.Mass;
            }

            if (Owner.Drag.WasChanged)
            {
                RigidBody.drag = Owner.Drag;
            }

            if (Owner.AngularDrag.WasChanged)
            {
                RigidBody.angularDrag = Owner.AngularDrag;
            }

            if (Owner.IsKinematic.WasChanged)
            {
                RigidBody.isKinematic = Owner.IsKinematic;
            }

            if (Owner.UseGravity.WasChanged)
            {
                RigidBody.useGravity = Owner.UseGravity;
            }

            if (Owner.InterpolationType.WasChanged)
            {
                switch (Owner.InterpolationType.Value)
                {
                    case FrooxEngine.RigidBody.Interpolation.None:
                        RigidBody.interpolation = UnityEngine.RigidbodyInterpolation.None;
                        break;
                    case FrooxEngine.RigidBody.Interpolation.Interpolate:
                        RigidBody.interpolation = UnityEngine.RigidbodyInterpolation.Interpolate;
                        break;
                    case FrooxEngine.RigidBody.Interpolation.Extrapolate:
                        RigidBody.interpolation = UnityEngine.RigidbodyInterpolation.Extrapolate;
                        break;
                }
            }

            if (Owner.CollisionDetectionType.WasChanged)
            {
                switch (Owner.CollisionDetectionType.Value)
                {
                    case FrooxEngine.RigidBody.CollisionDetection.Discrete:
                        RigidBody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Discrete;
                        break;
                    case FrooxEngine.RigidBody.CollisionDetection.Continuous:
                        RigidBody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Continuous;
                        break;
                    case FrooxEngine.RigidBody.CollisionDetection.ContinuousDynamic:
                        RigidBody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousDynamic;
                        break;
                }
            }

            if (Owner.FreezePositionX.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezePositionX;
                if (Owner.FreezePositionX.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezePositionX;
                }
            }

            if (Owner.FreezePositionY.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezePositionY;
                if (Owner.FreezePositionY.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezePositionY;
                }
            }

            if (Owner.FreezePositionZ.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezePositionZ;
                if (Owner.FreezePositionZ.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezePositionZ;
                }
            }

            if (Owner.FreezeRotationX.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezeRotationX;
                if (Owner.FreezeRotationX.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezeRotationX;
                }
            }

            if (Owner.FreezeRotationY.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezeRotationY;
                if (Owner.FreezeRotationY.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezeRotationY;
                }
            }

            if (Owner.FreezeRotationZ.WasChanged)
            {
                RigidBody.constraints = RigidBody.constraints & ~UnityEngine.RigidbodyConstraints.FreezeRotationZ;
                if (Owner.FreezeRotationZ.Value)
                {
                    RigidBody.constraints = RigidBody.constraints | UnityEngine.RigidbodyConstraints.FreezeRotationZ;
                }
            }
        }

        public void OnStop()
        {
            var reg = RigidBody.isKinematic;

            RigidBody.isKinematic = true;

            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            RigidBody.inertiaTensor = Vector3.one;
            RigidBody.inertiaTensorRotation = Quaternion.identity;

            RigidBody.isKinematic = reg;
        }

        private void OnPositionChange(float3 value)
        {
            RigidBody.MovePosition(value.ToUnity());
        }

        private void OnRotationChange(floatQ value)
        {
            RigidBody.MoveRotation(value.ToUnity());
        }

        private void OnScaleChange(float3 value)
        {
            go.transform.localScale = value.ToUnity();
        }

        public override void Destroy(bool destroyingWorld)
        {
            if (!destroyingWorld && (bool) RigidBody)
            {
                Object.Destroy(RigidBody);
            }
            RigidBody = null;
            base.Destroy(destroyingWorld);
        }

        public void ResetCenterOfMass()
        {
            RigidBody.ResetCenterOfMass();
        }

        public void ResetInteriaTensor()
        {
            RigidBody.ResetCenterOfMass();
        }

        public void Update()
        {
            if (RigidBody != null)
            {
                if (Owner.PositionDrive.Target is Sync<float3> targetPosition)
                {
                    if (!targetPosition.WasChanged)
                    {
                        targetPosition.DirectValue = RigidBody.gameObject.transform.position.ToNeos();
                    }
                }

                if (Owner.RotationDrive.Target is Sync<floatQ> targetRotation)
                {
                    if (!targetRotation.WasChanged)
                    {
                        targetRotation.DirectValue = RigidBody.gameObject.transform.rotation.ToNeos();
                    }
                }

                if (Owner.ScaleDrive.Target is Sync<float3> targetScale)
                {
                    if (!targetScale.WasChanged)
                    {
                        targetScale.DirectValue = RigidBody.gameObject.transform.localScale.ToNeos();
                    }
                }

                Owner.Speed.Value = RigidBody.velocity.magnitude;
                Owner.Velocity.Value = RigidBody.velocity.ToNeos();
                Owner.AngularVelocity.Value = RigidBody.angularVelocity.ToNeos();
                Owner.IntertiaTensor.Value = RigidBody.inertiaTensor.ToNeos();
                Owner.IntertiaTensorRotation.Value = RigidBody.inertiaTensorRotation.ToNeos();
                Owner.LocalCenterOfMass.Value = RigidBody.centerOfMass.ToNeos();
                Owner.GlobalCenterOfMass.Value = RigidBody.worldCenterOfMass.ToNeos();
            }
        }
    }
}
