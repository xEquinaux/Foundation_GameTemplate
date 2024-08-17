using FoundationR.Rew;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

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

		protected void Resize()
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
					//Resize();
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
				Background = 2,
				SqRed = 3,
				SqGreen = 4,
				SqBlue = 5,
				SqWhite = 6,
				SqBlack = 7;
		}
		protected override void LoadContent()
		{
			Texture.Add(Asset.LoadFromFile(@".\Textures\cans.rew"));
			Texture.Add(Asset.LoadFromFile(@".\Textures\bluepane.rew"));
			Texture.Add(Asset.LoadFromFile(@".\Textures\background.rew", false));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Red, System.Windows.Media.PixelFormats.Bgr32, false));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Green, System.Windows.Media.PixelFormats.Bgr32, false));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Blue, System.Windows.Media.PixelFormats.Bgr32, false));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.White, System.Windows.Media.PixelFormats.Bgr32, false));
			Texture.Add(REW.Create(50, 50, System.Drawing.Color.Black, System.Windows.Media.PixelFormats.Bgr32, false));
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

			REW clone = Texture[TextureID.Cans].Clone();
			LightPass.PreProcessing(
				new List<System.Drawing.Rectangle>() 
				{
					new System.Drawing.Rectangle(375, 275, 50, 50),
					new System.Drawing.Rectangle(425, 275, 50, 50) 
				}, 
				new List<Background>() 
				{
					new Background() 
					{
						position = Vector2.Zero, hitbox = new System.Drawing.Rectangle(0, 0, 800, 600), texture = clone 
					} 
				}, 
				new List<Lamp>() 
				{
					new Lamp(150f) { position = mouse.Position.ToVector2(), staticLamp = false, lampColor = System.Drawing.Color.Orange }
				}
			);

			rewBatch.Draw(clone, 0, 0);
			rewBatch.Draw(Texture[TextureID.BluePane], 0, 0);
			rewBatch.Draw(Texture[TextureID.Background], mouse.X, mouse.Y);
			rewBatch.Draw(Texture[TextureID.SqRed], 375, 275);
			rewBatch.Draw(Texture[TextureID.SqRed], 425, 275);

			byte[] result = new byte[BackBuffer.Length];
			for (int i = 0; i < BackBuffer.Length; i += 4)
			{
				result[i] = BackBuffer[i + 1];
				result[i + 1] = BackBuffer[i + 2];
				result[i + 2] = BackBuffer[i + 3];
				result[i + 3] = BackBuffer[i];
			}

			Texture2D tex = result.ArrayToTex2D(800, 600, GraphicsDevice);
			_spriteBatch.Draw(tex, Vector2.Zero, Color.White);
			tex.Dispose();

			clone = null;
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