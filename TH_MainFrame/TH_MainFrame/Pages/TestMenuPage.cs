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
using THMenuset.Menu;

namespace THPages.Pages
{
    public class TestMenuPage:DrawableGameComponent,IPage
    {
        List<GameComponent> generatedComponents;
        public void ContinuePage()
        {
            avaliable = true;
            testMenu.IsAvaliable = true;
        }
        public List<GameComponent> GetGeneratedComponents()
        {
            return generatedComponents;
        }
        public string getName()
        {
            return "TestMenuPage";
        }
        public void Reset()
        {
            toPrintString = "";
        }
        public string getSID()
        {
            return "TEST2";
        }
        public void UnloadPage()
        {

        }
        public void ReloadPage()
        {

        }
        public void InitPage()
        {
            menuBuilder = new MenuBuilder(gameLink);
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Start"),
                null, new Vector2(0, 0), "Game Start"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Continue"),
                null, new Vector2(0, 60), "Game Continue"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_HiScore"),
                null, new Vector2(0, 120), "Watch Hi-Score Table"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Setting"),
                null, new Vector2(0, 180), "Game Configuration"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Exit"),
                 delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("TEST1", 0,false); },
                 new Vector2(200, 150), "Game Exit"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Staff"),
                null, new Vector2(200, 30), "See Staffs"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_BackToTitle"),
                null, new Vector2(200, 90), "Just For Test"));
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Down);
            menuBuilder.DoubleLinkItem(1, 2, Arrow.Down);
            menuBuilder.DoubleLinkItem(2, 3, Arrow.Down);
            menuBuilder.DoubleLinkItem(3, 0, Arrow.Down);
            menuBuilder.DoubleLinkItem(5, 6, Arrow.Down);
            menuBuilder.DoubleLinkItem(6, 4, Arrow.Down);
            menuBuilder.DoubleLinkItem(4, 5, Arrow.Down);
            menuBuilder.SingleLinkItem(0, 5, Arrow.Right);
            menuBuilder.SingleLinkItem(5, 1, Arrow.Left);
            menuBuilder.SingleLinkItem(1, 6, Arrow.Right);
            menuBuilder.SingleLinkItem(6, 2, Arrow.Left);
            menuBuilder.SingleLinkItem(2, 4, Arrow.Right);
            menuBuilder.SingleLinkItem(4, 3, Arrow.Left);
            menuBuilder.SingleLinkItem(3, 4, Arrow.Right);
            testMenu = menuBuilder.getMenu();

            generatedComponents = new List<GameComponent>();
            generatedComponents.Add(this);
            generatedComponents.Add(testMenu);
        }
        public void PausePage()
        {
            testMenu.IsAvaliable = false;
            avaliable = false;
        }

        public TestMenuPage(Game game)
            : base(game)
        {
            gameLink = game;
            // TODO: Construct any child components here
        }

        public bool avaliable;
        Game gameLink;
        SpriteBatch spriteBatch;
        MenuBuilder menuBuilder;
        Menu testMenu;

        SpriteFont testFont;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont"); menuBuilder = new MenuBuilder(gameLink);
            
            //gameLink.Components.Add(testMenu);
        }
        string toPrintString="";
        public override void Update(GameTime gameTime)
        {
            if (avaliable) toPrintString = "Menu";
            else toPrintString = "Paused";
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(testFont, toPrintString, new Vector2(300, 200), Color.DarkOrange);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
