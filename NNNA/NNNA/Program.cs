namespace NNNA
{
	#if WINDOWS || XBOX
		static class Program
		{
			/// <summary>
			/// Point d'entrée principal de l'application.
			/// </summary>
			static void Main()
			{
				using (var game = new Game1())
				{ game.Run(); }
			}
		}
	#endif
}
