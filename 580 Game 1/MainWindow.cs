using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace SwordsDance
{

    public enum GameState
    {
        IntroScreen,
        Gameplay,
        Paused,
        SongOver
    }

    public class MainWindow : Game
    {
        private GameState _gamestate = GameState.IntroScreen;

        private GraphicsDeviceManager _graphics;

        private SpriteBatch spriteBatch;
        private SpriteBatch introSpriteBatch;
        private SpriteBatch gameSpriteBatch;
        private SpriteBatch songOverSpriteBatch;

        private bool fKeyDown = false;
        private bool jKeyDown = false;

        private bool escDown = false;

        //Intro textures
        private SpriteFont arial;
        private Texture2D title;
        private SwordSprite[] swords;
        //private Texture2D arrow;

        //Constant textures
        private Texture2D bg;
        private Texture2D star;

        //Game textures
        private FButton fButton;
        private JButton jButton;

        private NoteSprite note;

        private NoteSprite[] notes;

        private Texture2D gridLines;

        //Used for scoring
        private int score;
        private int highscore = 0;
        private int streak;
        private int multiplier;

        private bool alreadyHit = false;

        //Triggers
        private ResetBarrier reset = new ResetBarrier();

        //Song syncing
        private Song anyOtherWay;

        private TimeSpan endSong = new TimeSpan(0, 2, 51);

        private const float bpm = 132.1f;

        private TimeSpan songTime;
        private TimeSpan nextNote = new TimeSpan(0,0,0,0);

        private TimeSpan quarterNote = new TimeSpan((long)(1 / (bpm / 60.0) * 10000000));
        private TimeSpan eighthNote = new TimeSpan((long)(1 / (bpm / 30.0) * 10000000));
        private TimeSpan sixteenthNote = new TimeSpan((long)(1 / (bpm / 15.0) * 10000000));
        private TimeSpan threeSixteenthNote = new TimeSpan((long)(1 / (bpm / 45.0) * 10000000));
        private TimeSpan dottedQuarterNote = new TimeSpan((long)(1 / (bpm / 90.0) * 10000000));
        private TimeSpan halfNote = new TimeSpan((long)(1 / (bpm / 120.0) * 10000000));
        private TimeSpan wholeNote = new TimeSpan((long)(1 / (bpm / 240.0) * 10000000));
        private TimeSpan quarterSix = new TimeSpan((long)(1 / (bpm / 75.0) * 10000000));
        private TimeSpan eightBeats = new TimeSpan((long)(1 / (bpm / 480.0) * 10000000));
        private TimeSpan twoAndAHalf = new TimeSpan((long)(1 / (bpm / 150.0) * 10000000));

        private TimeSpan lineUpBridge = new TimeSpan(0, 0, 1, 26, 456);
        private TimeSpan lineUpDrop = new TimeSpan(0, 0, 0, 54, 600);
        private TimeSpan lineUpBuildup = new TimeSpan(0, 0, 1, 41, 500);
        private TimeSpan lineUpSecondDrop = new TimeSpan(0, 0, 1, 56, 520);

        private TimeSpan endOfSong = new TimeSpan(0, 1, 0, 0);
        private float songLengthSeconds = 0f;
        private float songLengthMinutes = 0f;

        private TimeSpan[] noteLayout;
        private TimeSpan[] introNoteLayout;
        private TimeSpan[] firstVerseLayout;
        private TimeSpan[] firstDropLayout;
        private TimeSpan[] secondVerseLayout;
        private TimeSpan[] bridgeLayout;
        private TimeSpan[] buildupLayout;
        private TimeSpan[] secondDropLayout;


        bool repeatDrop = false;

        int noteIterator = 0;

        int drawIterator = 0;

        int section = 0;

        //SongOver Textures
        private Texture2D dark;
        private SpriteFont bigArial;
        
        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initialize intro sprites
            swords = new SwordSprite[]{
            new SwordSprite() { Position = new Vector2(200,200), animationFrame = 0},
            new SwordSprite() {Position = new Vector2(500,200), animationFrame = 2}
            };

            //Initialize game sprites
            fButton = new FButton();
            jButton = new JButton();


            //Make the song
            note = new NoteSprite();
            notes = new NoteSprite[20] { new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(),
                                         new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite(), new NoteSprite()
                                        };

            introNoteLayout = new TimeSpan[] { quarterNote, wholeNote, eighthNote, dottedQuarterNote, dottedQuarterNote, quarterNote, dottedQuarterNote, dottedQuarterNote, quarterNote, wholeNote, 
                                          quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, quarterNote, eighthNote, quarterNote, quarterNote, 
                                          quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, sixteenthNote, 
                                          threeSixteenthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, threeSixteenthNote, quarterSix, threeSixteenthNote, quarterSix,
                                          quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote };

            firstVerseLayout = new TimeSpan[] { quarterNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, eighthNote, eighthNote, dottedQuarterNote, eighthNote, eighthNote, 
                                                quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote, 
                                                quarterNote, dottedQuarterNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, eighthNote, eighthNote,
                                                dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, dottedQuarterNote, dottedQuarterNote, quarterNote, halfNote, 
                                                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, eighthNote, 
                                                eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, 
                                                eighthNote, eighthNote, dottedQuarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote,
                                                dottedQuarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, lineUpDrop };

            firstDropLayout = new TimeSpan[] { quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, quarterNote, sixteenthNote,
                                               threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                                               eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote };

            secondVerseLayout = new TimeSpan[] { wholeNote, quarterNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, halfNote, eighthNote,
                                                 quarterNote, dottedQuarterNote, eighthNote, dottedQuarterNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote,
                                                halfNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, eighthNote, lineUpBridge };

            bridgeLayout = new TimeSpan[] { dottedQuarterNote, sixteenthNote, sixteenthNote, eighthNote, quarterNote, eighthNote, quarterNote, threeSixteenthNote, eighthNote, threeSixteenthNote, 
                                            quarterNote, eighthNote, threeSixteenthNote, eighthNote, threeSixteenthNote, quarterNote, eighthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, 
                                            eighthNote, quarterNote, eighthNote, quarterNote, eighthNote, sixteenthNote, sixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, 
                                            threeSixteenthNote, eighthNote, threeSixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, threeSixteenthNote, 
                                            eighthNote, threeSixteenthNote, lineUpBuildup };

            buildupLayout = new TimeSpan[] { eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote,
                                             eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote,
                                             eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote,
                                             eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote,
                                             eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote,
                                             eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, lineUpSecondDrop };

            secondDropLayout = new TimeSpan[] { quarterNote, quarterNote, quarterNote, quarterNote, sixteenthNote, eighthNote, quarterNote, sixteenthNote, eighthNote, eighthNote,
                                                sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote,
                                                eighthNote, eighthNote, sixteenthNote, eighthNote, eighthNote, sixteenthNote,

                                                threeSixteenthNote, quarterNote, quarterNote, quarterNote, quarterNote, sixteenthNote, eighthNote, quarterNote, sixteenthNote, eighthNote, eighthNote,
                                                sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote,
                                                eighthNote, eighthNote, sixteenthNote, eighthNote, eighthNote, sixteenthNote};

            noteLayout = introNoteLayout;

            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            introSpriteBatch = new SpriteBatch(GraphicsDevice);

            gameSpriteBatch = new SpriteBatch(GraphicsDevice);

            songOverSpriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Intro Textures
            arial = Content.Load<SpriteFont>("Arial");
            title = Content.Load<Texture2D>("Title");
            foreach (var sword in swords) sword.LoadContent(Content);
            //arrow = Content.Load<Texture2D>("Arrow");

            //Load Constant Textures
            bg = Content.Load<Texture2D>("CityBG");
            star = Content.Load<Texture2D>("Star");

            //Load Game Textures
            fButton.LoadContent(Content);
            jButton.LoadContent(Content);
            note.LoadContent(Content);
            foreach(NoteSprite n in notes)
            {
                n.LoadContent(Content);
            }
            gridLines = Content.Load<Texture2D>("GridLines");

            //Load Songs
            anyOtherWay = Content.Load<Song>("Any-Other-Way-Boom-Kitty");
            nextNote = new TimeSpan(0, 0, 0, 2);
            songLengthSeconds = anyOtherWay.Duration.Seconds;
            songLengthMinutes = anyOtherWay.Duration.Minutes;

            //Load SongOver Textures
            dark = Content.Load<Texture2D>("DarkScreen");
            bigArial = Content.Load <SpriteFont>("BigArial");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);

            switch (_gamestate)
            {
                case GameState.IntroScreen:
                    UpdateIntroScreen(gameTime);
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

        private void Reload()
        {
            section = 0;
            score = 0;
            streak = 0;
            multiplier = 1;
            noteIterator = 0;
            alreadyHit = false;
            repeatDrop = false;
            
        }

        private void UpdateIntroScreen(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
                Exit();

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;
                

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _gamestate = GameState.Gameplay;

                if(_gamestate == GameState.Gameplay)
                {
                    //TimeSpan startSong = new TimeSpan(0, 0, 2, 47, 600);
                    TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                    MediaPlayer.Play(anyOtherWay, startSong);
                }

            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            alreadyHit = false;
            note.Update(gameTime);

            if(MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(anyOtherWay, songTime);

            else if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();

            songTime = MediaPlayer.PlayPosition;

            if(songTime.Duration().CompareTo(endSong) > 0)
            {
                _gamestate = GameState.SongOver;
                MediaPlayer.Stop();
                songTime = new TimeSpan(0, 0, 0);
            }
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
            
            
            //Hard Code some notes in to hopefully prevent desyncing
            if (noteLayout[noteIterator] == lineUpBridge)
            {
                nextNote = lineUpBridge;
            }
            if (noteLayout[noteIterator] == lineUpDrop)
            {
                nextNote = lineUpDrop;
            }
            if (noteLayout[noteIterator] == lineUpBuildup)
            {
                nextNote = lineUpBuildup;
            }
            if (noteLayout[noteIterator] == lineUpSecondDrop)
            {
                nextNote = lineUpSecondDrop;
            }

            //Send notes out
            if (songTime.Duration().CompareTo(nextNote.Duration()) > 0)
            {
                notes[drawIterator].stopped = false;
                noteIterator++;
                if (noteIterator == noteLayout.Length)
                {
                    noteIterator = 0;
                    
                    if (section == 0)
                    {  
                        noteLayout = firstVerseLayout;
                    }
                    if (section == 1)
                    {
                        noteLayout = firstDropLayout;
                    }
                    
                    section++;

                    //Plays the drop twice (its the same notes)
                    if (section == 3)
                    {
                        if (!repeatDrop)
                        {
                            repeatDrop = true;
                            section = 2;
                        }
                        else
                        {
                            noteLayout = secondVerseLayout;
                        }

                    }

                    if (section == 4)
                    {
                        noteLayout = bridgeLayout;
                    }

                    if (section == 5)
                    {
                        noteLayout = buildupLayout;
                    }

                    if (section == 6)
                    {
                        noteLayout = secondDropLayout;
                        repeatDrop = false;
                    }
                    if (section >= 7)
                    {
                        noteLayout = firstDropLayout;
                    }
                    if (section >= 11)
                    {
                        repeatDrop = true;
                        noteLayout[0] = endOfSong;
                    }


                }

                drawIterator++;
                if(drawIterator == notes.Length)
                {
                    drawIterator = 0;
                }
                nextNote = songTime.Duration().Add(noteLayout[noteIterator].Duration());
            }

            if (songTime.Duration().Seconds >= songLengthSeconds && songTime.Duration().Minutes >= songLengthMinutes )
                _gamestate = GameState.SongOver;

            //f button pressed
            if (Keyboard.GetState().IsKeyDown(Keys.F) || GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
            {
                fButton.Color = Color.Red;
                foreach (NoteSprite n in notes)
                {
                    if(!fKeyDown && !alreadyHit)
                    {
                        if (n.Bounds.CollidesWith(fButton.Bounds))
                        {
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
                fKeyDown = false;
            }

            //J Button pressed
            if (Keyboard.GetState().IsKeyDown(Keys.J) || GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                jButton.Color = Color.Blue;
                foreach (NoteSprite n in notes)
                {
                    if (!jKeyDown && !alreadyHit)
                    {
                        if (n.Bounds.CollidesWith(jButton.Bounds))
                        {
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
                jKeyDown = false;
            }



            foreach (NoteSprite n in notes)
            {
                if (n.Bounds.CollidesWith(reset.Bounds))
                {
                    streak = 0;
                    n.Reset(gameTime);
                }
                else if (n.animationFrame == 14)
                {
                    n.Reset(gameTime);
                }

                if(MediaPlayer.State == MediaState.Playing) n.Update(gameTime);

            }

            //Pause game
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
            {
                escDown = true;
                _gamestate = GameState.Paused;
                MediaPlayer.Pause();
            }


        }

        private void UpdatePaused(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Escape)))
                escDown = false;

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && !escDown)
            {
                escDown = true;
                _gamestate = GameState.Gameplay;
            }


            if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R)))
            {
                _gamestate = GameState.Gameplay;
                Initialize();
                LoadContent();
                Reload();
                TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                MediaPlayer.Play(anyOtherWay, startSong);
            }

            if ((GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.E)))
            {
                _gamestate = GameState.IntroScreen;
                Initialize();
                LoadContent();
                Reload();
            }
                
        }

        private void UpdateEndScreen(GameTime gameTime)
        {
            if (score > highscore)
                highscore = score;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                escDown = true;
                _gamestate = GameState.IntroScreen;
                Initialize();
                LoadContent();
                Reload();
            }
               

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _gamestate = GameState.Gameplay;
                Initialize();
                LoadContent();
                Reload();
                //TimeSpan startSong = new TimeSpan(0, 0, 2, 47, 600);
                TimeSpan startSong = new TimeSpan(0, 0, 0, 0, 0);
                MediaPlayer.Play(anyOtherWay, startSong);
            }
                
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.DarkMagenta);

            

            spriteBatch.Begin();

            //Draw Stars
            spriteBatch.Draw(star, new Vector2(500, 20), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(50, 170), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(350, 250), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(780, 100), new Rectangle(0, 0, 64, 64), Color.White);

            //Draw Background
                spriteBatch.Draw(bg, new Vector2(-50, 100), new Rectangle(0, 0, 700, 500), Color.Purple);
                spriteBatch.Draw(bg, new Vector2(650, 100), new Rectangle(0, 0, 700, 500), Color.Purple);
            

            spriteBatch.End();

            switch (_gamestate)
            {
                case GameState.IntroScreen:
                    DrawIntro(gameTime);
                    break;

                case GameState.Gameplay:
                    DrawGameplay(gameTime);
                    break;

                case GameState.Paused:
                    DrawGameplay(gameTime);
                    DrawPaused(gameTime);
                    break;

                case GameState.SongOver:
                    DrawEndScreen(gameTime);
                    break;
            }
        }

        private void DrawIntro(GameTime gameTime)
        {
            introSpriteBatch.Begin();
            // TODO: Add your drawing code here

            //Draw text
            introSpriteBatch.DrawString(arial, "Press Start/Enter to start the game", new Vector2(290, 400), Color.White);
            introSpriteBatch.DrawString(arial, "Press Escape/B to exit the game", new Vector2(295, 450), Color.White);

            //Draw title
            introSpriteBatch.Draw(title, new Vector2(110, 30), new Rectangle(0, 0, 300, 100), Color.LightBlue, 0, new Vector2(0, 0), (float)2.0, SpriteEffects.None, 0);

            //Draw arrows

            /*
            for (int i = 1; i < 5; i++)
            {
                introSpriteBatch.Draw(arrow, new Vector2(50, (i - 1) * 100 + 50), new Rectangle(0, 0, 100, 100), Color.White, 0, new Vector2(0, 0), (float)0.25, SpriteEffects.None, 0);
                introSpriteBatch.Draw(arrow, new Vector2(725, i * 100 - 50), new Rectangle(0, 0, 100, 100), Color.White, 0, new Vector2(0, 0), (float)0.25, SpriteEffects.FlipVertically, 0);
            }
            */
            //Draw Swords
            foreach (var sword in swords)
            {
                sword.Draw(gameTime, introSpriteBatch);
            }


            introSpriteBatch.End();
        }

        private void DrawGameplay(GameTime gameTime)
        {
            gameSpriteBatch.Begin();
            //Draw grid lines
            gameSpriteBatch.Draw(gridLines, new Vector2(100, 80), new Rectangle(0, 0, 700, 80), Color.White);
            gameSpriteBatch.Draw(gridLines, new Vector2(100, 310), new Rectangle(0, 0, 700, 80), Color.White);

            //Draw buttons
            fButton.Draw(gameTime, gameSpriteBatch);
            jButton.Draw(gameTime, gameSpriteBatch);


            //testing values

            /*
            gameSpriteBatch.DrawString(arial, songTime.Duration().Seconds.ToString(), new Vector2(295, 450), Color.White);
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
            foreach (NoteSprite n in notes)
            {
                n.Draw(gameTime, gameSpriteBatch);
            }

            //Draw Score
            gameSpriteBatch.DrawString(arial, "Streak: " + streak.ToString(), new Vector2(680, 400), Color.White);
            gameSpriteBatch.DrawString(arial, "Multiplier: " + multiplier.ToString(), new Vector2(680, 425), Color.White);
            gameSpriteBatch.DrawString(arial, "Score: " + score.ToString(), new Vector2(680, 450), Color.White);

            gameSpriteBatch.DrawString(arial, "Escape/B to Pause", new Vector2(650, 10), Color.White);

            gameSpriteBatch.End();
        }

        private void DrawPaused(GameTime gameTime)
        {
            songOverSpriteBatch.Begin();

            //Darken Screen
            songOverSpriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            songOverSpriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);

            //Display Score
            songOverSpriteBatch.DrawString(bigArial, "Score:", new Vector2(300, 100), Color.White);
            songOverSpriteBatch.DrawString(bigArial, score.ToString(), new Vector2(300, 170), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press E/Left Trigger to exit to menu", new Vector2(300, 250), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Start/R to retry", new Vector2(300, 300), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Escape/B to resume", new Vector2(300, 350), Color.White);

            songOverSpriteBatch.DrawString(arial, "High Score: " + highscore.ToString(), new Vector2(300, 40), Color.White);

            songOverSpriteBatch.End();
        }

        private void DrawEndScreen(GameTime gameTime)
        {
            songOverSpriteBatch.Begin();

            //Darken Screen
            songOverSpriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            songOverSpriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);

            //Display Score
            songOverSpriteBatch.DrawString(bigArial, "Score:", new Vector2(300, 100), Color.White);
            songOverSpriteBatch.DrawString(bigArial, score.ToString(), new Vector2(300, 170), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Start/R to retry", new Vector2(300, 250), Color.White);
            songOverSpriteBatch.DrawString(arial, "Press Escape/B to exit song", new Vector2(300, 300), Color.White);
            songOverSpriteBatch.DrawString(arial, "High Score: " + highscore.ToString(), new Vector2(300, 40), Color.White);

            songOverSpriteBatch.End();
        }
    }
}
