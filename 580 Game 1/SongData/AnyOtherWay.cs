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
    public class AnyOtherWay : ISongs
    {
        //Song syncing
        public int songID
        {
            get
            {
                return 2;
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
                return new TimeSpan(0, 2, 51);
            }
        }


        private const float bpm = 132.1f;


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
                return new TimeSpan(0, 0, 0, 2, 0);
            }
        } 

        //End of song
        private TimeSpan endOfSong = new TimeSpan(0, 1, 0, 0);

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
                return 2;
            }
        }
        public int seconds
        {
            get
            {
                return 47;
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

        public void Initialize()
        {
            //Complete note layout
            noteLayout = new TimeSpan[] { 

                firstNote,
                //Intro
                wholeNote, eighthNote, dottedQuarterNote, dottedQuarterNote, quarterNote, dottedQuarterNote, dottedQuarterNote, quarterNote, wholeNote,
                quarterNote, eighthNote, eighthNote, quarterNote, dottedQuarterNote, eighthNote, quarterNote, eighthNote, quarterNote, quarterNote,
                quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, sixteenthNote,
                threeSixteenthNote, eighthNote, quarterNote, quarterNote, quarterNote, dottedQuarterNote, threeSixteenthNote, quarterSix, threeSixteenthNote, quarterSix,
                quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote,
                //first verse
                quarterNote, quarterNote, halfNote, eighthNote, eighthNote, eighthNote, eighthNote, dottedQuarterNote, eighthNote, eighthNote,
                    quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote,
                    quarterNote, halfNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, eighthNote, eighthNote,
                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, dottedQuarterNote, dottedQuarterNote, quarterNote, halfNote,
                    quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote, eighthNote,
                    eighthNote, eighthNote, halfNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote,
                    dottedQuarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote,
                //drop
                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, quarterNote, sixteenthNote,
                    threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote,

                //Repeat drop
                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, eighthNote, quarterNote,
                eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote,
                //Second Verse
                    fourAndAHalfBeats, quarterNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, halfNote, eighthNote,
                        quarterNote, dottedQuarterNote, eighthNote, dottedQuarterNote, eighthNote, eighthNote, eighthNote, sixteenthNote, sixteenthNote,
                    halfNote, quarterNote, dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, quarterNote, eighthNote, eightAndAHalfBeats, 
                //Bridge
                dottedQuarterNote, sixteenthNote, sixteenthNote, eighthNote, quarterNote, eighthNote, quarterNote, threeSixteenthNote, eighthNote, threeSixteenthNote,
                dottedQuarterNote, eighthNote, threeSixteenthNote, eighthNote, threeSixteenthNote, quarterNote, eighthNote, dottedQuarterNote, sixteenthNote, sixteenthNote,
                eighthNote, quarterNote, eighthNote, quarterNote, eighthNote, sixteenthNote, sixteenthNote, halfNote, sixteenthNote, sixteenthNote,
                threeSixteenthNote, eighthNote, threeSixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, dottedQuarterNote, sixteenthNote, sixteenthNote, threeSixteenthNote,
                eighthNote, threeSixteenthNote, wholeNote,

                //Buildup
                quarterNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote,
                    eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote,
                    eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote, eighthNote,
                    eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, eighthNote,
                    eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, quarterNote, eighthNote,
                    eighthNote, threeSixteenthNote, eighthNote, sixteenthNote, threeSixteenthNote, 

                    //Buildup Part 2
                    halfNote, quarterNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote, quarterNote, eighthNote, eighthNote,
                    sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote,
                    eighthNote, eighthNote, sixteenthNote, eighthNote, eighthNote, sixteenthNote,

                    threeSixteenthNote, quarterNote, quarterNote, quarterNote, quarterNote, sixteenthNote, eighthNote, quarterNote, sixteenthNote, eighthNote, eighthNote,
                    sixteenthNote, threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote, eighthNote,
                    eighthNote, eighthNote, sixteenthNote, eighthNote, eighthNote, sixteenthNote,

                    //Second Drop
                    halfNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, quarterNote, sixteenthNote,
                    threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote,

                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, eighthNote, quarterNote,
                    eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote, quarterNote,

                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, sixteenthNote, threeSixteenthNote, quarterNote, sixteenthNote,
                    threeSixteenthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote,


                    dottedQuarterNote, eighthNote, eighthNote, quarterNote, quarterNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, eighthNote, quarterNote,
                    eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, eighthNote, quarterNote, quarterNote, eighthNote,
                    eighthNote, eighthNote, eighthNote, sixteenthNote, threeSixteenthNote, eighthNote, sixteenthNote, sixteenthNote, endOfSong};

        }

        public void LoadContent(ContentManager content)
        {
            song = content.Load<Song>("Any-Other-Way-Boom-Kitty");
        }
    }
}
