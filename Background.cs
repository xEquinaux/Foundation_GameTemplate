using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FoundationR.Rew;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;


namespace Foundation_GameTemplate
{
	public class Background
	{
		public REW texture;
		public Vector2 position;
		public Vector2 Center => hitbox.Center();
		public Rectangle hitbox;
		public int Width => hitbox.Width;
		public float x => position.X;
		public float y => position.Y;
	}
}
