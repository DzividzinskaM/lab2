using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGLab2
{
    public enum Dimension
    {
        X, Y, Z
    };

    public class KDTree
    {
        public KDNode Root { get; set; }

        public List<Trig> TrianglesLst;
        public int depth = 0;

        public KDTree(List<Trig> lst)
        {
            Root = new KDNode();
            Root.Coord = new Vector3(0, 0, 0);
            Root.BoundX = 1;
            Root.BoundY = 1;
            Root.BoundZ = 1;
            TrianglesLst = lst;
            Root.trigs = TrianglesLst;
        }

        public const float MAX_X = 1;
        public const float MAX_Y = 1;
        public const float MAX_Z = 1;

        public float currX = MAX_X;
        public float currY = MAX_Y;
        public float currZ = MAX_Z;

        public void Build(KDNode root, Dimension dimension = Dimension.X)
        {
            root.Left = new KDNode();
            root.Right = new KDNode();
            getLeftAndRight(root, dimension);
            if(root.Left.trigs.Count > 100)
            {
                Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
                Console.WriteLine("go to left");
                getCoordForLeft(root, root.Left, dimension);
                Build(root.Left, getNextdimension(dimension));
                return;
            }else if(root.Right.trigs.Count > 100)
            {
                Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
                Console.WriteLine("go to right");
                getCoordForRight(root, root.Right, dimension);
                Build(root.Right, getNextdimension(dimension));
                return;
            }

            Console.WriteLine("end");

        }


       /* public void Build(KDNode root, Dimension dimension = Dimension.X)
        {
            if (root.Coord == null)
            {
                Console.WriteLine("null");
                root.Coord = new Vector3(0, 0, 0);
                root.BoundX = 1;
                root.BoundY = 1;
                root.BoundZ = 1;

            }
            if (root.Coord.Z == -0.75) return;


            root.Left = new KDNode();
            root.Right = new KDNode();
            getCoordForLeft(root, root.Left, getNextdimension(dimension));
            getCoordForRight(root, root.Right, getNextdimension(dimension));
            
*//*            
            Console.WriteLine();

            Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
            Console.WriteLine("go to left");
            Console.WriteLine($"Left coords {root.Left.Coord.X}, {root.Left.Coord.Y}, {root.Left.Coord.Z}");
            Build(root.Left, getNextdimension(dimension));
            Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
            Console.WriteLine("go to right");
            Console.WriteLine($"Right coords {root.Right.Coord.X}, {root.Right.Coord.Y}, {root.Right.Coord.Z}");
            Build(root.Right, getNextdimension(dimension));*//*

            getLeftAndRight(root, root.Left, root.Right, dimension);
            Console.WriteLine($"Right trig - {root.Right.trigs.Count} left trig - {root.Left.trigs.Count}");
            if (root.Left.trigs.Count > 160 || root.Right.trigs.Count > 160)
            {
                depth++;
                if (root.Left.trigs.Count > 160)
                {
                    Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
                    Console.WriteLine("got to left");
                    Build(root.Left, getNextdimension(dimension));
                    return;
                }
                else
                {
                    Console.WriteLine($"Root coords {root.Coord.X}, {root.Coord.Y}, {root.Coord.Z}");
                    Console.WriteLine("got to right");
                    Build(root.Right, getNextdimension(dimension));
                    return;
                }
            }
            else
            {
                Console.WriteLine(depth);
                root.IsLeaf = true;
            }



            //get left trig
            //get right trig

        }*/

        private void getCoordForRight(KDNode root, KDNode right, Dimension dimension)
        {
            right.BoundX = root.BoundX;
            right.BoundY = root.BoundY;
            right.BoundZ = root.BoundZ;
            if (dimension == Dimension.X)
            {
                float newX = root.Coord.X + root.BoundX / 2;
                right.Coord = new Vector3(newX, root.Coord.Y, root.Coord.Z);
                right.BoundX = root.BoundX / 2;
                
            }
            if (dimension == Dimension.Y)
            {
                float newY = root.Coord.Y + root.BoundY / 2;
                right.Coord = new Vector3(root.Coord.X, newY, root.Coord.Z);
                right.BoundY = root.BoundY / 2;
            }
            if (dimension == Dimension.Z)
            {
                float newZ = root.Coord.Z + root.BoundZ / 2;
                right.Coord = new Vector3(root.Coord.X, root.Coord.Y, newZ);
                right.BoundZ = root.BoundZ / 2;
            }
        }

        private void getCoordForLeft(KDNode root, KDNode left, Dimension dimension)
        {
            left.BoundX = root.BoundX;
            left.BoundY = root.BoundY;
            left.BoundZ = root.BoundZ;
            if (dimension == Dimension.X)
            {
                float newX = root.Coord.X - root.BoundX / 2;
                left.Coord = new Vector3(newX, root.Coord.Y, root.Coord.Z);
                left.BoundX = root.BoundX / 2;
            }
            if (dimension == Dimension.Y)
            {
                float newY = root.Coord.Y - root.BoundY / 2;
                left.Coord = new Vector3(root.Coord.X, newY, root.Coord.Z);
                left.BoundY = root.BoundY/ 2;
            }
            if (dimension == Dimension.Z)
            {
                float newZ = root.Coord.Z - root.BoundZ / 2;
                left.Coord = new Vector3(root.Coord.X, root.Coord.Y, newZ);
                left.BoundZ = root.BoundZ / 2;
            }
        }


        /*public void Build(KDNode root, Dimension dimension = Dimension.X)
        {
            //divide by x
            float current = 0;
            if(dimension == Dimension.X)
            {
                current = currX / 2;
                currX = current;
                root.Coord = new Vector3(currX, currY, currZ);
            }
            if (dimension == Dimension.Y)
            {
                current = currY / 2;
                currY = current;
                root.Coord = new Vector3(currX, currY, currZ);
            }
            if (dimension == Dimension.Z)
            {
                current = currZ / 2;
                currZ = current;
                root.Coord = new Vector3(currX, currY, currZ);
            }

            root.Left = new KDNode();
            root.Right = new KDNode();
            //root.Le.trigs = new List<Trig>();
            getLeftAndRight(root, root.Left, root.Right, dimension, current);
            if(root.Left.trigs.Count > 100 || root.Right.trigs.Count > 100)
            {
                depth++;
                if (root.Left.trigs.Count > 100)
                    Build(root.Left, getNextdimension(dimension));
                else Build(root.Right, getNextdimension(dimension));
            }
            else
            {
                Console.WriteLine(depth);
                root.IsLeaf = true;
            }

            
         
        }*/

        /*private void getRight(KDNode root, KDNode right, Dimension dimension, float current)
        {
            throw new NotImplementedException();
        }*/

        /*   private float getMedianValue(Dimension dimension, List<Trig> lst)
           {
               lst.OrderBy(t => t.a.X)
           }*/

        private Dimension getNextdimension(Dimension dimension)
        {
            if (dimension == Dimension.X)
                return Dimension.Y;
            if (dimension == Dimension.Y)
                return Dimension.Z;
            return Dimension.X;
        }

        private void getLeftAndRight(KDNode root, Dimension dimension)
        {
           // right.trigs = new List<Trig>();
            if(dimension == Dimension.X)
            {
               float value = root.Coord.X;
               for(int i=0; i<root.trigs.Count; i++)
               {
                    if (root.trigs[i].a.X <= value
                       || root.trigs[i].b.X <= value
                       || root.trigs[i].c.X <= value)
                        root.Left.trigs.Add(root.trigs[i]);
                    else root.Right.trigs.Add(root.trigs[i]);

               }
            }
            if (dimension == Dimension.Y)
            {
                float value = root.Coord.Y;
                for (int i = 0; i < root.trigs.Count; i++)
                {
                    if (root.trigs[i].a.Y <= value
                       || root.trigs[i].b.Y <= value
                       || root.trigs[i].c.Y <= value)
                        root.Left.trigs.Add(root.trigs[i]);
                    else root.Right.trigs.Add(root.trigs[i]);
                }
            }
            if (dimension == Dimension.Z)
            {
                float value = root.Coord.Z;
                for (int i = 0; i < root.trigs.Count; i++)
                {
                    if (root.trigs[i].a.Z <= value
                       || root.trigs[i].b.Z <= value
                       || root.trigs[i].c.Z <= value)
                        root.Left.trigs.Add(root.trigs[i]);
                    else root.Right.trigs.Add(root.trigs[i]);


                }
            }
        }

        private int countMoreTrig(float x)
        {
            int count = 0;
            for (int i = 0; i < TrianglesLst.Count; i++)
            {
                if (TrianglesLst[i].a.X > x) count++;
                if (TrianglesLst[i].b.X > x) count++;
                if (TrianglesLst[i].c.X > x) count++;

            }
            return count;
        }
    }
}
