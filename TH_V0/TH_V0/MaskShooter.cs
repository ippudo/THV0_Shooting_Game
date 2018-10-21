using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class MaskShooter : BarrageComponent
    {
        int iMaskHalfLength;
        int iMaskHalfHeight;
        float fMaskValue;
        Barragebatch.eMT eMaskType;
        int iMaskBulletType;
        List<LinkedList<Bullet>> highLightBullet;
        List<LinkedList<Bullet>> normalBullet;
        public MaskShooter(int ID,List<LinkedList<Bullet>> highLightBullet,List<LinkedList<Bullet>> normalBullet, Vector2 relativePosition, int iMaskStartTime,
            int iMaskLife, int iMaskHalfLength, int iMaskHalfHeight, float fMaskValue, Barragebatch.eMT eMaskType, int iMaskBulletType)
            : base(ID, relativePosition, iMaskStartTime, iMaskLife,-1)
        {
            this.highLightBullet = highLightBullet;
            this.normalBullet = normalBullet;
            this.iMaskHalfLength = iMaskHalfLength;
            this.iMaskHalfHeight = iMaskHalfHeight;

            this.fMaskValue = fMaskValue;
            this.eMaskType = eMaskType;
            this.iMaskBulletType = iMaskBulletType;
        }

        /*检测是否在遮罩内部*/
        public bool check(LinkedListNode<Bullet> b)
        {
            if (b.Value.maskAvailable)
            {
                Vector2 tmp = b.Value.getPosition();
                float x = this.position.X;
                float y = this.position.Y;
                if (((x - iMaskHalfLength) <= tmp.X) &&
                    ((x + iMaskHalfLength) >= tmp.X) &&
                    ((y - iMaskHalfHeight) <= tmp.Y) &&
                    ((y + iMaskHalfHeight) >= tmp.Y))
                    return true;
            }
            return false;
        }

        /*更新子弹信息*/
        public void update(ref LinkedListNode<Bullet> b)
        {
            if(iMaskBulletType!=999)
            b.Value.setID(iMaskBulletType);
            switch (eMaskType)
            {
                case Barragebatch.eMT.SPEED:
                    b.Value.speed = fMaskValue;
                    break;
                case Barragebatch.eMT.ACC:
                    b.Value.acceleration = fMaskValue;
                    break;
                case Barragebatch.eMT.KILLBULLET:
                    b.Value.setSpriteState(SpriteState.DEAD);
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
