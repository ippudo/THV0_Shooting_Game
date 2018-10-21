using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class DeadAnime:Sprite
    {
        static Vector2 originScaleRate = new Vector2(0.5f,0.5f);
        Vector2 scaleRate;
        const int animeLength = 30;
        Color blendColor;
        float alpha;
        public DeadAnime(ImageName imageName, Vector2 position, Color blendColor,Random ran)
            : base(imageName, new Point(60, 60), new Point(0, 0), position, 0)
        {
            alpha = 1;
            this.isOneFrame = true;
            rotateDegree = (float)ran.NextDouble() * MathHelper.TwoPi;
            scaleRate = originScaleRate;
            myState = SpriteState.LIVE;
            this.blendColor = blendColor;
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (spriteFrameNumber <= animeLength)
            {
                scaleRate.X += 0.1f;
                alpha -= 0.033f;
            }
            else
            {
                myState = SpriteState.DISAPPEAR;
            }
            base.Update(gameTime, clientBounds);
        }
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle(0,0,frameSize.X,frameSize.Y),
                blendColor*alpha, rotateDegree, new Vector2(30,30), scaleRate, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(textureImage, position, new Rectangle(0, 0, frameSize.X, frameSize.Y),
                blendColor*alpha, rotateDegree, new Vector2(30, 30), 1, SpriteEffects.None, 0.1f);
        }
        public override Vector2 direction
        {
            get { return Vector2.Zero; }
        }
    }
}
