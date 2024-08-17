using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace Foundation_GameTemplate
{
	public class Lamp
	{
		public Vector2 position;
		public float range;
		public static Color TorchLight => Color.Orange;
		public Vector2 Center => new Vector2(position.X, position.Y);
		public Color lampColor = TorchLight;
		public bool isProj = false;
		public bool staticLamp;
		public Lamp(float range)
		{
			this.range = range;
		}
		public void PreDraw(Graphics graphics)
		{
			return;
		}
		public void PostDraw(Graphics graphics)
		{
			return;
		}
		public static int AddLamp(Lamp lamp)
		{
			return 0;
		}
		public static int NewLamp(int x, int y, float range, bool staticLamp = false, int owner = 255)
		{
			return 0;
		}
	}
}
