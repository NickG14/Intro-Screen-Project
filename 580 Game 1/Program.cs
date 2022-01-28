using System;

namespace _580_Game_1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TitleScreen())
                game.Run();
        }
    }
}
