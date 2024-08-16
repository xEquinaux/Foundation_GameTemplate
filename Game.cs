﻿using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Timers;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Timer = System.Timers.Timer;
using FoundationR.Rew;
using FoundationR.Loader;
using static System.Net.Mime.MediaTypeNames;

namespace Foundation_GameTemplate
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static Game Instance;
		private GraphicsDeviceManager _graphicsMngr;
		private SpriteBatch _spriteBatch;
		private Viewport _viewport
		{
			get { return GraphicsDevice.Viewport; }
			set { GraphicsDevice.Viewport = value; }
		}
		static BufferedGraphicsContext context = BufferedGraphicsManager.Current;

		public RewBatch rewBatch;

		bool init = false;
		private int _portX => _viewport.X;
		private int _portY => _viewport.Y;
		private Rectangle _size => new Rectangle(0, 0, _bounds.Width, _bounds.Height);
		private Size _oldBounds;
		private Size _bounds;

		private static Point _position;
		private static Point _oldPosition;
		public static Point Position => _position;
		public static Camera CAMERA = new Camera();

		public static string SavePath => Path.Combine(new[] { Environment.GetEnvironmentVariable("USERPROFILE"), "Documents", "My Games", "CotF" });
		public static string PlayerSavePath => Path.Combine(SavePath, "Players");
		public static string WorldSavePath => Path.Combine(SavePath, "World");

		public static IList<REW> Texture = new List<REW>();
		public static Texture2D bluePane;

		public static byte[] BackBuffer
		{
			get => RewBatch.backBuffer;
			set => RewBatch.backBuffer = value;
		}

		public Game()
		{
			_graphicsMngr = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Instance = this;
		}

		protected override void Initialize()
		{
			RewBatch.Option = FoundationR.Ext.RenderOption.GDI;
			rewBatch = new RewBatch(800, 600);
			BackBuffer = new byte[800 * 600 * 4];
			base.Initialize();
		}

		protected void Resize(object sender, EventArgs e)
		{
			_graphicsMngr.PreferredBackBufferWidth = _bounds.Width;
			_graphicsMngr.PreferredBackBufferHeight = _bounds.Height;
			_graphicsMngr.ApplyChanges();
		}

		protected override bool BeginDraw()
		{
			GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			{
				_position = Window.Position;
				if (_oldBounds != _bounds || _oldPosition != _position)
				{
					//ResizeEvent.Invoke(this, new EventArgs());
				}
				_oldBounds = _bounds;
				_oldPosition = _position;
			}

			return base.BeginDraw();
		}

		protected override void EndDraw()
		{
			_spriteBatch.End();
			GraphicsDevice.Present();
		}

		protected override void UnloadContent()
		{
			base.UnloadContent();
		}

		public class TextureID
		{
			public const int
				Cans = 0,
				BluePane = 1,
				SqRed = 2,
				SqGreen = 3,
				SqBlue = 4,
				SqWhite = 5,
				SqBlack = 6;
		}
		protected override void LoadContent()
		{
			Texture.Add(Asset.LoadFromFile(@".\Textures\cans.rew"));
			Texture.Add(Asset.LoadFromFile(@".\Textures\bluepane.rew"));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Red, System.Windows.Media.PixelFormats.Bgr32, true));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Green, System.Windows.Media.PixelFormats.Bgr32, true));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Blue, System.Windows.Media.PixelFormats.Bgr32, true));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.White, System.Windows.Media.PixelFormats.Bgr32, true));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Black, System.Windows.Media.PixelFormats.Bgr32, true));
			Texture.Add(REW.Create(800, 600, System.Drawing.Color.White, System.Windows.Media.PixelFormats.Bgr32, true));
			_viewport = new Viewport(_portX, _portY, 800, 600);
			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				if (!init)
				{
					Instance = this;
					init = true;
				}
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			if (GraphicsDevice == null) return;
			
			BackBuffer = new byte[800 * 600 * 4];

			var mouse = Mouse.GetState();

			rewBatch.Draw(Texture[7], 0, 0);
			rewBatch.Draw(Texture[TextureID.Cans], 0, 0);
			//rewBatch.Draw(Texture[TextureID.BluePane], 0, 0);

			Texture2D tex = Pipeline.ArrayToTex2D(BackBuffer, 800, 600, GraphicsDevice);
			_spriteBatch.Draw(tex, Vector2.Zero, Color.White);
			tex.Dispose();

			BackBuffer = null;
			
			base.Draw(gameTime);
		}

		protected void Settings()
		{
			this.Window.Title = "cotf";
			this.Window.IsBorderless = true;
			this.Window.AllowUserResizing = true;
			this.Window.AllowAltF4 = false;
		}
	}
	public class Camera
	{
		public Vector2 oldPosition;
		public Vector2 position;
		public Vector2 velocity;
		public bool isMoving => velocity != Vector2.Zero || oldPosition != position;
		public bool follow = false;
		public bool active = false;
	}
}