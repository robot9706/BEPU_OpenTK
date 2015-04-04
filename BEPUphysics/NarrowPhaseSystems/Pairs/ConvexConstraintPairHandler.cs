using System;
using OpenTK.BEPUphysics.BroadPhaseSystems;
using OpenTK.BEPUphysics.BroadPhaseEntries;
using OpenTK.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using OpenTK.BEPUphysics.CollisionTests;
using OpenTK.BEPUphysics.CollisionTests.CollisionAlgorithms.GJK;
using OpenTK.BEPUphysics.CollisionTests.Manifolds;
using OpenTK.BEPUphysics.Constraints.Collision;
using OpenTK.BEPUphysics.PositionUpdating;
using OpenTK.BEPUphysics.Settings;

using OpenTK.BEPUphysics.CollisionShapes.ConvexShapes;
using OpenTK.BEPUutilities;

namespace OpenTK.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Pair handler that manages a pair of two boxes.
    ///</summary>
    public abstract class ConvexConstraintPairHandler : ConvexPairHandler
    {
        private ConvexContactManifoldConstraint contactConstraint;


        /// <summary>
        /// Gets the contact constraint used by the pair handler.
        /// </summary>
        public override ContactManifoldConstraint ContactConstraint
        {
            get { return contactConstraint; }
        }

        protected ConvexConstraintPairHandler()
        {
            contactConstraint = new ConvexContactManifoldConstraint(this);
        }

        protected internal override void GetContactInformation(int index, out ContactInformation info)
        {
            info.Contact = ContactManifold.contacts.Elements[index];
            //Find the contact's normal force.
            float totalNormalImpulse = 0;
            info.NormalImpulse = 0;
            for (int i = 0; i < contactConstraint.penetrationConstraints.Count; i++)
            {
                totalNormalImpulse += contactConstraint.penetrationConstraints.Elements[i].accumulatedImpulse;
                if (contactConstraint.penetrationConstraints.Elements[i].contact == info.Contact)
                {
                    info.NormalImpulse = contactConstraint.penetrationConstraints.Elements[i].accumulatedImpulse;
                }
            }
            //Compute friction force.  Since we are using central friction, this is 'faked.'
            float radius;
            Vector3.Distance(ref contactConstraint.slidingFriction.manifoldCenter, ref info.Contact.Position, out radius);
            if (totalNormalImpulse > 0)
                info.FrictionImpulse = (info.NormalImpulse / totalNormalImpulse) * (contactConstraint.slidingFriction.accumulatedImpulse.Length + contactConstraint.twistFriction.accumulatedImpulse * radius);
            else
                info.FrictionImpulse = 0;
            //Compute relative velocity
            Vector3 velocity;
            //If the pair is handling some type of query and does not actually have supporting entities, then consider the velocity contribution to be zero.
            if (EntityA != null)
            {
                Vector3.Subtract(ref info.Contact.Position, ref EntityA.position, out velocity);
                Vector3.Cross(ref EntityA.angularVelocity, ref velocity, out velocity);
                Vector3.Add(ref velocity, ref EntityA.linearVelocity, out info.RelativeVelocity);
            }
            else
                info.RelativeVelocity = new Vector3();

            if (EntityB != null)
            {
                Vector3.Subtract(ref info.Contact.Position, ref EntityB.position, out velocity);
                Vector3.Cross(ref EntityB.angularVelocity, ref velocity, out velocity);
                Vector3.Add(ref velocity, ref EntityB.linearVelocity, out velocity);
                Vector3.Subtract(ref info.RelativeVelocity, ref velocity, out info.RelativeVelocity);
            }


            info.Pair = this;

        }

    }

}
