using System;
namespace NNNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

