using System;
using System.Collections.Generic;
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
		private Vector2 _dimensions;
		public Vector2 Dimensions
		{
			get { return _dimensions; }
			set { _dimensions = value; }
		}

		public Minimap(int x, int y, int width, int height)
		{ _reducedMap = new Rectangle(x + width/2, y, width*3/4, height*3/4); }
		public void LoadContent(Map map)
		{
			float ratio1 = _reducedMap.Width / (float)map.MapWidth;
			float ratio2 = _reducedMap.Height / (float)map.MapHeight;
			_ratio = ratio1 >= ratio2 ? ratio2 : ratio1;
			_reducedMap = new Rectangle(_reducedMap.X, _reducedMap.Y, (int)(map.MapWidth * _ratio), (int)(map.MapHeight * _ratio));
		}

		public void Draw(List<MovibleSprite> units, List<Building> buildings, Joueur joueur, int decay, SpriteBatch spriteBatch)
		{
			var textureColor = new Color[_texture.Width * _texture.Height];
			_baseTexture.GetData(textureColor);
			foreach (Unit unit in units)
			{
				Vector2 m = Game1.Xy2Matrice(unit.PositionCenter);
				textureColor[(int)((m.X * _texture.Width) / _dimensions.X) + _texture.Width * (int)((m.Y * _texture.Height) / _dimensions.Y)] = unit.Joueur.Color;
			}
			foreach (Building building in buildings)
			{
				Vector2 m = Game1.Xy2Matrice(building.PositionCenter);
				textureColor[(int)((m.X * _texture.Width) / _dimensions.X) + _texture.Width * (int)((m.Y * _texture.Height) / _dimensions.Y)] = building.Joueur.Color;
			}
			var texture = new Texture2D(_texture.GraphicsDevice, _texture.Width, _texture.Height);
			texture.SetData(textureColor);
			spriteBatch.Draw(texture, new Rectangle(_reducedMap.X, _reducedMap.Y + decay, _reducedMap.Width, _reducedMap.Height), null, Color.White, (float)(Math.PI / 4), Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}