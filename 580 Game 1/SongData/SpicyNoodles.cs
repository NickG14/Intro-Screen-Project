using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using SwordsDance;

namespace SwordsDance.SongData
{
    public class SpicyNoodles : ISongs
    {
        //Song syncing
        public int songID
        {
            get
            {
                return 0;
            }
        }

        private int _highScore;
        public int highScore
        {
            get
            {
                return _highScore;
            }

            set
            {
                _highScore = value;
            }
        }


        public TimeSpan endSong
        {
            get
            {
                return new TimeSpan(0, 1, 49);
            }
        }


        private const float bpm = 131.5f;


        //Different types of notes
        private TimeSpan quarterNote = new TimeSpan((long)(1 / (bpm / 60.0) * 10000000));
        private TimeSpan eighthNote = new TimeSpan((long)(1 / (bpm / 30.0) * 10000000));
        private TimeSpan sixteenthNote = new TimeSpan((long)(1 / (bpm / 15.0) * 10000000));
        private TimeSpan threeSixteenthNote = new TimeSpan((long)(1 / (bpm / 45.0) * 10000000));
        private TimeSpan dottedQuarterNote = new TimeSpan((long)(1 / (bpm / 90.0) * 10000000));
        private TimeSpan halfNote = new TimeSpan((long)(1 / (bpm / 120.0) * 10000000));
        private TimeSpan wholeNote = new TimeSpan((long)(1 / (bpm / 240.0) * 10000000));
        private TimeSpan quarterSix = new TimeSpan((long)(1 / (bpm / 75.0) * 10000000));
        private TimeSpan eightAndAHalfBeats = new TimeSpan((long)(1 / (bpm / 510.0) * 10000000));
        private TimeSpan fourAndAHalfBeats = new TimeSpan((long)(1 / (bpm / 270.0) * 10000000));
        private TimeSpan twoAndAHalf = new TimeSpan((long)(1 / (bpm / 150.0) * 10000000));


        public TimeSpan firstNote
        {
            get
            {
                return new TimeSpan(0, 0, 0, 29, 0);
            }
        }


        //End of song
        private TimeSpan endOfSong = new TimeSpan(0, 1, 0, 0);
        private float songLengthSeconds = 0f;
        private float songLengthMinutes = 0f;

        //Note layouts
        private TimeSpan[] noteLayout;
        public TimeSpan[] songNoteLayout
        {
            get
            {
                return noteLayout;
            }
        }


        public int minutes
        {
            get
            {
                return 1;
            }
        }
        public int seconds
        {
            get
            {
                return 48;
            }
        }

        private bool _complete = false;
        public bool completed
        {
            get
            {
                return _complete;
            }

            set
            {
                _complete = value;
            }
        }

        Song song;

        public void Play()
        {
            MediaPlayer.Play(song, TimeSpan.Zero);
        }

        public void Play(TimeSpan skip)
        {
            MediaPlayer.Play(song, skip);
        }

        public void Initialize()
        {
            //Complete note layout
            noteLayout = new TimeSpan[] {
               
                firstNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote,
                quarterNote, quarterNote, 
                wholeNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote,
                quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, sixteenthNote,
                sixteenthNote, quarterNote, eighthNote, eighthNote, quarterNote, sixteenthNote, sixteenthNote, eighthNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote,
                quarterNote, quarterNote, threeSixteenthNote, sixteenthNote, eighthNote, eighthNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, 
                eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote,
                eighthNote, eighthNote, eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, halfNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, twoAndAHalf,
                 quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote,
                quarterNote, quarterNote, quarterNote, eighthNote, sixteenthNote, sixteenthNote, quarterNote, eighthNote, eighthNote, quarterNote, sixteenthNote, sixteenthNote, eighthNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, halfNote, quarterNote,
                threeSixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote, eighthNote, threeSixteenthNote,
                threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote, sixteenthNote, threeSixteenthNote, threeSixteenthNote,
                quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, quarterNote, eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote,
                threeSixteenthNote, threeSixteenthNote, quarterNote, eighthNote, sixteenthNote, eighthNote, threeSixteenthNote, endOfSong
            };
        }

        public void LoadContent(ContentManager content)
        {
            song = content.Load<Song>("Spicy-Noodles-Boom-Kitty");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D dark, SpriteFont sf)
        {
            //Darken Screen
            spriteBatch.Draw(dark, new Vector2(0, 0), null, Color.White);
            spriteBatch.Draw(dark, new Vector2(700, 0), null, Color.White);
            spriteBatch.DrawString(sf, "Song: Spicy Noodles by Boom Kitty", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(sf, "Press the F and J keys when a note passes over their respective zones.", new Vector2(100,100), Color.White);
            spriteBatch.DrawString(sf, "The more notes you hit in a row, the more points you get for each note!", new Vector2(100, 200), Color.White);
            spriteBatch.DrawString(sf, "Missing a note or hitting F or J when no note is over their zone will result in the streak being reset", new Vector2(100, 300), Color.White);
            spriteBatch.DrawString(sf, "Go for a high score!", new Vector2(100, 400), Color.White);
            spriteBatch.DrawString(sf, "Press Enter to skip", new Vector2(652, 35), Color.White);
        }

    }
}
