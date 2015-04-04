﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.BEPUutilities;

namespace OpenTK.BEPUphysics.CollisionShapes
{
    public struct EntityShapeVolumeDescription
    {
        public Matrix3x3 VolumeDistribution;
        public float Volume;
    }
}
