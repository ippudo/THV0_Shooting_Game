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

namespace THPages.Pages
{
    public class TestPage:DrawableGameComponent,IPage
    {
        List<GameComponent> generatedComponents;
        public void ContinuePage()
        {
            avaliable = true;
        }
        public List<GameComponent> GetGeneratedComponents()
        {
            return generatedComponents;
        }
        public string getName()
        {
            return "TestPage";
        }
        public void Reset()
        {
            toPrintString = "";
        }
        public string getSID()
        {
            return "MainGame";
        }
        public void UnloadPage()
        {

        }
        public void ReloadPage()
        {

        }
        public void InitPage()
        {
            generatedComponents = new List<GameComponent>();
            generatedComponents.Add(this);
        }
        public void PausePage()
        {
            avaliable = false;
        }

        public TestPage(Game game)
            : base(game)
        {
            gameLink = game;
            // TODO: Construct any child components here
        }

        public bool avaliable;
        Game gameLink;
        SpriteBatch spriteBatch;

        SpriteFont testFont;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
        }
        string toPrintString="";
        public override void Update(GameTime gameTime)
        {
            if (avaliable) toPrintString = DateTime.Now.Second.ToString();
            else toPrintString = "Paused";
            if (Keyboard.GetState().IsKeyDown(Keys.T)) ((Game1)gameLink).JumpToPage("TEST2", 0);
            if (Keyboard.GetState().IsKeyDown(Keys.M)) ((Game1)gameLink).JumpToPage("MainMenu", 0);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(testFont, toPrintString, new Vector2(200, 200), Color.LightGreen);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
