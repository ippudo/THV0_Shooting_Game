using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    
    class Bullet:Sprite
    {
        const int deadSpendTime = 10;//消失需要用的帧数，从消失开始子弹不进行判定
        int sinceDeadTime = 0;//已经开始死去的帧数
        const int twinkleTime = 20;//子弹产生时闪现的时间
        float deepth;
        public bool maskAvailable//是否响应遮罩
        {
            set;
            get;
        }
        public bool reflexAvailable//是否响应反射板
        {
            set;
            get;
        }
        public bool forceAvailable//是否响应力场
        {
            set;
            get;
        }
        public bool outDisappear//是否出屏消除
        {
            set;
            get;
        }
        List<int> reflexShooterID;
        Sprite target;
        float bulletHeadAngle;
        List<BarrageEvent> myEvent;
        List<BarrageEvent> currentEvent;
        int eventID;
        int eventLoopFrame;
        public int ID
        {
            get;
            set;
        }
        float speedDirection;//表示子弹的朝向,采用弧度
        directionType directType;
        float speedX;
        float speedY;
        public float acceleration
        {
            set;
            get;
        }
        float accelerationDegree //表示子弹加速度的角度，采用角度
        {
            set;
            get;
        }
        public override Vector2 direction
        {
            get {
                speedX = speed * (float)Math.Cos(speedDirection);
                speedY = speed * (float)Math.Sin(speedDirection);
                return new Vector2(speedX,speedY); 
            }
        }
        //更新函数
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
 
            CheckState();
            switch (myState)
            {
                case SpriteState.BORN:
                    CheckEvent();
                    CheckDirectType();
                    CheckAcceleration();
                    position += direction;
                    break;
                case SpriteState.LIVE:
                    CheckEvent();
                    CheckDirectType();
                    CheckAcceleration();
                    position += direction;
                    break;
                case SpriteState.DEAD:
                    sinceDeadTime++;
                    drawEffect = DrawEffect.FAINT;
                    break;
                case SpriteState.DISAPPEAR:
                    break;
            }
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            float alpha = 1.0f;
            Color color = Color.White;
            Vector2 scaleRate = new Vector2(1, 1);
            float alpha2 = 1.0f;
            if(ID == 42||ID==44)
            alpha2 = 0.6f;
            switch (drawEffect)
            {
                case DrawEffect.CONST:
                    scaleRate = new Vector2(frameSize.X / (float)lightSize.X, frameSize.Y / (float)lightSize.Y);
                    spriteBatch.Draw(textureImage, position, new Rectangle(framePosition.X,
                           framePosition.Y, frameSize.X, frameSize.Y),
                           Color.White * alpha2, rotateDegree, spriteOrigin, 1f, SpriteEffects.None, deepth);
                    break;
                case DrawEffect.FAINT:
                    alpha = 1 - (float)(sinceDeadTime) / (float)deadSpendTime;
                    color *= alpha;
                    spriteBatch.Draw(textureImage, position, new Rectangle(framePosition.X,
                           framePosition.Y, frameSize.X, frameSize.Y),
                           color * alpha2, rotateDegree, spriteOrigin, 1f, SpriteEffects.None, 1f);
                    break;
                case DrawEffect.TWINKLE:
                    scaleRate = new Vector2(2 * frameSize.X / (float)lightSize.X,2 * frameSize.Y / (float)lightSize.X);
                    alpha = 1f - (float)(spriteFrameNumber) / (float)twinkleTime;
                    color *= alpha;
                    spriteBatch.Draw(textureImage, position, new Rectangle(framePosition.X,
                           framePosition.Y, frameSize.X, frameSize.Y),
                           Color.White * alpha2, rotateDegree, spriteOrigin, 1f, SpriteEffects.None, deepth);
                    spriteBatch.Draw(ImageHelper.getImage(ImageName.LIGHT), position, new Rectangle(0, 0, lightSize.X, lightSize.Y),
                           color , rotateDegree, new Vector2(lightSize.X / 2, lightSize.Y / 2), scaleRate, SpriteEffects.None, 0.19f);
                    break;
            }
        }

        void CheckState()
        {
            if (spriteFrameNumber == twinkleTime&&myState != SpriteState.DEAD)
            {
                myState = SpriteState.LIVE;
                drawEffect = DrawEffect.CONST;
            }
            if (spriteFrameNumber == life)
            {
                myState = SpriteState.DEAD;
                drawEffect = DrawEffect.FAINT;
            }
            if (sinceDeadTime == deadSpendTime)
            {
                myState = SpriteState.DISAPPEAR;
            }
        }

        
        
        void CheckAcceleration()
        {
            if (acceleration != 0)
            {
                float acRadian = MathHelper.ToRadians(accelerationDegree);
                speedX = speed * (float)Math.Cos(speedDirection);
                speedY = speed * (float)Math.Sin(speedDirection);
                speedX += (float)(acceleration * Math.Cos(acRadian));
                speedY += (float)(acceleration * Math.Sin(acRadian));
                speedDirection = (float)Math.Atan2(speedY, speedX);
                if(speed > 0)
                    speed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                else
                    speed = -(float)Math.Sqrt(speedX * speedX + speedY * speedY);
            }
        }

        void CheckDirectType()
        {
            Vector2 dire;
            switch (directType)
            {
                case directionType.SPEED:
                    rotateDegree = speedDirection + MathHelper.PiOver2;
                    if (speed < 0)
                    {
                        rotateDegree = speedDirection;
                    }
                    break;
                case directionType.CONST:
                    rotateDegree = MathHelper.ToRadians(this.bulletHeadAngle) + MathHelper.Pi;
                    break;
                case directionType.TARGET:
                    if (target != null)
                    {
                        dire = target.getPosition() - position;
                        acceleration = 0.8f;
                        accelerationDegree = (float)Math.Atan2(dire.Y, dire.X);
                        accelerationDegree = (float)MathHelper.ToDegrees(accelerationDegree);
                        rotateDegree = speedDirection + MathHelper.PiOver2;
                    }
                    if (speed < 0)
                    {
                        rotateDegree = speedDirection;
                    }
                    rotateDegree = speedDirection + MathHelper.PiOver2;
                    break;
                case directionType.PLAYER:
                    if (target != null)
                    {
                        if (target.getSpriteState() == SpriteState.DISAPPEAR)
                        {
                            directType = directionType.SPEED;
                        }
                        dire = target.getPosition() - position;
                        speedDirection = (float)Math.Atan2(dire.Y, dire.X); ;
                    }
                    rotateDegree = speedDirection + MathHelper.PiOver2;
                    break;
            }
        }

        

        void CheckEvent()
        {
                while (eventID<myEvent.Count&&spriteFrameNumber % eventLoopFrame == myEvent[eventID].startFrame)
                {
                    if (myEvent[eventID].isLoop||spriteFrameNumber < eventLoopFrame)
                    {
                        currentEvent.Add(myEvent[eventID]);
                        eventID++;
                    }
                }
                if (spriteFrameNumber % eventLoopFrame == 0)
                {
                    eventID = 0;
                    currentEvent.RemoveRange(0, currentEvent.Count);
                    ExecuteEvent(eventLoopFrame);
                }
                else
                {
                    ExecuteEvent(spriteFrameNumber % eventLoopFrame);
                }
        }

        void ExecuteEvent(int currentTime)
        {
            for (int i = 0; i < currentEvent.Count; i++)
            {
                BarrageEvent be = currentEvent[i];
                switch (be.eAttribute)
                {
                    case Attribute.ID:
                        setID(Convert.ToInt32(be.result));
                        break;
                    case Attribute.DIRECTTYPE:
                        this.directType = StringToDirectType(be.result);
                        break;
                    case Attribute.HEADANGLE:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.bulletHeadAngle);
                        }
                        this.bulletHeadAngle = be.Update(bulletHeadAngle, currentTime);
                        break;
                    case Attribute.ACC:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.acceleration);
                        }
                        this.acceleration = be.Update(acceleration, currentTime);
                        break;
                    case Attribute.ACCDIRECT:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.accelerationDegree);
                        }
                        this.accelerationDegree = be.Update(accelerationDegree,currentTime);
                        break;
                    case Attribute.SPEED:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(this.speed);
                        }
                        this.speed = be.Update(speed, currentTime);
                        break;
                    case Attribute.SPEEDDIRECT:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(getSpeedDirection());
                        }
                        this.speedDirection = be.Update(getSpeedDirection(), currentTime);
                        setSpeedDireciont(this.speedDirection);
                        break;
                    default: break;
                }
                if (currentTime >= be.spendTime + be.startFrame)
                {
                    currentEvent.RemoveAt(i);
                    --i;
                }
            }
        }

        public directionType StringToDirectType(string str)
        {
            directionType temp = directionType.SPEED;
            switch (str)
            {
                case "玩家":
                    temp = directionType.PLAYER;
                    break;
                case "速度":
                    temp = directionType.SPEED;
                    break;
                case "固定":
                    temp = directionType.CONST;
                    break;
            }
            return temp;
        }

        public bool isHaveOnceReflex(int reflexID)
        {
            for (int i = 0; i < reflexShooterID.Count; i++)
            {
                if (reflexShooterID[i] == reflexID)
                {
                    return true;
                }
            }
                return false;
        }

        public void addReflexID(int reflexID)
        {
            this.reflexShooterID.Add(reflexID);
        }

        public void setTarget(Sprite Target)
        {
            this.target = Target;
        }
        public void setBulletHeadAngle(float degree)
        {
            bulletHeadAngle = degree;
        }

        public void setEventLoopFrame(int loopFrame)
        {
            this.eventLoopFrame = loopFrame;
        }

        public void setEvent(List<BarrageEvent> barrageEvent)
        {
            BarrageEvent newEvent;
            for (int i = 0; i < barrageEvent.Count; i++)
            {
                newEvent = new BarrageEvent(barrageEvent[i]);
                myEvent.Add(newEvent);
            }
        }

        /// <summary>
        /// 获取子弹速度的方向
        /// </summary>
        /// <returns>
        /// 以角度返回子弹速度的方向
        /// </returns>
        public float getSpeedDirection() { return MathHelper.ToDegrees(this.speedDirection); }
        /// <summary>
        /// 设定子弹速度的方向
        /// </summary>
        /// <param name="degree">
        /// 以角度设定子弹速度的方向
        /// </param>
        public void setSpeedDireciont(float degree)
        {
            this.speedDirection = MathHelper.ToRadians(degree);
        }


        /// <summary>
        /// 少女子弹的构造函数
        /// </summary>
        /// <param name="ID">子弹的类型</param>
        /// <param name="position">子弹产生的位置
        /// </param><param name="speed">子弹的速度</param>
        /// <param name="speedDirec">子弹速度的方向</param>
        /// <param name="acceleration">子弹加速度</param>
        /// <param name="accelerationDegree">子弹加速度的方向</param>
        /// <param name="directType">子弹的朝向方式</param>
        public Bullet(int ID, Vector2 position, float speed, float speedDirec, float acceleration, float accelerationDegree, directionType directType) :
            this(ID, position, speed, speedDirec, acceleration, accelerationDegree, 100000, directType,0.7f)//少女用子弹不需要声明生命
        {
            this.drawEffect = DrawEffect.CONST;
        }
        
        /// <summary>
        /// 普通子弹的构造函数
        /// </summary>
        /// <param name="ID">子弹的类型</param>
        /// <param name="position">子弹的位置</param>
        /// <param name="speed">子弹的速度</param>
        /// <param name="speedDirec">子弹速度的方向</param>
        /// <param name="acceleration">子弹加速度</param>
        /// <param name="accelerationDegree">子弹加速度的方向</param>
        /// <param name="life">子弹生命</param>
        /// <param name="directType">子弹朝向方式</param>
        public Bullet(int ID, Vector2 position, float speed, float speedDirec, float acceleration, float accelerationDegree, int life, directionType directType,float deepth)
            : this(new Point(Sprite.bulletData[ID][2], Sprite.bulletData[ID][3]), new Point(Sprite.bulletData[ID][0], Sprite.bulletData[ID][1]),
            position, new Vector2(Sprite.bulletData[ID][4], Sprite.bulletData[ID][5]), Sprite.bulletData[ID][6], speed, speedDirec, acceleration,
            accelerationDegree, life, directType,deepth)
        {
            this.ID = ID;
            this.drawEffect = DrawEffect.TWINKLE;
        }

        public Bullet(ImageName imageName, Point frameSize, Point framePosition, Vector2 position, float speed,
            float speedDirec, float acceleration,float deepth)
            : base(imageName, frameSize, framePosition, position, new Vector2(Laser.laserFrameSize.X / 2, 0), 0, speed)
        {
            this.accelerationDegree = speedDirec;
            this.directType = directionType.SPEED;
            this.life = 1000000;
            eventID = 0;
        }

        public Bullet(Point frameSize, Point framePosition, Vector2 position, Vector2 spriteOrigin, int hitRadius, float speed,
            float speedDirec, float acceleration, float accelerationDegree, int life, directionType directType,float deepth)
            : base(ImageName.BULLET, frameSize, framePosition, position, spriteOrigin, hitRadius, speed)
        {
            this.speedDirection = MathHelper.ToRadians(speedDirec);
            this.acceleration = acceleration;
            this.accelerationDegree = accelerationDegree;
            this.directType = directType;
            this.life = life;
            this.bulletHeadAngle = 0;
            this.myEvent = new List<BarrageEvent>();
            this.currentEvent = new List<BarrageEvent>();
            this.reflexShooterID = new List<int>();
            eventID = 0;
            eventLoopFrame = life;
            this.reflexAvailable = true;
            this.maskAvailable = true;
            this.forceAvailable = true;
            this.myState = SpriteState.BORN;
            this.sinceDeadTime = 0;
            this.deepth = deepth;
        }
    }
}
