using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TH_V0
{
    class MultiShooter:BarrageComponent
    {
        int frequency;
        float randomRadius;//发射随机半径
        BulletInfo bulletData;
        int angleRange;//发射角范围
        int lines;//发射条数
        bool isSelfSnipe;//是否为自机狙
        bool isHighLight;//是否高光
        //PS：发射的起始角为bulletData.speedDirec
        float anglePerLine;
        Barragebatch x;
        //子弹事件
        List<BarrageEvent> bulletEvent;
        //发射器事件
        List<BarrageEvent> myEvent;
        List<BarrageEvent> currentEvent;
        int eventID;
        int eventLoopFrame;
        EventReader myEventReader;
        Vector2 randomPosition;
        Vector2 circlePosition;
        float fireCircleRadius;//如果需要在发射器周围发射，则设定发射圆的半径
        bool fireInCircle;
        public void setSelfSnipe(bool isSS)
        {
            isSelfSnipe = isSS;
        }
        public void setRandomRadius(int radius)
        {
            this.randomRadius = radius;
        }

        public MultiShooter(Barragebatch x, LinkedList<Bullet> mBullet, Vector2 position, Sprite target) :
            base(x.iGunID,position, x.iGunStartTime, x.iGunEndTime - x.iGunStartTime, x.iBindingID)
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

        public MultiShooter(int ID,LinkedList<Bullet> mBullet, BulletInfo bulletData,int frequency,
            int angleRange, int lines, Vector2 relativePosition, int startFrame, int life) 
            : this(ID,mBullet, bulletData,frequency, angleRange, lines, relativePosition, startFrame, life, null,"",life,0,false,-1,false)
        { }
        public MultiShooter(int ID,LinkedList<Bullet> mBullet, BulletInfo bulletData, int frequency,
            int angleRange,int lines,Vector2 relativePosition, int startFrame, int life,
            Sprite target,string eventString,int eventLoopFrame,float randomRadius,bool isSelfSnipe,int bindingID,bool relativeBinding)
            : base(ID,relativePosition, startFrame, life,bindingID)
        {
            this.bulletData = bulletData;
            this.localBullet = mBullet;
            this.frequency = frequency;
            this.angleRange = angleRange;
            this.lines = lines;
            this.randomRadius = randomRadius;
            this.fireCircleRadius = bulletData.fireRadius;
            this.fireInCircle = bulletData.isFireInCircle;
            this.target = null;
            this.isSelfSnipe = isSelfSnipe;
            this.isHighLight = bulletData.isHighLight;
            this.target = target;
            anglePerLine = (float)angleRange / lines;
            this.randomPosition = Vector2.Zero;
            this.circlePosition = Vector2.Zero;
            this.relativeBinding = relativeBinding;
            //事件处理
            this.bulletEvent = new List<BarrageEvent>();
            this.myEvent = new List<BarrageEvent>();
            this.myEventReader = new EventReader();
            this.currentEvent = new List<BarrageEvent>();
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
            this.angleRange = x.iGunAngleRange;
            this.lines = x.iFireNumber;
            this.randomRadius = x.fLaserLength;
            this.isSelfSnipe = x.bSelfSnipe;
            this.isHighLight = bulletData.isHighLight;
            this.fireCircleRadius = bulletData.fireRadius;
            this.fireInCircle = bulletData.isFireInCircle;
            anglePerLine = (float)angleRange / lines;
            this.randomPosition = Vector2.Zero;
            this.circlePosition = Vector2.Zero;
            eventID = 0;
            this.eventLoopFrame = x.iGunEventCycleTime;
            base.Initialize(iStartFrame);
        }

        public void setEventLoopFrame(int loopFrame)
        {
            eventLoopFrame = loopFrame;
        }

        public override void Function()
        {
            CheckEvent();
            if (frequency != 0)
            {

                if (frameCount!=0&&frameCount % frequency == 0)
                {
                    if (randomRadius != 0)//检查随机半径
                    {
                        randomPosition = Vector2.Zero;
                        Random ran = new Random();
                        this.randomPosition.X += (float)(2 * randomRadius * ran.NextDouble() - randomRadius);
                        this.randomPosition.Y += (float)(2 * randomRadius * ran.NextDouble() - randomRadius);
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
                
        }

        private void AddBullet(Vector2 position,float relativeDegree)
        {
            int i = 0;
            if (isSelfSnipe)//检查自机狙
            {
                Vector2 dire = this.target.getPosition() - position;
                float snipeDegree = MathHelper.ToDegrees((float)Math.Acos(dire.X / dire.Length()));
                if (dire.Y < 0)
                {
                    snipeDegree = 360 - snipeDegree;
                }
                if (lines % 2 != 0)
                {
                    bulletData.speedDirec = snipeDegree;
                }
                else
                {
                    bulletData.speedDirec = snipeDegree - anglePerLine / 2;
                }
            }
            while (i < lines)
            {
                if (fireInCircle)//设置发射半径
                {
                    circlePosition = Vector2.Zero;
                    float fireAngle = MathHelper.ToRadians(bulletData.speedDirec + anglePerLine * i);
                    this.circlePosition.X += (float)(this.fireCircleRadius * Math.Cos(fireAngle));
                    this.circlePosition.Y += (float)(this.fireCircleRadius * Math.Sin(fireAngle));
                }
                Bullet newBullet = new Bullet(bulletData.ID, position + randomPosition + circlePosition, bulletData.speed,relativeDegree + bulletData.speedDirec + anglePerLine * i
                        , bulletData.acceleration, bulletData.accelerationDegree, bulletData.life, bulletData.directType, (localBullet.Count % 500.0f) * 0.001f + 0.2f);

                //设置布尔变量值
                newBullet.forceAvailable = bulletData.forceAvailable;
                newBullet.reflexAvailable = bulletData.reflexAvailable;
                newBullet.maskAvailable = bulletData.maskAvailable;
                newBullet.outDisappear = bulletData.outScreenDisappear;

                //添加子弹事件
                if (bulletEvent.Count != 0)
                {
                    newBullet.setEvent(bulletEvent);
                }
                //子弹事件添加完毕
                if (this.target != null)
                {
                    newBullet.setTarget(this.target);
                }
                localBullet.AddFirst(newBullet);
                i++;
            }
        }

      
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
                ExecuteEvent(eventLoopFrame);
                currentEvent.RemoveRange(0, currentEvent.Count);
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
            }
        }

    }//end class
}//end namespace
