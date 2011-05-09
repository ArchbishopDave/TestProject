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
using TestGame.BattleClasses;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameBase : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BattleBase bb;

        Texture2D p_icon;
        Texture2D bar_edge;
        Texture2D bar_mid;
        SpriteFont bar_font;

        Unit barDisplayUnit;

        UnitController tempController;

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);
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
            Window.Title = "Test Game";
            bb = new BattleBase();

            InitGraphicsMode(800, 800, false);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p_icon = Content.Load<Texture2D>("P_Icon");
            bar_edge = Content.Load<Texture2D>("Bar_Edge");
            bar_mid = Content.Load<Texture2D>("Bar_Mid");
            bar_font = Content.Load<SpriteFont>("BarFont");

            // TODO: use this.Content to load your game content here

            Unit a = new Unit(50, 80, 0);
            a.setTexture(p_icon, Color.Yellow);
            a.setStats(250, 170, 100, 100, 100, 100, 100, 100, 1, 0, 4000);
            Unit b = new Unit(154, 154, 0);
            b.setTexture(p_icon, Color.Red);

            bb.addUnit(a);
            bb.addUnit(b);

            barDisplayUnit = a;

            tempController = new UnitController(a);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            List<String> temp = new List<String>();

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 0.2)
                temp.Add("UNIT-TURN-LEFT");

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.2)
                temp.Add("UNIT-TURN-RIGHT");

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.2)
                temp.Add("UNIT-MOVE-FORWARD");

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.2)
                temp.Add("UNIT-MOVE-BACKWARD");

            // TODO: Add your update logic here
            tempController.handleCommand(temp, getTimePercentage(gameTime));

            barDisplayUnit.TESTCHANGER();
            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(80,65,60));
            spriteBatch.Begin();
            bb.drawUnits(spriteBatch);
            TEMPDRAWBAR();
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void TEMPDRAWBAR()
        {
            int Height = GraphicsDevice.Viewport.Height-64;
            int Start = 32;
            int HP = barDisplayUnit.getHP();
            spriteBatch.Draw(bar_mid, new Vector2(Start,Height), null, Color.LightBlue, 0.0f, new Vector2(0, 0), new Vector2(((float)HP-16)/32.0f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(bar_font, "" + HP, new Vector2(Start + 8, Height), Color.DarkBlue);
            spriteBatch.Draw(bar_edge, new Vector2(HP+16, Height), null, Color.LightBlue, 0.0f, new Vector2(0, 0), new Vector2(0.5f, 0.75f), SpriteEffects.None, 0.0f);

            int FP = barDisplayUnit.getFP();
            spriteBatch.Draw(bar_mid, new Vector2(Start, Height+32), null, Color.LightGreen, 0.0f, new Vector2(0, 0), new Vector2(((float)FP - 16) / 32.0f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(bar_font, "" + FP, new Vector2(Start + 8, Height+32), new Color(0,55,0));
            spriteBatch.Draw(bar_edge, new Vector2(FP + 16, Height+32), null, Color.LightGreen, 0.0f, new Vector2(0, 0), new Vector2(0.5f, 0.75f), SpriteEffects.None, 0.0f);
        }
        private float getTimePercentage(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
        }

        private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (bFullScreen == false)
            {
                if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    graphics.PreferredBackBufferWidth = iWidth;
                    graphics.PreferredBackBufferHeight = iHeight;
                    graphics.IsFullScreen = bFullScreen;
                    graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate thorugh the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == iWidth) && (dm.Height == iHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        graphics.PreferredBackBufferWidth = iWidth;
                        graphics.PreferredBackBufferHeight = iHeight;
                        graphics.IsFullScreen = bFullScreen;
                        graphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }


    }
}