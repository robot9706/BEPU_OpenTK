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
 
using OpenTK.BEPUphysics.CollisionShapes.ConvexShapes;

namespace OpenTK.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Pair handler that manages a pair of two boxes.
    ///</summary>
    public class BoxPairHandler : ConvexConstraintPairHandler
    {
        ConvexCollidable<BoxShape> boxA;
        ConvexCollidable<BoxShape> boxB;

        BoxContactManifold contactManifold = new BoxContactManifold();

        public override Collidable CollidableA
        {
            get { return boxA; }
        }

        public override Collidable CollidableB
        {
            get { return boxB; }
        }

        public override Entities.Entity EntityA
        {
            get { return boxA.entity; }
        }

        public override Entities.Entity EntityB
        {
            get { return boxB.entity; }
        }
        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold
        {
            get { return contactManifold; }
        }



        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            boxA = entryA as ConvexCollidable<BoxShape>;
            boxB = entryB as ConvexCollidable<BoxShape>;

            if (boxA == null || boxB == null)
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

            boxA = null;
            boxB = null;
            
        }




    }

}
