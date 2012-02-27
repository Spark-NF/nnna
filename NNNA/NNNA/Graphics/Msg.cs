using Microsoft.Xna.Framework;

namespace NNNA
{
	class Msg
	{
		private string _message;
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		private Color _color;
		public Color Couleur
		{
			get { return _color; }
			set { _color = value; }
		}

		private int _timeout;
		public int Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}

		public Msg(string message, Color color, int timeout)
		{
			_message = message;
			_color = color;
			_timeout = timeout;
		}
	}
}
