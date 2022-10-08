using System;
using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using NEOSPlus;

namespace FrooxEngine
{
    public abstract class RigidBody : ImplementableComponent, ICustomInspector
    {
        public event Action<float3> OnApplyPos;
        public event Action<floatQ> OnApplyRot;
        public event Action<float3> OnApplyScale;
        public event Action OnReset;
        
        public readonly Sync<float> Mass;
        public readonly Sync<float> Drag;
        public readonly Sync<float> AngularDrag;
        public readonly Sync<bool> UseGravity;
        public readonly Sync<bool> IsKinematic;
        public readonly Sync<Interpolation> InterpolationType;
        public readonly Sync<CollisionDetection> CollisionDetectionType;

        public readonly Sync<bool> FreezePositionX;
        public readonly Sync<bool> FreezePositionY;
        public readonly Sync<bool> FreezePositionZ;
        public readonly Sync<bool> FreezeRotationX;
        public readonly Sync<bool> FreezeRotationY;
        public readonly Sync<bool> FreezeRotationZ;

        public readonly FieldDrive<float3> PositionDrive;
        public readonly FieldDrive<floatQ> RotationDrive;
        public readonly FieldDrive<float3> ScaleDrive;

        //public readonly Sync<float> Speed;
        //public readonly Sync<float3> Velocity;
        //public readonly Sync<float3> AngularVelocity;
        //public readonly Sync<float3> IntertiaTensor;
        //public readonly Sync<float3> IntertialTensorRotation;
        //public readonly Sync<float3> LocalCenterOfMass;
        //public readonly Sync<float3> WorldCenterOfMass;

        protected override void OnAttach()
        {
            base.OnAttach();
            Mass.Value = 1f;
            Drag.Value = 0f;
            AngularDrag.Value = 0.05f;
            UseGravity.Value = false;
            IsKinematic.Value = false;
            InterpolationType.Value = Interpolation.None;
            CollisionDetectionType.Value = CollisionDetection.Discrete;
            FreezePositionX.Value = false;
            FreezePositionY.Value = false;
            FreezePositionZ.Value = false;
            FreezeRotationX.Value = false;
            FreezeRotationY.Value = false;
            FreezeRotationZ.Value = false;

            PositionDrive.Target = Slot.Position_Field;
            RotationDrive.Target = Slot.Rotation_Field;
            ScaleDrive.Target = Slot.Scale_Field;
        }

        protected override void OnInit()
        {
            base.OnInit();
            PositionDrive.SetupValueSetHook(SetPos);
            RotationDrive.SetupValueSetHook(SetRot);
            ScaleDrive.SetupValueSetHook(SetScale);
        }

        private void SetPos(IField<float3> field, float3 value)
        {
            OnApplyPos?.Invoke(value);
        }
        private void SetRot(IField<floatQ> field, floatQ value)
        {
            OnApplyRot?.Invoke(value);
        }

        private void SetScale(IField<float3> field, float3 value)
        {
            OnApplyScale?.Invoke(value);
        }

        [ImpulseTarget]
        public void Reset() { OnReset?.Invoke(); }
        
        public enum Interpolation
        {
            None,
            Interpolate,
            Extrapolate
        }
        
        public enum CollisionDetection
        {
            Discrete,
            Continuous,
            ContinuousDynamic
        }

        public void BuildInspectorUI(UIBuilder ui)
        {
            WorkerInspector.BuildInspectorUI(this, ui);
            ui.Button("Stop Simulation".AsLocaleKey()).SetUpActionTrigger(Reset);
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            if (Connector is IRigidBodyUpdatable updatable)
            {
                updatable?.Update();
            }
        }
    }
}
