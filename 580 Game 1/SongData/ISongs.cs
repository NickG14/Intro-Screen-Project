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
    public interface ISongs
    {
        int songID { get; }

        int highScore { get; set; }

        TimeSpan[] songNoteLayout { get; }

        void Play();

        void Initialize();

        void LoadContent(ContentManager content);

        TimeSpan firstNote { get; }

        TimeSpan endSong { get; }

        int minutes { get; }

        int seconds { get; }

        bool completed { get; set; }
    }
}
