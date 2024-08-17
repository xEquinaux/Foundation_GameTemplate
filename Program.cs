using FoundationR.Lib;

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
			string type = string.Empty;
			if (args.Length > 0)
			{
				type = args[0];
			}
			switch (type)
			{
				case "-f":
					Foundation_GameTemplate.Main m = null;
					Thread t = new Thread(() => { (m = new Main()).Run(SurfaceType.WindowHandle_Loop, new FoundationR.Lib.Surface(StartX, StartY, Width, Height, Title, BitsPerPixel)); });
					t.SetApartmentState(ApartmentState.STA);
					t.Start();
					while (Console.ReadLine() != "exit") ;
					Environment.Exit(0);
					break;
				case "-mg":
					using (var game = new Game())
						game.Run();
					break;
			}
		}
	}
	public class Main : Foundation
	{
		public static byte[] BackBuffer => Game.BackBuffer;

		internal Main()
		{
		}

		public override void RegisterHooks(Form form)
		{
			Foundation.UpdateEvent += Update;
			Foundation.ResizeEvent += Resize;
			Foundation.DrawEvent += Draw;
			Foundation.InitializeEvent += Initialize;
			Foundation.LoadResourcesEvent += LoadResources;
			Foundation.MainMenuEvent += MainMenu;
			Foundation.PreDrawEvent += PreDraw;
			Foundation.ViewportEvent += Viewport;
		}

		private bool Resize(ResizeArgs e)
		{
			return false;
		}

		private void Viewport(ViewportArgs e)
		{
		}

		protected void PreDraw(PreDrawArgs e)
		{
		}

		protected void MainMenu(DrawingArgs e)
		{
		}

		protected void LoadResources()
		{
		}

		protected void Initialize(InitializeArgs e)
		{
		}

		protected void Draw(DrawingArgs e)
		{
		}

		protected void Update(UpdateArgs e)
		{
		}
	}
}
