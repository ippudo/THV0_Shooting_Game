using System;

namespace THPages
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*TH_BugReporter.Program.PreMain();
            try
            {*/
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            /*}
            catch (Exception e)
            {
                TH_BugReporter.Program.ThrowExceptionWindow(e);
            }*/
        }
    }
#endif
}

