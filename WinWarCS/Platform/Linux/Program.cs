using System;
using WinWarRT;

namespace WinWarCS.Linux
{
	static class Program
	{
		private static MainGame game;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			game = new MainGame ();
			game.Run ();
		}
	}
}

