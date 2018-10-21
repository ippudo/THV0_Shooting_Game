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
using System.Threading;

namespace THPages
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FadeManager fadeManager;
        PageManager pageManager;
        Pages.TestPage testPage;
        Pages.TestMenuPage testMenuPage;
        TestGame.TestGame testGame;

        BasicFadeStyle testStyle;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fadeManager = new FadeManager(this);
            Components.Add(fadeManager);
            pageManager = new PageManager(this, fadeManager);
            Components.Add(pageManager);

            /*testPage = new Pages.TestPage(this);
            testMenuPage = new Pages.TestMenuPage(this);
            Components.Add(testPage);
            Components.Add(testMenuPage);*/

            BGMManager.CreateThread(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            testStyle = new BasicFadeStyle(
                Content.Load<Texture2D>(@"fade"),
                new TimeSpan(0, 0, 0, 0, 499),
                new TimeSpan(0, 0, 0, 0, 500));
            testTexture = Content.Load<Texture2D>(@"blackvideo");
            KeyConfig.Init();
            hiscorestorage.Init();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void JumpToPage(string sidto, int fspnoto,bool letbgmstop)
        {
            //其实这段用singleton的PageManager就可以解决了
            pageManager.JumpToPage(sidto, fspnoto,letbgmstop);
        }


        Texture2D testTexture;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(testTexture, Vector2.Zero, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
