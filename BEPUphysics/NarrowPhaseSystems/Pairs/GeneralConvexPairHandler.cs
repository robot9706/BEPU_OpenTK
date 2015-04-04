using System;
using OpenTK.BEPUphysics.BroadPhaseEntries;
using OpenTK.BEPUphysics.BroadPhaseSystems;
using OpenTK.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using OpenTK.BEPUphysics.CollisionTests;
using OpenTK.BEPUphysics.CollisionTests.CollisionAlgorithms.GJK;
using OpenTK.BEPUphysics.CollisionTests.Manifolds;
using OpenTK.BEPUphysics.Constraints.Collision;
using OpenTK.BEPUphysics.PositionUpdating;
using OpenTK.BEPUphysics.Settings;
 

namespace OpenTK.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a convex-convex collision pair.
    ///</summary>
    public class GeneralConvexPairHandler : ConvexConstraintPairHandler
    {
        ConvexCollidable convexA;
        ConvexCollidable convexB;

        GeneralConvexContactManifold contactManifold = new GeneralConvexContactManifold();


        public override Collidable CollidableA
        {
            get { return convexA; }
        }
        public override Collidable CollidableB
        {
            get { return convexB; }
        }    
        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold
        {
            get { return contactManifold; }
        }
        public override Entities.Entity EntityA
        {
            get { return convexA.entity; }
        }
        public override Entities.Entity EntityB
        {
            get { return convexB.entity; }
        }

        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {

            convexA = entryA as ConvexCollidable;
            convexB = entryB as ConvexCollidable;

            if (convexA == null || convexB == null)
            {
                throw new ArgumentException("Inappropriate types used to initialize pair.");
            }

            base.Initialize(entryA, entryB);


        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            convexA = null;
            convexB = null;



        }




    }
}
