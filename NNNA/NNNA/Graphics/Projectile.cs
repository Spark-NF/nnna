using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    [Serializable]
    public class Projectile : Sprite
    {
        protected int _speed;
        protected Vector2 _direction, _distanceRestante, _textureCenter, _but, _lastPosition;
        protected double _distanceIni;
        public bool Touche { get; protected set; }
        protected Rectangle _rect;
        protected float _realityOffset;
        public bool Fin_parcour { get; private set; }

        #region Get / Set
        public Vector2 But
        {
            get { return _but; }
            set { _but = value; }
        }


        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        #endregion Get / Set

        public Projectile(ContentManager content, int x, int y, int speed, Vector2 but, string assetName)
            : base(x, y)
        {
            Touche = false;
            Fin_parcour = false;
            _lastPosition = _position;
            _speed = speed;
            _but = but;
            _texture = new Image(content, assetName);
            _distanceRestante = but - _position;
            _distanceIni = _distanceRestante.Length();
            _realityOffset = 0;
        	if (_texture != null)
        	{
        		_textureCenter = new Vector2(_texture.Width / (2 * _texture.Width), _texture.Height / (2 * _texture.Height));
        		_rect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        	}
        }

        protected void Mouvement()
        {
            var angle = Math.Atan2(_but.Y - _position.Y, _but.X - _position.X);
            _direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            _lastPosition = _position;
            _position += _direction * _speed;
            _rect.X = (int)_position.X;
            _rect.Y = (int)_position.Y;
            if (_realityOffset < Math.PI / 2)
            { _position.Y -= _realityOffset; }
            else
            { _position.Y += _realityOffset; }
            _distanceRestante = _but - _position;
            Fin_parcour = _realityOffset < -0.7f;
        }

        public void Draw(SpriteBatch spritebatch, Camera2D camera)
		{ _texture.Draw(spritebatch, _position - camera.Position, Color.White); }

        public void DrawRotation(SpriteBatch spritebatch, Camera2D camera)
        {
            _rect.X -= (int) camera.Position.X;
            _rect.Y -= (int)camera.Position.Y;
            spritebatch.Draw(_texture.Texture, _rect, null, Color.White, (float)(Math.Atan2(_position.Y - _lastPosition.Y, _position.X - _lastPosition.X)), _textureCenter, SpriteEffects.None, 0f);
        }
    }
}