using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class ReflexShooter : BarrageComponent
    {
        int iReflexAngle;       //发射板倾角，必须为45的整数倍
        int iReflexLength;      //反射板长
        bool bReflexOnce;       //是否只反射一次
        bool bChangeType;       //是否改变子弹图案类型
        int iReflexBulletType;  //反射板反射后的子弹图案类型，当图案不存在时不改变
        List<LinkedList<Bullet>> highLightBullet;
        List<LinkedList<Bullet>> normalBullet;
        public ReflexShooter(int ID, List<LinkedList<Bullet>> highLightBullet, List<LinkedList<Bullet>> normalBullet, Vector2 relativePosition, int iReflexAngle, 
            int iReflexStartTime, int iReflexLife, int iReflexLength, bool bReflexOnce,bool bChangeType, int iReflexBulletType)
            : base(ID, relativePosition, iReflexStartTime, iReflexLife,-1) 
        {
            this.highLightBullet = highLightBullet;
            this.normalBullet = normalBullet;
            this.iReflexAngle = iReflexAngle;
            this.iReflexLength = iReflexLength;
            this.bReflexOnce = bReflexOnce;
            this.bChangeType = bChangeType;
            this.iReflexBulletType = iReflexBulletType;
        }

        /*计算叉积*/
        double multi(Vector2 p1, Vector2 p2, Vector2 p0)
        {
            return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        }

        /*线段相交*/
        bool across(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            if ((Math.Max(a1.X, a2.X) >= Math.Min(b1.X, b2.X)) &&
               (Math.Max(b1.X, b2.X) >= Math.Min(a1.X, a2.X)) &&
               (Math.Max(a1.Y, a2.Y) >= Math.Min(b1.Y, b2.Y)) &&
               (Math.Max(b1.Y, b2.Y) >= Math.Min(a1.Y, a2.Y)) &&
               (multi(b1, a2, a1) * multi(a2, b2, a1) >= 0) &&
               (multi(a1, b2, b1) * multi(b2, a2, b1) >= 0))
                return true;
            else return false;
        }

        /*检测是否与挡板相撞*/
        public bool check(LinkedListNode<Bullet> b) 
        {
            if (b.Value.reflexAvailable)
            {
                Vector2 a1, a2, b1, b2;
                /*子弹路径*/
                Vector2 tmp = b.Value.getPosition();
                a1.X = tmp.X;
                a1.Y = tmp.Y;
                a2.X = a1.X + b.Value.direction.X;
                a2.Y = a1.Y + b.Value.direction.Y;
                Vector2 distance = this.position - tmp;
                if (distance.Length() < iReflexLength / 2)
                {
                    /*挡板端点*/
                    switch (iReflexAngle)
                    {
                        case 0:
                            b1.X = this.position.X - iReflexLength / 2;
                            b1.Y = this.position.Y;
                            b2.X = this.position.X + iReflexLength / 2;
                            b2.Y = this.position.Y;
                            break;
                        case 45:
                            b1.X = this.position.X - ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b1.Y = this.position.Y - ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b2.X = this.position.X + ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b2.Y = this.position.Y + ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            break;
                        case 90:
                            b1.X = this.position.X;
                            b1.Y = this.position.Y + iReflexLength / 2;
                            b2.X = this.position.X;
                            b2.Y = this.position.Y - iReflexLength / 2;
                            break;
                        case 135:
                            b1.X = this.position.X - ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b1.Y = this.position.Y + ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b2.X = this.position.X + ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            b2.Y = this.position.Y - ((float)iReflexLength / 2 / (float)Math.Sqrt(2));
                            break;
                        default:
                            b1.X = b1.Y = b2.X = b2.Y = 0;
                            break;
                    }
                    if (across(a1, a2, b1, b2))
                    {
                        if (bReflexOnce)
                        {
                            if (b.Value.isHaveOnceReflex(this.ID))
                                return false;
                            else
                                b.Value.addReflexID(this.ID);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /*更新子弹信息*/
        public void update(ref LinkedListNode<Bullet> b)
        {
            float tmp = b.Value.getSpeedDirection();
            if(bChangeType)
            b.Value.setID(iReflexBulletType);
            switch (iReflexAngle)
            {
                case 0:
                    b.Value.setSpeedDireciont(360 - tmp);
                    break;
                case 45:
                    tmp = 90 - tmp;
                    b.Value.setSpeedDireciont(tmp);
                    break;
                case 90:
                    tmp = 180 - tmp; 
                    b.Value.setSpeedDireciont(tmp);
                    break;
                case 135:
                    tmp = 270 - tmp;
                    b.Value.setSpeedDireciont(tmp);
                    break;
                default:
                    break;
            }
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
