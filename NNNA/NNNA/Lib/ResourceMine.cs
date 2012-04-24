using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine : StaticSprite
	{
		private Resource _resource;

		private int _quantity;
		public int Quantity
		{ get { return _quantity; } }

		public ResourceMine(int x, int y, Resource resource, int quantity, Texture2D texture)
			: base(x, y)
		{
			_decouvert = false;
			_texture = new Image(texture);
			_resource = resource;
			_quantity = quantity;
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
			if (_quantity > 0)
			{
				if (weather == 1)
				{
					if (mul > 0.25f)
					{ _decouvert = true; }
					mul = (_decouvert && mul < 0.25f) ? 0.25f : mul;
				}
				spritebatch.Draw(_resource.Texture(ere), _position - camera.Position, new Color(mul, mul, mul));
			}
		}

		/// <summary>
		/// Mine la ressource.
		/// </summary>
		/// <param name="joueur">Le joueur minant la ressource.</param>
		/// <param name="unit">L'unité minant la ressource.</param>
		public void Mine(Joueur joueur, Unit unit)
		{
			int qtty = _quantity >= 1 ? 1 : _quantity;
			joueur.Resource(_resource.Id).Add(qtty);
			_quantity -= qtty;
		}
	}
}
