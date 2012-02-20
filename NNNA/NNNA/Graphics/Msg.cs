using Microsoft.Xna.Framework;

namespace NNNA
{
	class Msg
	{
		private string m_message;
		public string Message
		{
			get { return m_message; }
			set { m_message = value; }
		}

		private Color m_color;
		public Color Couleur
		{
			get { return m_color; }
			set { m_color = value; }
		}

		private int m_timeout;
		public int Timeout
		{
			get { return m_timeout; }
			set { m_timeout = value; }
		}

		public Msg(string message, Color color, int timeout)
		{
			m_message = message;
			m_color = color;
			m_timeout = timeout;
		}
	}
}
