using System;
using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using NEOSPlus;

namespace FrooxEngine
{
    // Possible TODO: Make RigidBodies kinematic while grabbed
    public abstract class RigidBody : ImplementableComponent, ICustomInspector
    {
        // Connector Events
        public event Action<float3> OnApplyPos;
        public event Action<floatQ> OnApplyRot;
        public event Action<float3> OnApplyScale;
        public event Action OnStop;
        public event Action OnResetCenterOfMass;
        public event Action OnResetInteriaTensor;

        // Rigid body properties
        public readonly Sync<float> SleepThreshold;
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

        // Physics material properties
        public readonly Sync<float> DynamicFriction;
        public readonly Sync<float> StaticFriction;
        public readonly Sync<float> Bounciness;
        public readonly Sync<Combine> FrictionCombine;
        public readonly Sync<Combine> BounceCombine;

        // "Readonly" simulation properties
        public readonly Sync<float> Speed;
        public readonly Sync<float3> Velocity;
        public readonly Sync<float3> AngularVelocity;
        public readonly Sync<float3> IntertiaTensor;
        public readonly Sync<floatQ> IntertiaTensorRotation;
        public readonly Sync<float3> LocalCenterOfMass;
        public readonly Sync<float3> GlobalCenterOfMass;
        public readonly Sync<SleepState> SleepStatus;

        // Slot drives
        public readonly FieldDrive<float3> PositionDrive;
        public readonly FieldDrive<floatQ> RotationDrive;
        public readonly FieldDrive<float3> ScaleDrive;

        //public readonly SyncRef<IGrabbable> Grabber;
        //private IGrabbable _registeredGrabbable;
        //private bool lastKinematicState;

        protected override void OnAttach()
        {
            base.OnAttach();
            Mass.Value = 1f;
            Drag.Value = 10f;
            SleepThreshold.Value = 0.005f;
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

            DynamicFriction.Value = 0.6f;
            StaticFriction.Value = 0.6f;
            Bounciness.Value = 0f;
            FrictionCombine.Value = Combine.Average;
            BounceCombine.Value = Combine.Average;

            PositionDrive.Target = Slot.Position_Field;
            RotationDrive.Target = Slot.Rotation_Field;
            ScaleDrive.Target = Slot.Scale_Field;

            //Grabber.Target = Slot.GetComponent<Grabbable>();
            //if (Grabber.Target != _registeredGrabbable)
            //{
            //    Unregister();
            //    if (Grabber != null)
            //    {
            //        Grabber.Target.OnLocalGrabbed += OnLocalGrabbed;
            //        Grabber.Target.OnLocalReleased += OnLocalReleased;
            //    }
            //    _registeredGrabbable = Grabber.Target;
            //}
        }

        //private void Unregister()
        //{
        //    if (_registeredGrabbable != null)
        //    {
        //        _registeredGrabbable.OnLocalGrabbed -= OnLocalGrabbed;
        //        _registeredGrabbable.OnLocalReleased -= OnLocalReleased;
        //    }
        //    _registeredGrabbable = null;
        //}

        //private void OnLocalGrabbed(IGrabbable obj)
        //{
        //    RunSynchronously(delegate
        //    {
        //        lastKinematicState = IsKinematic.Value;
        //        IsKinematic.Value = false;
        //    });
        //}

        //private void OnLocalReleased(IGrabbable obj)
        //{
        //    RunSynchronously(delegate
        //    {
        //        IsKinematic.Value = lastKinematicState;
        //    });
        //}

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
        public void Stop() { OnStop?.Invoke(); }


        [ImpulseTarget]
        public void ResetCenterOfMass() { OnResetCenterOfMass?.Invoke(); }


        [ImpulseTarget]
        public void ResetInteriaTensor() { OnResetInteriaTensor?.Invoke(); }

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

        public enum Combine
        {
            Average,
            Minimum,
            Mulitply,
            Maximum
        }

        public enum SleepState
        {
            Awake,
            Asleep
        }

        public void BuildInspectorUI(UIBuilder ui)
        {
            WorkerInspector.BuildInspectorUI(this, ui);
            ui.Button("Stop Simulation".AsLocaleKey()).SetUpActionTrigger(Stop);
            ui.Button("Reset Center Of Mass".AsLocaleKey()).SetUpActionTrigger(ResetCenterOfMass);
            ui.Button("Reset Interia Tensor".AsLocaleKey()).SetUpActionTrigger(ResetInteriaTensor);
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            if (Connector is IRigidBodyUpdatable updatable)
            {
                updatable?.Update();
            }
        }

        //protected override void OnDispose()
        //{
        //    Unregister();
        //    base.OnDispose();
        //}
    }
}
