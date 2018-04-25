using System;

namespace SolarSystemDefense
{
#if WINDOWS
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (Main game = new Main())
            {
                game.Run();
            }
        }
    }
#endif
}

