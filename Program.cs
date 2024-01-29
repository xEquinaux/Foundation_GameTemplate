﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixelFormats = System.Windows.Media.PixelFormats;
using cotf;
using FoundationR;
using Microsoft.Win32;

namespace CastleOfTheFlame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CastleOfTheFlame.Main m = null;
            Thread t = new Thread(() => { (m = new Main()).Run(new Surface(0, 0, 640, 480, "Castle of the Flame", 32)); });
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
        public override void Initialize()
        {
            Lamp.NewLamp(80, 80, 200, Color.Orange, true);
        }
        public override void LoadResources()
        {
            Lib.SetDimensions(640, 480);
            Lib.Initialize(8, new Size(20, 20));
            Texture.GenerateColorTextureFiles(Tile.Instance.TexturePrefix, Color.Gray, new Size(20, 20));
            Texture.GenerateColorTextureFiles(cotf.Background.Instance.TexturePrefix, Color.DarkGray, new Size(20, 20));
            Lib.InitArray();
        }
        public override void Update()
        {
        }
        public override void Draw(RewBatch rewBatch)
        {
            rewBatch.Draw(REW.Extract((Bitmap)Bitmap.FromFile(@"C:\Users\nolan\Pictures\frame_splashed_by_nolantheturtle_d2u6wkk-fullview.jpg"), 32), 0, 0);
            //rewBatch.Draw(REW.Extract((Bitmap)Bitmap.FromFile(@"C:\Users\nolan\source\repos\test_env\Textures\bluepane.png"), 32), 0, 0);
        }
    }
}
