using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class ResourceMine
	{
		Resource m_resource;

		private Vector2 m_position;
		public Vector2 Position
		{
			get { return m_position; }
			set { m_position = value; }
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
        /// (SpriteBatch spriteBatch, Camera2D camera, float mul)
		public void Draw(SpriteBatch spritebatch, int ere, Camera2D camera, float mul)
		{ spritebatch.Draw(m_resource.Texture(ere), m_position - camera.Position, new Color(mul, mul, mul)); }
	}
}
