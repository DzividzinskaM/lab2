using System;
using System.Diagnostics;

namespace RendererApp
{
    class Program
    {
        
        static void Main(string[] args)
        {


            /*   string filePath = "D:/testValue/cow.obj";
               string output = "D:/testValue/cow.ppm";
               string[] args = { $"--source={filePath}", $"--output={output}" };
               RendererApp app = new RendererApp();
               app.Start(args);
   */
            try
            {
                RendererApp app = new RendererApp();
                app.Start(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
