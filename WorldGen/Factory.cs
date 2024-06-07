using CirclePrefect.Native;
using Color = System.Drawing.Color;
using FoundationR;
using FoundationR.Rew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace cotf.WorldGen
{
	public class Factory
	{
		public static float Distance(Vector2 a, Vector2 b)
		{
			return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
		}
		private static int
			Width = 150,
			Height;
		internal static int
			Top => /*Main.UnderworldLayer -*/ Height;
		public static short
			Air = 1,
			Tile = 0,//ArchaeaWorld.factoryBrick,
			Wall = 0,//ArchaeaWorld.factoryBrickWallUnsafe,
			Tile2 = 0,//ArchaeaWorld.Ash,
			ConveyerL = 0,//TileID.ConveyorBeltLeft,
			ConveyerR = 0,//TileID.ConveyorBeltRight,
			Door = 0;//ArchaeaWorld.factoryMetalDoor;
		//public static IList<Room> room = new List<Room>();
		public static Random rand = new Random(DateTime.Now.Millisecond);
		public void CastleGen(out Pixel[,] tile, out ushort[,] background, int width, int height, int size = 4, int maxNodes = 50, float nodeDistance = 60)
		{
			Width = width;
			Height = height;

			var brush = new Pixel[width + size * 2, height + size * 2];
			background = new ushort[width, height];

			Vector2[] nodes = new Vector2[maxNodes];
			int numNodes = 0;

			//  Generating vector nodes
			int randX = 0,
				randY = 0;
			while (numNodes < maxNodes)
			{
				foreach (Vector2 node in nodes)
				{
					do
					{
						randX = rand.Next(size, width);
						randY = rand.Next(size, height - size * 4);
						nodes[numNodes] = new Vector2(randX, randY);   
					} while (nodes.All(t => Distance(t, nodes[numNodes]) < nodeDistance));
					numNodes++;
				}
			}

			//  Carve out rooms
			int W = 0, H = 0;
			int maxSize = 7;
			int border = 8;
			foreach (Vector2 node in nodes)
			{
				//Room r = new Room(0);//(short)rand.Next(RoomID.Total));
				int _rand = 0;//rand.Next(2);
				switch (_rand)
				{
					case 0:
						W = rand.Next(4, maxSize) * size;
						H = rand.Next(4, maxSize) * size;
						//  Room construction
						int X1 = (int)node.X - W / 2;
						int X2 = (int)node.X + W / 2;
						int Y1 = (int)node.Y - H / 2;
						int Y2 = (int)node.Y + H / 2;
						//TODO
						//r.bound = new Rectangle(X1 - border, Y1 - border, W + border, H + border);
						//if (room.FirstOrDefault(t => t.bound.Intersects(r.bound)) != default)
						//{
						//    continue;
						//}
						for (int i = X1 - border; i < X2 + border; i++)
						{
							for (int j = Y1 - border; j < Y2 + border; j++)
							{
								//  If tile in-bounds
								if (i > 0 && j > 0 && i < width && j < height)
								{
									if (i < brush.GetLength(0) && j < brush.GetLength(1))
									{
										brush[i, j].SetColor(Color.Transparent);
										if (i <= X1 || i >= X2 || j <= Y1 || j >= Y2)
										{
											if (i > X1 && i < X2 && j >= Y2)
											{
												//  Floor
												brush[i, j].SetColor(Color.Gray);
												continue;
											}
											//  Ceiling and walls
											brush[i, j].SetColor(Color.Gray);
											continue;
										}
									}
								}
							}
						}
						//room.Add(r);
						break;
					default:
						break;
				}
			}

			//  Generating hallways
			nodes = nodes.OrderBy(t => Distance(t, new Vector2(0, height / 2))).ToArray();
			for (int k = 1; k < nodes.Length; k++)
			{
				int X, Y;
				Vector2 start,
						end;

				//  Normal pass
				start = nodes[k - 1];
				end = nodes[k];

				#region Hallway carving
				if (start.X < end.X && start.Y < end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (Y++ <= (start.Y + end.Y) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				else if (start.X > end.X && start.Y < end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (Y++ <= (start.Y + end.Y) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				else if (start.X < end.X && start.Y > end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (X++ <= (start.X + end.X) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				else if (start.X > end.X && start.Y > end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (X++ <= (start.X + end.X) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				#endregion
				#region Reversed pass
				start = nodes[k];
				end = nodes[k - 1];

				if (start.X < end.X && start.Y < end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (Y++ <= (start.Y + end.Y) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);
					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);
					}
				}
				else if (start.X > end.X && start.Y < end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (Y++ <= (start.Y + end.Y) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);
					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);
					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);
					}
				}
				else if (start.X < end.X && start.Y > end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (X++ <= (start.X + end.X) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				else if (start.X > end.X && start.Y > end.Y)
				{
					X = (int)start.X + (int)start.X % size;
					Y = (int)start.Y + (int)start.Y % size;

					while (X++ <= (start.X + end.X) / 2 + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (Y++ <= end.Y + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
					while (X++ <= end.X + size)
					{
						CarveHall(ref brush, ref background, X, Y, 6);

					}
				}
				#endregion
			}

			//  Clear center column
			for (int i = 0; i < brush.GetLength(0); i++)
			{
				for (int j = 0; j < brush.GetLength(1); j++)
				{
					int cx = brush.GetLength(0) / 2 - 10;
					int cx2 = brush.GetLength(0) / 2 + 10;
					if (i >= cx && i <= cx2)
					{
						brush[i, j].SetColor(Color.Transparent);
					}
				}
			}

			//  Return value
			tile = brush;
		}
		private void CarveHall(ref Pixel[,] tile, ref ushort[,] wall, int x, int y, int size = 10)
		{
			int border = 4;
			//bool flag = rand.NextBool(8);
			//bool flag2 = rand.NextBool();
			for (int i = -border; i < size + border; i++)
			{
				for (int j = -border; j < size + border; j++)
				{
					int X = Math.Max(0, Math.Min(x + i, Width - 1));
					int Y = Math.Max(0, Math.Min(y + j, Height - 1));
					//TODO
					//var r = room.FirstOrDefault(t => t.bound.Intersects(new Rectangle(X, Y, size + border, size + border)));
					//if (r != default)
					//{
					//    continue;
					//}
					if (wall[X, Y] != Wall)
					{
						tile[X, Y].SetColor(Color.Gray);
					}
					//if (flag && j == size - 1)
					{
						//tile[X, Y].type = flag2 ? ConveyerL : ConveyerR;
					}
				}
			}
			for (int j = 0; j < size; j++)
			{
				for (int i = 0; i < size; i++)
				{
					int X = Math.Max(0, Math.Min(x + i, Width - 1));
					int Y = Math.Max(0, Math.Min(y + j, Height - 1));

					//if (!GetSafely(X, Y - 1).Active && GetSafely(X, Y + 1).Active && (tile[X, Y].type == ConveyerL || tile[X, Y].type == ConveyerR))
					//{
					//	continue;
					//}
					tile[X, Y].SetColor(Color.Transparent);
					//wall[X, Y] = Wall;
					//if (j == 0 && rand.NextBool(60))
					//{
					//	for (int l = 0; l < 6; l++)
					//	{
					//		tile[X, Y + l].type = Door;
					//	}
					//}
				}
			}
		}
		//  Beams, steps, balconies, chains, platforms
		static ushort[,] stepsRight = new ushort[,]
		{
			{ 0, 1, 1, 1, 2 },
			{ 0, 0, 1, 1, 2 },
			{ 0, 0, 0, 1, 2 },
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0 }
		};
		static ushort[,] stepsLeft = new ushort[,]
		{
			{ 0, 1, 1, 1, 2 },
			{ 0, 0, 1, 1, 2 },
			{ 0, 0, 0, 1, 2 },
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0 }
		};
	}
}
