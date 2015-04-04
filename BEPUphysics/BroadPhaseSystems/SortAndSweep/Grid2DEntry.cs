using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.BEPUphysics.BroadPhaseEntries;

namespace OpenTK.BEPUphysics.BroadPhaseSystems.SortAndSweep
{
    class Grid2DEntry
    {
        internal void Initialize(BroadPhaseEntry entry)
        {
            this.item = entry;
            Grid2DSortAndSweep.ComputeCell(ref entry.boundingBox.Min, out previousMin);
            Grid2DSortAndSweep.ComputeCell(ref entry.boundingBox.Max, out previousMax);
        }


        internal BroadPhaseEntry item;
        internal Int2 previousMin;
        internal Int2 previousMax;
    }
}
