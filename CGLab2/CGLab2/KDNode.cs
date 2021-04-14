using System;
using System.Collections.Generic;
using System.Text;

namespace CGLab2
{
    public class KDNode
    {
        public float BoundX { get; set; }
        public float BoundY { get; set; }
        public float BoundZ { get; set; }

        public Vector3 Coord { get; set; }
        public KDNode Right { get; set; }

        public KDNode Left { get; set; }

        public bool IsLeaf { get; set; }

        public List<Trig> trigs = new List<Trig>();

    }
}
