using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class DirectShooter:BarrageComponent
    {
        int frequency;//射击频率
        float randomRadius;//随机半径
        BulletInfo bulletData;
        bool isSelfSnipe;//是否为自机狙
        bool isHighLight;
        Vector2 randomPosition;
        Vector2 circlePosition;
        List<BarrageEvent> bulletEvent;//子弹事件
        List<BarrageEvent> myEvent;//发射器事件
        List<BarrageEvent> currentEvent;//当前待处理事件
        int eventID;//当前事件的位置
        int eventLoopFrame;//事件循环帧
        EventReader myEventReader;//事件读取器
        Barragebatch x;
        float fireCircleRadius;//在以发射器为中心的圆发射时的发射半径
        bool fireInCircle;
        public void setRandomRadius(int radius)
        {
            randomRadius = radius;
        }

        public void setFireCircleRadius(int inputRadius)
        {
            this.fireCircleRadius = inputRadius;
        }

        public void setSelfSnipe(bool isSelfSnip)
        {
            this.isSelfSnipe = isSelfSnip;
        }

        public DirectShooter(Barragebatch x,LinkedList<Bullet> mBullet,Vector2 position,Sprite target):
            base (x.iGunID,position,x.iGunStartTime,x.iGunEndTime - x.iGunStartTime,x.iBindingID)
        {
            this.x = x;
            this.target = target;
            this.localBullet = mBullet;
            this.isAbsolutePos = x.bIsAbsolute;
            //调用初始化函数，初始化除了事件以外的所有成员变量
            Initialize(0);
            //处理发射器事件以及子弹事件
            this.bulletEvent = new List<BarrageEvent>();
            this.myEvent = new List<BarrageEvent>();
            this.myEventReader = new EventReader();
            this.currentEvent = new List<BarrageEvent>();
            this.relativeBinding = x.bRelativeBinding;
            //处理子弹事件
            if (bulletData.bulletEvent.Length != 0)
            {
                bulletEvent = myEventReader.ReadEvent(bulletData.bulletEvent);
            }
            if (x.strGunEvent.Length != 0)
            {
                myEvent = myEventReader.ReadEvent(x.strGunEvent);
            }
        }

        //默认的eventLoopFrame是life,randomRadius是0
        public DirectShooter(int ID,LinkedList<Bullet> mBullet, BulletInfo bulletData,  int frequency, 
            Vector2 relativePosition, int startFrame, int life)
            : this(ID,mBullet, bulletData, frequency, relativePosition, startFrame, life, null,"",life,0,false,-1,false) { }

        public DirectShooter(int ID,LinkedList<Bullet> mBullet, BulletInfo bulletData,  int frequency,
            Vector2 relativePosition, int startFrame, int life, Sprite target, string eventString, 
            int eventLoopFrame, float randomRadius, bool isSelfSnipe,int bindingID,bool relativeBinding)
            : base(ID, relativePosition, startFrame, life,bindingID)
        {
            this.localBullet = mBullet;
            this.bulletData = bulletData;
            this.frequency = frequency;
            this.randomRadius = randomRadius;
            this.target = target;
            this.isSelfSnipe = isSelfSnipe;
            this.isHighLight = bulletData.isHighLight;
            this.fireCircleRadius = bulletData.fireRadius;
            this.fireInCircle = bulletData.isFireInCircle;
            this.randomPosition = Vector2.Zero;
            this.circlePosition = Vector2.Zero;
            this.bulletEvent = new List<BarrageEvent>();
            this.myEvent = new List<BarrageEvent>();
            this.myEventReader = new EventReader();
            this.currentEvent = new List<BarrageEvent>();
            this.relativeBinding = relativeBinding;
            //处理子弹事件
            if (bulletData.bulletEvent.Length != 0)
            {
                bulletEvent = myEventReader.ReadEvent(bulletData.bulletEvent);
            }
            if (eventString.Length != 0)
            {
                myEvent = myEventReader.ReadEvent(eventString);
            }
            eventID = 0;
            this.eventLoopFrame = eventLoopFrame;
        }

        public override void Initialize(int iStartFrame)
        {
            //修正发射器位置
            if (isAbsolutePos)
            {
                this.relativePosition = new Vector2(x.iGunPosX2, x.iGunPosY2);
                this.position = relativePosition;
            }
            else
            {
                this.relativePosition = new Vector2(x.iGunPosX, x.iGunPosY);
                this.position = getSpritePosition() + relativePosition;
            }

            bulletData = new BulletInfo(
                        x.iBulletType, x.fBulletSpeed,
                        x.fFireAngle, x.fBulletAcc,
                        x.iBulletAccAngle, x.iBulletLife,
                        x.eBulletToward, x.iBulletHeadAngle,
                        x.strBulletEvent, x.iBulletEventCycleTime,
                        x.bHighLight, x.fLocationRadius,
                        x.eBulletCreatePos, x.bKillOutBullet,
                        x.bMask, x.bReflex, x.bForce);
            this.frequency = x.iCycleTime;
            this.randomRadius = x.fLaserLength;
            this.isSelfSnipe = x.bSelfSnipe;
            this.isHighLight = bulletData.isHighLight;
            this.fireCircleRadius = bulletData.fireRadius;
            this.fireInCircle = bulletData.isFireInCircle;
            this.randomPosition = Vector2.Zero;
            this.circlePosition = Vector2.Zero;
            eventID = 0;
            this.eventLoopFrame = x.iGunEventCycleTime;
            base.Initialize(iStartFrame);
        }

        public override void Function()
        {
            if (frequency != 0)
            {
                if (frameCount!=0&&frameCount % frequency == 0)
                {
                    
                    //检查随机发射半径
                    if (randomRadius != 0)
                    {
                        randomPosition = Vector2.Zero;
                        Random ran = new Random();
                        this.randomPosition.X += (float)(2 * randomRadius * ran.NextDouble() - randomRadius);
                        this.randomPosition.Y += (float)(2 * randomRadius * ran.NextDouble() - randomRadius);
                    }
                    //检测发射半径
                    if (fireInCircle)
                    {
                        circlePosition = Vector2.Zero;
                        float fireAngle = MathHelper.ToRadians(bulletData.speedDirec);
                        this.circlePosition.X += (float)(this.fireCircleRadius * Math.Cos(fireAngle));
                        this.circlePosition.Y += (float)(this.fireCircleRadius * Math.Sin(fireAngle));
                    }
                    
                    if (this.bindingID == -1)
                    {
                        AddBullet(this.position,0);
                    }
                    else
                    {   
                        LinkedListNode<Bullet> b = bindingBullet.First;
                        while (b != null)
                        {
                            if (relativeBinding)
                                AddBullet(b.Value.getPosition(), b.Value.getSpeedDirection());
                            else
                                AddBullet(b.Value.getPosition(), 0);
                            b = b.Next;
                        }
                    }
                }
            }
            //每一次Function()执行完了以后发射器的位置都会重新置为指定值
            CheckEvent();
        }

        void AddBullet(Vector2 position,float relativeDegree)
        {
            //检测自机狙
            if (isSelfSnipe)
            {
                Vector2 dire = this.target.getPosition() - position;
                float snipeDegree = MathHelper.ToDegrees((float)Math.Acos(dire.X / dire.Length()));
                if (dire.Y < 0)
                {
                    snipeDegree = 360 - snipeDegree;
                }
                bulletData.speedDirec = snipeDegree;
            }
            Bullet newBullet;
            newBullet = new Bullet(bulletData.ID, position + randomPosition + circlePosition, bulletData.speed, relativeDegree + bulletData.speedDirec
                            , bulletData.acceleration, bulletData.accelerationDegree, bulletData.life, bulletData.directType, (localBullet.Count % 500.0f) * 0.001f + 0.2f);
            //设置布尔变量值
            newBullet.forceAvailable = bulletData.forceAvailable;
            newBullet.reflexAvailable = bulletData.reflexAvailable;
            newBullet.maskAvailable = bulletData.maskAvailable;
            newBullet.outDisappear = bulletData.outScreenDisappear;
            switch (bulletData.directType)
            {
                case directionType.PLAYER:
                    newBullet.setTarget(this.target);
                    break;
                case directionType.TARGET:
                    if (this.target != null)
                    {
                        newBullet.setTarget(this.target);
                    }
                    break;
                case directionType.CONST:
                    newBullet.setBulletHeadAngle(bulletData.bulletHeadAngle);
                    break;
                case directionType.SPEED:
                    break;
            }
            //添加子弹事件
            if (bulletEvent.Count != 0)
            {
                newBullet.setEvent(bulletEvent);
            }
            if (this.target != null)
            {
                newBullet.setTarget(this.target);
            }
            localBullet.AddFirst(newBullet);
        }


        /// <summary>
        /// 事件处理
        /// </summary>
        protected void CheckEvent()
        {
            while (eventID < myEvent.Count && frameCount % eventLoopFrame >= myEvent[eventID].startFrame)
            {
                if (myEvent[eventID].isLoop || frameCount < eventLoopFrame)
                {
                    currentEvent.Add(myEvent[eventID]);
                    eventID++;
                }
            }
            if (frameCount % eventLoopFrame == 0)
            {
                eventID = 0;
                currentEvent.RemoveRange(0, currentEvent.Count);
                ExecuteEvent(eventLoopFrame);
            }
            else
            {
                ExecuteEvent(frameCount % eventLoopFrame);
            }
        }

        protected void ExecuteEvent(int currentTime)
        {
            for (int i = 0; i < currentEvent.Count; i++)
            {
                BarrageEvent be = currentEvent[i];
                switch (be.eAttribute)
                {
                    case Attribute.ACC:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(bulletData.acceleration);
                        }
                        bulletData.acceleration = be.Update(bulletData.acceleration, currentTime);
                        break;
                    case Attribute.SPEED:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(bulletData.speed);
                        }
                        bulletData.speed = be.Update(bulletData.speed, currentTime);
                        break;
                    case Attribute.SPEEDDIRECT:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(bulletData.speedDirec);
                        }
                        bulletData.speedDirec = be.Update(bulletData.speedDirec, currentTime);
                        break;
                    case Attribute.FREQUENCY:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.frequency);
                        }
                        this.frequency = (int)be.Update(this.frequency, currentTime);
                        break;
                    case Attribute.RANDOMRADIUS:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.randomRadius);
                        }
                        this.randomRadius = (int)be.Update(this.randomRadius, currentTime);
                        break;
                    case Attribute.POSITION:
                        this.BindSprite(target);
                        this.relativePosition = Vector2.Zero;
                        break;
                    default: break;
                }
                if (currentTime >= be.spendTime + be.startFrame)
                {
                    currentEvent.RemoveAt(i);
                    i--;
                }
            }//end for
        }
        //事件处理完成

    }//end class
}//end namespace
