using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
namespace TH_V0
{
    class Shoujo:Sprite
    {
        const int shoujoBornTime = 30;
        const int shoujoTwinkleTime = 180;
        Vector2 bornPosition;
        public bool isUnbeatable;
        bool isClear;
        bool showHitCircle;
        int clearFrameCount;
        const int clearTime = 120;
        public Shoujo(ImageName imageName,Point frameSize,Point sheetSize, Vector2 spriteOrigin, int hitRadius, int speed)
            : base(imageName, frameSize,sheetSize, spriteOrigin, hitRadius, speed)
        {
            rotateDegree = 0;
            myState = SpriteState.BORN;
            isUnbeatable = true;
            showHitCircle = false;
            isClear = false;
            bornPosition = position;
            clearFrameCount = 0;
        }
        public override Vector2 direction
        {
            get {
                float tempSpeed = speed;
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                }
                if (inputDirection.Length() != 0)
                {
                    inputDirection.Normalize();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    tempSpeed = speed / 2;
                    showHitCircle = true;
                }
                else
                {
                    showHitCircle = false;
                }
                return inputDirection * new Vector2 (tempSpeed,tempSpeed);
            }
        }
        public void CheckAnimation()
        {
            if (direction.X == 0)
                currentFrame.Y = 0;
            if (direction.X < 0)
            {
                if (currentFrame.Y != 1)
                {
                    currentFrame.Y = 1;
                    currentFrame.X = 0;
                }
            }
            if (direction.X > 0)
            {
                if (currentFrame.Y != 2)
                {
                    currentFrame.Y = 2;
                    currentFrame.X = 0;
                }
            }

        }
        public override void Update(GameTime gameTime,Rectangle clientBounds)
        {
            CheckState();
            switch (myState)
            {
                case SpriteState.LIVE:
                    position += direction;
                if (position.X < spriteOrigin.X + clientBounds.X)
                    position.X = spriteOrigin.X + clientBounds.X;
                if (position.Y < spriteOrigin.Y + clientBounds.Y)
                    position.Y = spriteOrigin.Y + clientBounds.Y;
                if (position.X > clientBounds.Width - frameSize.X + spriteOrigin.X + clientBounds.X)
                    position.X = clientBounds.Width - frameSize.X + spriteOrigin.X + clientBounds.X;
                if (position.Y > clientBounds.Height - frameSize.Y + spriteOrigin.Y + clientBounds.Y)
                    position.Y = clientBounds.Height - frameSize.Y + spriteOrigin.Y + clientBounds.Y;
                break;
                case SpriteState.BORN:
                position += new Vector2(0, -3);
                break;
                default:
                break;
            }
            rotateDegree += MathHelper.ToRadians(3);
            CheckAnimation();
            base.Update(gameTime,clientBounds);
        }
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            if (isUnbeatable && spriteFrameNumber % 5 != 0)
            {
                spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X,
                           currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                           Color.White, 0, spriteOrigin, 1f, SpriteEffects.None, 0);
            }
            if (!isUnbeatable)
            {
                spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X,
                           currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                           Color.White, 0, spriteOrigin, 1f, SpriteEffects.None, 0);
            }
            if (showHitCircle)
            {
                spriteBatch.Draw(ImageHelper.getImage(ImageName.POINT), position, new Rectangle(0,0,64,64),
                           Color.White, rotateDegree/2, new Vector2(31,30), 1f, SpriteEffects.None, 0.89f);
            }
            spriteBatch.Draw(ImageHelper.getImage(ImageName.BOW), position + new Vector2(48, 0), new Rectangle(0, 0, 16, 16),
                Color.White, rotateDegree, new Vector2(7, 7), 1f ,SpriteEffects.None, 0);
            spriteBatch.Draw(ImageHelper.getImage(ImageName.BOW), position + new Vector2(-48, 0), new Rectangle(0, 0, 16, 16),
                Color.White, rotateDegree, new Vector2(7, 7), 1f, SpriteEffects.None, 0);
        }

        public void CheckState()
        {
            if (spriteFrameNumber == shoujoBornTime)
            {
                myState = SpriteState.LIVE;
            }
            if (spriteFrameNumber == shoujoTwinkleTime)
            {
                isUnbeatable = false;
            }
            if (isClear)
            {
                clearFrameCount++;
            }
            if (clearFrameCount >= clearTime)
            {
                myState = SpriteState.BORN;
            }
        }

        public void setDead()
        {
            myState = SpriteState.BORN;
            spriteFrameNumber = 0;
            this.position = bornPosition;
            isUnbeatable = true;
            isClear = false;
            clearFrameCount = 0;
        }
        public void setClear()
        {
            clearFrameCount = 0;
            isClear = true;
        }
    }
}
