using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class LaserShooter:BarrageComponent
    {

        Barragebatch x;
        int frequency;
        LaserInfo laserData;
        bool isSelfSnipe;//是否为自机狙
        bool isHighLight;//是否高亮
        int angleRange;//发射角范围
        int lines;//发射条数
        float anglePerLine;//每个的间隔
        List<BarrageEvent> myEvent;//发射器事件
        List<BarrageEvent> currentEvent;//当前待处理事件
        int eventID;//当前事件的位置
        int eventLoopFrame;//事件循环帧
        EventReader myEventReader;//事件读取器

        public LaserShooter(Barragebatch x, LinkedList<Bullet> myLaser, Vector2 position, Sprite target) :
            base(x.iGunID,position, x.iGunStartTime, x.iGunEndTime - x.iGunStartTime, x.iBindingID)
        {
            this.x = x;
            this.target = target;
            this.localBullet = myLaser;
            Initialize(0);
            this.isAbsolutePos = x.bIsAbsolute;
            //处理发射器事件以及子弹事件
            this.myEventReader = new EventReader();
            this.myEvent = new List<BarrageEvent>();
            this.currentEvent = new List<BarrageEvent>();
            this.relativeBinding = x.bRelativeBinding;
            //处理子弹事件
            if (x.strBulletEvent.Length != 0)
            {
                myEvent = myEventReader.ReadEvent(x.strBulletEvent);
            }
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

            laserData = new LaserInfo(
                            x.iBulletType,
                            x.fLaserLength,
                            x.fLaserWidth,
                            x.fBulletSpeed,
                            x.fLaserFireAngle,
                            x.fBulletAcc,
                            x.bHighLight
                            );
            this.frequency = x.iCycleTime;
            this.angleRange = x.iGunAngleRange;
            this.lines = x.iFireNumber;
            this.isSelfSnipe = x.bSelfSnipe;
            this.isHighLight = laserData.isHighLight;
            anglePerLine = (float)angleRange / lines;
            eventID = 0;
            this.eventLoopFrame = x.iGunEventCycleTime;
            base.Initialize(iStartFrame);
        }

        public override void Function()
        {
            if (frequency != 0)
            {
                if (frameCount % frequency == 0)
                {

                    if (isSelfSnipe)//检查自机狙
                    {
                        Vector2 dire = this.target.getPosition() - this.position;
                        float snipeDegree = MathHelper.ToDegrees((float)Math.Acos(dire.X / dire.Length()));
                        if (dire.Y < 0)
                        {
                            snipeDegree = 360 - snipeDegree;
                        }
                        if (lines % 2 != 0)
                        {
                            laserData.speedDirec = snipeDegree;
                        }
                        else
                        {
                            laserData.speedDirec = snipeDegree - anglePerLine / 2;
                        }
                    }
                    if (bindingID == -1)
                        AddLaser(this.position, 0);
                    else
                    {
                        LinkedListNode<Bullet> b = bindingBullet.First;
                        while (b != null)
                        {
                            if (relativeBinding)
                                AddLaser(b.Value.getPosition(), b.Value.getSpeedDirection());
                            else
                                AddLaser(b.Value.getPosition(), 0);
                            b = b.Next;
                        }
                    }
                }
            }
            CheckEvent();
        }

        void AddLaser(Vector2 position,float relativeDegree)
        {
            Laser newLaser;
            int i = 0;
            while (i < lines)
            {
                newLaser = new Laser(laserData.ID, position, laserData.hight, laserData.width, laserData.speed,
                    relativeDegree + laserData.speedDirec + anglePerLine * i, laserData.acceleration, (localBullet.Count % 500.0f) * 0.001f + 0.2f);
                localBullet.AddFirst(newLaser);
                newLaser.outDisappear = x.bKillOutBullet;
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
                            be.setOriginValue(laserData.acceleration);
                        }
                        laserData.acceleration = be.Update(laserData.acceleration, currentTime);
                        break;
                    case Attribute.SPEED:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(laserData.speed);
                        }
                        laserData.speed = be.Update(laserData.speed, currentTime);
                        break;
                    case Attribute.SPEEDDIRECT:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(laserData.speedDirec);
                        }
                        laserData.speedDirec = be.Update(laserData.speedDirec, currentTime);
                        break;
                    case Attribute.WIDTH:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(laserData.width);
                        }
                        laserData.width = (int)be.Update((float)laserData.width, currentTime);
                        break;
                    case Attribute.SELFSNIPE:
                        if (be.result == "开")
                            this.isSelfSnipe = true;
                        else
                            this.isSelfSnipe = false;
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
