using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class BarrageManager
    {
        List<BarrageComponent> myBarrage;
        string barrageName;
        Sprite owner;
        Sprite target;
        List<LinkedList<Bullet>> highLightBullet;
        List<LinkedList<Bullet>> normalBullet;
        List<Barragebatch> barrage;
        int startFrame;
        int totalFrame;
        int frameCount;
        int pauseFrame;
        int pauseTime;
        public BarrageManager(List<Barragebatch> barrage, Sprite owner, Sprite target, 
            List<LinkedList<Bullet>> highLightBullet, List<LinkedList<Bullet>> normalBullet)
        {
            this.barrage = barrage;
            this.owner = owner;
            this.target = target;
            myBarrage = new List<BarrageComponent>();
            this.startFrame = 0;
            this.totalFrame = 0;
            this.frameCount = 0;
            this.highLightBullet = highLightBullet;
            this.normalBullet = normalBullet;
            pauseTime = 120;
            pauseFrame = pauseTime;
            LoadBarrageComponent();
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (pauseFrame < pauseTime)
            {
                pauseFrame++;
            }
            else
            {
                BarrageComponent bc;
                frameCount++;
                for (int i = 0; i < myBarrage.Count; i++)
                {
                    bc = myBarrage[i];
                    bc.Update();
                }
                if (frameCount > totalFrame)
                {
                    frameCount = startFrame;
                    for (int i = 0; i < myBarrage.Count; i++)
                    {
                        bc = myBarrage[i];
                        bc.Initialize(startFrame);
                    }
                }
            }
        }

        public void Pause(int pTime)
        {
            this.pauseTime = pTime;
            this.pauseFrame = 0;
        }

        public bool isMyOwner(Sprite o)
        {
            return owner.Equals(o);
        }

        public void setTarget(Sprite target)
        {
            this.target = target;
            for (int i = 0; i < myBarrage.Count; i++)
            {
                myBarrage[i].setTarget(target);
            }
        }

        /// <summary>
        /// 载入弹幕组件
        /// </summary>
        void LoadBarrageComponent()
        {
            BarrageComponent bc;
            startFrame = barrage[0].iCycleStart;
            totalFrame = barrage[0].iFrameNum;
            Barragebatch x = new Barragebatch();
            for (int i = 0; i < barrage.Count; i++)
            {
                
                Vector2 position;
                x = (Barragebatch)barrage[i];
                if (barrage[0].bIsAbsolute)
                {
                    position = new Vector2(x.iGunPosX2, x.iGunPosY2);
                }
                else
                {
                    position = new Vector2(x.iGunPosX, x.iGunPosY);
                }
                if ((int)x.eGunType <= 5)//属于发射器
                {
                    LinkedList<Bullet> eBullet = new LinkedList<Bullet>();
                    switch (x.eGunType)
                    {
                    case Barragebatch.eGT.MULTI:
                        bc = new MultiShooter(x,eBullet,position,target);
                        break;
                    case Barragebatch.eGT.DIRECT:
                        bc = new DirectShooter(x,eBullet,position,target);
                        break;
                    case Barragebatch.eGT.LASER:
                        bc = new LaserShooter(x,eBullet,position,target);
                        break;
                    case Barragebatch.eGT.RANDOM:
                        bc = new RandomShooter(x,eBullet,position,target);
                        break;
                    default:
                        bc = new DefaultBarrageComponent();//一般请不要使用这个类
                        break;
                    }
                    if (x.bHighLight)
                    {
                        highLightBullet.Add(eBullet);
                    }
                    else
                    {
                        normalBullet.Add(eBullet);
                    }
                }
                else
                {
                    switch (x.eGunType)
                    {
                        case Barragebatch.eGT.REFLEX:
                            bc = new ReflexShooter(x.iGunID,highLightBullet,normalBullet, new Vector2(x.iGunPosX2, x.iGunPosY2), x.iReflexAngle,
                                x.iReflexStartTime, x.iReflexLife, x.iReflexLength, x.bReflexOnce, x.bReflexChangeType, x.iReflexBulletType);
                            break;
                        case Barragebatch.eGT.FORCE:
                            bc = new ForceShooter(x.iGunID, highLightBullet, normalBullet, new Vector2(x.iGunPosX2, x.iGunPosY2), x.iForceStartTime,
                                x.iForceLife, x.iForceHalfLength, x.iForceHalfHeight, x.fForceStrength, x.iForceDirection, x.bForceCenter);
                            break;
                        case Barragebatch.eGT.MASK:
                            bc = new MaskShooter(x.iGunID, highLightBullet, normalBullet, new Vector2(x.iGunPosX2, x.iGunPosY2), x.iMaskStartTime,
                                x.iMaskLife, x.iMaskHalfLength, x.iMaskHalfHeight, x.fMaskValue, x.eMaskType, x.iMaskBulletType);
                            break;
                        default:
                            bc = new DefaultBarrageComponent();//一般请不要使用这个类
                            break;
                    }
                }
                if (!x.bIsAbsolute)
                {
                    bc.BindSprite(owner);
                }
                myBarrage.Add(bc);
            }

            //检测绑定子弹
            foreach (BarrageComponent ba in myBarrage)
                if (ba.bindingID != -1)
                {
                    for (int i = 0; i < myBarrage.Count; i++)
                    {
                        if (ba.bindingID == myBarrage[i].ID)
                            ba.setBindingBullet(myBarrage[i].getLocalBullet());
                    }
                }
        }

        public void AddBarrageComponent(BarrageComponent bc)
        {
            myBarrage.Add(bc);
        }

        public int getFrameCount()
        {
            return frameCount;
        }
    }
}
