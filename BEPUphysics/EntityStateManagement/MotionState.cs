 
using System;
using OpenTK.BEPUutilities;

namespace OpenTK.BEPUphysics.EntityStateManagement
{
    ///<summary>
    /// State describing the position, orientation, and velocity of an entity.
    ///</summary>
    public struct MotionState : IEquatable<MotionState>
    {
        ///<summary>
        /// Position of an entity.
        ///</summary>
        public Vector3 Position;
        ///<summary>
        /// Orientation of an entity.
        ///</summary>
        public Quaternion Orientation;
        ///<summary>
        /// Orientation Matrix4 of an entity.
        ///</summary>
        public Matrix4 OrientationMatrix4
        {
            get
            {
                Matrix4 toReturn;
                Matrix4.CreateFromQuaternion(ref Orientation, out toReturn);
                return toReturn;
            }
        }
        ///<summary>
        /// World transform of an entity.
        ///</summary>
        public Matrix4 WorldTransform
        {
            get
            {
                Matrix4 toReturn;
                Matrix4.CreateFromQuaternion(ref Orientation, out toReturn);
                toReturn.Translation = Position;
                return toReturn;
            }
        }
        ///<summary>
        /// Linear velocity of an entity.
        ///</summary>
        public Vector3 LinearVelocity;
        ///<summary>
        /// Angular velocity of an entity.
        ///</summary>
        public Vector3 AngularVelocity;


        public bool Equals(MotionState other)
        {
            return other.AngularVelocity == AngularVelocity &&
                   other.LinearVelocity == LinearVelocity &&
                   other.Position == Position &&
                   other.Orientation == Orientation;
        }
    }
}
