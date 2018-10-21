using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace THPages
{
    public class BasicFadeStyle
    {
        public BasicFadeStyle(Texture2D tto, TimeSpan toLoadSafeTime,TimeSpan toFinalTime)
        {
            FinalTime = toFinalTime;
            LoadSafeTime = toLoadSafeTime;
            pattern = new List<Texture2D>();
            basicPattern = tto;
        }

        Game gamelink;
        public virtual void LoadContent(Game Gamelinkto)
        {
            gamelink = Gamelinkto;
        }

        Color fadeOpacityColor = new Color(255, 255, 255, 0);

        public virtual void Draw(SpriteBatch sbto,GameTime gameTime,TimeSpan nowFrame,FadeState fadeto)
        {
            if (nowFrame > FinalTime) nowFrame = FinalTime;
            if (fadeto == FadeState.FadeOut)
            {
                fadeOpacityColor.A = (Byte)(nowFrame.TotalMilliseconds / FinalTime.TotalMilliseconds * 255);
            }
            if (fadeto == FadeState.FadeIn)
            {
                fadeOpacityColor.A = (Byte)((FinalTime - nowFrame).TotalMilliseconds / FinalTime.TotalMilliseconds * 255);
            }
            sbto.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            if (fadeOpacityColor.A > 0) sbto.Draw(basicPattern, Vector2.Zero,
                  null, fadeOpacityColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            sbto.End();
        }

        /// <summary>
        ///  使用的基本Texture2D对象
        /// </summary>
        public List<Texture2D> pattern;


        public Texture2D basicPattern
        {
            get { return pattern[0]; }
            set { if (pattern.Count > 0) pattern[0] = value; else pattern.Add(value); }
        }


        /// <summary>
        /// 载入安全祯
        /// </summary>
        public TimeSpan FadeInLoadSafeTime, FadeOutLoadSafeTime;

        /// <summary>
        /// 结束祯
        /// </summary>
        public TimeSpan FadeInFinalTime,FadeOutFinalTime;

        public TimeSpan FinalTime
        {
            set { FadeInFinalTime = value; }
            get { return FadeInFinalTime; }
        }

        public TimeSpan LoadSafeTime
        {
            set { FadeInLoadSafeTime = value; }
            get { return FadeInLoadSafeTime; }
        }
    }
}
