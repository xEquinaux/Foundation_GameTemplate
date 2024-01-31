using FoundationR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf
{
    public static class Ext
    {
        public static REW Clone(this REW input)
        {
            return REW.Create(input.Width, input.Height, input.GetPixels(), input.BitsPerPixel);
        }
    }
}
