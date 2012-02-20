using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NNNA
{
	class Camera2D
	{
		private int m_speed;
		public int Speed
		{
			get { return m_speed; }
			set { m_speed = value; }
		}

		private Vector2 m_cameraPosition;
		public Vector2 Position
		{
			get { return m_cameraPosition; }
			set { m_cameraPosition = value; }
		}

		/// <summary>
		/// Crée une caméra.
		/// </summary>
		/// <param name="x">Coordonées initiales de la caméra en X.</param>
		/// <param name="y">Coordonées initiales de la caméra en Y.</param>
		/// <param name="speed">Vitesse de la caméra. Mettez à 0 pour une caméra statique.</param>
		public Camera2D(int x, int y, int speed = 10)
		{
			m_cameraPosition = new Vector2(x, y);
			m_speed = speed;
		}

		/// <summary>
		/// Met à jour la position de la caméra en fonction de la position de la caméra.
		/// </summary>
		public void Update(Sprite curseur, GraphicsDeviceManager graphics)
		{
			m_cameraPosition.X +=
				(Keyboard.GetState().IsKeyDown(Keys.Right) ? m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.D) ? m_speed : 0) +
				(curseur.Position.X <= graphics.PreferredBackBufferWidth && curseur.Position.X > (graphics.PreferredBackBufferWidth - 10) ? m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.Left) ? -m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.Q) ? -m_speed : 0) + 
				(curseur.Position.X >= 0 && curseur.Position.X < 10 ? -m_speed : 0);
			m_cameraPosition.Y +=
				(Keyboard.GetState().IsKeyDown(Keys.Down) ? m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.S) ? m_speed : 0) +
				(curseur.Position.Y <= (graphics.PreferredBackBufferHeight) && curseur.Position.Y > (graphics.PreferredBackBufferHeight - 10) ? m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.Up) ? -m_speed : 0) +
				(Keyboard.GetState().IsKeyDown(Keys.Z) ? -m_speed : 0) + 
				(curseur.Position.Y >= 0 && curseur.Position.Y < 10 ? -m_speed : 0);
		}
	}
}