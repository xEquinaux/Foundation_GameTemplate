﻿using FoundationR;
using FoundationR.Lib;
using FoundationR.Rew;
using FoundationR.Loader;
using FoundationR.Ext;
using FoundationR.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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