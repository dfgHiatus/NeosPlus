using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Logix.Physics.RigidBodies
{
    [NodeName("Rigid Body Data")]
    [Category(new string[] { "LogiX/Physics" })]
    public class RigidBodyData : LogixNode
    {
        public readonly Input<RigidBody> Instance;
        
        public readonly Output<float> Speed;
        public readonly Output<float3> Velocity;
        public readonly Output<float3> AngularVelocity;
        public readonly Output<float3> IntertiaTensor;
        public readonly Output<floatQ> IntertiaTensorRotation;
        public readonly Output<float3> LocalCenterOfMass;
        public readonly Output<float3> GlobalCenterOfMass;

        protected override void OnEvaluate()
        {
            var body = Instance.Evaluate();
            
            if (body is null)
            {
                Speed.Value = 0;
                Velocity.Value = float3.Zero;
                AngularVelocity.Value = float3.Zero;
                IntertiaTensor.Value = float3.Zero;
                IntertiaTensorRotation.Value = floatQ.Identity;
                LocalCenterOfMass.Value = float3.Zero;
                GlobalCenterOfMass.Value = float3.Zero;
                return;
            }

            Speed.Value = body.Speed;
            Velocity.Value = body.Velocity;
            AngularVelocity.Value = body.AngularVelocity;
            IntertiaTensor.Value = body.IntertiaTensor;
            IntertiaTensorRotation.Value = body.IntertiaTensorRotation;
            LocalCenterOfMass.Value = body.LocalCenterOfMass;
            GlobalCenterOfMass.Value = body.GlobalCenterOfMass;
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}
