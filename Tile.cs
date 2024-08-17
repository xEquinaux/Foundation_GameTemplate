using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Foundation_GameTemplate
{
	public class Tile
	{
		private bool lit = false;
		internal bool Active;
		public const int Size = 50;
		public int i;
		public int j;
		public new int X;
		public new int Y;
		public const float Range = 150f;
		public Rectangle hitbox;
		private Brush brush = Brushes.White;
		public Vector2 position => new Vector2(X, Y);
		private Point point => new Point(hitbox.X + Size / 2, hitbox.Y + Size / 2);
		public Vector2 Center => new Vector2(X + Size / 2, Y + Size / 2);
		internal bool onScreen = false;
		public Color color;

		public Tile()
		{
		}
		public Tile(int i, int j)
		{
		}
		public bool active(bool active)
		{
			return Active = active;
		}
		private SolidBrush Opacity(byte value = 125)
		{
			return new SolidBrush(Color.FromArgb(value, color.R, color.G, color.B));
		}
		private void Init()
		{
		}
		private bool PreUpdate()
		{
			return true;
		}
		public void Update()
		{
			
		}
		public virtual void Draw(Graphics graphics)
		{
		}
		public override string ToString()
		{
			return $"\"Active:{Active}, i:{i}, j:{j}\"";
		}
	}
	sealed class OutOfBoundsException : Exception
	{
		int i, j;
		public OutOfBoundsException(int i, int j)
		{
			this.i = i;
			this.j = j;
		}
		public override string Message => $"Attempting to access an index of: i:{i}, j:{j} outside the array.";
	}
}
