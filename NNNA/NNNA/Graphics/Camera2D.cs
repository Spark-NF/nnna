using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NNNA
{
	class Camera2D
	{
		private int _speed;
		public int Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		private Vector2 _cameraPosition;
		public Vector2 Position
		{
			get { return _cameraPosition; }
			set { _cameraPosition = value; }
		}

		/// <summary>
		/// Crée une caméra.
		/// </summary>
		/// <param name="x">Coordonées initiales de la caméra en X.</param>
		/// <param name="y">Coordonées initiales de la caméra en Y.</param>
		/// <param name="speed">Vitesse de la caméra. Mettez à 0 pour une caméra statique.</param>
		public Camera2D(int x, int y, int speed = 10)
		{
			_cameraPosition = new Vector2(x, y);
			_speed = speed;
		}

		/// <summary>
		/// Met à jour la position de la caméra en fonction de la position de la caméra.
		/// </summary>
		public void Update(Sprite curseur, GraphicsDeviceManager graphics)
		{
			_cameraPosition.X +=
				(Clavier.Get().Pressed(Keys.Right) && !Clavier.Get().GetText ? _speed : 0) +
				(Clavier.Get().Pressed(Keys.D) && !Clavier.Get().GetText ? _speed : 0) +
				(curseur.Position.X <= graphics.PreferredBackBufferWidth && curseur.Position.X > (graphics.PreferredBackBufferWidth - 10) ? _speed : 0) +
				(Clavier.Get().Pressed(Keys.Left) && !Clavier.Get().GetText ? -_speed : 0) +
				(Clavier.Get().Pressed(Keys.Q) && !Clavier.Get().GetText ? -_speed : 0) + 
				(curseur.Position.X >= 0 && curseur.Position.X < 10 ? -_speed : 0);
			_cameraPosition.Y +=
				(Clavier.Get().Pressed(Keys.Down) && !Clavier.Get().GetText ? _speed : 0) +
				(Clavier.Get().Pressed(Keys.S) && !Clavier.Get().GetText ? _speed : 0) +
				(curseur.Position.Y <= (graphics.PreferredBackBufferHeight) && curseur.Position.Y > (graphics.PreferredBackBufferHeight - 10) && !Game1.SmartHud ? _speed : 0) +
				(Clavier.Get().Pressed(Keys.Up) && !Clavier.Get().GetText ? -_speed : 0) +
				(Clavier.Get().Pressed(Keys.Z) && !Clavier.Get().GetText ? -_speed : 0) + 
				(curseur.Position.Y >= 0 && curseur.Position.Y < 10 ? -_speed : 0);
		}
	}
}