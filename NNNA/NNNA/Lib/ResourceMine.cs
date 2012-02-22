using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine
	{
		Resource m_resource;

        private bool decouvert = false;

		private Vector2 m_position;
		public Vector2 Position
		{
			get { return m_position; }
			set { m_position = value; }
		}

        public Vector2 Position_Center
        {
            get { return m_position + new Vector2(m_resource.Texture(1).Width / 2, m_resource.Texture(1).Height / 2); }
        }


		public ResourceMine(int x, int y, ContentManager content, Resource resource)
        {
			m_resource = resource;
            m_position = new Vector2(x, y);
        }

		/// <summary>
		/// Affiche la ressource à l'écran.
		/// </summary>
		/// <param name="spritebatch">Le SpriteBatch à utiliser pour afficher la ressource.</param>
		/// <param name="ere">L'ère courante.</param>
		/// <param name="camera">La caméra actuelle.</param>
		/// <param name="mul">La teinte de gris à utiliser pour l'affichage.</param>
		public void Draw(SpriteBatch spritebatch, int ere, Camera2D camera, float mul)
		{
            if (mul > 0.25f) decouvert = true;
            mul = (decouvert && mul < 0.25f) ? 0.25f : mul;
            spritebatch.Draw(m_resource.Texture(ere), m_position - camera.Position, new Color(mul, mul, mul));
        }
	}
}
