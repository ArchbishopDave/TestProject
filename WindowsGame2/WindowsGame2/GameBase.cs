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
        SpriteFont ko_font;

        Unit barDisplayUnit;

        UnitInputHandler m_inputHandler;

        const int SCREEN_WIDTH  = 1600;
        const int SCREEN_HEIGHT = 900;

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

            List<int> dimensions = InitGraphicsMode(SCREEN_WIDTH, SCREEN_HEIGHT, false);

            CommandTimer.formTemplates();
            BattleGroup.setConstants(dimensions[0], dimensions[1], 10);
            UnitController.m_battleBase = bb;

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
            ko_font = Content.Load<SpriteFont>("KOCountFont");

            setupDefaultAnimations();

            Unit a = new Unit(50, 80, 0, "Player");
            a.setTexture(p_icon, Color.Azure);
            a.setStats(250, 170, 100, 100, 100, 100, 100, 120, 1, 0, 4000);
            a.m_important = true;
            UnitController PLAYERCONTROLLER = new UnitController(a);
            BattleGroup bgA = new BattleGroup(PLAYERCONTROLLER, 1);

            Weapon.addNewWeaponTemplate("Warhammer", Color.PowderBlue, Content.Load<Texture2D>("W_TestHammer"), new Dictionary<string, int>());
            Weapon.addSwingToWeapon("Warhammer", 16.0f, 1.0f, -1.0f, 1.0f, 10);

            Weapon w = Weapon.getWeaponFromTemplate("Warhammer");
            w.m_unitHeld = PLAYERCONTROLLER;
            a.m_weapon = w;

            Random ran = new Random();
            for (int i = 0; i < 6; i++)
            {
                Unit l = new Unit(ran.Next(600) - 300 * i, ran.Next(300) + 50 * i, 0);
                l.setStats(200, 100, 100, 100, 100, 100, 100, 60, 1, 0, 4000);
                l.setTexture(p_icon, Color.Blue);
                UnitController UCL = new UnitController(l);
                BattleGroup BGX = new BattleGroup(UCL, 1);
                Weapon lw = Weapon.getWeaponFromTemplate("Warhammer");
                lw.m_unitHeld = UCL;
                l.m_weapon = lw;
                for (int j = 0; j < 150; j++)
                {
                    Unit f = new Unit(ran.Next(300) - 300 * i, ran.Next(300) + 50 * i, 0);
                    f.setStats(60, 50, 100, 100, 100, 100, 100, 60, 1, 0, 4000);
                    f.setTexture(p_icon, Color.LightBlue);
                    UnitController UCF = new UnitController(f);
                    BGX.addUnit(UCF);
                    Weapon fw = Weapon.getWeaponFromTemplate("Warhammer");
                    fw.m_unitHeld = UCF;
                    f.m_weapon = fw;
                }
                bb.addUnitGroup("FriendlyFodder:"+i,BGX);
            }

            for (int i = 0; i < 8; i++)
            {
                Unit l = new Unit(ran.Next(300) + 300 * i, ran.Next(300) + 50 * i, 3.0f);
                l.setStats(200, 100, 100, 100, 100, 100, 100, 60, 1, 0, 4000);
                l.setTexture(p_icon, Color.Red);
                UnitController UCL = new UnitController(l);
                BattleGroup BGX = new BattleGroup(UCL, 2);
                Weapon lw = Weapon.getWeaponFromTemplate("Warhammer");
                lw.m_unitHeld = UCL;
                l.m_weapon = lw;
                for (int j = 0; j < 200; j++)
                {
                    Unit f = new Unit(ran.Next(300) + 300 * i, ran.Next(300) + 50 * i, 3.0f);
                    f.setStats(50, 50, 100, 100, 100, 100, 100, 60, 1, 0, 4000);
                    f.setTexture(p_icon, Color.LightPink);
                    UnitController UCF = new UnitController(f);
                    BGX.addUnit(UCF);
                    Weapon fw = Weapon.getWeaponFromTemplate("Warhammer");
                    fw.m_unitHeld = UCF;
                    f.m_weapon = fw;
                }
                bb.addUnitGroup("Fodder:" + i, BGX);
            }

            bb.addFaction("Player Army", 1);
            bb.addFaction("Enemy Army", 2);

            bb.addUnit(PLAYERCONTROLLER);

            barDisplayUnit = a;
            bb.addUnitGroup("Player",bgA);

            bb.m_camera.setScreenSize(SCREEN_WIDTH, SCREEN_HEIGHT);
            bb.m_camera.setUnitFocus("Player");

            m_inputHandler = new UnitInputHandler(PlayerIndex.One, bb.getUnitController(a));



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

            m_inputHandler.handleInputs(getTimePercentage(gameTime));

            bb.checkVisible(getTimePercentage(gameTime));
            bb.checkArcs(getTimePercentage(gameTime));
            bb.updateGameState(getTimePercentage(gameTime));

            bb.setVisible();
//bb.checkDiagnostics();
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

            base.Draw(gameTime);
        }

        private void TEMPDRAWBAR()
        {
            int Height = GraphicsDevice.Viewport.Height-64;
            int Start = 32;
            int HP = barDisplayUnit.m_stats["HP"];
            spriteBatch.Draw(bar_mid, new Vector2(Start,Height), null, Color.LightBlue, 0.0f, new Vector2(0, 0), new Vector2(((float)HP-16)/32.0f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(bar_edge, new Vector2(HP+16, Height), null, Color.LightBlue, 0.0f, new Vector2(0, 0), new Vector2(0.5f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(bar_font, "" + HP, new Vector2(Start + 8, Height), Color.DarkBlue);

            int FP = barDisplayUnit.m_stats["FP"];
            spriteBatch.Draw(bar_mid, new Vector2(Start, Height+32), null, Color.LightGreen, 0.0f, new Vector2(0, 0), new Vector2(((float)FP - 16) / 32.0f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(bar_edge, new Vector2(FP + 16, Height+32), null, Color.LightGreen, 0.0f, new Vector2(0, 0), new Vector2(0.5f, 0.75f), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(bar_font, "" + FP, new Vector2(Start + 8, Height + 32), new Color(0, 55, 0));

            spriteBatch.DrawString(ko_font, "KOs: " + barDisplayUnit.m_killCount, new Vector2(GraphicsDevice.Viewport.Width - 164, Height+16), Color.Black);
        }

        private float getTimePercentage(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
        }

        public void setupDefaultAnimations()
        {
            Animation.initialize();
            Animation.s_animations.Add("UNIT-CHARGE-EXPLODE", new Animation(Content.Load<Texture2D>("UM_CenterFlashTest"), 8, 1));
        }

        private List<int> InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (bFullScreen == false)
            {
                while (!(iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    || !(iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    iWidth -= 16;
                    iHeight -= 9;
                }
                graphics.PreferredBackBufferWidth = iWidth;
                graphics.PreferredBackBufferHeight = iHeight;
                graphics.IsFullScreen = bFullScreen;
                graphics.ApplyChanges();
                List<int> ret = new List<int>();
                ret.Add(iWidth); ret.Add(iHeight);
                return ret;
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
                        return new List<int>();
                    }
                }
            }
            return new List<int>();
        }


    }
}