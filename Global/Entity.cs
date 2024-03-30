using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Foundation_GameTemplate.Global
{
    public class Entity
    {
        public float X, Y;
        public int width, height;
        public int type;
        public bool active;
        public Rectangle hitbox;
        public Vector2 position;
        public Vector2 velocity;
        public virtual void Initialize()
        {
        }
        public virtual void AI()
        {
        }
    }
}
