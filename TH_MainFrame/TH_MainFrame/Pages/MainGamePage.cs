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
    public class MainGamePage:DrawableGameComponent,IPage
    {
        List<GameComponent> generatedComponents;
        public void ContinuePage()
        {
            overMenu.IsAvaliable = overMenuPauseState;
            inGameMenu.IsAvaliable = inGameMenuPauseState;
            testGame.Continue();
            avaliable = true;
        }
        public List<GameComponent> GetGeneratedComponents()
        {
            return generatedComponents;
        }
        public string getName()
        {
            return "MainGamePage";
        }
        public void Reset()
        {
            overMenu.IsAvaliable = false;
            inGameMenu.IsAvaliable = false;
            hiscorename = "";
            testGame.Reset(5,GameConfig.FBgmvol,GameConfig.FSevol);
        }
        public string getSID()
        {
            return "MainGame";
        }
        public void UnloadPage()
        {
            if (testGame.isGameOver() == 2)
            {
                hiscorestorage.AddHiScore(new hiscoredata(testGame.Scorecount,hiscorename,DateTime.Now));
            }
        }
        public void ReloadPage()
        {
            //读游戏的脚本？
        }
        public void InitPage()
        {
            menuBuilder = new MenuBuilder(gameLink);
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Continue"),
                delegate(object sender, EventArgs e) { inGameMenu.IsAvaliable = false;testGame.Continue(); },
                new Vector2(0, 0), "Game Start"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_BackToTitle"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("MainMenu", 0,true); },
                new Vector2(0, 60), "Back To Title"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Restart"),
                delegate(object sender, EventArgs e) { inGameMenu.IsAvaliable = false; testGame.Reset(5,GameConfig.FBgmvol,GameConfig.FSevol); },
                new Vector2(0, 120), "Restart"));
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Down);
            menuBuilder.DoubleLinkItem(1, 2, Arrow.Down);
            menuBuilder.DoubleLinkItem(2, 0, Arrow.Down);
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Right);
            menuBuilder.DoubleLinkItem(1, 2, Arrow.Right);
            menuBuilder.DoubleLinkItem(2, 0, Arrow.Right);
            menuBuilder.SetButtonSelectSE(gameLink.Content.Load<SoundEffect>(@"Sound/select"));
            menuBuilder.SetPosition(new Vector2(250,300));
            inGameMenu = menuBuilder.getMenu();

            menuBuilder.NewMenu();
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_BackToTitle"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("MainMenu", 0,true); },
                new Vector2(0, 60), "Back To Title"));
            menuBuilder.AddItem(new BasicMenuItem(
                gameLink.Content.Load<Texture2D>(@"Menu/Item/MainMenu_Restart"),
                delegate(object sender, EventArgs e) { ((Game1)gameLink).JumpToPage("MainGame", 0,true); },
                new Vector2(0, 120), "Restart"));
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Down);
            menuBuilder.DoubleLinkItem(1, 0, Arrow.Down);
            menuBuilder.DoubleLinkItem(0, 1, Arrow.Right);
            menuBuilder.DoubleLinkItem(1, 0, Arrow.Right);
            menuBuilder.SetButtonSelectSE(gameLink.Content.Load<SoundEffect>(@"Sound/select"));
            menuBuilder.SetPosition(new Vector2(250, 300));
            overMenu = menuBuilder.getMenu();

            //testGame = new TestGame.TestGame(new Rectangle(30, 50, 384, 512), gameLink);
            testGame = new TH_V0.SpriteManager(Game);

            generatedComponents = new List<GameComponent>();
            generatedComponents.Add(testGame);
            generatedComponents.Add(this);
            generatedComponents.Add(inGameMenu);
            generatedComponents.Add(overMenu);

            counterList = new List<LongCounterDisplay>();
            counterList.Add(new LongCounterDisplay(new Vector2(700,80),
                new StarLevelCounterDisplayStyle(Game.Content.Load<Texture2D>(@"Counter/Font_Star")), 1));
            counterList.Add(new LongCounterDisplay(new Vector2(700, 110),
                new StarLevelCounterDisplayStyle(Game.Content.Load<Texture2D>(@"Counter/Font_Star")), 1));
            counterList.Add(new LongCounterDisplay(new Vector2(700, 140),
                new NumberCounterDisplayStyle(Game.Content.Load<Texture2D>(@"Counter/suuji_combo"), new Vector2(17, 30)), 100));
        }
        public void PausePage()
        {
            overMenuPauseState = overMenu.IsAvaliable;
            inGameMenuPauseState = inGameMenu.IsAvaliable;
            overMenu.IsAvaliable = false;
            inGameMenu.IsAvaliable = false;
            testGame.Pause();
            avaliable = false;
        }

        public MainGamePage(Game game)
            : base(game)
        {
            gameLink = game;
            // TODO: Construct any child components here
        }

        public bool avaliable;
        Game gameLink;
        SpriteBatch spriteBatch;
        MenuBuilder menuBuilder;
        Menu inGameMenu,overMenu;
        bool overMenuPauseState, inGameMenuPauseState;
        //TestGame.TestGame testGame;
        TH_V0.SpriteManager testGame;

        List<LongCounterDisplay> counterList;
        SpriteFont testFont;
        Texture2D t2bg, t2logo,t2fade;
        Texture2D[] t2textures;

        Color halfRedToumei=new Color(128,0,0,64),charaToumei=new Color(255,255,255,0);
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"testFont");
            t2bg = gameLink.Content.Load<Texture2D>(@"Koishi_MainGame");
            t2logo = gameLink.Content.Load<Texture2D>(@"Koishi_Logo");
            t2fade = gameLink.Content.Load<Texture2D>(@"fade");
            t2textures = new Texture2D[10];
            t2textures[0] = gameLink.Content.Load<Texture2D>(@"Maingame/Font_Lives");
            t2textures[1] = gameLink.Content.Load<Texture2D>(@"Maingame/Font_Power");
            t2textures[2] = gameLink.Content.Load<Texture2D>(@"Maingame/Font_Score");
            t2textures[4] = gameLink.Content.Load<Texture2D>(@"Fade/Loading_Chara");
            t2textures[5] = gameLink.Content.Load<Texture2D>(@"Maingame/Result_Cleared");
            
            //gameLink.Components.Add(testMenu);
        }

        string hiscorename;
        public override void Update(GameTime gameTime)
        {
            if (avaliable)
            {
                if (!inGameMenu.IsAvaliable)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        inGameMenu.IsAvaliable = true;
                        testGame.Pause();
                    }
                }
                if (testGame.isGameOver() == 1)
                {
                    overMenu.IsAvaliable = true;
                    testGame.Pause();
                }
                if (testGame.isGameOver() == 2)
                {
                    if (hiscorename == "")
                    {
                        this.PausePage();
                        hiscorename = TH_BugReporter.Program.GetDataDialog("请输入姓名", "请使用英文", "确定");
                        foreach (char ch in hiscorename)
                        {
                            if (ch < '0' || ch > 'z') hiscorename = "";
                        }
                        this.ContinuePage();
                    }
                    else
                    {
                        overMenu.IsAvaliable = true;
                        testGame.Pause();
                    }
                }

                counterList[0].SetNumber(testGame.Lifecount);
                counterList[1].SetNumber(testGame.Bombcount);
                counterList[2].SetNumber(testGame.Scorecount);

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(t2bg, Vector2.Zero, Color.White);

            foreach (LongCounterDisplay theCounter in counterList)
            {
                theCounter.Draw(gameTime, spriteBatch);
            }

            //spriteBatch.DrawString(testFont, "Main Game Page Display List", new Vector2(480, 50), Color.DarkOrange);
            //spriteBatch.DrawString(testFont, "Life: ", new Vector2(480, 80), Color.White);
            //spriteBatch.DrawString(testFont, "Bomb: ", new Vector2(480, 110), Color.White);
            //spriteBatch.DrawString(testFont, "Score: ", new Vector2(480, 140), Color.White);
            spriteBatch.Draw(t2textures[0], new Vector2(480, 80), Color.White);
            spriteBatch.Draw(t2textures[1], new Vector2(480, 110), Color.White);
            spriteBatch.Draw(t2textures[2], new Vector2(480, 140), Color.White);

            //spriteBatch.DrawString(testFont, "Press ESC to call inGameMenu", new Vector2(480, 170), Color.LightGreen);

            spriteBatch.Draw(t2textures[4], new Vector2(400, 180), charaToumei);
            
            spriteBatch.Draw(t2logo, new Vector2(600, 450), null, Color.White, 0,
                new Vector2(t2logo.Width / 2, t2logo.Height / 2), 1f, SpriteEffects.None, 1);

            if (testGame != null && testGame.IsPaused)
                spriteBatch.Draw(t2fade,Vector2.Zero,halfRedToumei);

            if (testGame.isGameOver() ==2)
            {
                spriteBatch.Draw(t2textures[5], new Vector2(200, 100), Color.White);
                spriteBatch.Draw(t2textures[2], new Vector2(200, 300), Color.White);
                spriteBatch.DrawString(testFont,testGame.Scorecount+" By "+hiscorename,new Vector2(220+t2textures[2].Width,300),Color.White);
                testGame.Pause();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
