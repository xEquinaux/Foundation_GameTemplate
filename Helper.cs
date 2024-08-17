using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Matrix = System.Drawing.Drawing2D.Matrix;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using FoundationR.Rew;
using System.Diagnostics;

namespace Foundation_GameTemplate
{
	public static class Ext
	{
		public static Pixel FromColor(this Color clr)
		{
			return new Pixel(clr.B, clr.G, clr.R, clr.A);
		}
		public static REW Clone(this REW rew)
		{
			REW result = REW.Dummy(rew.Width, rew.Height, rew.BitsPerPixel, true);
			result.data = (byte[])rew.data.Clone();
			return result;
		}
		public static double Distance(this Vector2 one, Vector2 v2)
		{
			return Math.Sqrt(Math.Pow(v2.X - one.X, 2) + Math.Pow(v2.Y - one.Y, 2));
		}
		public static Vector2 ToVector2(this Point a)
		{
			return new Vector2(a.X, a.Y);
		}
		public static bool Contains(this Rectangle one, Vector2 position)
		{
			return position.X > one.Left && position.X < one.Right && position.Y > one.Top && position.Y < one.Bottom;
		}
		public static Vector2 Center(this Rectangle one)
		{
			return new Vector2(one.Location.X, one.Location.Y) + new Vector2(one.Width / 2, one.Height / 2);
		}
		public static Vector2 Position(this Rectangle r)
		{
			return new Vector2(r.Left, r.Top);
		}
		public static float MaxNormal(this Vector2 v2)
		{
			return new float[] { Math.Abs(v2.X), Math.Abs(v2.Y) }.Max();
		}
		public static float Max(this Vector2 v2)
		{
			return new float[] { v2.X, v2.Y }.Max();
		}
		public static float Min(this Vector2 v2)
		{
			return new float[] { v2.X, v2.Y }.Min();
		}
		public static Color FromFloat(float a, float r, float g, float b)
		{
			int A = (int)Math.Min(255f * a, 255),
				 R = (int)Math.Min(255f * r, 255),
				 G = (int)Math.Min(255f * g, 255),
				 B = (int)Math.Min(255f * b, 255);
			return Color.FromArgb(A, R, G, B);
		}
		public static Color Transparency(this Color one, float alpha)
		{
			int a = (int)(255f * alpha),
				 r = 0,
				 g = 0,
				 b = 0;
			r = one.R;
			g = one.G;
			b = one.B;
			return Color.FromArgb(a, r, g, b);
		}
		public static Color Average(this Color one, Color two)
		{
			int r = 0, g = 0, b = 0;
			r = one.R + two.R;
			g = one.G + two.G;
			b = one.B + two.B;
			r /= 2;
			g /= 2;
			b /= 2;
			return Color.FromArgb(r, g, b);
		}
		public static Color Average(Color[] array)
		{
			int r = 0, g = 0, b = 0;
			for (int i = 0; i < array.Length; i++)
			{
				r += array[i].R;
				g += array[i].G;
				b += array[i].B;
			}
			r /= array.Length;
			g /= array.Length;
			b /= array.Length;
			return Color.FromArgb(r, g, b);
		}
		public static Color Average(Color[,] array)
		{
			int r = 0, g = 0, b = 0;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					r += array[i, j].R;
					g += array[i, j].G;
					b += array[i, j].B;
				}
			}
			r /= array.Length;
			g /= array.Length;
			b /= array.Length;
			return Color.FromArgb(r, g, b);
		}
		private static Color Subtractive(this Color one, Color two)
		{
			int r = (int)Math.Max(Math.Min(one.R - Math.Abs(two.R - 255f), 255), 0);
			int g = (int)Math.Max(Math.Min(one.G - Math.Abs(two.G - 255f), 255), 0);
			int b = (int)Math.Max(Math.Min(one.B - Math.Abs(two.B - 255f), 255), 0);
			return Color.FromArgb(r, g, b);
		}
		public static Color SubtractiveV2(this Color one, Color two)
		{
			int r = (int)Math.Max(Math.Min(one.R - two.R, 255), 0);
			int g = (int)Math.Max(Math.Min(one.G - two.G, 255), 0);
			int b = (int)Math.Max(Math.Min(one.B - two.B, 255), 0);
			return Color.FromArgb(r, g, b);
		}
		public static Color SubtractiveV2(this Color one, Color two, float distance)
		{
			int r = (int)Math.Max(Math.Min(one.R - two.R * distance, 255), 0);
			int g = (int)Math.Max(Math.Min(one.G - two.G * distance, 255), 0);
			int b = (int)Math.Max(Math.Min(one.B - two.B * distance, 255), 0);
			return Color.FromArgb(r, g, b);
		}
		private static Color Additive(this Color one, Color two)
		{
			int r = (int)Math.Min(one.R + Math.Abs(two.R - 255f), 255);
			int g = (int)Math.Min(one.G + Math.Abs(two.G - 255f), 255);
			int b = (int)Math.Min(one.B + Math.Abs(two.B - 255f), 255);
			return Color.FromArgb(r, g, b);
		}
		private static Color Additive(this Color color, Color newColor, float distance)
		{
			return Color.FromArgb(
				 (int)(color.A * distance),
				 (int)Math.Max(Math.Min(color.R + Math.Abs(newColor.R - 255f) * distance, 255), 0),
				 (int)Math.Max(Math.Min(color.G + Math.Abs(newColor.G - 255f) * distance, 255), 0),
				 (int)Math.Max(Math.Min(color.B + Math.Abs(newColor.B - 255f) * distance, 255), 0));
		}
		public static Color AdditiveV2(this Color color, Color newColor)
		{
			return Color.FromArgb(
				 color.A,
				 (int)Math.Min(color.R + newColor.R, 255),
				 (int)Math.Min(color.G + newColor.G, 255),
				 (int)Math.Min(color.B + newColor.B, 255));
		}
		public static Color AdditiveV2(this Color color, Color newColor, float distance)
		{
			return Color.FromArgb(
				 color.A,
				 (int)Math.Min(color.R + newColor.R * distance, 255),
				 (int)Math.Min(color.G + newColor.G * distance, 255),
				 (int)Math.Min(color.B + newColor.B * distance, 255));
		}
		public static Color FlatAdditive(this Color color, Color newColor, float distance)
		{
			return Color.FromArgb(
				 color.A,
				 (int)Math.Max(Math.Min(color.R + Math.Abs(newColor.R - 255f) * distance, 255), 0),
				 (int)Math.Max(Math.Min(color.G + Math.Abs(newColor.G - 255f) * distance, 255), 0),
				 (int)Math.Max(Math.Min(color.B + Math.Abs(newColor.B - 255f) * distance, 255), 0));
		}
		public static Color Divide(this Color one, Color two)
		{
			int a = 255;
			int r = (int)Math.Min(Math.Max(one.R, 1f) / ((two.R / 255f) + 1), 255);
			int g = (int)Math.Min(Math.Max(one.G, 1f) / ((two.G / 255f) + 1), 255);
			int b = (int)Math.Min(Math.Max(one.B, 1f) / ((two.B / 255f) + 1), 255);
			return Color.FromArgb(a, r, g, b);
		}
		public static Color Multiply(this Pixel one, Pixel two)
		{
			byte a = 255;
			byte r = (byte)Math.Min(Math.Max(one.R, 1f) * ((two.R / 255f) + 1), 255);
			byte g = (byte)Math.Min(Math.Max(one.G, 1f) * ((two.G / 255f) + 1), 255);
			byte b = (byte)Math.Min(Math.Max(one.B, 1f) * ((two.B / 255f) + 1), 255);
			return Color.FromArgb(a, r, g, b);
		}
		public static Color Multiply(this Pixel one, Pixel two, float alpha)
		{
			int a = (int)Math.Max(Math.Min(255f * Math.Min(alpha, 1f), 255), 1f);
			int r = (int)Math.Min(Math.Max(one.R, 1f) * ((two.R / 255f) + 1), 255);
			int g = (int)Math.Min(Math.Max(one.G, 1f) * ((two.G / 255f) + 1), 255);
			int b = (int)Math.Min(Math.Max(one.B, 1f) * ((two.B / 255f) + 1), 255);
			return Color.FromArgb(b, g, r, a);
		}
		public static Color MultiplyV2(this Color one, Color two, float range)
		{
			int a = (int)Math.Max(Math.Min(255f * Math.Min(range, 1f), 255), 1f);
			int r = (int)Math.Min(Math.Max(one.R, 1f) * Math.Max((two.R / 255f + 1f) * range, 1f), 255);
			int g = (int)Math.Min(Math.Max(one.G, 1f) * Math.Max((two.G / 255f + 1f) * range, 1f), 255);
			int b = (int)Math.Min(Math.Max(one.B, 1f) * Math.Max((two.B / 255f + 1f) * range, 1f), 255);
			return Color.FromArgb(a, r, g, b);
		}
		public static Color NonAlpha(this Color color)
		{
			int a = 255;
			int r = color.R;
			int g = color.G;
			int b = color.B;
			return Color.FromArgb(a, r, g, b);
		}
		public static Color Clone(this Color copy)
		{
			int a = copy.A;
			int r = copy.R;
			int g = copy.G;
			int b = copy.B;
			return Color.FromArgb(a, r, g, b);
		}
		public static void Write(this BinaryWriter bw, Vector2 vector2)
		{
			bw.Write(vector2.X);
			bw.Write(vector2.Y);
		}
		public static void Write(this BinaryWriter bw, Color color)
		{
			bw.Write(color.A);
			bw.Write(color.R);
			bw.Write(color.G);
			bw.Write(color.B);
		}
		public static Vector2 ReadVector2(this BinaryReader br)
		{
			Vector2 v2 = Vector2.Zero;
			v2.X = br.ReadSingle();
			v2.Y = br.ReadSingle();
			return v2;
		}
		public static Color ReadColor(this BinaryReader br)
		{
			byte a = br.ReadByte();
			byte r = br.ReadByte();
			byte g = br.ReadByte();
			byte b = br.ReadByte();
			return Color.FromArgb(a, r, g, b);
		}
	}
	public static class Helper
	{
		public static Vector2 AngleBased(Vector2 position, float angle, float radius)
		{
			float cos = position.X + (float)(radius * Math.Cos(angle));
			float sine = position.Y + (float)(radius * Math.Sin(angle));
			return new Vector2(cos, sine);
		}
		public static Vector2 AngleBased(float angle, float radius)
		{
			float cos = (float)(radius * Math.Cos(angle));
			float sine = (float)(radius * Math.Sin(angle));
			return new Vector2(cos, sine);
		}
		public static void Clamp(ref int input, int min, int max, out int result)
		{
			if (input < min)
				input = min;
			if (input > max)
				input = max;
			result = input;
		}
		public const float Radian = 0.017f;
		public static double ToRadian(double degrees)
		{
			return degrees * Radian;
		}
		public static float ToRadian(float degrees)
		{
			return degrees * Radian;
		}
		public static double ToDegrees(double radians)
		{
			return radians / Radian;
		}
		public static float ToDegrees(float radians)
		{
			return radians / Radian;
		}
		public static float Ratio(float width, float height)
		{
			return width / height;
		}
		public static float RatioConvert(float ratio, float width)
		{
			return width * ratio;
		}
		public static double Distance(Vector2 one, Vector2 two)
		{
			return Math.Sqrt(Math.Pow(two.X - one.X, 2) + Math.Pow(two.Y - one.Y, 2));
		}
		public static float NormalizedRadius(float distance, float radius)
		{
			return (float)Math.Abs(Math.Min(distance / radius, 1f) - 1f);
		}
		public static float NormalizedRadius(Vector2 one, Vector2 two, float radius)
		{
			return (float)Math.Abs(Math.Min(Helper.Distance(one, two) / radius, 1f) - 1f);
		}
		public static float AngleTo(Vector2 from, Vector2 to)
		{
			return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
		}
		public static Vector2 AngleToSpeed(float angle, float amount)
		{
			float cos = (float)(amount * Math.Cos(angle));
			float sine = (float)(amount * Math.Sin(angle));
			return new Vector2(cos, sine);
		}
		public static double WrapRadianAngle(double angle)
		{
			while (angle < Math.PI * -1 || angle > Math.PI)
			{
				double rem = 0d;
				if (angle > Math.PI)
				{
					rem = (Math.PI * -1) % (angle * -1);
					angle = Math.PI * -1 + rem;
				}
				if (angle < Math.PI * -1)
				{
					rem = Math.PI % (angle * -1);
					angle = Math.PI * -1 + rem;
				}
			}
			return angle;
		}
		public static double[] GetAngle(int max)
		{
			double[] result = new double[max];
			double start = Math.PI * 2f / max;
			for (int i = 0; i < max; i++)
			{
				result[i] = start * (i + 1);
			}
			return result;
		}
	}
	public enum CollisionType
	{
		None,
		Top,
		Right,
		Bottom,
		Left,
		Unbuffered
	}

	public static class Drawing
	{
		sealed class Error
		{
			internal static int[,] GetArray(int width, int height, int size = 16)
			{
				int i = width / size;
				int j = height / size;
				int[,] brush = new int[i, j];
				int num = -1;
				for (int n = 0; n < brush.GetLength(1); n++)
				{
					for (int m = 0; m < brush.GetLength(0); m++)
					{
						if (n > 0 && m == 0)
						{
							num = brush[m, n - 1] * -1;
							_write(ref brush, m, n, num);
							continue;
						}
						_write(ref brush, m, n, num *= -1);
					}
				}
				return brush;
			}
			static void _write(ref int[,] brush, int m, int n, int value)
			{
				brush[m, n] = value;
			}
		}
		public static Color LightAverage(Bitmap bitmap)
		{
			Color[,] map = new Color[bitmap.Width, bitmap.Height];
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					map[i, j] = bitmap.GetPixel(i, j);
				}
			}
			return Ext.Average(map);
		}
		public static Bitmap ErrorResult(int width, int height, int size = 16)
		{
			Bitmap result = new Bitmap(width, height);
			using (Graphics gfx = Graphics.FromImage(result))
			{
				int[,] brush = Error.GetArray(width, height, size);
				for (int i = 0; i < brush.GetLength(0); i++)
				{
					for (int j = 0; j < brush.GetLength(1); j++)
					{
						int x = i * size;
						int y = j * size;
						switch (brush[i, j])
						{
							case -1:
								gfx.FillRectangle(Brushes.MediumPurple, new Rectangle(x, y, size, size));
								gfx.DrawRectangle(Pens.Purple, new Rectangle(x, y, size - 1, size - 1));
								break;
							case 1:
								gfx.FillRectangle(Brushes.Black, new Rectangle(x, y, size, size));
								gfx.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(50, 50, 50))), new Rectangle(x, y, size - 1, size - 1));
								break;
							default:
								break;
						}
					}
				}
			}
			return result;
		}
		public static Bitmap Mask_Circle(int size, Color mask)
		{
			float offset = 0.95f;
			Bitmap result = new Bitmap(size, size);
			using (Graphics gfx = Graphics.FromImage(result))
			{
				gfx.FillRectangle(new SolidBrush(mask), new RectangleF(0, 0, size, size));
				gfx.FillEllipse(Brushes.Black, new RectangleF(0, 0, size * offset, size * offset));
				result.MakeTransparent(Color.Black);
			}
			return result;
		}
		public static Image TextureMask(Bitmap image, Bitmap mask, Color transparency)
		{
			Bitmap result = new Bitmap(image.Width, image.Height);
			using (Graphics _mask = Graphics.FromImage(mask))
			{
				using (Graphics _image = Graphics.FromImage(image))
				{
					if (mask.Width < image.Width && mask.Height < image.Height)
					{
						_mask.ScaleTransform((float)image.Width / mask.Width, (float)image.Height / mask.Width);
					}
					_image.DrawImage(mask, Point.Empty);
				}
				image.MakeTransparent(transparency);
				Graphics gfx3 = Graphics.FromImage(result);
				gfx3.DrawImage(image, Point.Empty);
				gfx3.Dispose();
			}
			return result;
		}
		private static bool dynamic(List<Rectangle> brush, Vector2 pixel, Vector2 topLeft, Lamp light, float range)
		{
			Vector2 c = light.position;
			for (int n = 0; n < brush.Count; n++)
			{
				Vector2[] corner = new Vector2[]
				{
					brush[n].Position(),
					brush[n].Position() + new Vector2(brush[n].Width, 0),
					brush[n].Position() + new Vector2(0, brush[n].Height),
					brush[n].Position() + new Vector2(brush[n].Width, brush[n].Height)
				};
				corner = corner.OrderByDescending(t => Helper.Distance(c, t)).ToArray();
				Vector2[] v2 = new Vector2[] { corner[1], corner[2] };
				Vector2 _pixel = topLeft + pixel;
				double a0 = Helper.AngleTo(v2[0], c);
				double a1 = Helper.AngleTo(v2[1], c);
				double angle = Helper.AngleTo(_pixel, c);
				if (light.position.X < brush[n].Center().X)
				{
					a0 = Helper.AngleTo(c, v2[0]);
					a1 = Helper.AngleTo(c, v2[1]);
					angle = Helper.AngleTo(c, _pixel);
				}
				Vector2[] _corner = corner.Concat(new Vector2[] { brush[n].Center() }).ToArray();
				_corner = _corner.OrderBy(t => Helper.Distance(c, t)).ToArray();

				float dist = (float)Helper.Distance(c, _pixel);
				float dist2 = (float)Helper.Distance(c, _corner[1]);
				double[] _angle = new double[] { a0, a1 }.OrderByDescending(t => t).ToArray();
				if (angle > _angle[1] && angle < _angle[0])
				{
					if (dist > dist2)
					{
						return false;
					}
				}
			}
			return true;
		}
		public static REW Lightpass0(List<Rectangle> brush, REW texture, Vector2 topLeft, Lamp light, float range)
		{
			REW layer0 = texture.Clone();
			REW layer1 = texture;
			Parallel.For(0, texture.Width, i =>
			{
				for (int j = 0; j < texture.Height; j++)
				{
					float distance = (float)Helper.Distance(topLeft + new Vector2(i, j), light.position);
					float radius = Helper.NormalizedRadius(distance, range);
					if (radius > 0f && dynamic(brush, new Vector2(i, j), topLeft, light, range))
					{
						Pixel srcPixel = layer0.GetPixel(i, j);
						layer1.SetPixel(i, j, Ext.Multiply(srcPixel, light.lampColor.FromColor(), radius));
					}
				}
			});
			return layer1;
		}
		public static void TextureLighting(Image texture, Rectangle hitbox, ref Color color, Color startColor, ref float alpha, Graphics graphics, ImageAttributes attr)
		{
			using (Bitmap bitmap = new Bitmap(hitbox.Width, hitbox.Height))
			{
				using (Graphics gfx = Graphics.FromImage(bitmap))
				{
					gfx.DrawImage(texture, new Rectangle(0, 0, hitbox.Width, hitbox.Height));
					graphics.DrawImage(bitmap, hitbox);
					if (alpha > 0f)
					{
						attr = Drawing.SetColor(color, alpha);
						graphics.DrawImage(bitmap, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, attr);
					}
				}
			}
			alpha = 0f;
			color = startColor;
		}
		public static void TextureLighting(Image texture, Rectangle hitbox, ref Color color, Color startColor, ref float alpha, Graphics graphics)
		{
			using (Bitmap bitmap = new Bitmap(hitbox.Width, hitbox.Height))
			{
				using (Graphics gfx = Graphics.FromImage(bitmap))
				{
					var attr = Drawing.SetColor(startColor);
					gfx.DrawImage(texture, new Rectangle(0, 0, hitbox.Width, hitbox.Height));
					graphics.DrawImage(bitmap, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, attr);
					if (alpha != 0f)
					{
						attr = Drawing.SetColor(color, alpha);
						graphics.DrawImage(bitmap, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, attr);
					}
				}
			}
			alpha = 0f;
			color = startColor;
		}
		public static Bitmap RecolorTexture(ref Bitmap texture, Color color)
		{
			using (Graphics gfx = Graphics.FromImage(texture))
			{
				gfx.DrawImage(texture, new Rectangle(0, 0, texture.Width, texture.Height), 0, 0, texture.Width, texture.Height, GraphicsUnit.Pixel, Drawing.SetColor(color));
			}
			return texture;
		}
		public static Color TranslucentColorShift(Color color, float distance)
		{
			return Color.FromArgb(
				 (int)Math.Max(Math.Min(color.A * distance, 255), 0),
				 color.R,
				 color.G,
				 color.B);
		}
		public static void DrawScale(Image image, Vector2 position, int width, int height, Color transparency, Graphics graphics, ImageAttributes attr, float scaleX = 1f, float scaleY = 1f)
		{
			MemoryStream mem = new MemoryStream();
			using (Bitmap clone = (Bitmap)image.Clone())
			{
				clone.MakeTransparent(transparency);
				using (Bitmap bmp = new Bitmap(image.Width, image.Height))
				{
					using (Graphics gfx = Graphics.FromImage(bmp))
					{
						gfx.ScaleTransform(scaleX, scaleY);
						gfx.DrawImage(clone, Point.Empty);
						bmp.Save(mem, ImageFormat.Png);
					}
				}
				graphics.DrawImage(Bitmap.FromStream(mem), new Rectangle((int)position.X, (int)position.Y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
			}
			mem.Dispose();
		}
		public static void DrawTexture(Image image, Rectangle rectangle, int width, int height, Graphics graphics, ImageAttributes attr)
		{
			graphics.DrawImage(image, rectangle, 0, 0, width, height, GraphicsUnit.Pixel, attr);
		}
		public static void DrawRotate(Image image, Vector2 position, RectangleF rectangle, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics, float scale = 1f)
		{
			MemoryStream mem = new MemoryStream();
			using (Bitmap clone = (Bitmap)image.Clone())
			{
				clone.MakeTransparent(transparency);
				using (Bitmap bmp = new Bitmap(image.Width, image.Height))
				{
					using (Graphics gfx = Graphics.FromImage(bmp))
					{
						switch (type)
						{
							case RotateType.MatrixTransform:
								var matrix = new Matrix();
								matrix.RotateAt(angle, origin);
								gfx.Transform = matrix;
								break;
							case RotateType.GraphicsTransform:
								gfx.TranslateTransform(origin.X, origin.Y);
								gfx.RotateTransform(angle);
								gfx.TranslateTransform(-origin.X, -origin.Y);
								break;
							default:
								break;
						}
						gfx.ScaleTransform(scale, scale);
						gfx.DrawImage(clone, Point.Empty);
						bmp.Save(mem, ImageFormat.Png);
					}
				}
				graphics.DrawImage(Bitmap.FromStream(mem), position.X, position.Y, rectangle, GraphicsUnit.Pixel);
			}
			mem.Dispose();

		}
		public static void DrawRotate(Image image, Vector2 position, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics, float scale = 1f)
		{
			MemoryStream mem = new MemoryStream();
			using (Bitmap clone = (Bitmap)image.Clone())
			{
				clone.MakeTransparent(transparency);
				using (Bitmap bmp = new Bitmap(image.Width, image.Height))
				{
					using (Graphics gfx = Graphics.FromImage(bmp))
					{
						switch (type)
						{
							case RotateType.MatrixTransform:
								var matrix = new Matrix();
								matrix.RotateAt(angle, origin);
								gfx.Transform = matrix;
								break;
							case RotateType.GraphicsTransform:
								gfx.TranslateTransform(origin.X, origin.Y);
								gfx.RotateTransform(angle);
								gfx.TranslateTransform(-origin.X, -origin.Y);
								break;
							default:
								break;
						}
						gfx.ScaleTransform(scale, scale);
						gfx.DrawImage(clone, Point.Empty);
						bmp.Save(mem, ImageFormat.Png);
					}
				}
				graphics.DrawImage(Bitmap.FromStream(mem), new PointF(position.X, position.Y));
			}
			mem.Dispose();
		}
		public static void DrawRotate(Image image, Rectangle rect, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics)
		{
			MemoryStream mem = new MemoryStream();
			using (Bitmap clone = (Bitmap)image.Clone())
			{
				clone.MakeTransparent(transparency);
				using (Bitmap bmp = new Bitmap(image.Width, image.Height))
				{
					using (Graphics gfx = Graphics.FromImage(bmp))
					{
						switch (type)
						{
							case RotateType.MatrixTransform:
								var matrix = new Matrix();
								matrix.RotateAt(angle, origin);
								gfx.Transform = matrix;
								break;
							case RotateType.GraphicsTransform:
								gfx.TranslateTransform(origin.X, origin.Y);
								gfx.RotateTransform(angle);
								gfx.TranslateTransform(-origin.X, -origin.Y);
								break;
							default:
								break;
						}
						gfx.DrawImage(clone, Point.Empty);
						bmp.Save(mem, ImageFormat.Png);
					}
				}
				graphics.DrawImage(Bitmap.FromStream(mem), rect);
			}
			mem.Dispose();
		}
		public static void DrawRotate(Image image, Rectangle rect, Rectangle sourceRect, float angle, PointF origin, Color newColor, Color transparency, RotateType type, Graphics graphics)
		{
			ImageAttributes attributes = new ImageAttributes();
			ColorMatrix transform = new ColorMatrix(new float[][]
			{
					 new float[] { newColor.R / 255f, 0, 0, 0, 0 },
					 new float[] { 0, newColor.G / 255f, 0, 0, 0 },
					 new float[] { 0, 0, newColor.B / 255f, 0, 0 },
					 new float[] { 0, 0, 0, newColor.A / 255f, 0 },
					 new float[] { 0, 0, 0, 0, 0 }
			});
			attributes.SetColorMatrix(transform);

			MemoryStream mem = new MemoryStream();
			using (Bitmap clone = (Bitmap)image.Clone())
			{
				clone.MakeTransparent(transparency);
				using (Bitmap bmp = new Bitmap(image.Width, image.Height))
				{
					using (Graphics gfx = Graphics.FromImage(bmp))
					{
						switch (type)
						{
							case RotateType.MatrixTransform:
								var matrix = new Matrix();
								matrix.RotateAt(angle, origin);
								gfx.Transform = matrix;
								break;
							case RotateType.GraphicsTransform:
								gfx.TranslateTransform(origin.X, origin.Y);
								gfx.RotateTransform(angle);
								gfx.TranslateTransform(-origin.X, -origin.Y);
								break;
							default:
								break;
						}
						gfx.DrawImage(clone, Point.Empty);
						bmp.Save(mem, ImageFormat.Png);
					}
				}
				graphics.DrawImage(Bitmap.FromStream(mem), rect, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height, GraphicsUnit.Pixel, attributes);
			}
			mem.Dispose();
		}
		public static ImageAttributes ReColor(Color color, Color newColor, float alpha = 1f)
		{
			ImageAttributes attributes = new ImageAttributes();
			ColorMatrix transform = new ColorMatrix(new float[][]
			{
					 new float[] { color.R / 255f, 0, 0, 0, 0 },
					 new float[] { 0, color.G / 255f, 0, 0, 0 },
					 new float[] { 0, 0, color.B / 255f, 0, 0 },
					 new float[] { 0, 0, 0, color.A / 255f, 0 },
					 new float[] {   newColor.R / 255f,
										  newColor.G / 255f,
										  newColor.B / 255f,
										  alpha, 0 }
			});
			attributes.SetColorMatrix(transform);
			return attributes;
		}
		public static ImageAttributes SetColor(Color color, float alpha = 1f)
		{
			ImageAttributes attributes = new ImageAttributes();
			ColorMatrix transform = new ColorMatrix(new float[][]
			{
					 new float[] { color.R / 255f, 0, 0, 0, 0 },
					 new float[] { 0, color.G / 255f, 0, 0, 0 },
					 new float[] { 0, 0, color.B / 255f, 0, 0 },
					 new float[] { 0, 0, 0, Math.Max(0f, Math.Min(1f, alpha)), 0 },
					 new float[] { 0, 0, 0, 0, 0 }
			});
			attributes.SetColorMatrix(transform);
			return attributes;
		}
		public static ImageAttributes SetColor(Color color)
		{
			ImageAttributes attributes = new ImageAttributes();
			ColorMatrix transform = new ColorMatrix(new float[][]
			{
					 new float[] { color.R / 255f, 0, 0, 0, 0 },
					 new float[] { 0, color.G / 255f, 0, 0, 0 },
					 new float[] { 0, 0, color.B / 255f, 0, 0 },
					 new float[] { 0, 0, 0, color.A / 255f, 0 },
					 new float[] { 0, 0, 0, 0, 0 }
			});
			attributes.SetColorMatrix(transform);
			return attributes;
		}
		public static SolidBrush Opacity(Color color, float value)
		{
			byte min = (byte)(Math.Min(value, 1f) * 255);
			return new SolidBrush(Color.FromArgb(min, color.R, color.G, color.B));
		}
		public static void DrawColorTransform(Image image, Vector2 one, Vector2 two, Rectangle hitbox, Color start, Color end, float radius, Graphics graphics)
		{
			float f = Helper.NormalizedRadius(one, two, radius);
			if (f < 1f && f != 0f)
			{
				graphics.DrawImage(image, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, Drawing.ReColor(start, end, f / 2f));
			}
		}
		public static void DrawColorOverlay(Vector2 one, Vector2 two, Rectangle hitbox, Color start, Color end, float radius, Graphics graphics)
		{
			float f = Helper.NormalizedRadius(one, two, radius);
			if (f < 1f && f != 0f)
			{
				graphics.DrawImage(new Bitmap(hitbox.Width, hitbox.Height), hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, Drawing.ReColor(start, end, f));
			}
		}
	}
	public enum RotateType
	{
		MatrixTransform,
		GraphicsTransform
	}
	public static class Draw
	{
		public const float radian = 0.017f;
		public static float radians(float distance)
		{
			return radian * (45f / distance);
		}
	}
	public class Treasures
	{
		public int offset;
		private ushort floorID;
		private ushort newTileID;
		private ushort wallID;
		private List<Vector2> list;
		public void Initialize(int offset, ushort newTileID, ushort floorID, ushort wallID)
		{
			this.offset = offset;
			this.newTileID = newTileID;
			this.floorID = floorID;
			this.wallID = wallID;
			//list = ArchaeaWorld.origins;
		}
		public bool PlaceTile(int i, int j, ushort tileType, bool genPlace = false, bool force = false, int proximity = -1, bool wall = false, int style = 0)
		{
			//Tile tile = Main.tile[i, j];
			//if (proximity != -1 && Vicinity(new Vector2(i, j), proximity, tileType))
			//      return false;
			//if (!genPlace)
			//{
			//      tile.HasTile = true;
			//      tile.TileType = tileType;
			//}
			//else
			//{
			//      WorldGen.PlaceTile(i, j, tileType, true, force, -1, style);
			//}
			//if (tile.TileType == tileType)
			//      return true;
			return false;
		}
		public static Vector2[] FindAll(Vector2 region, int width, int height, bool overflow = false, ushort[] floorIDs = null)
		{
			int index = width * height * floorIDs.Length;
			int amount = (int)Math.Sqrt(index) / 10;
			int count = 0;
			var tiles = new Vector2[index];
			foreach (ushort floorType in floorIDs)
				for (int i = (int)region.X; i < (int)region.X + width; i++)
					for (int j = (int)region.Y; j < (int)region.Y + height; j++)
					{
						//if (!ArchaeaWorld.Inbounds(i, j)) continue;
						//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
						//Tile origin = Main.tile[i, j];
						//Tile ceiling = Main.tile[i, j - 1];
						//Tile ground = Main.tile[i, j + 1];
						//Tile right = Main.tile[i + 1, j];
						//Tile ieft = Main.tile[i - 1, j];
						//if (origin.HasTile && Main.tileSolid[origin.TileType]) continue;
						//if (ceiling.HasTile && Main.tileSolid[ceiling.TileType] && ceiling.TileType == floorType || 
						//      ground.HasTile && Main.tileSolid[ground.TileType] && ground.TileType == floorType || 
						//      right.HasTile && Main.tileSolid[right.TileType] && right.TileType == floorType || 
						//      ieft.HasTile && Main.tileSolid[ieft.TileType] && ieft.TileType == floorType)
						//{
						//      if (count < tiles.Length)
						//      {
						//         tiles[count] = new Vector2(i, j);
						//         count++;
						//      }
						//}
					}
			return tiles;
		}
		public static Vector2[] GetFloor(Vector2 region, int width, int height, bool overflow = false, ushort[] floorIDs = null)
		{
			int index = width * height * floorIDs.Length;
			int amount = (int)Math.Sqrt(index) / 10;
			int count = 0;
			var tiles = new Vector2[index];
			foreach (ushort floorType in floorIDs)
				for (int i = (int)region.X; i < (int)region.X + width; i++)
					for (int j = (int)region.Y; j < (int)region.Y + height; j++)
					{
						//if (!ArchaeaWorld.Inbounds(i, j)) continue;
						//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
						//Tile floor = Main.tile[i, j];
						//Tile ground = Main.tile[i, j + 1];
						//if (floor.HasTile && Main.tileSolid[floor.TileType]) continue;
						//if (ground.HasTile && Main.tileSolid[ground.TileType] && ground.TileType == floorType)
						//{
						//      if (count < tiles.Length)
						//      {
						//         tiles[count] = new Vector2(i, j);
						//         count++;
						//      }
						//}
					}
			return tiles;
		}
		public static Vector2[] GetFloor(int i, int j, int width, int height, ushort floorType)
		{
			//if (!ArchaeaWorld.Inbounds(i, j))
			//{
			//      return new Vector2[] { Vector2.Zero };
			//}
			List<Vector2> list = new List<Vector2>();
			for (int m = i; m < i + width; m++)
			{
				for (int n = j; n < j + height; n++)
				{
					//if (!ArchaeaWorld.Inbounds(i, j))
					//{
					//   if (list.Count == 0)
					//   { 
					//         return new Vector2[] { Vector2.Zero };
					//   }
					//   else return list.ToArray();
					//}
					//if (Main.tile[m, n].TileType != 0)
					//{
					//   if (!Main.tile[m, n - 1].HasTile)
					//   { 
					//         list.Add(new Vector2(m, n));
					//   }
					//}
				}
			}
			return list.ToArray();
		}
		public static Vector2[] GetCeiling(Vector2 region, int radius, bool overflow = false, ushort tileType = 0)
		{
			int index = (int)Math.Pow(radius * 2, 2);
			int count = 0;
			var tiles = new Vector2[index];
			for (int i = (int)region.X - radius; i < (int)region.X + radius; i++)
				for (int j = (int)region.Y - radius; j < (int)region.Y + radius; j++)
				{
					//if (!ArchaeaWorld.Inbounds(i, j)) continue;
					//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
					//Tile roof = Main.tile[i, j];
					//Tile ceiling = Main.tile[i, j + 1];
					//if (ceiling.HasTile && Main.tileSolid[ceiling.TileType]) continue;
					//if (roof.HasTile && Main.tileSolid[roof.TileType] && roof.TileType == tileType)
					//{
					//   if (count < tiles.Length)
					//   {
					//         tiles[count] = new Vector2(i, j);
					//         count++;
					//   }
					//}
				}
			return tiles;
		}
		public static Vector2[] GetCeiling(Vector2 region, int width, int height, bool overflow = false, ushort tileType = 0)
		{
			var tiles = new List<Vector2>();
			for (int i = (int)region.X; i < width; i++)
				for (int j = (int)region.Y; j < height; j++)
				{
					//if (!ArchaeaWorld.Inbounds(i, j)) continue;
					//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
					//Tile roof = Main.tile[i, j];
					//Tile ceiling = Main.tile[i, j + 1];
					//if (ceiling.HasTile && Main.tileSolid[ceiling.TileType]) continue;
					//if (roof.HasTile && Main.tileSolid[roof.TileType] && roof.TileType == tileType)
					//   tiles.Add(new Vector2(i, j + 1));
				}
			return tiles.ToArray();
		}
		public static Vector2[] GetRegion(Vector2 region, int width, int height, bool overflow = false, bool attach = false, ushort[] tileTypes = null)
		{
			int index = width * height * tileTypes.Length;
			int count = 0;
			var tiles = new Vector2[index];
			foreach (ushort tileType in tileTypes)
				for (int i = (int)region.X; i < (int)region.X + width; i++)
					for (int j = (int)region.Y; j < (int)region.Y + height; j++)
					{
						//if (count >= tiles.Length) continue;
						//if (!ArchaeaWorld.Inbounds(i, j)) continue;
						//if (attach && Main.tile[i, j].TileType != tileType) continue;
						//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
						//tiles[count] = new Vector2(i, j);
						//count++;
					}
			return tiles;
		}
		public static Vector2[] GetWall(Vector2 region, int width, int height, bool overflow = false, bool attach = false, ushort[] attachTypes = null)
		{
			int index = width * height * attachTypes.Length;
			int count = 0;
			var tiles = new Vector2[index];
			foreach (ushort tileType in attachTypes)
				for (int i = (int)region.X; i < (int)region.X + width; i++)
					for (int j = (int)region.Y; j < (int)region.Y + height; j++)
					{
						//if (count >= tiles.Length) continue;
						//if (!ArchaeaWorld.Inbounds(i, j)) continue;
						//if (overflow & WorldGen.genRand.Next(5) == 0) continue;
						//Tile tile = Main.tile[i, j];
						//Tile wallL = Main.tile[i - 1, j];
						//Tile wallR = Main.tile[i + 1, j];
						//if (wallL.HasTile && Main.tileSolid[wallL.TileType])
						//      if (!tile.HasTile || !Main.tileSolid[tile.TileType])
						//      {
						//         if (attach && wallL.TileType != tileType) continue;
						//         tiles[count] = new Vector2(i, j);
						//      }
						//if (wallR.HasTile && Main.tileSolid[wallR.TileType])
						//      if (!tile.HasTile || !Main.tileSolid[tile.TileType])
						//      {
						//         if (attach && wallR.TileType != tileType) continue;
						//         tiles[count] = new Vector2(i, j);
						//      }
						//count++;
					}
			return tiles;
		}
		public static Vector2[] GetWall(int x, int y, int width, int height, ushort[] tileTypes = null, int radius = -1)
		{
			int count = 0;
			List<Vector2> list = new List<Vector2>();
			foreach (ushort tileType in tileTypes)
				for (int i = x; i < width; i++)
					for (int j = y; j < width; j++)
					{
						//if (!ArchaeaWorld.Inbounds(i, j))
						//      continue;
						//if (radius != -1 && Vicinity(new Vector2(i, j), radius, tileType))
						//      continue;
						//Tile up = Main.tile[i, j - 1];
						//Tile left = Main.tile[i - 1, j];
						//Tile right = Main.tile[i + 1, j];
						//if ((left.TileType == tileType || right.TileType == tileType) && !up.HasTile)
						//{
						//      list.Add(new Vector2(i, j));
						//      count++;
						//}
					}
			return list.ToArray();
		}
		public static bool Vicinity(Vector2 region, int radius, short tileType)
		{
			int x = (int)region.X;
			int y = (int)region.Y;
			for (int i = x - radius; i < x + radius; i++)
				for (int j = y - radius; j < y + radius; j++)
				{
					//   if (!ArchaeaWorld.Inbounds(i, j)) continue;
					//   if (Main.tile[i, j].TileType == tileType)
					//      return true;
				}
			return false;
		}
		public static int Vicinity(Vector2 region, int radius, short[] tileType)
		{
			Func<int> count = delegate ()
			{
				int x = (int)region.X;
				int y = (int)region.Y;
				int tiles = 0;
				for (int i = x - radius; i < x + radius; i++)
					for (int j = y - radius; j < y + radius; j++)
					{
						//if (!ArchaeaWorld.Inbounds(i, j)) continue;
						//foreach (ushort type in tileType)
						//      if (Main.tile[i, j].TileType == type && Main.tile[i, j].HasTile)
						//      {
						//         tiles++;
						//         break;
						//      }
					}
				return tiles;
			};
			return count();
		}
		public static bool Vicinity(Vector2 region, int radius, short[] tileType, int limit)
		{
			Func<bool> count = delegate ()
			{
				int x = (int)region.X;
				int y = (int)region.Y;
				int tiles;
				foreach (ushort type in tileType)
				{
					tiles = 0;
					//for (int i = x - radius; i < x + radius; i++)
					//   for (int j = y - radius; j < y + radius; j++)
					//   {
					//         if (!ArchaeaWorld.Inbounds(i, j)) continue;
					//         if (Main.tile[i, j].TileType == type && Main.tile[i, j].HasTile)
					//         {
					//            if (tiles++ > limit)
					//               return true;
					//         }
					//   }
				}
				return false;
			};
			return count();
		}
		public static int ProximityCount(Vector2 region, int radius, short tileType)
		{
			int x = (int)region.X;
			int y = (int)region.Y;
			int count = 0;
			for (int i = x - radius; i < x + radius; i++)
				for (int j = y - radius; j < y + radius; j++)
				{
					//if (!ArchaeaWorld.Inbounds(i, j)) continue;
					//Tile tile = Main.tile[i, j];
					//if (tile.TileType == tileType)
					//   count++;
				}
			return count;
		}
	}
}
