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
    public class Showdown : ISongs
    {
        //Song syncing
        public int songID
        {
            get
            {
                return 1;
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


        private const float bpm = 116f;


        //Different types of notes
        private TimeSpan quarterNote = new TimeSpan((long)(1 / (bpm / 60.0) * 10000000));
        private TimeSpan eighthNote = new TimeSpan((long)(1 / (bpm / 30.0) * 10000000));
        private TimeSpan sixteenthNote = new TimeSpan((long)(1 / (bpm / 15.0) * 10000000));
        private TimeSpan threeSixteenthNote = new TimeSpan((long)(1 / (bpm / 45.0) * 10000000));
        private TimeSpan dottedQuarterNote = new TimeSpan((long)(1 / (bpm / 90.0) * 10000000));
        private TimeSpan halfNote = new TimeSpan((long)(1 / (bpm / 120.0) * 10000000));
        private TimeSpan wholeNote = new TimeSpan((long)(1 / (bpm / 240.0) * 10000000));
        private TimeSpan quarterSix = new TimeSpan((long)(1 / (bpm / 75.0) * 10000000));
        private TimeSpan quarterThreeSix = new TimeSpan((long)(1 / (bpm / 105.0) * 10000000));
        private TimeSpan eightAndAHalfBeats = new TimeSpan((long)(1 / (bpm / 510.0) * 10000000));
        private TimeSpan fourAndAHalfBeats = new TimeSpan((long)(1 / (bpm / 270.0) * 10000000));
        private TimeSpan twoAndAHalf = new TimeSpan((long)(1 / (bpm / 150.0) * 10000000));


        public TimeSpan firstNote
        {
            get
            {
                return new TimeSpan(0, 0, 0, 0, 800);
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
                return 36;
            }
        }

        Song song;

        public void Play()
        {
            MediaPlayer.Play(song, new TimeSpan(0,0,0,0,0));
        }


        public void Initialize()
        {
            //Complete note layout
            noteLayout = new TimeSpan[] {

                firstNote, eighthNote, quarterNote, threeSixteenthNote, threeSixteenthNote, quarterNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, 
                quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote,

                 threeSixteenthNote, threeSixteenthNote, quarterNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote,
                quarterNote, quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, dottedQuarterNote,

                eighthNote, quarterNote, threeSixteenthNote, threeSixteenthNote, quarterNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote,
                quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote,

                eighthNote, threeSixteenthNote, threeSixteenthNote, quarterNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote,
                quarterNote, quarterNote, quarterNote, threeSixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, dottedQuarterNote,
                
                quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, quarterNote, sixteenthNote, threeSixteenthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, quarterSix,

                 sixteenthNote, threeSixteenthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, quarterNote, sixteenthNote, threeSixteenthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, quarterThreeSix,
                sixteenthNote, eighthNote, quarterNote, quarterNote, eighthNote, quarterNote, sixteenthNote, eighthNote, quarterNote, quarterNote, eighthNote, quarterNote, sixteenthNote, eighthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, wholeNote,
                
                eighthNote, quarterNote, sixteenthNote, sixteenthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, eighthNote,
                threeSixteenthNote, threeSixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, eighthNote, threeSixteenthNote, threeSixteenthNote, halfNote, eighthNote, quarterNote, quarterNote,
                dottedQuarterNote, sixteenthNote, threeSixteenthNote, threeSixteenthNote, threeSixteenthNote, eighthNote, dottedQuarterNote, sixteenthNote, threeSixteenthNote,
                dottedQuarterNote, quarterNote, sixteenthNote, threeSixteenthNote, quarterNote, quarterNote, wholeNote,
                
                sixteenthNote, sixteenthNote, quarterSix, sixteenthNote, threeSixteenthNote, sixteenthNote, threeSixteenthNote, sixteenthNote, eighthNote, sixteenthNote, sixteenthNote,
                quarterSix, sixteenthNote, threeSixteenthNote, quarterNote, sixteenthNote, eighthNote, sixteenthNote, sixteenthNote,
                quarterThreeSix, sixteenthNote, threeSixteenthNote, sixteenthNote, eighthNote, sixteenthNote, quarterSix, sixteenthNote, quarterNote, threeSixteenthNote, threeSixteenthNote, sixteenthNote, sixteenthNote,
                quarterSix, sixteenthNote, threeSixteenthNote, sixteenthNote, threeSixteenthNote, sixteenthNote, eighthNote, sixteenthNote, sixteenthNote,
                quarterSix, sixteenthNote, threeSixteenthNote, threeSixteenthNote, quarterNote, threeSixteenthNote, sixteenthNote, threeSixteenthNote, sixteenthNote, eighthNote, sixteenthNote, sixteenthNote, eighthNote, eighthNote, wholeNote, sixteenthNote, wholeNote, wholeNote, sixteenthNote, wholeNote, endOfSong
            };
        }

        public void LoadContent(ContentManager content)
        {
            song = content.Load<Song>("Showdown-Creo");
        }

    }
}
