using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine : StaticSprite
	{
		public Resource Resource { get; private set; }
		public int Quantity { get; private set; }

		public ResourceMine(int x, int y, Resource resource, int quantity, Image texture, int matriceX = - 1, int matriceY = -1)
			: base(x, y)
		{
			_decouvert = false;
			_texture = texture;
			Resource = resource;
			Quantity = quantity;
            _positionMatrice = new Vector2(matriceX, matriceY);
		}

		/// <summary>
		/// Affiche la ressource à l'écran.
		/// </summary>
		/// <param name="spritebatch">Le SpriteBatch à utiliser pour afficher la ressource.</param>
		/// <param name="ere">L'ère courante.</param>
		/// <param name="camera">La caméra actuelle.</param>
		/// <param name="mul">La teinte de gris à utiliser pour l'affichage.</param>
		/// <param name="weather">La météo actuelle.</param>
		public void Draw(SpriteBatch spritebatch, int ere, Camera2D camera, float mul, int weather)
		{
			if (Quantity > 0)
			{
				if (weather == 1)
				{
					if (mul > 0.25f)
					{ _decouvert = true; }
					mul = (_decouvert && mul < 0.25f) ? 0.25f : mul;
				}
				spritebatch.Draw(Resource.Texture(ere), _position - camera.Position, new Color(mul, mul, mul));
			}
		}

		/// <summary>
		/// Mine la ressource.
		/// </summary>
		/// <param name="joueur">Le joueur minant la ressource.</param>
		/// <param name="unit">L'unité minant la ressource.</param>
		/// <returns>La quantité venant d'être minée.</returns>
		public int Mine(int max, Unit unit)
		{
			int qtty = Quantity >= max ? max : Quantity;
			qtty = qtty >= 1 ? 1 : qtty;
			Quantity -= qtty;
			return qtty;
		}
	}
}
