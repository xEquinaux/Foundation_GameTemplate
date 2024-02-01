using System;
using System.Threading;                      
using System.Threading.Tasks;
using FoundationR;
using Microsoft.Win32;

namespace Foundation_GameTemplate
{
    internal class Program
    {
        static int StartX => 0;
        static int StartY => 0;
        static int Width => 640;
        static int Height => 480;
        static int BitsPerPixel => 32;
        static string Title = "Foundation_GameTemplate";
        static void Main(string[] args)
        {
            Foundation_GameTemplate.Main m = null;
            Thread t = new Thread(() => { (m = new Main()).Run(new FoundationR.Surface(StartX, StartY, Width, Height, Title, BitsPerPixel)); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            while (Console.ReadLine() != "exit");
            t.Abort();
            Environment.Exit(0);
        }
    }
    public class Main : Foundation
    {
        internal Main()
        {
        }

        public override void RegisterHooks()
        {
            Foundation.UpdateEvent += Update;
            Foundation.ResizeEvent += Resize;
            Foundation.DrawEvent += Draw;
            Foundation.InitializeEvent += Initialize;
            Foundation.LoadResourcesEvent += LoadResources;
            Foundation.MainMenuEvent += MainMenu;
            Foundation.PreDrawEvent += PreDraw;
            Foundation.CameraEvent += Camera;
        }

        protected void Camera(CameraArgs e)
        {
        }

        protected void PreDraw(PreDrawArgs e)
        {
        }

        protected void MainMenu(DrawingArgs e)
        {
        }

        protected void LoadResources()
        {
        }

        protected void Initialize(InitializeArgs e)
        {
        }

        protected void Draw(DrawingArgs e)
        {
        }

        protected new bool Resize()
        {
            Foundation.UpdateEvent += Update;
            Foundation.ResizeEvent += Resize;
            Foundation.DrawEvent += Draw;
            Foundation.InitializeEvent += Initialize;
            Foundation.LoadResourcesEvent += LoadResources;
            Foundation.MainMenuEvent += MainMenu;
            Foundation.PreDrawEvent += PreDraw;
            Foundation.CameraEvent += Camera;
        }

        protected void Camera(CameraArgs e)
        {
        }

        protected void PreDraw(PreDrawArgs e)
        {
        }

        protected void MainMenu(DrawingArgs e)
        {
        }

        protected void LoadResources()
        {
        }

        protected void Initialize(InitializeArgs e)
        {
        }

        protected void Draw(DrawingArgs e)
        {
        }

        protected new bool Resize()
        {
            return false;
        }

        protected void Update(UpdateArgs e)

        protected void Update(UpdateArgs e)
        }
    }
}
