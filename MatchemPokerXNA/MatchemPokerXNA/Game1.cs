using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using MatchemPokerXNA;


namespace MatchemPokerXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public TileGameRendererXNA renderer;
        public ITileGame game;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            renderer = new TileGameRendererXNA();
            game = new TileNpc(renderer, 0, 0, 1.0f, 800.0f / 480.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            renderer.XNAInit(Content, GraphicsDevice);
            game.XNAInit(Content);
        }

        protected override void Dispose(bool disposing)
        {
            // Save the state before exiting
            game.Save();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Control the game with first touch-event. 
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count > 0)
            {
                MouseEventType etype = MouseEventType.eMOUSEEVENT_MOUSEDRAG;
                switch (tc[0].State)
                {
                    case TouchLocationState.Pressed:
                        etype = MouseEventType.eMOUSEEVENT_BUTTONDOWN;
                        break;

                    case TouchLocationState.Released:
                        etype = MouseEventType.eMOUSEEVENT_BUTTONUP;
                        break;

                    case TouchLocationState.Moved:
                        etype = MouseEventType.eMOUSEEVENT_MOUSEDRAG;
                        break;
                }

                game.Click(tc[0].Position.X / graphics.PreferredBackBufferWidth,
                           tc[0].Position.Y / graphics.PreferredBackBufferWidth, etype);
            };

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            game.Run((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            renderer.XNAStartFrame();
            game.Draw();
            renderer.XNAEndFrame();
            base.Draw(gameTime);
        }
    }
}
