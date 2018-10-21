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
    public class StaffPage:DrawableGameComponent,IPage
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
            return "StaffPage";
        }
        public void Reset()
        {
            toPrintString = "";
        }
        public string getSID()
        {
            return "StaffPage";
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

        public StaffPage(Game game)
            : base(game)
        {
            gameLink = game;
            // TODO: Construct any child components here
        }

        public bool avaliable;
        Game gameLink;
        SpriteBatch spriteBatch;

        SpriteFont testFont;
        Texture2D picStaff, bg;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
            bg = gameLink.Content.Load<Texture2D>(@"Koishi_BG");
            picStaff = gameLink.Content.Load<Texture2D>(@"Staff\StaffPage");
        }
        string toPrintString="";
        public override void Update(GameTime gameTime)
        {
            if (avaliable) toPrintString = DateTime.Now.Second.ToString();
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) ((Game1)gameLink).JumpToPage("MainMenu", 0);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg,Vector2.Zero,Color.White);
            spriteBatch.Draw(picStaff, Vector2.Zero, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
