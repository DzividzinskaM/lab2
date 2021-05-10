using System;
using System.Collections.Generic;
using System.Text;

namespace RendererApp
{
    public class Triangle
    {
        public Vector3 a { get; set; }
        public Vector3 b { get; set; }
        public Vector3 c { get; set; }

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Triangle()
        {

        }
    }
}
