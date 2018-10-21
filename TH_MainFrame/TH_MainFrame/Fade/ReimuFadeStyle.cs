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
    public class ReimuFadeStyle:BasicFadeStyle
    {
        public ReimuFadeStyle(Texture2D tto, TimeSpan toLoadSafeTime,TimeSpan toFinalTime)
            :base(tto, toLoadSafeTime,toFinalTime)
        {
            texturesColor[0] = new Color(255, 255, 255, 255);
            texturesColor[1] = new Color(255, 255, 255, 255);
        }

        Game gamelink;
        public override void LoadContent(Game Gamelinkto)
        {
            gamelink = Gamelinkto;
            pattern.Add(gamelink.Content.Load<Texture2D>(@"Fade/Loading_Chara"));
            pattern.Add(gamelink.Content.Load<Texture2D>(@"Fade/Loading_Font_V2"));
            pattern.Add(gamelink.Content.Load<Texture2D>(@"Fade/Loading_Ball_V2_Inner"));
            pattern.Add(gamelink.Content.Load<Texture2D>(@"Fade/Loading_Ball_V2_Hen"));
        }

        Color fadeOpacityColor = new Color(255, 255, 255, 0);
        Color[] texturesColor = new Color[4];
        float ballRotateRate=0;

        public override void Draw(SpriteBatch sbto,GameTime gameTime,TimeSpan nowFrame,FadeState fadeto)
        {
            ballRotateRate += 3.14f / 25f ;
            if (nowFrame > FinalTime) nowFrame = FinalTime;
            if (fadeto == FadeState.FadeOut)
            {
                fadeOpacityColor.A = (Byte)(nowFrame.TotalMilliseconds / FinalTime.TotalMilliseconds * 255);
                texturesColor[0].A = (Byte)(Math.Max(nowFrame.TotalMilliseconds / FinalTime.TotalMilliseconds * 255 * 1.2 - 255 * 0.2, 0));
                texturesColor[1].A = (Byte)(Math.Max(nowFrame.TotalMilliseconds / FinalTime.TotalMilliseconds * 255 * 1.4 - 255 * 0.4, 0));
            }
            if (fadeto == FadeState.FadeIn)
            {
                fadeOpacityColor.A = (Byte)((FinalTime - nowFrame).TotalMilliseconds / FinalTime.TotalMilliseconds * 255);
                texturesColor[0].A = (Byte)(Math.Max((FinalTime - nowFrame).TotalMilliseconds / FinalTime.TotalMilliseconds * 255 * 1.2 - 255 * 0.2, 0));
                texturesColor[1].A = (Byte)(Math.Max((FinalTime - nowFrame).TotalMilliseconds / FinalTime.TotalMilliseconds * 255 * 1.4 - 255 * 0.4, 0));
            }
            sbto.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            if (fadeOpacityColor.A > 0) sbto.Draw(basicPattern, Vector2.Zero,
                  null, fadeOpacityColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            sbto.End();
            if (fadeto == FadeState.FadeOut)
            {
                sbto.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                sbto.Draw(pattern[1], new Vector2(400, 180), texturesColor[0]);
                sbto.Draw(pattern[2], new Vector2(400+70, 180+250), texturesColor[1]);
                sbto.Draw(pattern[3], new Vector2(400 + 25 + 58, 180 + 185 + 58), null, texturesColor[1], ballRotateRate, new Vector2(29,29),
                    1f, SpriteEffects.None, 1f);
                sbto.End();
            }
            else
            {
                sbto.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                sbto.Draw(pattern[1], new Vector2(400, 180), texturesColor[0]);
                sbto.Draw(pattern[2], new Vector2(400+70, 180+250), texturesColor[1]);
                sbto.Draw(pattern[3], new Vector2(400 + 25 + 58, 180 + 185 + 58), null, texturesColor[1], ballRotateRate, new Vector2(29, 29),
                    1f, SpriteEffects.None, 1f);
                sbto.End();
            }
        }
    }
}
