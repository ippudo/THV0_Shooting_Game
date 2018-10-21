using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class Boss:Enemy
    {
        const int bossPicturePerFrame = 10;
        public int remainSpell;
        int totalHP;
        public Boss(ImageName imageName, Vector2 position, Vector2 spriteOrigin, int hitRadius,
            int speed, float speedDirection, int HP, string eventStr)
            : base(imageName,new Point(64,96),new Point(4,3),new Point(0,0),position,spriteOrigin,hitRadius,speed,speedDirection,HP,eventStr)
        {
            this.totalHP = HP;
            this.isOneFrame = false;
            rotateDegree = 0;
            this.myEventReader = new EventReader();
            if (eventStr.Length != 0)
            {
                myEvent = myEventReader.ReadEvent(eventStr);
            }
            currentEvent = new List<BarrageEvent>();
            eventID = 0;
            eventLoopFrame = 10000;
            animeFrameNumber = 0;
            picturePerFrame = 10;
            myState = SpriteState.BORN;
            remainSpell = 5;
        }

        public void nextSpell(int HP,string eventStr)
        {
            if (eventStr.Length != 0)
                myEvent = myEventReader.ReadEvent(eventStr);
            else
                myEvent = new List<BarrageEvent>();
            currentEvent = new List<BarrageEvent>();
            eventID = 0;
            this.spriteFrameNumber = 0;
            this.HP = HP;
            this.totalHP = HP;
            remainSpell--;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X,
                           currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                           Color.White, 0, spriteOrigin, 1f, SpriteEffects.None, 0.89f);
            spriteBatch.Draw(ImageHelper.getImage(ImageName.BAR),new Vector2(30,80),new Rectangle(0,0,30,(int)((float)HP/(float)totalHP*384.0f)),
                Color.White,MathHelper.ToRadians(270),
                Vector2.Zero,1f,SpriteEffects.None,0.9f);
            spriteBatch.Draw(ImageHelper.getImage(ImageName.MAHO), position, new Rectangle(0, 0, 128, 128), Color.White, rotateDegree,
                new Vector2(64, 64), 1f, SpriteEffects.None, 0.1f);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
            CheckAnimation();
            rotateDegree += MathHelper.ToRadians(2);
            if (rotateDegree >= MathHelper.TwoPi)
            {
                rotateDegree = 0;
            }
        }

        public void CheckAnimation()
        {
            if (speedX <= 0.002 && speedX >= -0.002)
            {
                currentFrame.Y = 0;
            }
            if (speedX < -0.002)
            {
                if (currentFrame.Y != 2)
                {
                    currentFrame.Y = 2;
                    currentFrame.X = 0;
                }
            }
            if (speedX > 0.002)
            {
                if (currentFrame.Y != 1)
                {
                    currentFrame.Y = 1;
                    currentFrame.X = 0;
                }
            }

        }
    }
}
