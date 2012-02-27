using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Minimap
	{
		private Rectangle _reducedMap;
		public Rectangle ReducedMap
		{
			get { return _reducedMap; }
			set { _reducedMap = value; }
		}
		private float _ratio;
		public float Ratio
		{
			get { return _ratio; }
			set { _ratio = value; }
		}
		private Texture2D _baseTexture;
		public Texture2D BaseTexture
		{
			get { return _baseTexture; }
			set { _baseTexture = value; }
		}
		private Texture2D _texture;
		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		public Minimap(int x, int y, int width, int height)
		{
			_reducedMap = new Rectangle(x + width/2, y, width*3/4, height*3/4);
		}
		public void LoadContent(Map map)
		{
			float ratio1 = _reducedMap.Width / (float)map.MapWidth;
			float ratio2 = _reducedMap.Height / (float)map.MapHeight;
			_ratio = ratio1 >= ratio2 ? ratio2 : ratio1;
			_reducedMap = new Rectangle(_reducedMap.X, _reducedMap.Y, (int)(map.MapWidth * _ratio), (int)(map.MapHeight * _ratio));
		}
		//public void Update(List<Movible_Sprite> units, List<Building> buildings, List<Unit> selectedList, Joueur joueur)
		//{
		//    Color[] texture_Color = new Color[texture.Width*texture.Height];
		//    base_texture.GetData(texture_Color);
		//    foreach (Unit unit in units)
		//    {
		//        Vector2 m = Game1.xy2matrice(unit.Position_Center);
		//        texture_Color[(int) m.X * (int) m.Y + (int) (2 * texture.Width) + (int) (4 * (m.X - 0.5))] = joueur.Color;
		//    }
		//    texture.SetData(texture_Color);
		//}
		public void Draw(int decay, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, new Rectangle(_reducedMap.X, _reducedMap.Y + decay, _reducedMap.Width, _reducedMap.Height), null, Color.White, (float)(Math.PI / 4), Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}