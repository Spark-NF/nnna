using Microsoft.Xna.Framework;

namespace NNNA
{
	class Msg
	{
		public string Message { get; set; }
		public Color Couleur { get; set; }
		public int Timeout { get; set; }

		public Msg(string message, Color color, int timeout)
		{
			Message = message;
			Couleur = color;
			Timeout = timeout;
		}
	}
}
