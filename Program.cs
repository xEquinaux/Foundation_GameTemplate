using System;
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
using System.IO;
using cotf.Base;
using cotf.Assets;

namespace CastleOfTheFlame
{
    internal class Program
    {
        public static int Width = 500;
        public static int Height = 400;
        static void Main(string[] args)
        {
            CastleOfTheFlame.Main m = null;
            Thread t = new Thread(() => { (m = new Main()).Run(new Surface(0, 0, Width, Height, "Castle of the Flame", 32)); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            while (Console.ReadLine() != "exit");
            t.Abort();
            Environment.Exit(0);
        }
    }
    public class Main : Foundation
    {
        Tile[,] tile;
        Wall[,] wall;
        Lamp[] light;
        Lightmap[,] map;
        int width => Program.Width;
        int height => Program.Height;
        static int size = 50;
        REW[] image = new REW[2];
        float range = 200f;
        Size _size = new Size(size, size);
        bool init;
        int lightXY = 100;
        internal Main()
        {
        }
        public override void Initialize()
        {
            tile = new Tile[width / size, height / size];
            wall = new Wall[width / size, height / size];
            map = new Lightmap[width / size, height / size];
            light = new Lamp[10];
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    tile[i, j] = new Tile(i, j, range, _size, false);
                    tile[i, j].texture = image[1].Clone();
                }
            }
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    wall[i, j] = new Wall(i, j, range, _size);
                    wall[i, j].texture = image[0].Clone();
                }
            }
            light[0] = new Lamp(lightXY, lightXY, range, Color.Blue);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new Lightmap(i, j, range, _size);
                }
            }
        }
        public override void LoadResources()
        {
            image[0] = Asset<REW>.Load("Textures/tile");
            image[1] = REW.Create(size, size, Color.GhostWhite, PixelFormats.Bgr32);
        }
        public override void Update()
        {
            for (int i = 1; i < tile.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < tile.GetLength(1) - 1; j++)
                {
                    tile[i, j].active = false;
                }
            }
        }
        public override void PreDraw(RewBatch graphics)
        {
            if (!init)
            {
                init = true;
                LightPass.PreProcessing(light, wall, tile);
            }
        }
        public override void Draw(RewBatch rewBatch)
        {
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    rewBatch.Draw(wall[i, j].texture, i * size, j * size);
                }
            }
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    if (tile[i, j].active)
                    { 
                        //rewBatch.Draw(tile[i, j].texture, i * size, j * size);
                    }
                }
            }
        }
    }
    public static class Ext
    {
        public static REW Clone(this REW input)
        {
            return REW.Create(input.Width, input.Height, input.GetPixels(), input.BitsPerPixel);
        }
    }
}
