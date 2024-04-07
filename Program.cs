using System;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;                      
using System.Threading.Tasks;
using System.Windows.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
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
            Thread t = new Thread(() => { (m = new Main()).Run(SurfaceType.WindowHandle_Loop, new FoundationR.Surface(StartX, StartY, Width, Height, Title, BitsPerPixel)); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            while (Console.ReadLine() != "exit");
            t.Abort();
            Environment.Exit(0);
        }
    }
    public class Main : Foundation
    {
        Point mouse;
        WindowUtils.RECT window_frame;
        REW pane;
        REW tile;
        REW cans;
        REW solidColor;
        internal Main()
        {
        }

        public override void RegisterHooks()
        {
            Foundation.UpdateEvent += Update;
            Foundation.ResizeEvent += Resize;
            Foundation.InputEvent += Input;
            Foundation.DrawEvent += Draw;
            Foundation.InitializeEvent += Initialize;
            Foundation.LoadResourcesEvent += LoadResources;
            Foundation.MainMenuEvent += MainMenu;
            Foundation.PreDrawEvent += PreDraw;
            Foundation.CameraEvent += Camera;
            Foundation.ExitEvent += ExitEvent;
        }

        protected bool ExitEvent(ExitArgs e)
        {
            return false;
        }

        protected void Input(InputArgs e)
        {
            int x = e.mousePosition.X + RewBatch.Viewport.X;
            int y = e.mousePosition.Y + RewBatch.Viewport.Y;
            mouse = new Point(x, y);
        }

        protected void Camera(CameraArgs e)
        {
            return;
        }

        protected void PreDraw(PreDrawArgs e)
        {
        }

        protected void MainMenu(DrawingArgs e)
        {
        }

        protected void LoadResources()
        {
            Asset.LoadFromFile(@".\Textures\bluepane.rew", out pane);
            Asset.LoadFromFile(@".\Textures\background.rew", out tile);
            Asset.LoadFromFile(@".\Textures\cans.rew", out cans);
        }

        protected void Initialize(InitializeArgs e)
        {
        }

        protected void Draw(DrawingArgs e)
        {
            e.rewBatch.Draw(cans, 0, 0);
            e.rewBatch.Draw(pane, 0, 0);
            if (mouse.X >= 640 || mouse.Y >= 480 || mouse.X <= 0 || mouse.Y <= 0)
                goto COLORS;
            e.rewBatch.Draw(tile.GetPixels(), mouse.X, mouse.Y, 50, 50);
            COLORS:
            e.rewBatch.Draw(REW.Create(50, 50, Color.White, Ext.GetFormat(4)), 0, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.Red, Ext.GetFormat(4)), 50, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.Green, Ext.GetFormat(4)), 100, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.Blue, Ext.GetFormat(4)), 150, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.Gray, Ext.GetFormat(4)), 200, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.Black, Ext.GetFormat(4)), 250, 0);
            e.rewBatch.Draw(REW.Create(50, 50, Color.White, Ext.GetFormat(4)), 640, 50);
            e.rewBatch.DrawString("Arial", "Test_value_01", 50, 50, 200, 100, Color.White);
        }

        protected void Update(UpdateArgs e)
        {
        }
        
        protected new bool Resize(ResizeArgs e)
        {
            return false;
        }
    }
}
