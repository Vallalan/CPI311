using System;

namespace CPI311
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Microsoft.Xna.Framework.Game game = new UIGame())
            {
                game.Run();
            }
        }
    }
#endif
}

