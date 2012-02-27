using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine : Static_Sprite
	{
		Resource m_resource;

        private bool decouvert = false;

		public ResourceMine(int x, int y, ContentManager content, Resource resource)
            : base(x, y)
        {
            m_texture = resource.Texture(1);
			m_resource = resource;
        }

		/// <summary>
		/// Affiche la ressource à l'écran.
		/// </summary>
		/// <param name="spritebatch">Le SpriteBatch à utiliser pour afficher la ressource.</param>
		/// <param name="ere">L'ère courante.</param>
		/// <param name="camera">La caméra actuelle.</param>
		/// <param name="mul">La teinte de gris à utiliser pour l'affichage.</param>
		public void Draw(SpriteBatch spritebatch, int ere, Camera2D camera, float mul, int weather)
		{
            if (weather == 1)
            {
                if (mul > 0.25f) decouvert = true;
                mul = (decouvert && mul < 0.25f) ? 0.25f : mul;
            }
            spritebatch.Draw(m_resource.Texture(ere), m_position - camera.Position, new Color(mul, mul, mul));
        }
	}
}
