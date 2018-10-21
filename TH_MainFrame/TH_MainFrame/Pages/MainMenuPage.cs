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
    public class MainMenuPage:DrawableGameComponent,IPage
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
            return "MainMenu";
        }
        public void UnloadPage()
        {
        }
        public void ReloadPage()
        {
            BGMManager.ChangeSong("");
            BGMManager.ChangeSong(@"BGM/th128_01");
            /*MediaPlayer.Play(bgm);
            MediaPlayer.Volume = GameConfig.FBgmvol;
            MediaPlayer.Pause();
            MediaPlayer.Resume();*/
        }
        public void InitPage()
        {
            menuBuilder = new MenuBuilder(gameLink);
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Start"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("MainGame", 1, true); seok.Play(GameConfig.FSevol, 0f, 0f); },
                new Vector2(0, 0), "Game Start"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_HiScore"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("HiScorePage", 0, false); seok.Play(GameConfig.FSevol, 0f, 0f); },
                new Vector2(0, 50), "Watch Hi-Score Table"));
            
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Setting"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("ConfigPage", 0, false); seok.Play(GameConfig.FSevol, 0f, 0f); },
                new Vector2(0, 100), "Game Configuration"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Staff"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("StaffPage", 0, false); seok.Play(GameConfig.FSevol, 0f, 0f); },
                new Vector2(0, 150), "See Staffs"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Exit"),
                 delegate(object sender, EventArgs e) { seok.Play(GameConfig.FSevol, 0f, 0f); gameLink.Exit(); },
                 new Vector2(0, 200), "Game Exit"));
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Down);
            menuBuilder.DoubleLinkItem(1, 2, Arrow.Down);
            menuBuilder.DoubleLinkItem(2, 3, Arrow.Down);
            menuBuilder.DoubleLinkItem(3, 4, Arrow.Down);
            menuBuilder.DoubleLinkItem(4, 0, Arrow.Down);
            menuBuilder.SetButtonSelectSE(gameLink.Content.Load<SoundEffect>(@"Sound/select"));
            menuBuilder.SetPosition(new Vector2(250,200));
            testMenu = menuBuilder.getMenu();

            generatedComponents = new List<GameComponent>();
            generatedComponents.Add(this);
            generatedComponents.Add(testMenu);
            bgm = gameLink.Content.Load<Song>(@"BGM/th128_01");
        }
        public void PausePage()
        {
            testMenu.IsAvaliable = false;
            avaliable = false;
        }

        public MainMenuPage(Game game)
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
        Song bgm;
        SoundEffect seok, seselect;

        SpriteFont testFont;
        Texture2D t2bg, t2logo;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
            t2bg = gameLink.Content.Load<Texture2D>(@"Koishi_BG");
            t2logo = gameLink.Content.Load<Texture2D>(@"Koishi_Logo");
            seok = gameLink.Content.Load<SoundEffect>(@"Sound/ok");
            seselect = gameLink.Content.Load<SoundEffect>(@"Sound/select");
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.DrawString(testFont, toPrintString, new Vector2(300, 200), Color.DarkOrange);
            spriteBatch.Draw(t2bg, Vector2.Zero, Color.White);
            spriteBatch.Draw(t2logo, new Vector2(400, 100), null, Color.White, 0,
                new Vector2(t2logo.Width / 2, t2logo.Height / 2), 1f, SpriteEffects.None, 1);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
