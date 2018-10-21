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
    public class PageManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public PageManager(Game game,FadeManager fadeManagerto)
            : base(game)
        {
            fadeManager = fadeManagerto;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            fadeStylePool = new List<BasicFadeStyle>();
            fadeStylePool.Add(new BasicFadeStyle(
                Game.Content.Load<Texture2D>(@"fade"),
                new TimeSpan(0, 0, 0, 0, 299),
                new TimeSpan(0, 0, 0, 0, 300)));
            fadeStylePool.Add(new ReimuFadeStyle(
                Game.Content.Load<Texture2D>(@"fade"),
                new TimeSpan(0, 0, 0, 0, 499),
                new TimeSpan(0, 0, 0, 0, 500)));

            foreach (BasicFadeStyle thefs in fadeStylePool)
            {
                thefs.LoadContent(Game);
            }

            pageList = new List<IPage>();
            pageList.Add(new Pages.MainMenuPage(Game));
            pageList.Add(new Pages.MainGamePage(Game));
            pageList.Add(new Pages.StaffPage(Game));
            pageList.Add(new Pages.ConfigPage(Game));
            pageList.Add(new Pages.HiScorePage(Game));
            foreach (IPage thepage in pageList)
            {
                thepage.InitPage();
            }

            //Nowpage Start
            nowPage = pageList[0]; 
            nowPageComponents = nowPage.GetGeneratedComponents();
            foreach (GameComponent theComponent in nowPageComponents)
            {
                Game.Components.Add(theComponent);
            }
            nowPage.ReloadPage();
            nowPage.Reset();
            nowPage.ContinuePage();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            nowGameTime = gameTime;


            if (changePageState > 0) switch (changePageState)
                {
                    case 1:
                        if (fadeManager.LoadSafeSign)
                        {
                            changePageState = 0;
                            changePagePt2();
                        }
                        break;
                    case 2:
                        if (fadeManager.LoadSafeSign)
                        {
                            changePageState = 0;
                            changePagePt3();
                        }
                        break;
                    default: break;
                }
            else
            {

            }
            base.Update(gameTime);
        }
        GameTime nowGameTime;
        
        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }

        List<IPage> pageList;

        IPage nowPage;
        List<GameComponent> nowPageComponents;

        IPage toChangePage;
        BasicFadeStyle toChangeStyle;
        int changePageState;
        FadeManager fadeManager;
        List<BasicFadeStyle> fadeStylePool;

        /// <summary>
        /// 实现页面的跳转
        /// </summary>
        /// <param name="sidto">目标页面的SID</param>
        /// <param name="fspnoto">使用的渐变编号</param>
        /// <param name="isbgmstop">是否停止BGM</param>
        public void JumpToPage(string sidto, int fspnoto,bool letbgmstop)
        {
            if (letbgmstop) if (MediaPlayer.State == MediaState.Playing) MediaPlayer.Stop();
            JumpToPage(sidto, fspnoto);
        }

        /// <summary>
        /// 实现页面的跳转，不停止BGM
        /// </summary>
        /// <param name="sidto">目标页面的SID</param>
        /// <param name="fspnoto">使用的渐变编号</param>
        public void JumpToPage(string sidto, int fspnoto)
        {
            foreach (IPage thepage in pageList)
            {
                if (thepage.getSID() == sidto)
                {
                    changePage(thepage, fadeStylePool[fspnoto]);
                    break;
                }
            }
        }

        void changePage(IPage pageto, BasicFadeStyle fsto)
        {
            toChangePage = pageto;
            toChangeStyle = fsto;
            nowPage.PausePage();
            fadeManager.FadeOut(nowGameTime,fsto);
            changePageState = 1;
        }
        void changePagePt2()
        {
            nowPage.UnloadPage();
            foreach (GameComponent theComponent in nowPageComponents)
            {
                Game.Components.Remove(theComponent);
            }
            nowPage = toChangePage;
            nowPage.ReloadPage();
            nowPageComponents = nowPage.GetGeneratedComponents();
            foreach (GameComponent theComponent in nowPageComponents)
            {
                Game.Components.Add(theComponent);
            }
            nowPage.Reset();
            nowPage.PausePage();
            fadeManager.FadeIn(nowGameTime, toChangeStyle);
            changePageState = 2;
        }
        void changePagePt3()
        {
            nowPage.ContinuePage();
            changePageState = 0;
        }
    }
}
