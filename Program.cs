using System;
using System.Drawing;
using System.Threading;
using FoundationR;

namespace cotf
{
    internal class Program
    {
        public static Settings settings;
        public static int Width = 500;
        public static int Height = 400;
        static void Main(string[] args)
        {
            settings = new Settings(args);
            Main m = null;
            Thread t = new Thread(() => { (m = new Main()).Run(new FoundationR.Surface(0, 0, Width, Height, "Castle of the Flame", 32)); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            while (Console.ReadLine() != "exit");
            t.Abort();
            Environment.Exit(0);
        }
    }
    internal class Settings
    {
        public Settings(string[] args)
        {
            if (args.Length >= 2)
            {
                int.TryParse(args[0], out Width);
                int.TryParse(args[1], out Height);
            }
        }
        public int Width;
        public int Height;
        public Camera camera;
        public Rectangle viewport;
        public RewBatch rewBatch;
        public FoundationR.Surface surface;
    }
}
