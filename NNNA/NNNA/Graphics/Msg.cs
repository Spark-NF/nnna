using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

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
