using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using SwordsDance.SongData;
using System;

namespace SwordsDance
{

    public enum GameState
    {
        IntroScreen,
        LevelSelect,
        Gameplay,
        Paused,
        SongOver
    }

    public class MainWindow : Game
    {
        private GameState _gamestate = GameState.IntroScreen;

        private GraphicsDeviceManager _graphics;

        //Spritebatches for different states of the game
        private SpriteBatch spriteBatch;
        private SpriteBatch introSpriteBatch;
        private SpriteBatch gameSpriteBatch;
        private SpriteBatch alphaSpriteBatch;
        private SpriteBatch songOverSpriteBatch;

        //Checks to see if keys are already down
        private bool fKeyDown = false;
        private bool jKeyDown = false;
        private bool escDown = false;
        private bool enterDown = false;
        private bool downDown = false;
        private bool upDown = false;

      
        //Intro textures
        private SpriteFont arial;
        private Texture2D title;
        private SwordSprite[] swords;
        private FlareSprite[] flares;
        //private Texture2D arrow;

        //Constant textures
        private Texture2D bg;
        private Texture2D star;

        //Level Select textures
        private Texture2D selectBar;
        private Vector2 barPosition = new Vector2(110, 90);

        //Game textures
        private FButton fButton;
        private JButton jButton;
        private FButton3D fButton3D;
        private JButton3D jButton3D;
        private Texture2D flare;
        private Texture2D track;

        //Dynamic Colors
        private int red;
        private int green;
        private int blue;
        private int maxIntensity;
        private int minIntensity;
        private Color rainbow;

        //The notes
        private NoteSprite note;
        private NoteSprite[] notes;

        //The (new) 3D notes
        private Note3D[] notes3D;
        private bool is3D = true;

        //Gridlines
        private Texture2D gridLines;

        //Used for scoring
        private int score;
        private int streak;
        private int multiplier;

        private bool alreadyHit = false;

        //Triggers
        private ResetBarrier reset = new ResetBarrier();

        //Sound Effects
        private SoundEffect startSound;
        private SoundEffect noteHit;

        //Song info
        private TimeSpan songTime;
        private TimeSpan nextNote = new TimeSpan(0, 0, 0, 0);
        private ISongs[] allSongs = new ISongs[10];
        private int songLengthMinutes;
        private int songLengthSeconds;

        //Songs
        private int songID = 0;
        private ISongs currSong;
        private AnyOtherWay anyOtherWay;
        private SpicyNoodles spicyNoodles;
        private Showdown showDown;

        //Note layouts
        private TimeSpan[] noteLayout;
        private TimeSpan[] allNoteLayout;

        //Iterators for notes/sending notes
        int noteIterator = 0;
        int drawIterator = 0;

        //SongOver Textures
        private Texture2D dark;
        private SpriteFont bigArial;

        //Tutorial stuff
        private bool skipped;
        
        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            skipped = false;
            //Initialize intro sprites
            swords = new SwordSprite[]{
            new SwordSprite() { Position = new Rectangle(200, 250, 100, 150), animationFrame = 0},
            new SwordSprite() {Position = new Rectangle(550, 250, 100, 150), animationFrame = 0}
            };

            flares = new FlareSprite[]
            {
            new FlareSprite() { Position = new Vector2(0,0)},
            new FlareSprite() { Position = new Vector2(700,0)},
            new FlareSprite() { Position = new Vector2(1400,0)}
            };

            //Initialize game sprites
            fButton = new FButton();
            jButton = new JButton();
            fButton3D = new FButton3D();
            jButton3D = new JButton3D();


            //Initialize Colors
            red = 110;
            green = 19;
            blue = 19;
            rainbow = new Color(red, green, blue);
            maxIntensity = red;
            minIntensity = blue;



            //Make the song
            note = new NoteSprite();
            notes = new NoteSprite[20] { new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite()
                                        };

            if(allSongs[0] == null)
            {
                anyOtherWay = new AnyOtherWay();
                spicyNoodles = new SpicyNoodles();
                showDown = new Showdown();
                anyOtherWay.Initialize();
                spicyNoodles.Initialize();
                showDown.Initialize();
            }



            allSongs[spicyNoodles.songID] = spicyNoodles;
            allSongs[anyOtherWay.songID] = anyOtherWay;
            allSongs[showDown.songID] = showDown;
            

            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Load Spritebatches
            spriteBatch = new SpriteBatch(GraphicsDevice);
            introSpriteBatch = new SpriteBatch(GraphicsDevice);
            gameSpriteBatch = new SpriteBatch(GraphicsDevice);
            alphaSpriteBatch = new SpriteBatch(GraphicsDevice);
            songOverSpriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Intro Textures
            arial = Content.Load<SpriteFont>("Arial");
            title = Content.Load<Texture2D>("Title");
            foreach (var sword in swords) sword.LoadContent(Content);
            foreach (var flare in flares) flare.LoadContent(Content);

            //Load Constant Textures
            bg = Content.Load<Texture2D>("CityBGWhite");
            star = Content.Load<Texture2D>("Star");

            //Load 3D notes
            notes3D = new Note3D[notes.Length];
            
            //Load Level Select Textures
            selectBar = Content.Load<Texture2D>("SelectBar");

            //Load Game Textures
            fButton.LoadContent(Content);
            jButton.LoadContent(Content);
            fButton3D.LoadContent(Content);
            jButton3D.LoadContent(Content);
            note.LoadContent(Content);
            for(int i = 0; i < notes.Length; i++)
            {
                int random = notes[i].LoadContent(Content);
                notes3D[i] = new Note3D(this, random);
                notes[i].stopped = true;
                notes3D[i].stopped = true;
            }
            gridLines = Content.Load<Texture2D>("GridLines");
            flare = Content.Load<Texture2D>("flare");
            track = Content.Load<Texture2D>("NoteTrack");

            //Load Sound Effects
            startSound = Content.Load<SoundEffect>("StartSound");
            noteHit = Content.Load<SoundEffect>("NoteHitSound");

            //Load Songs
            anyOtherWay.LoadContent(Content);
            spicyNoodles.LoadContent(Content);
            showDown.LoadContent(Content);

            if(currSong == null)
            {
                currSong = allSongs[0];
            }
            MediaPlayer.Volume = .5f;
            nextNote = currSong.firstNote;
            songLengthSeconds = currSong.seconds;
            songLengthMinutes = currSong.minutes;


            //Load SongOver Textures
            dark = Content.Load<Texture2D>("DarkScreen");
            bigArial = Content.Load <SpriteFont>("BigArial");

        }

        /// <summary>
        /// The base update function that is called every frame
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Check game state
            switch (_gamestate)
            {
                case GameState.IntroScreen:
                    UpdateIntroScreen(gameTime);
                    break;

                case GameState.LevelSelect:
                    UpdateLevelSelect(gameTime);
                    break;

                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;

                case GameState.Paused:
                    UpdatePaused(gameTime);
                    break;

                case GameState.SongOver:
                    UpdateEndScreen(gameTime);
                    break;
            }
        }

        /// <summary>
        /// Reloads the level
        /// </summary>
        private void Reload()
        {
            score = 0;
            streak = 0;
            multiplier = 1;
            noteIterator = 0;
            alreadyHit = false;
            startSound.Play();
        }

        /// <summary>
        /// Update function for the intro screen
        /// </summary>
        /// <param name="gameTime">the Game Time</param>
        private void UpdateIntroScreen(GameTime gameTime)
        {
            MediaPlayer.Stop();

            //Exit game
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
                Exit();
            //Prevent holding escape
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;
                
            //Enter pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _gamestate = GameState.LevelSelect;
                enterDown = true;
            }
        }

        /// <summary>
        /// Update fuction for the level select
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateLevelSelect(GameTime gameTime)
        {
            //To Intro Screen
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
                _gamestate = GameState.IntroScreen;
            //Prevent holding escape
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;

            //Enter Pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter) && !enterDown && songID <= 2)
            {
                _gamestate = GameState.Gameplay;
                enterDown = true;
                startSound.Play();

                if (_gamestate == GameState.Gameplay)
                {
                    currSong = allSongs[songID];
                    nextNote = currSong.firstNote;
                    noteLayout = new TimeSpan[currSong.songNoteLayout.Length];
                    int i = 0;
                    foreach (TimeSpan t in currSong.songNoteLayout)
                    {
                        noteLayout[i] = noteLayout[i].Add(t);
                        i++;
                    }


                    TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                    currSong.Play();
                }

            }
            //Prevent holding enter
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Enter)))
                enterDown = false;

            //Move select bar down
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !downDown && songID < 3){
                barPosition.Y += 100;
                songID++;
                downDown = true;
            }
                   
            //Prevent holding down
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                downDown = false;

            //Move select bar up
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !upDown && songID > 0)
            {
                barPosition.Y -= 100;
                songID--;
                upDown = true;
            }

            //Prevent holding up
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                upDown = false;
        }

        /// <summary>
        /// Update function for the gameplay loop
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void UpdateGameplay(GameTime gameTime)
        {
            alreadyHit = false;
            note.Update(gameTime);

            //Skip tutorial
            if(currSong.songID == 0 && (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter)) && !enterDown && !skipped)
            {
                skipped = true;
                MediaPlayer.Stop();
                spicyNoodles.Play(new TimeSpan(0,0,0,29,0));
            }

            //prevent skip from being pressed after song start
            if(nextNote != noteLayout[0])
            {
                skipped = true;
            }

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Enter)))
                enterDown = false;

            //Make sure MediaPlayer is playing
            //if(MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(anyOtherWay, songTime);
            if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();

            //Get song position and check for end of song
            songTime = MediaPlayer.PlayPosition;
            if (songTime.Duration().Seconds >= currSong.seconds && songTime.Duration().Minutes >= currSong.minutes) { 
                _gamestate = GameState.SongOver;
                //MediaPlayer.Stop();
                songTime = new TimeSpan(0, 0, 0);
            }

            //Change multiplier based on how many notes have been hit in a row
            if(streak >= 20)
            {
                multiplier = 5;
            }
            else if (streak >= 15)
            {
                multiplier = 4;
            }
            else if(streak >= 10)
            {
                multiplier = 3;
            }
            else if (streak >= 5)
            {
                multiplier = 2;
            }
            else
            {
                multiplier = 1;
            }
            
            //Send notes out at correct times
            if (songTime.Duration().CompareTo(nextNote.Duration()) > 0)
            {
                notes[drawIterator].stopped = false;
                notes3D[drawIterator].stopped = false;
                noteIterator++;
                
               
                if (noteIterator == noteLayout.Length)
                {
                    noteIterator = 0;
                }
               
                    drawIterator++;
                if(drawIterator == notes.Length)
                {
                    drawIterator = 0;
                }
                nextNote += noteLayout[noteIterator];
            }



            //f button pressed
            if (Keyboard.GetState().IsKeyDown(Keys.F) || GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
            {
                fButton.Color = Color.Red;
                fButton3D.Color = Color.Red;
                for(int i = 0; i < notes.Length; i++)
                {
                    NoteSprite n = notes[i];
                    Note3D n3D = notes3D[i];
                    if(!fKeyDown && !alreadyHit)
                    {
                        if (n.Bounds.CollidesWith(fButton.Bounds))
                        {
                            //noteHit.Play();
                            score = score + (multiplier * 250);
                            streak++;
                            alreadyHit = true;
                            if (notes[0].Bounds.CollidesWith(fButton.Bounds) && notes[19].Bounds.CollidesWith(fButton.Bounds))
                            {
                                notes[19].Color = Color.Black;
                                notes[19].hit = true;
                            }
                            else
                            {
                                n.Color = Color.Black;
                                n.hit = true;
                                n3D.hit = true;
                            }
                        }                  
                    }          
                }
                if (!alreadyHit && !fKeyDown)
                {
                    streak = 0;
                }
                fKeyDown = true;
            }

            else
            {
                fButton.Color = Color.White;
                fButton3D.Color = Color.White;
                fKeyDown = false;
            }

            //J Button pressed
            if (Keyboard.GetState().IsKeyDown(Keys.J) || GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                jButton.Color = Color.Blue;
                jButton3D.Color = Color.Blue;
                for (int i = 0; i < notes.Length; i++)
                {
                    NoteSprite n = notes[i];
                    Note3D n3D = notes3D[i];
                    if (!jKeyDown && !alreadyHit)
                    {
                        if (n.Bounds.CollidesWith(jButton.Bounds))
                        {
                            //noteHit.Play();
                            score = score + (multiplier * 250);
                            streak++;
                            alreadyHit = true;
                            if (notes[0].Bounds.CollidesWith(jButton.Bounds) && notes[19].Bounds.CollidesWith(jButton.Bounds))
                            {
                                notes[19].Color = Color.Black;
                                notes[19].hit = true;
                            }
                            else
                            {
                                n.Color = Color.Black;
                                n.hit = true;
                                n3D.hit = true;
                            }
                        }

                    }
                }
                if (!alreadyHit && !jKeyDown)
                {
                    streak = 0;
                }
                jKeyDown = true;
            }

            else
            {
                jButton.Color = Color.White;
                jButton3D.Color = Color.White;
                jKeyDown = false;
            }


            //Reset note if it goes off screen or gets hit
            for(int i = 0; i < notes.Length; i++)
            {
                NoteSprite n = notes[i];
                Note3D n3D = notes3D[i];
                if (n.Bounds.CollidesWith(reset.Bounds))
                {
                    streak = 0;
                    int random = n.Reset(gameTime);
                    n3D.Reset(random);
                }
                else if (n.animationFrame == 14 && !is3D)
                {
                    int random = n.Reset(gameTime);
                    n3D.Reset(random);
                }
                else if (n.hit && is3D)
                {
                    int random = n.Reset(gameTime);
                    n3D.Reset(random);
                }
                if (MediaPlayer.State == MediaState.Playing)
                {
                    n.Update(gameTime);
                    n3D.Update(gameTime);
                }

            }

            foreach(Note3D n in notes3D)
            {
                if (MediaPlayer.State == MediaState.Playing) n.Update(gameTime);
            }

            //Pause game
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
            {
                escDown = true;
                _gamestate = GameState.Paused;
                startSound.Play();
                MediaPlayer.Pause();
            }

            //Update Background Color
            
            if (red == maxIntensity && blue < maxIntensity && green < maxIntensity) blue++;
            else if (blue == maxIntensity && red > minIntensity && green < maxIntensity) red--;
            else if (blue == maxIntensity && red < maxIntensity && green < maxIntensity) green++;
            else if (green == maxIntensity && blue > minIntensity && red < maxIntensity) blue--;
            else if (green == maxIntensity && blue < maxIntensity && red < maxIntensity) red++;
            else if (red == maxIntensity && blue < maxIntensity && green > minIntensity) green--;

            rainbow = new Color(red, green, blue);



        }

        /// <summary>
        /// Update function for the Paused state
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void UpdatePaused(GameTime gameTime)
        {
            //Prevent holding escape
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;

            //Resume gameplay
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
            {
                escDown = true;
                _gamestate = GameState.Gameplay;
            }
            //Reset level
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R)))
            {
                _gamestate = GameState.Gameplay;
                Initialize();
                LoadContent();
                Reload();
                TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                currSong.Play();
            }

            //Exit to Title Screen
            if ((GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.E)))
            {
                _gamestate = GameState.IntroScreen;
                Initialize();
                LoadContent();
                Reload();
            }
                
        }

        /// <summary>
        /// Update function for when song ends
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void UpdateEndScreen(GameTime gameTime)
        {
            //Set new highscore if applicable
            if (score > currSong.highScore)
                currSong.highScore = score;

            //Exit to Title Screen
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                escDown = true;
                _gamestate = GameState.IntroScreen;
                Initialize();
                LoadContent();
                Reload();
            }
               
            //Reset level
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _gamestate = GameState.Gameplay;
                Initialize();
                LoadContent();
                Reload();
                //TimeSpan startSong = new TimeSpan(0, 0, 2, 47, 600);
                TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                currSong.Play();
            }
                
        }

        /// <summary>
        /// The base draw function that is called every frame
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //Set color of background
            GraphicsDevice.Clear(rainbow);

            //Draw textures that will always be on screen
            spriteBatch.Begin();

            //Draw Stars
            spriteBatch.Draw(star, new Vector2(500, 20), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(50, 110), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(300, 250), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(780, 100), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.End();


            //Draw different things based on game state
            switch (_gamestate)
            {
                case GameState.IntroScreen:
                    DrawIntro(gameTime);
                    break;

                case GameState.LevelSelect:
                    DrawLevelSelect(gameTime);
                    break;

                case GameState.Gameplay:
                    DrawGameplay(gameTime, false);
                    break;

                case GameState.Paused:
                    DrawGameplay(gameTime, true);
                    DrawPaused(gameTime);
                    break;

                case GameState.SongOver:
                    DrawEndScreen(gameTime);
                    break;
            }
        }

        /// <summary>
        /// The draw function for the Title Screen
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void DrawIntro(GameTime gameTime)
        {
            spriteBatch.Begin(blendState: BlendState.Additive);
            //Draw Background
            spriteBatch.Draw(bg, new Vector2(-50, 100), new Rectangle(0, 0, 700, 500), rainbow);
            spriteBatch.Draw(bg, new Vector2(650, 100), new Rectangle(0, 0, 700, 500), rainbow);

            //Draw flares          
            foreach (var flare in flares)
            {
                flare.Draw(gameTime, spriteBatch, false);
            }

            spriteBatch.End();


            introSpriteBatch.Begin();

            //Draw text
            introSpriteBatch.DrawString(arial, "Press Start/Enter to start the game", new Vector2(290, 400), Color.White);
            introSpriteBatch.DrawString(arial, "Press Escape/B to exit the game", new Vector2(295, 450), Color.White);

            //Draw title
            introSpriteBatch.Draw(title, new Vector2(110, 30), new Rectangle(0, 0, 300, 100), Color.LightBlue, 0, new Vector2(0, 0), (float)2.0, SpriteEffects.None, 0);

            //Draw Swords
            foreach (var sword in swords)
            {
                sword.Draw(gameTime, introSpriteBatch);
            }


            introSpriteBatch.End();

        }

        /// <summary>
        /// The draw function for level select
        /// </summary>
        /// <param name="gameTime"></param>
        private void DrawLevelSelect(GameTime gameTime)
        {

            Matrix transform;
            transform = Matrix.CreateScale(2);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: transform);
            //Draw Background
            spriteBatch.Draw(bg, new Vector2(-50, -100), new Rectangle(0, 0, 700, 500), rainbow);
            spriteBatch.Draw(bg, new Vector2(650, -100), new Rectangle(0, 0, 700, 500), rainbow);

            //Draw flares          
            foreach (var flare in flares)
            {
                flare.Draw(gameTime, spriteBatch, false);
            }

            spriteBatch.End();


            gameSpriteBatch.Begin();

            //Darken Screen
            gameSpriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            gameSpriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);

            gameSpriteBatch.End();

            alphaSpriteBatch.Begin(blendState: BlendState.Additive);
            alphaSpriteBatch.Draw(selectBar, barPosition, null, Color.White);

            
            //Display Score
            alphaSpriteBatch.DrawString(arial, "Level Select:", new Vector2(350, 25), Color.White);
            alphaSpriteBatch.DrawString(bigArial, "Tutorial", new Vector2(225, 100), Color.White);
            alphaSpriteBatch.DrawString(bigArial, "Showdown", new Vector2(225, 200), Color.White);
            alphaSpriteBatch.DrawString(bigArial, "Any Other Way", new Vector2(225, 300), Color.White);
            alphaSpriteBatch.DrawString(bigArial, "Coming Soon...", new Vector2(225, 400), Color.White);

            alphaSpriteBatch.End();

        }

        /// <summary>
        /// The draw function for the gameplay loop
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void DrawGameplay(GameTime gameTime, bool paused)
        {
            spriteBatch.Begin();
            //Draw Background
            spriteBatch.Draw(bg, new Vector2(-50, 100), new Rectangle(0, 0, 700, 500), rainbow);
            spriteBatch.Draw(bg, new Vector2(650, 100), new Rectangle(0, 0, 700, 500), rainbow);

            //Draw flares          
            foreach (var flare in flares)
            {
                flare.Draw(gameTime, spriteBatch, paused);
            }

            //Draw track
            spriteBatch.Draw(track, new Vector2(60, 100), new Rectangle(0, 0, 700, 400), Color.White);

            //Draw 3D buttons
            fButton3D.Draw(gameTime, spriteBatch);
            jButton3D.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            gameSpriteBatch.Begin();

            //Draw grid lines
            //gameSpriteBatch.Draw(gridLines, new Vector2(200, 80), new Rectangle(0, 0, 700, 80), Color.White);
            //gameSpriteBatch.Draw(gridLines, new Vector2(200, 310), new Rectangle(0, 0, 700, 80), Color.White);



            //Draw buttons
            //fButton.Draw(gameTime, gameSpriteBatch);
            //jButton.Draw(gameTime, gameSpriteBatch);




            //testing values
            /*
            gameSpriteBatch.DrawString(arial, songTime.Duration().Minutes.ToString(), new Vector2(295, 350), Color.White);
            gameSpriteBatch.DrawString(arial, songTime.Duration().Seconds.ToString(), new Vector2(295, 450), Color.White);

            gameSpriteBatch.DrawString(arial, currSong.minutes.ToString(), new Vector2(395, 350), Color.White);
            gameSpriteBatch.DrawString(arial, currSong.seconds.ToString(), new Vector2(395, 450), Color.White);
            
            gameSpriteBatch.DrawString(arial, songLengthSeconds.ToString(), new Vector2(295, 400), Color.White);
            gameSpriteBatch.DrawString(arial, _gamestate.ToString(), new Vector2(295, 350), Color.White);
            

            gameSpriteBatch.DrawString(arial, noteLayout[0].ToString(), new Vector2(295, 300), Color.White);

            gameSpriteBatch.DrawString(arial, nextNote.ToString(), new Vector2(295, 400), Color.White);

            /*
            gameSpriteBatch.DrawString(arial, notes[0].Bounds.X.ToString(), new Vector2(295, 350), Color.White);

            gameSpriteBatch.DrawString(arial, fButton.Bounds.X.ToString(), new Vector2(100, 350), Color.White);
            gameSpriteBatch.DrawString(arial, fButton.Bounds.Y.ToString(), new Vector2(150, 350), Color.White);
            */

            //Draw notes
            /*
            foreach (NoteSprite n in notes)
            {
                n.Draw(gameTime, gameSpriteBatch);
            }
            */

            //Draw 3D notes
            for (int i = notes3D.Length - 1; i >= 0; i--)
            {
                Note3D n3D = notes3D[i];
                if (!n3D.stopped)
                {
                    n3D.Draw();
                }

            }

            //Draw Score
            gameSpriteBatch.DrawString(arial, "Streak: " + streak.ToString(), new Vector2(680, 400), Color.White);
            gameSpriteBatch.DrawString(arial, "Multiplier: " + multiplier.ToString(), new Vector2(680, 425), Color.White);
            gameSpriteBatch.DrawString(arial, "Score: " + score.ToString(), new Vector2(680, 450), Color.White);

            gameSpriteBatch.DrawString(arial, "Escape/B to Pause", new Vector2(650, 10), Color.White);

            gameSpriteBatch.End();

            //Draw tutorial if correct level
            if(currSong.songID == 0 && nextNote == noteLayout[0] && !paused)
            {
                spriteBatch.Begin();
                spicyNoodles.Draw(gameTime, spriteBatch, dark, arial);
                spriteBatch.End();
            }

        }

        /// <summary>
        /// The draw function for the paused state
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void DrawPaused(GameTime gameTime)
        {
            songOverSpriteBatch.Begin();

            //Darken Screen
            songOverSpriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            songOverSpriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);

            //Display Score
            songOverSpriteBatch.DrawString(bigArial, "Score:", new Vector2(300, 100), Color.White);
            songOverSpriteBatch.DrawString(bigArial, score.ToString(), new Vector2(300, 170), Color.White);
            songOverSpriteBatch.DrawString(arial, "High Score: " + currSong.highScore.ToString(), new Vector2(300, 40), Color.White);

            //Display options
            songOverSpriteBatch.DrawString(arial, "Press E/Left Trigger to exit to menu", new Vector2(300, 250), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Start/R to retry", new Vector2(300, 300), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Escape/B to resume", new Vector2(300, 350), Color.White);

            songOverSpriteBatch.End();
        }

        /// <summary>
        /// The draw function for when a song ends
        /// </summary>
        /// <param name="gameTime">The Game Time</param>
        private void DrawEndScreen(GameTime gameTime)
        {
            songOverSpriteBatch.Begin();

            //Darken Screen
            songOverSpriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            songOverSpriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);

            //Display Score
            songOverSpriteBatch.DrawString(bigArial, "Score:", new Vector2(300, 100), Color.White);
            songOverSpriteBatch.DrawString(bigArial, score.ToString(), new Vector2(300, 170), Color.White);
            songOverSpriteBatch.DrawString(arial, "High Score: " + currSong.highScore.ToString(), new Vector2(300, 40), Color.White);

            //Display Options
            songOverSpriteBatch.DrawString(arial, "Press Start/R to retry", new Vector2(300, 250), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Escape/B to exit song", new Vector2(300, 300), Color.White);


            songOverSpriteBatch.End();
        }
    }
}
