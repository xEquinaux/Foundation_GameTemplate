using Microsoft.Xna.Framework.Graphics;

namespace Foundation_GameTemplate
{
	public static class Pipeline
	{
		public static Texture2D ArrayToTex2D(this IntPtr data, int width, int height, GraphicsDevice device)
		{
			Texture2D tex = new Texture2D(device, width, height);
			tex.SetData(data);
			return tex;
		}
		public static Texture2D ArrayToTex2D(this byte[] data, int width, int height, GraphicsDevice device)
		{
			Texture2D tex = new Texture2D(device, width, height);
			tex.SetData(data);
			return tex;
		}
		public static Texture2D BitmapToTex2D(this Bitmap image, GraphicsDevice device)
		{
			Texture2D tex = new Texture2D(device, image.Width, image.Height);
			var bits = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			tex.SetData(bits.Scan0);
			image.UnlockBits(bits);
			return tex;
		}
		public static Texture2D BitmapToTex2D(this Image texture, GraphicsDevice device)
		{
			Bitmap image = (Bitmap)texture;
			Texture2D tex = new Texture2D(device, image.Width, image.Height);
			var bits = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			tex.SetData(bits.Scan0);
			image.UnlockBits(bits);
			return tex;
		}
	}
}