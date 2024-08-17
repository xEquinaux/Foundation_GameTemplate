using FoundationR.Rew;
using System.IO;

namespace Foundation_GameTemplate
{
	internal class Asset
	{
		public static REW LoadFromFile(string path, bool argb = false)
		{
			REW rew;
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				(rew = new REW()).ReadData(new BinaryReader(fs), argb);
			}
			return rew;
		}
		public static void LoadFromFile(string path, out REW image)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				(image = new REW()).ReadData(new BinaryReader(fs));
			}
		}
	}
}