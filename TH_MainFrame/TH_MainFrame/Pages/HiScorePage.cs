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
    public class HiScorePage:DrawableGameComponent,IPage
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
            return "HiScorePage";
        }
        public void Reset()
        {
            toPrintString = "";
        }
        public string getSID()
        {
            return "HiScorePage";
        }
        public void UnloadPage()
        {

        }
        public void ReloadPage()
        {

        }
        public void InitPage()
        {
            NumberCounterDisplayStyle cstyle = new NumberCounterDisplayStyle(gameLink.Content.Load<Texture2D>(@"Counter/suuji_combo"),new Vector2(17,30));
            menuBuilder = new MenuBuilder(gameLink);
            /*menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_KeyConfig"),
                null, new Vector2(0, 100), "Clear Score"));*/
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Back"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("MainMenu", 0,false); }
                , new Vector2(0, 350), "Back"));
            //menuBuilder.DoubleLinkItem(3, 0, Arrow.Down);
            menuBuilder.SetPosition(new Vector2(250,140));
            testMenu = menuBuilder.getMenu();

            generatedComponents = new List<GameComponent>();
            generatedComponents.Add(this);
            generatedComponents.Add(testMenu);
        }

        void saveSetting()
        {
            int tosetbgm=((VolumeScrollMenuItem)testMenu.MenuItems[0]).getValue();
            int tosetse=((VolumeScrollMenuItem)testMenu.MenuItems[0]).getValue();
            if (tosetbgm > 100) tosetbgm = 100;
            if (tosetse > 100) tosetse = 100;
            if (tosetbgm < 0) tosetbgm = 0;
            if (tosetse < 0) tosetse = 0;
            THPages.GameConfig.setVol(tosetbgm, tosetse);
        }

        public void PausePage()
        {
            testMenu.IsAvaliable = false;
            avaliable = false;
        }

        public HiScorePage(Game game)
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
        Texture2D bg,bg2;

        SpriteFont testFont;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont"); 
            menuBuilder = new MenuBuilder(gameLink);
            bg = gameLink.Content.Load<Texture2D>(@"HiScore/Koishi_HiScore");
            bg2 = gameLink.Content.Load<Texture2D>(@"Koishi_BG");
            //gameLink.Components.Add(testMenu);
        }
        string toPrintString="";
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        int iforHiScore,maxscore;
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg2, Vector2.Zero, Color.White);
            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
            for (int i = 0; i < 10 && i < hiscorestorage.hiscorelist.Count; i++)
            {
                spriteBatch.DrawString(testFont, (i+1)+". "+hiscorestorage.hiscorelist[i].ToString(), new Vector2(200, 140 + 30 * i), Color.White);
            }
                //spriteBatch.DrawString(testFont, toPrintString, new Vector2(300, 200), Color.DarkOrange);
                spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
