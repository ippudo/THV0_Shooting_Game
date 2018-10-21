using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class ForceShooter : BarrageComponent
    {
        int iForceHalfLength;
        int iForceHalfHeight;
        float fForceStrength;
        int iForceDirection;
        bool bForceCenter;
        List<LinkedList<Bullet>> highLightBullet;
        List<LinkedList<Bullet>> normalBullet;
        public ForceShooter(int ID, List<LinkedList<Bullet>> highLightBullet,List<LinkedList<Bullet>> normalBullet, Vector2 relativePosition, int iForceStartTime, 
            int iForceLife, int iForceHalfLength, int iForceHalfHeight, float fForceStrength, int iForceDirection, bool bForceCenter)
            : base(ID,relativePosition, iForceStartTime, iForceLife,-1)
        {
            this.ID = ID;
            this.highLightBullet = highLightBullet;
            this.normalBullet = normalBullet;
            this.iForceHalfLength = iForceHalfLength;
            this.iForceHalfHeight = iForceHalfHeight;
            this.fForceStrength = fForceStrength;
            this.iForceDirection = iForceDirection;
            this.bForceCenter = bForceCenter;
        }

        /*检测是否在力场内部*/
        public bool check(LinkedListNode<Bullet> b)
        {
            if (b.Value.forceAvailable)
            {
                Vector2 tmp = b.Value.getPosition();
                float x = this.position.X;
                float y = this.position.Y;
                if (((x - iForceHalfLength) <= tmp.X) &&
                    ((x + iForceHalfLength) >= tmp.X) &&
                    ((y - iForceHalfHeight) <= tmp.Y) &&
                    ((y + iForceHalfHeight) >= tmp.Y))
                    return true;
            }
            return false;
        }

        /*更新子弹信息*/
        public void update(ref LinkedListNode<Bullet> b)
        {
            float val = b.Value.speed;
            float ang = b.Value.getSpeedDirection();
            float x = val * (float)Math.Cos(MathHelper.ToRadians(ang));
            float y = val * (float)Math.Sin(MathHelper.ToRadians(ang));

            if (bForceCenter)
            {
                Vector2 tmp = b.Value.getPosition();
                float tang = (float)Math.Atan2(this.position.Y-tmp.Y, this.position.X-tmp.X);
                x += fForceStrength * (float)Math.Cos(tang);
                y += fForceStrength * (float)Math.Sin(tang);
            }
            else
            {   
                x += fForceStrength * (float)Math.Cos(MathHelper.ToRadians(iForceDirection));
                y += fForceStrength * (float)Math.Sin(MathHelper.ToRadians(iForceDirection));
            }
            b.Value.speed = (float)Math.Sqrt(x * x + y * y);
            b.Value.setSpeedDireciont(MathHelper.ToDegrees((float)Math.Atan2(y, x)));
        }

        public override void Function()
        {
            foreach (LinkedList<Bullet> buletList in highLightBullet)
            {
                LinkedListNode<Bullet> b = buletList.First;
                while (b != null)
                {
                    if (check(b)) update(ref b);
                    b = b.Next;
                }
            }
            foreach (LinkedList<Bullet> buletList in normalBullet)
            {
                LinkedListNode<Bullet> b = buletList.First;
                while (b != null)
                {
                    if (check(b)) update(ref b);
                    b = b.Next;
                }
            }
        }
    }//End Class
}
