using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Minimap
	{
		private Rectangle _reducedMap;
		private float _ratio;

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
			var ratio1 = _reducedMap.Width / (float)map.MapWidth;
			var ratio2 = _reducedMap.Height / (float)map.MapHeight;
			_ratio = ratio1 >= ratio2 ? ratio2 : ratio1;
			_reducedMap = new Rectangle(_reducedMap.X, _reducedMap.Y, (int)(map.MapWidth * _ratio), (int)(map.MapHeight * _ratio));
		}

        public void Update(Souris s, Camera2D camera, Vector2 screenSize, int decay)
        {
            //Pour les tests enlever la condition : on y voit plus clair.
            if (
                !new Rectangle(_reducedMap.X - _reducedMap.Width/2, _reducedMap.Y + decay, _reducedMap.Width,
                               _reducedMap.Height).Intersects(new Rectangle(s.X, s.Y, 1, 1))) return;
            // C'est ça qui est foireux !
            Vector2 pos = new Vector2(s.X - _reducedMap.X + _texture.Width / 2, s.Y - _reducedMap.Y + decay);
            // A partir de là, ça le fait normalement.
            Rotate(pos, Dimensions / 2, -45d);
            camera.Position = Game1.Matrice2Xy(pos) - screenSize / 2;
        }

		public void Draw(IEnumerable<MovibleSprite> units, IEnumerable<Building> buildings, Vector2 cameraCenteredPos, int decay, SpriteBatch spriteBatch)
		{
		    var textureColor = new Color[_texture.Width * _texture.Height];
			_baseTexture.GetData(textureColor);

			// Affichage des unités et des bâtiments sur la carte
			foreach (Unit unit in units)
			{
			    if (!unit.Visible) continue;
			    var m = Game1.Xy2Matrice(unit.PositionCenter);
			    textureColor[(int) ((m.X*_texture.Width)/_dimensions.X) + _texture.Width*(int) ((m.Y*_texture.Height)/_dimensions.Y)] = unit.Joueur.Color;
			}
			foreach (Building building in buildings)
			{
			    if (!building.Visible) continue;
			    var m = Game1.Xy2Matrice(building.PositionCenter);
			    textureColor[(int) ((m.X*_texture.Width)/_dimensions.X) + _texture.Width*(int) ((m.Y*_texture.Height)/_dimensions.Y)] = building.Joueur.Color;
			}

			// Position de la caméra
			var vect = Game1.Xy2Matrice(cameraCenteredPos);
            SetPixel(textureColor, vect, Color.White);
            SetPixel(textureColor, vect + new Vector2(1, 1), Color.White);
            SetPixel(textureColor, vect + new Vector2(-1, -1), Color.White);
            SetPixel(textureColor, vect + new Vector2(1, -1), Color.White);
            SetPixel(textureColor, vect + new Vector2(-1, 1), Color.White);
            SetPixel(textureColor, vect + new Vector2(2, 2), Color.White);
            SetPixel(textureColor, vect + new Vector2(-2, -2), Color.White);
            SetPixel(textureColor, vect + new Vector2(2, -2), Color.White);
            SetPixel(textureColor, vect + new Vector2(-2, 2), Color.White);
            SetPixel(textureColor, vect + new Vector2(3, 3), Color.White);
            SetPixel(textureColor, vect + new Vector2(-3, -3), Color.White);
            SetPixel(textureColor, vect + new Vector2(3, -3), Color.White);
            SetPixel(textureColor, vect + new Vector2(-3, 3), Color.White);

			// Mise à jour de la texture
			var texture = new Texture2D(_texture.GraphicsDevice, _texture.Width, _texture.Height);
			texture.SetData(textureColor);
			spriteBatch.Draw(texture, new Rectangle(_reducedMap.X, _reducedMap.Y + decay, _reducedMap.Width, _reducedMap.Height), null, Color.White, (float)(Math.PI / 4), Vector2.Zero, SpriteEffects.None, 0f);
		}

        public static Vector2 Rotate(Vector2 position, Vector2 pivot, double radians)
        {
            var offset = new Vector2(position.X - pivot.X, position.Y - pivot.Y);
            var nx = Math.Cos(radians) * offset.X - Math.Sin(radians) * offset.Y;
            var ny = Math.Sin(radians) * offset.X + Math.Cos(radians) * offset.Y;
            return new Vector2((float)nx * 1.2f + pivot.X, (float)ny * 1.2f + pivot.Y);
        }

        private void SetPixel(IList<Color> textureColor, Vector2 vect, Color color)
        {
            if (vect.X >= 0 && vect.Y >= 0 && vect.X < _dimensions.X && vect.Y < _dimensions.Y)
            { textureColor[(int)((vect.X * _texture.Width) / _dimensions.X) + _texture.Width * (int)((vect.Y * _texture.Height) / _dimensions.Y)] = color; }
        }
	}
}