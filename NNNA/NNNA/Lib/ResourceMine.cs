using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine : StaticSprite
	{
		private Resource _resource;
		private bool _decouvert;

		public ResourceMine(int x, int y, Resource resource, Texture2D texture)
			: base(x, y)
		{
			_decouvert = false;
			_texture = texture;
			_resource = resource;
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
			if (weather == 1)
			{
				if (mul > 0.25f)
				{ _decouvert = true; }
				mul = (_decouvert && mul < 0.25f) ? 0.25f : mul;
			}
			spritebatch.Draw(_resource.Texture(ere), _position - camera.Position, new Color(mul, mul, mul));
		}
	}
}
