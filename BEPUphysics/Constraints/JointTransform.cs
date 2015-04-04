﻿using System;
using OpenTK.BEPUutilities;
using OpenTK;
 

namespace OpenTK.BEPUphysics.Constraints
{
    /// <summary>
    /// Defines a three dimensional orthonormal basis used by a constraint.
    /// </summary>
    public class JointBasis3D
    {
        internal Vector3 localPrimaryAxis = Vector3.Backward;
        internal Vector3 localXAxis = Vector3.Right;
        internal Vector3 localYAxis = Vector3.Up;
        internal Vector3 primaryAxis = Vector3.Backward;
        internal Matrix3x3 rotationMatrix4 = Matrix3x3.Identity;
        internal Vector3 xAxis = Vector3.Right;
        internal Vector3 yAxis = Vector3.Up;

        /// <summary>
        /// Gets the primary axis of the transform in local space.
        /// </summary>
        public Vector3 LocalPrimaryAxis
        {
            get { return localPrimaryAxis; }
        }

        /// <summary>
        /// Gets or sets the local transform of the basis.
        /// </summary>
        public Matrix3x3 LocalTransform
        {
            get
            {
                var toReturn = new Matrix3x3 {Right = localXAxis, Up = localYAxis, Backward = localPrimaryAxis};
                return toReturn;
            }
            set { SetLocalAxes(value); }
        }

        /// <summary>
        /// Gets the X axis of the transform in local space.
        /// </summary>
        public Vector3 LocalXAxis
        {
            get { return localXAxis; }
        }

        /// <summary>
        /// Gets the Y axis of the transform in local space.
        /// </summary>
        public Vector3 LocalYAxis
        {
            get { return localYAxis; }
        }

        /// <summary>
        /// Gets the primary axis of the transform.
        /// </summary>
        public Vector3 PrimaryAxis
        {
            get { return primaryAxis; }
        }

        /// <summary>
        /// Gets or sets the rotation Matrix4 used by the joint transform to convert local space axes to world space.
        /// </summary>
        public Matrix3x3 RotationMatrix4
        {
            get { return rotationMatrix4; }
            set
            {
                rotationMatrix4 = value;
                ComputeWorldSpaceAxes();
            }
        }

        /// <summary>
        /// Gets or sets the world transform of the basis.
        /// </summary>
        public Matrix3x3 WorldTransform
        {
            get
            {
                var toReturn = new Matrix3x3 {Right = xAxis, Up = yAxis, Backward = primaryAxis};
                return toReturn;
            }
            set { SetWorldAxes(value); }
        }

        /// <summary>
        /// Gets the X axis of the transform.
        /// </summary>
        public Vector3 XAxis
        {
            get { return xAxis; }
        }

        /// <summary>
        /// Gets the Y axis of the transform.
        /// </summary>
        public Vector3 YAxis
        {
            get { return yAxis; }
        }


        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="yAxis">Third axis in the transform.</param>
        /// <param name="rotationMatrix4">Matrix4 to use to transform the local axes into world space.</param>
        public void SetLocalAxes(Vector3 primaryAxis, Vector3 xAxis, Vector3 yAxis, Matrix3x3 rotationMatrix4)
        {
            this.rotationMatrix4 = rotationMatrix4;
            SetLocalAxes(primaryAxis, xAxis, yAxis);
        }


        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="yAxis">Third axis in the transform.</param>
        public void SetLocalAxes(Vector3 primaryAxis, Vector3 xAxis, Vector3 yAxis)
        {
            if (Math.Abs(Vector3.Dot(primaryAxis, xAxis)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(primaryAxis, yAxis)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(xAxis, yAxis)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform do not form an orthonormal basis.  Ensure that each axis is perpendicular to the other two.");

            localPrimaryAxis = Vector3.Normalize(primaryAxis);
            localXAxis = Vector3.Normalize(xAxis);
            localYAxis = Vector3.Normalize(yAxis);
            ComputeWorldSpaceAxes();
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="Matrix4">Rotation Matrix4 representing the three axes.
        /// The Matrix4's backward vector is used as the primary axis.  
        /// The Matrix4's right vector is used as the x axis.
        /// The Matrix4's up vector is used as the y axis.</param>
        public void SetLocalAxes(Matrix3x3 Matrix4)
        {
            if (Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Right)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Up)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(Matrix4.Right, Matrix4.Up)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform do not form an orthonormal basis.  Ensure that each axis is perpendicular to the other two.");

            localPrimaryAxis = Vector3.Normalize(Matrix4.Backward);
            localXAxis = Vector3.Normalize(Matrix4.Right);
            localYAxis = Vector3.Normalize(Matrix4.Up);
            ComputeWorldSpaceAxes();
        }


        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="yAxis">Third axis in the transform.</param>
        /// <param name="rotationMatrix4">Matrix4 to use to transform the local axes into world space.</param>
        public void SetWorldAxes(Vector3 primaryAxis, Vector3 xAxis, Vector3 yAxis, Matrix3x3 rotationMatrix4)
        {
            this.rotationMatrix4 = rotationMatrix4;
            SetWorldAxes(primaryAxis, xAxis, yAxis);
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="yAxis">Third axis in the transform.</param>
        public void SetWorldAxes(Vector3 primaryAxis, Vector3 xAxis, Vector3 yAxis)
        {
            if (Math.Abs(Vector3.Dot(primaryAxis, xAxis)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(primaryAxis, yAxis)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(xAxis, yAxis)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform do not form an orthonormal basis.  Ensure that each axis is perpendicular to the other two.");

            this.primaryAxis = Vector3.Normalize(primaryAxis);
            this.xAxis = Vector3.Normalize(xAxis);
            this.yAxis = Vector3.Normalize(yAxis);
            Matrix3x3.TransformTranspose(ref this.primaryAxis, ref rotationMatrix4, out localPrimaryAxis);
            Matrix3x3.TransformTranspose(ref this.xAxis, ref rotationMatrix4, out localXAxis);
            Matrix3x3.TransformTranspose(ref this.yAxis, ref rotationMatrix4, out localYAxis);
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="Matrix4">Rotation Matrix4 representing the three axes.
        /// The Matrix4's backward vector is used as the primary axis.  
        /// The Matrix4's right vector is used as the x axis.
        /// The Matrix4's up vector is used as the y axis.</param>
        public void SetWorldAxes(Matrix3x3 Matrix4)
        {
            if (Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Right)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Up)) > Toolbox.BigEpsilon ||
                Math.Abs(Vector3.Dot(Matrix4.Right, Matrix4.Up)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform do not form an orthonormal basis.  Ensure that each axis is perpendicular to the other two.");

            primaryAxis = Vector3.Normalize(Matrix4.Backward);
            xAxis = Vector3.Normalize(Matrix4.Right);
            yAxis = Vector3.Normalize(Matrix4.Up);
            Matrix3x3.TransformTranspose(ref this.primaryAxis, ref rotationMatrix4, out localPrimaryAxis);
            Matrix3x3.TransformTranspose(ref this.xAxis, ref rotationMatrix4, out localXAxis);
            Matrix3x3.TransformTranspose(ref this.yAxis, ref rotationMatrix4, out localYAxis);
        }

        internal void ComputeWorldSpaceAxes()
        {
            Matrix3x3.Transform(ref localPrimaryAxis, ref rotationMatrix4, out primaryAxis);
            Matrix3x3.Transform(ref localXAxis, ref rotationMatrix4, out xAxis);
            Matrix3x3.Transform(ref localYAxis, ref rotationMatrix4, out yAxis);
        }
    }

    /// <summary>
    /// Defines a two axes which are perpendicular to each other used by a constraint.
    /// </summary>
    public class JointBasis2D
    {
        internal Vector3 localPrimaryAxis = Vector3.Backward;
        internal Vector3 localXAxis = Vector3.Right;
        internal Vector3 primaryAxis = Vector3.Backward;
        internal Matrix3x3 rotationMatrix4 = Matrix3x3.Identity;
        internal Vector3 xAxis = Vector3.Right;

        /// <summary>
        /// Gets the primary axis of the transform in local space.
        /// </summary>
        public Vector3 LocalPrimaryAxis
        {
            get { return localPrimaryAxis; }
        }

        /// <summary>
        /// Gets the X axis of the transform in local space.
        /// </summary>
        public Vector3 LocalXAxis
        {
            get { return localXAxis; }
        }

        /// <summary>
        /// Gets the primary axis of the transform.
        /// </summary>
        public Vector3 PrimaryAxis
        {
            get { return primaryAxis; }
        }

        /// <summary>
        /// Gets or sets the rotation Matrix4 used by the joint transform to convert local space axes to world space.
        /// </summary>
        public Matrix3x3 RotationMatrix4
        {
            get { return rotationMatrix4; }
            set
            {
                rotationMatrix4 = value;
                ComputeWorldSpaceAxes();
            }
        }

        /// <summary>
        /// Gets the X axis of the transform.
        /// </summary>
        public Vector3 XAxis
        {
            get { return xAxis; }
        }


        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="rotationMatrix4">Matrix4 to use to transform the local axes into world space.</param>
        public void SetLocalAxes(Vector3 primaryAxis, Vector3 xAxis, Matrix3x3 rotationMatrix4)
        {
            this.rotationMatrix4 = rotationMatrix4;
            SetLocalAxes(primaryAxis, xAxis);
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        public void SetLocalAxes(Vector3 primaryAxis, Vector3 xAxis)
        {
            if (Math.Abs(Vector3.Dot(primaryAxis, xAxis)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform are not perpendicular.  Ensure that the specified axes form a valid constraint.");

            localPrimaryAxis = Vector3.Normalize(primaryAxis);
            localXAxis = Vector3.Normalize(xAxis);
            ComputeWorldSpaceAxes();
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="Matrix4">Rotation Matrix4 representing the three axes.
        /// The Matrix4's backward vector is used as the primary axis.  
        /// The Matrix4's right vector is used as the x axis.</param>
        public void SetLocalAxes(Matrix3x3 Matrix4)
        {
            if (Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Right)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform are not perpendicular.  Ensure that the specified axes form a valid constraint.");
            localPrimaryAxis = Vector3.Normalize(Matrix4.Backward);
            localXAxis = Vector3.Normalize(Matrix4.Right);
            ComputeWorldSpaceAxes();
        }


        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        /// <param name="rotationMatrix4">Matrix4 to use to transform the local axes into world space.</param>
        public void SetWorldAxes(Vector3 primaryAxis, Vector3 xAxis, Matrix3x3 rotationMatrix4)
        {
            this.rotationMatrix4 = rotationMatrix4;
            SetWorldAxes(primaryAxis, xAxis);
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="primaryAxis">First axis in the transform.  Usually aligned along the main axis of a joint, like the twist axis of a TwistLimit.</param>
        /// <param name="xAxis">Second axis in the transform.</param>
        public void SetWorldAxes(Vector3 primaryAxis, Vector3 xAxis)
        {
            if (Math.Abs(Vector3.Dot(primaryAxis, xAxis)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform are not perpendicular.  Ensure that the specified axes form a valid constraint.");
            this.primaryAxis = Vector3.Normalize(primaryAxis);
            this.xAxis = Vector3.Normalize(xAxis);
            Matrix3x3.TransformTranspose(ref this.primaryAxis, ref rotationMatrix4, out localPrimaryAxis);
            Matrix3x3.TransformTranspose(ref this.xAxis, ref rotationMatrix4, out localXAxis);
        }

        /// <summary>
        /// Sets up the axes of the transform and ensures that it is an orthonormal basis.
        /// </summary>
        /// <param name="Matrix4">Rotation Matrix4 representing the three axes.
        /// The Matrix4's backward vector is used as the primary axis.  
        /// The Matrix4's right vector is used as the x axis.</param>
        public void SetWorldAxes(Matrix3x3 Matrix4)
        {
            if (Math.Abs(Vector3.Dot(Matrix4.Backward, Matrix4.Right)) > Toolbox.BigEpsilon)
                throw new ArgumentException("The axes provided to the joint transform are not perpendicular.  Ensure that the specified axes form a valid constraint.");
            primaryAxis = Vector3.Normalize(Matrix4.Backward);
            xAxis = Vector3.Normalize(Matrix4.Right);
            Matrix3x3.TransformTranspose(ref this.primaryAxis, ref rotationMatrix4, out localPrimaryAxis);
            Matrix3x3.TransformTranspose(ref this.xAxis, ref rotationMatrix4, out localXAxis);
        }

        internal void ComputeWorldSpaceAxes()
        {
            Matrix3x3.Transform(ref localPrimaryAxis, ref rotationMatrix4, out primaryAxis);
            Matrix3x3.Transform(ref localXAxis, ref rotationMatrix4, out xAxis);
        }
    }
}