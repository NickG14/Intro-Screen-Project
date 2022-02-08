using System;

namespace SwordsDance
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainWindow())
                game.Run();
        }
    }
}
