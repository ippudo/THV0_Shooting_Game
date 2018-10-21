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


namespace THMenuset.Menu
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game gameLink;
        public List<MenuItem> MenuItems;
        public Texture2D BasicPattern;
        public Texture2D SelectBox;
        public Vector2 SelectBoxPosition=Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        //public Vector2 Size;
        public bool IsAvaliable=true;
        public bool IsMouseAvaliable = false;
        //public bool IsTargetOn;
        private SpriteBatch spriteBatch;
        public MenuItem SelectedMenuItem;

        /// <summary>
        /// 切换不同菜单项时的声音
        /// </summary>
        public SoundEffect ButtonSelectSE;

        public static readonly TimeSpan ArrowWaitTime=new TimeSpan(0,0,0,0,100);
        TimeSpan lastScrollTime;

        public SpriteFont testFont;
        MouseState tempNowMouseState;
        bool tempIsMouseInSelect;
        
        public void OnArrowKey(int arrowto)
        {
            if (SelectedMenuItem != null)
            {
                changeSelected(SelectedMenuItem.OnArrowKey(arrowto));
                if (ButtonSelectSE != null) ButtonSelectSE.Play(THPages.GameConfig.FSevol*0.7f,0f,0f);
            }
        }

        void changeSelected(MenuItem theItem)
        {
            if (theItem!=null&&MenuItems.Contains(theItem)&&theItem.IsAvaliable)
            {
                if (SelectedMenuItem != null)
                {
                    SelectedMenuItem.IsSelected = false;
                }
                SelectedMenuItem = theItem;
                theItem.IsSelected = true;
            }
            else throw new Exception("改变选定item失败。菜单中没有这个选项。");
        }

        public Menu(Game game)
            : base(game)
        {
            gameLink = game; 
            MenuItems = new List<MenuItem>();
            IsAvaliable = true;
            //IsTargetOn = false;
            spriteBatch = new SpriteBatch(gameLink.GraphicsDevice);
            testFont = gameLink.Content.Load<SpriteFont>(@"Testfont");
            SelectBox = gameLink.Content.Load<Texture2D>(@"Menu/MenuCursor");
            // TODO: Construct any child components here
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
            if (IsAvaliable)
            {
                if (lastScrollTime + ArrowWaitTime < gameTime.TotalGameTime)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        lastScrollTime = gameTime.TotalGameTime;
                        OnArrowKey(Arrow.Up);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        lastScrollTime = gameTime.TotalGameTime;
                        OnArrowKey(Arrow.Down);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        lastScrollTime = gameTime.TotalGameTime;
                        OnArrowKey(Arrow.Left);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        lastScrollTime = gameTime.TotalGameTime;
                        OnArrowKey(Arrow.Right);
                    }
                }
                if (IsMouseAvaliable)
                {
                    tempNowMouseState = Mouse.GetState();
                    tempIsMouseInSelect = false;
                    foreach (MenuItem theItem in MenuItems)
                    {
                        if ((tempNowMouseState.X > Position.X + theItem.Position.X) &&
                           (tempNowMouseState.X < Position.X + theItem.Position.X + theItem.Size.X) &&
                           (tempNowMouseState.Y > Position.Y + theItem.Position.Y) &&
                           (tempNowMouseState.Y < Position.Y + theItem.Position.Y + theItem.Size.Y))
                        {
                            changeSelected(theItem);
                            tempIsMouseInSelect = true;
                            break;
                        }
                    }
                    if (tempIsMouseInSelect && tempNowMouseState.LeftButton == ButtonState.Pressed)
                    {
                        SelectedMenuItem.Operate();
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Z)&&SelectedMenuItem!=null) SelectedMenuItem.Operate();

                foreach (MenuItem theItem in MenuItems)
                {
                    theItem.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (IsAvaliable)
            {
                spriteBatch.Begin();
                foreach (MenuItem theItem in MenuItems)
                {
                    theItem.Draw(spriteBatch,Position,gameTime);
                }
                if (SelectedMenuItem != null && SelectBox != null)
                    spriteBatch.Draw(
                        SelectBox,
                        SelectedMenuItem.Position + SelectBoxPosition+Position,
                        null,
                        Color.White,
                        0,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        0.8f);
                //spriteBatch.DrawString(testFont, SelectedMenuItem != null ? SelectedMenuItem.intro : "Null", new Vector2(200, 0), Color.White);
                spriteBatch.End();
            }
        }
        protected override void LoadContent()
        {
            
        }
    }
}
