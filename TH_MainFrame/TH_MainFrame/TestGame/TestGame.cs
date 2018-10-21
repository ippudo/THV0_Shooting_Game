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


namespace THPages.TestGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TestGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public TestGame(Vector2 psto,Vector2 szto,Game game)
            : base(game)
        {
            position = psto;
            size = szto;
            gameLink = game;
            Reset(10, 10, "LevelTest");//Will Removed?
            // TODO: Construct any child components here
        }

        public TestGame(Rectangle rectto, Game game)
            : this(new Vector2(rectto.X,rectto.Y),new Vector2(rectto.Width,rectto.Height),game)
        {
        }

        public void Reset(int lifeto,int bombto,string lvto)
        {
            Scorecount = 0;
            Lifecount = lifeto;
            Bombcount = bombto;
            LevelName = lvto;
            ShoujoPosition = new Vector2(50, 50);
            GameOver = false;
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Continue()
        {
            IsPaused = false;
        }
        
        Vector2 position;
        Vector2 size;
        Rectangle displayRectangle;
        Rectangle DisplayRectangle
        {
            get
            {
                if (displayRectangle != null) return displayRectangle;
                else
                {
                    displayRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
                    return displayRectangle;
                }
            }
        }

        Game gameLink;

        SpriteFont testFont;
        Texture2D testTexture;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
            testTexture = gameLink.Content.Load<Texture2D>(@"TestPic\note_blue");
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

        //Game Data
        public int Lifecount;
        public int Bombcount;
        public int Scorecount;
        public string LevelName;
        public bool GameOver;
        public Vector2 ShoujoPosition;
        public bool IsPaused;

        KeyboardState keyboardState;

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (!IsPaused)
            {
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Q)) Scorecount++;
                if (keyboardState.IsKeyDown(Keys.W)) Bombcount--;
                if (keyboardState.IsKeyDown(Keys.E)) Lifecount--;
                if (keyboardState.IsKeyDown(Keys.R)) Reset(10,10,"Reseted Level");
                if (keyboardState.IsKeyDown(Keys.Up)) ShoujoPosition.Y -= 4;
                if (keyboardState.IsKeyDown(Keys.Down)) ShoujoPosition.Y += 4;
                if (keyboardState.IsKeyDown(Keys.Left)) ShoujoPosition.X -= 4;
                if (keyboardState.IsKeyDown(Keys.Right)) ShoujoPosition.X += 4;

                if (ShoujoPosition.X < 0) ShoujoPosition.X =1;
                if (ShoujoPosition.X > size.X) ShoujoPosition.X = size.X - 1;
                if (ShoujoPosition.Y < 0) ShoujoPosition.Y = 1;
                if (ShoujoPosition.Y > size.Y) ShoujoPosition.Y = size.Y - 1;

                if (Lifecount < 0) GameOver = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(testFont, "Game Stats", position+new Vector2(30,20), Color.LightGreen);
            spriteBatch.DrawString(testFont, "Level Name:" + LevelName, position + new Vector2(30, 60), Color.White);
            spriteBatch.DrawString(testFont, "Life:" + Lifecount, position + new Vector2(30, 100), Color.White);
            spriteBatch.DrawString(testFont, "Bomb:" + Bombcount, position + new Vector2(30, 140), Color.White);
            spriteBatch.DrawString(testFont, "Position:" + ShoujoPosition, position + new Vector2(30, 180), Color.White);
            spriteBatch.DrawString(testFont, "Score:" + Scorecount, position + new Vector2(30, 220), Color.White);
            spriteBatch.DrawString(testFont, "IsPaused:" + IsPaused, position + new Vector2(30, 260), Color.White);
            spriteBatch.DrawString(testFont, "Q:Score++ W:Bomb-- E:Life-- R:ResetLevel", position + new Vector2(30, 300), Color.LightGreen);
            spriteBatch.DrawString(testFont, "Arrow Key:Control Character", position + new Vector2(30, 340), Color.LightGreen);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Draw(testTexture, ShoujoPosition+position,
                  null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        SpriteBatch spriteBatch;
    }
}
