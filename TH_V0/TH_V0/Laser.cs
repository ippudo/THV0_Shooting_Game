using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class Laser:Bullet
    {
        const int twinkleTime = 30;//激光产生时闪现的时间
        public static Point laserFrameSize = new Point (18,361);//激光图片的大小
        new int ID;
        float width;
        float hight;
        float speedX;//速度的X分量
        float speedY;//速度的Y分量
        float speedDirection;
        new float acceleration;//激光的加速度，方向只能沿速度方向
        float laserTempHight;


        /// <summary>
        /// 激光构造函数
        /// </summary>
        /// <param name="ID">激光的类型</param>
        /// <param name="position">激光的位置</param>
        /// <param name="width">激光的宽</param>
        /// <param name="hight">激光的长</param>
        /// <param name="speed">激光的速度</param>
        /// <param name="speedDirection">激光发射的角度</param>
        public Laser(int ID,Vector2 position,float hight,float width,float speed,float speedDirection,float acceleration,float deepth) :
            base(ImageName.LASER,laserFrameSize,new Point(laserFrameSize.X * ID,0),position,speed,speedDirection,acceleration,deepth)
        {
            this.ID = ID;
            this.width = width;
            this.hight = hight;
            this.speedDirection = MathHelper.ToRadians(speedDirection);
            this.acceleration = acceleration;
            this.laserTempHight = 0;
            this.myState = SpriteState.LIVE;
            this.maskAvailable = false;
            this.reflexAvailable = false;
            this.forceAvailable = false;
        }
        public override Vector2  direction
        {
	        get 
            { 
                speedX = speed * (float)Math.Cos(speedDirection);
                speedY = speed * (float)Math.Sin(speedDirection);
                return new Vector2(speedX,speedY); 
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (spriteFrameNumber > laserBornTime)
            {
                position += direction;
            }
            if (spriteFrameNumber > life||myState ==SpriteState.DEAD)
                myState = SpriteState.DISAPPEAR;
            CheckAcceleration();
            CheckRotateDegree();
            spriteFrameNumber++;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            float alpha = 1.0f;
            Color color = Color.White;
            Vector2 scaleRate = new Vector2(1, 1);
            scaleRate = new Vector2(width / (float)lightSize.X, width / (float)lightSize.Y);
            alpha = 1 - (float)(spriteFrameNumber) / (float)twinkleTime;
            color *= alpha;
            spriteBatch.Draw(ImageHelper.getImage(ImageName.LIGHT), position, new Rectangle(0, 0, lightSize.X, lightSize.Y),
                   color, rotateDegree, new Vector2(lightSize.X/2, lightSize.Y/2), scaleRate, SpriteEffects.None, 0.1f);
            if (laserTempHight < hight)
            {
                laserTempHight = hight / laserBornTime * spriteFrameNumber;
            }
            scaleRate = new Vector2(width / 18.0f, laserTempHight / 361.0f);
            spriteBatch.Draw(textureImage, position, new Rectangle(framePosition.X + currentFrame.X * frameSize.X,
                framePosition.Y + currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, rotateDegree, spriteOrigin, scaleRate, SpriteEffects.None, 0f);
        }

        void CheckAcceleration()
        {
            speed += acceleration;
        }

        void CheckRotateDegree()
        {
            rotateDegree = speedDirection - MathHelper.PiOver2;
            if (speed < 0)
            {
                rotateDegree = speedDirection;
            }
        }

        public override bool isIntersect(Circle targetCircle)
        {
            if (this.width > 5)//对于宽度5以上的激光进行碰撞检测
            {
                Vector2 distance = targetCircle.origin - this.position;
                float distanceAngle = (float) Math.Atan2(distance.Y, distance.X);
                distanceAngle = speedDirection - distanceAngle;
                double x = distance.Length() * Math.Cos(distanceAngle);
                double y = distance.Length() * Math.Sin(distanceAngle);
                y = Math.Abs(y);
                if (x > 0 &&x < this.laserTempHight && y < this.width/2 + targetCircle.radius )
                {
                    return true;
                }
            }
            return false;
        }
        public override bool isOutOfBounds(Rectangle clientRect)
        {
            if (this.position.X < 0 || this.position.X > clientRect.Width || this.position.Y < 0 || this.position.Y > clientRect.Height)
                return true;
            else
                return false;
        }


    }//end class
}
