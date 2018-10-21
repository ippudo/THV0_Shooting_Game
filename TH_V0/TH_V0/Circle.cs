using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class Circle
    {
        public Vector2 origin
        {
            get;
            set;
        }
        public float radius
        {
            set;
            get;
        }
        public Circle(Vector2 origin, float radius)
        {
            this.origin = origin;
            this.radius = radius;
        }
        public bool IsInterSect(Circle otherCircle)
        {
            Vector2 distance = this.origin - otherCircle.origin;
            if (distance.Length() < this.radius + otherCircle.radius)
                return true;
            else
                return false;
        }
    }
}
