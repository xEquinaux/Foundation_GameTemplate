using System;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
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
        }

        protected void Input(InputArgs e)
        {
            int x = e.mouse.X + RewBatch.Viewport.X;
            int y = e.mouse.Y + RewBatch.Viewport.Y;
            mouse = new Point(x, y);
        }

        protected void Camera(CameraArgs e)
        {
            if (KeyDown(Key.D))
            {
                e.CAMERA.position.X++;
            }
            if (KeyDown(Key.A))
            {
                e.CAMERA.position.X--;
            }
            if (KeyDown(Key.W))
            {
                e.CAMERA.position.Y--;
            }
            if (KeyDown(Key.S))
            {
                e.CAMERA.position.Y++;
            }
            return;
            // Assume you have an image represented as a byte array 'imageBytes'
            // Each pixel is stored as ARGB (4 bytes per pixel)
            byte[] imageBytes = e.backBuffer;
                                    
            for (int y = 0; y < e.screen.Height; y++)
            {
                for (int x = 0; x < e.screen.Width; x++)
                {
                    // Calculate the index in the byte array for the current pixel
                    int index = (y * e.screen.Width + x) * 4;
                                                
                    // Extract ARGB values
                    byte alpha = imageBytes[index];
                    byte red = imageBytes[index + 1];
                    byte green = imageBytes[index + 2];
                    byte blue = imageBytes[index + 3];

                    // Map ARGB values to screen coordinates (x_screen, y_screen)
                    // You can use linear interpolation or other techniques here
                    int x_screen = (int)e.CAMERA.position.X;
                    int y_screen = (int)e.CAMERA.position.Y;
                    index = ((y + y_screen) * e.screen.Width + (x + x_screen)) * 4;

                    // Set the transformed pixel back into the image
                    imageBytes[index] = alpha;
                    imageBytes[index + 1] = red;
                    imageBytes[index + 2] = green;
                    imageBytes[index + 3] = blue;
                }
            }
            e.backBuffer = imageBytes;
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
            e.rewBatch.Draw(tile, mouse.X, mouse.Y);
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
        
        protected new bool Resize()
        {
            return false;
        }

        new bool KeyDown(Key k)
        {
            return Keyboard.PrimaryDevice.IsKeyDown(k);
        }
    }
}
