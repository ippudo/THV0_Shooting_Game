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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FadeManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public FadeManager(Game game)
            : base(game)
        {
            gameLink = game;
            // TODO: Construct any child components here
        }

        Game gameLink;
        //
        // 摘要:
        //     Called when graphics resources need to be loaded. Override this method to
        //     load any component-specific graphics resources.

        SpriteFont testFont;

        Texture2D[] textures;//将会并入Style
        Color[] texturesColor;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
            textures = new Texture2D[3];
            textures[0] = gameLink.Content.Load<Texture2D>(@"Fade/Loading_Chara");
            textures[1] = gameLink.Content.Load<Texture2D>(@"Fade/Loading_Font");
            texturesColor = new Color[3];
            texturesColor[0] = new Color(255, 255, 255, 255);
            texturesColor[1] = new Color(255, 255, 255, 255);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (MState == MediaState.Playing)
            {
                //nowFrame += gameTime.ElapsedGameTime;
                if (nowFrame >= operatingStyle.LoadSafeTime)
                {
                    loadSafeSign = true;
                }
                if (nowFrame >= operatingStyle.FinalTime)
                {
                    MState = MediaState.Stopped;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            /*if (MState == MediaState.Playing)
            {
                nowFrame += gameTime.ElapsedGameTime;
                if (nowFrame>operatingStyle.FinalTime) nowFrame=operatingStyle.FinalTime;
                if (FState == FadeState.FadeOut)
                {
                    fadeOpacityColor.A = (Byte)(nowFrame.TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255);
                    texturesColor[0].A = (Byte)(Math.Max(nowFrame.TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255 * 1.2 - 255 * 0.2, 0));
                    texturesColor[1].A = (Byte)(Math.Max(nowFrame.TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255 * 1.4 - 255 * 0.4, 0));
                }
                if (FState == FadeState.FadeIn)
                {
                    fadeOpacityColor.A = (Byte)((operatingStyle.FinalTime - nowFrame).TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255);
                    texturesColor[0].A = (Byte)(Math.Max((operatingStyle.FinalTime - nowFrame).TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255 * 1.2 - 255 * 0.2, 0));
                    texturesColor[1].A = (Byte)(Math.Max((operatingStyle.FinalTime - nowFrame).TotalMilliseconds / operatingStyle.FinalTime.TotalMilliseconds * 255 * 1.4 - 255 * 0.4, 0));
                }
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            if (fadeOpacityColor.A > 0) spriteBatch.Draw(operatingStyle.basicPattern, Vector2.Zero,
                  null, fadeOpacityColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            spriteBatch.End();
            if (FState == FadeState.FadeOut)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(textures[0], new Vector2(400, 180), texturesColor[0]);
                spriteBatch.Draw(textures[1], new Vector2(400, 180), texturesColor[1]);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                spriteBatch.Draw(textures[0], new Vector2(400, 180), texturesColor[0]);
                spriteBatch.Draw(textures[1], new Vector2(400, 180), texturesColor[1]);
                spriteBatch.End();
            }*/
            if (MState == MediaState.Playing)
            {
                nowFrame += gameTime.ElapsedGameTime;
            }
            if (operatingStyle!=null) operatingStyle.Draw(spriteBatch, gameTime, nowFrame, FState);

            base.Draw(gameTime);
        }

        SpriteBatch spriteBatch;

        Color fadeOpacityColor;

        /// <summary>
        /// 播放状态
        /// </summary>
        public MediaState MState;

        /// <summary>
        /// 播放的Fade
        /// </summary>
        public FadeState FState;

        /// <summary>
        /// 当前祯
        /// </summary>
        TimeSpan nowFrame;
        public TimeSpan NowFrame
        {
            get{return nowFrame;}
        }

        /// <summary>
        /// 本次动画播放的起始祯
        /// </summary>
        public TimeSpan StartTime;

        BasicFadeStyle operatingStyle;

        public void FadeOut(GameTime gameTime, BasicFadeStyle styleto)
        {
            gameLink.Components.Remove(this);
            gameLink.Components.Add(this);
            StartTime = gameTime.TotalGameTime;
            nowFrame = TimeSpan.Zero;
            operatingStyle = styleto;
            MState = MediaState.Playing;
            FState = FadeState.FadeOut;
            loadSafeSign = false;
            fadeOpacityColor = Color.White;
        }

        public void FadeIn(GameTime gameTime, BasicFadeStyle styleto)
        {
            gameLink.Components.Remove(this);
            gameLink.Components.Add(this);
            StartTime = gameTime.TotalGameTime;
            nowFrame = TimeSpan.Zero;
            operatingStyle = styleto;
            MState = MediaState.Playing;
            FState = FadeState.FadeIn;
            loadSafeSign = false;
            fadeOpacityColor = Color.White;
        }

        bool loadSafeSign;
        public bool LoadSafeSign
        {
            get { return loadSafeSign; }
        }
    }

    public enum FadeState
    {
        None = 0, FadeIn = 1, FadeOut = 2
    }

}
