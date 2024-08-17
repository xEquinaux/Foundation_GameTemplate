using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using FoundationR.Rew;

namespace Foundation_GameTemplate
{
	public sealed class LightPass
	{
		public static List<Rectangle> NearbyTile(List<Rectangle> occlude, Lamp lamp)
		{
			List<Rectangle> brush = new List<Rectangle>();
			for (int i = 0; i < occlude.Count; i++)
			{
				if (Helper.Distance(occlude[i].Center(), lamp.Center) < lamp.range)
				{
					brush.Add(occlude[i]);
				} 
			}
			return brush;
		}
		public static List<Background> NearbyFloor(List<Background> floor, Lamp lamp)
		{
			List<Background> brush = new List<Background>();
			for (int i = 0; i < floor.Count; i++)
			{ 
				if (floor[i] != null)
				{
					if (Helper.Distance(floor[i].Center, lamp.position) < lamp.range + floor[i].Width / 2)
					{
						brush.Add(floor[i]);
					}
				}
			}
			return brush;
		}
		public static void PreProcessing(List<Rectangle> occlude, List<Background> _floor, List<Lamp> lamp)
		{
			for (int n = 0; n < lamp.Count; n++)
			{
				Lamp _lamp = lamp[n];
				if (lamp == null)
					continue;

				List<Rectangle> brush = NearbyTile(occlude, _lamp);
				List<Background> floor = NearbyFloor(_floor, _lamp);

				for (int i = 0; i < floor.Count; i++)
				{ 
					if (Helper.Distance(floor[i].Center, _lamp.Center) <= _lamp.range + floor[i].Width / 2)
					{
						floor[i].texture = Drawing.Lightpass0(brush, floor[i].texture, floor[i].position, _lamp, _lamp.range);
					}
				}
			}
		}
	}
}
