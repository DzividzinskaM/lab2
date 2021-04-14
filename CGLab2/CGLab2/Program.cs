using ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.IO;

namespace CGLab2
{
    class Program
    {
       

        static void Write_color(StreamWriter file, Vector3 pixel_color)
        {
            // Write the translated [0,255] value of each color component.
            file.WriteLine($"{(int)(255 * pixel_color.X)} {(int)(255 * pixel_color.Y)} {(int)(255 * pixel_color.Z)}");
        }

        static bool Hit_sphere(Vector3 center, float radius, Ray r)
        {
            Vector3 oc = r.Orig - center;
            var a = r.Dir.DotProduct(r.Dir);
            var b = 2.0f * oc.DotProduct(r.Dir);
            var c = oc.DotProduct(oc) - radius * radius;
            var discriminant = b * b - 4 * a * c;
            return (discriminant > 0);
        }

        static bool Hit_triangle(Ray ray, Trig inTriangle)
        {
            var vertex0 = inTriangle.a;
            var vertex1 = inTriangle.b;
            var vertex2 = inTriangle.c;
            var edge1 = vertex1 - vertex0;
            var edge2 = vertex2 - vertex0;
            var h = ray.Dir.CrossProduct(edge2);
            var a = edge1.DotProduct(h);
            var EPSILON = 1e-5f;
            if (a > -EPSILON && a < EPSILON)
            {
                return false;
            }
            var f = 1 / a;
            var s = ray.Orig - vertex0;
            var u = f * s.DotProduct(h);
            /* if (u == 0.0 || u == 1.0)
             {
                 return false;
             }*/
            if (u < 0.0 || u > 1.0)
            {
                return false;
            }
            var q = s.CrossProduct(edge1);
            var v = f * ray.Dir.DotProduct(q);
           /* if (v == 0.0 || u + v == 1.0)
            {
                return false;
            }*/
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }

            // At this stage we can compute t to find out where the intersection point is on the line.
            var t = f * edge2.DotProduct(q);
            return t > EPSILON;
        }

        private static List<Trig> getTrigList(LoadResult obj)
        {
            List<Trig> TrigsLst = new List<Trig>();
            var faces = obj.Groups[0].Faces;
            var vertices = obj.Vertices;
            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var vertexIndex1 = face[0].VertexIndex - 1;
                var vertex1 = new Vector3(vertices[vertexIndex1].X, vertices[vertexIndex1].Y, vertices[vertexIndex1].Z);
                var vertexIndex2 = face[1].VertexIndex - 1;
                var vertex2 = new Vector3(vertices[vertexIndex2].X, vertices[vertexIndex2].Y, vertices[vertexIndex2].Z);
                var vertexIndex3 = face[2].VertexIndex - 1;
                var vertex3 = new Vector3(vertices[vertexIndex3].X, vertices[vertexIndex3].Y, vertices[vertexIndex3].Z);
                var triangle = new Trig(vertex1, vertex2, vertex3);


                TrigsLst.Add(rotateX(triangle));
            }

            return TrigsLst;
        }


        static Vector3 Ray_color(Ray r, List<Trig> TrigsLst)
        {

            /*List<Trig> TrigsLst = new List<Trig>();
            var faces = obj.Groups[0].Faces;
            var vertices = obj.Vertices;
            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var vertexIndex1 = face[0].VertexIndex - 1;
                var vertex1 = new Vector3(vertices[vertexIndex1].X, vertices[vertexIndex1].Y, vertices[vertexIndex1].Z);
                var vertexIndex2 = face[1].VertexIndex - 1;
                var vertex2 = new Vector3(vertices[vertexIndex2].X, vertices[vertexIndex2].Y, vertices[vertexIndex2].Z);
                var vertexIndex3 = face[2].VertexIndex - 1;
                var vertex3 = new Vector3(vertices[vertexIndex3].X, vertices[vertexIndex3].Y, vertices[vertexIndex3].Z);
                var triangle = new Trig(vertex1, vertex2, vertex3);
                TrigsLst.Add(triangle);

                Trig newTrig = rotateY(triangle);


                *//*if (Hit_triangle(r, newTrig))
                    return new Vector3(0, 0, 0);*//*
                if (Hit_triangle(r, newTrig))
                {  
                    return computeLightening(r) * new Vector3(0, 1, 0);
                }
            }*/

            for (int i = 0; i < TrigsLst.Count; i++)
            {
                if (Hit_triangle(r, TrigsLst[i]))
                {
                    //return new Vector3(0, 1, 0);
                    return computeLightening(r) * new Vector3(0, 1, 0);
                }
            }

            return new Vector3(1,1,1);


            //Trig triangle = new Trig(new Vector3(0, 0, -1), new Vector3(0, 1, -1), new Vector3(1, 0, -1));

            //translate by x trig
            //Trig newTrig = rotateZ(triangle);
            /*
                        if (Hit_sphere(new Vector3(0, 0, -1), 0.5f, r))
                            return computeLightening(r) * new Vector3(1, 0, 0);

                        return new Vector3(0, 0, 0);
            */

            /* if (Hit_triangle(r, newTrig))
                 return new Vector3(1, 0, 0);

             return new Vector3(0, 0, 0);*/
            /*
                        Vector3 unit_direction = r.Dir.Norm();
                        var t = 0.5f * (unit_direction.Y + 1.0f);
                        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);*/

        }


        private static float computeLightening(Ray r)
        {
            float i = 0;
            Vector3 lightPos = new Vector3(5, 2, -2);
            Vector3 L = lightPos - r.Orig;
            float n_dot_l = r.Dir.DotProduct(L);
            if (n_dot_l > 0)
                i += (0.5f * n_dot_l) / (r.Dir.Length * L.Length);

            return i;

        }

        static void Main(string[] args)
        {
            long start = DateTime.Now.Ticks;
            var loadedObj = loadObj();

            float aspectRatio = 16.0f / 9.0f;
            int imageWidth = 600;

            int image_height = (int)(imageWidth / aspectRatio);

            string filePath = "D:/testValue/test1.ppm";
            //byte[] rgb = new byte[imageWidth * image_height * 3];

            StreamWriter file = new StreamWriter("D:/testValue/test1.ppm");
            

            file.WriteLine($"P3\n{imageWidth} {image_height}\n255");

            float viewportHeight = 2.0f;
            float viewportWidth = aspectRatio * viewportHeight;
            float focal_length = 1.0f;

            var origin = new Vector3(0, 0, 1);
            var horizontal = new Vector3(viewportWidth, 0, 0);
            var vertical = new Vector3(0, viewportHeight, 0);
            var lowerLeftCorner = origin - 0.5f * horizontal - 0.5f * vertical - new Vector3(0, 0, focal_length);

            /* List<Trig> lst= getTrigLst(loadedObj);
             buildTree(lst);*/

            List<Trig> triangles = getTrigList(loadedObj);
            using (file)
            {
                for (int j = image_height - 1; j >= 0; --j)
                {
                    for (int i = 0; i < imageWidth; ++i)
                    {
                        var u = (float)i / (imageWidth - 1);
                        var v = (float)j / (image_height - 1);
                        Ray r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
                        Vector3 pixel_color = Ray_color(r, triangles);
                        //int index = i * 3;
                        /*rgb[index] = (byte)pixel_color.X;
                        rgb[index + 1] = (byte)pixel_color.Y;
                        rgb[index + 2] = (byte)pixel_color.Z;*/

                        

                         Write_color(file, pixel_color);
                    }
                }
            }


            //Image image = new Image(imageWidth, image_height, rgb);
            //PPMWriter pPMWriter = new PPMWriter();
            //pPMWriter.Write("D:/testValue/test1.ppm", image);
            DateTime currentDate = DateTime.Now;
            long elapsedTicks = currentDate.Ticks - start;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            Console.WriteLine("Потрачено тактов на выполнение: " + elapsedSpan.TotalSeconds);
        }

        

        private static void buildTree(List<Trig> lst)
        {
            KDTree tree = new KDTree(lst);
            tree.Build(tree.Root);
        }

        private static List<Trig> getTrigLst(LoadResult obj)
        {
            List<Trig> TrigsLst = new List<Trig>();
            var faces = obj.Groups[0].Faces;
            var vertices = obj.Vertices;
            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                var vertexIndex1 = face[0].VertexIndex - 1;
                var vertex1 = new Vector3(vertices[vertexIndex1].X, vertices[vertexIndex1].Y, vertices[vertexIndex1].Z);
                var vertexIndex2 = face[1].VertexIndex - 1;
                var vertex2 = new Vector3(vertices[vertexIndex2].X, vertices[vertexIndex2].Y, vertices[vertexIndex2].Z);
                var vertexIndex3 = face[2].VertexIndex - 1;
                var vertex3 = new Vector3(vertices[vertexIndex3].X, vertices[vertexIndex3].Y, vertices[vertexIndex3].Z);
                var triangle = new Trig(vertex1, vertex2, vertex3);
                TrigsLst.Add(triangle);

                /* Trig newTrig = rotateY(triangle);


                 if (Hit_triangle(r, newTrig))
                     return new Vector3(0, 0, 0);*/
            }
            return TrigsLst;
        }

        private static LoadResult loadObj()
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();

            FileStream fileStream = new FileStream("D:/testValue/cow.obj", FileMode.Open);
            var result = objLoader.Load(fileStream);
            return result;
        }

        private static Trig translateX(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {1, 0, 0, 2},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        private static Trig translateY(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {0, 0, 0, 0},
                {0, 1, 0, 1.5f},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        private static Trig translateZ(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0.5f},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        private static Trig rotateX(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {1, 0, 0, 0},
                {0, 0, 1, 0},
                {0, -1, 0, 0},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        private static Trig rotateY(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {0, 0, 1, 0},
                {0, 1, 0, 0},
                {-1, 0, 0, 0},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        private static Trig rotateZ(Trig trig)
        {
            float[,] translateXArr = new float[4, 4] {
                {0, -1, 0, 0},
                {1, 0, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            Vector3 a = MultipleArrs(translateXArr, trig.a);
            Vector3 b = MultipleArrs(translateXArr, trig.b);
            Vector3 c = MultipleArrs(translateXArr, trig.c);

            return new Trig(a, b, c);
        }

        static Vector3 MultipleArrs(float[,] matrixA, Vector3 vector)
        {
            //vector to matrixB

            float[,] matrixB = new float[4, 1] { { vector.X }, { vector.Y }, { vector.Z }, { 1 } };

            int matrixARows = 4;
            int matrixACols = 4;
            int matrixBRows = 4;
            int matrixBCols = 1;
            float[,] matrixC = new float[matrixARows, matrixBCols];

            for (var i = 0; i < matrixARows; i++)
            {
                for (var j = 0; j <matrixBCols; j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < matrixACols; k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            //matrix to vector
            Vector3 resVector = new Vector3(matrixC[0,0], matrixC[1,0], matrixC[2, 0]);

            //Console.WriteLine($"new v {resVector.X}, {resVector.Y}, {resVector.Z}");

            return resVector;
        }
    }
}
