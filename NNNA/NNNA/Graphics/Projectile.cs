using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Projectile : Sprite
    {
        protected int speed;
        protected Vector2 direction, distance_restante, Texture_Center, but, last_position;
        protected double distance_ini;
        private Rectangle _rect;
        protected float reality_offset;

        #region Get / Set
        public Vector2 But
        {
            get { return but; }
            set { but = value; }
        }


        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        #endregion Get / Set

        public Projectile(ContentManager content, int x, int y, int speed, Vector2 but, string assetName)
            : base(x, y)
        {
            last_position = m_position;
            this.speed = speed;
            this.but = but;
            m_texture = content.Load<Texture2D>(assetName);
            distance_restante = but - m_position;
            distance_ini = distance_restante.Length();
            reality_offset = 0;
            Texture_Center = new Vector2(m_texture.Width / (2 * m_texture.Width), m_texture.Height / (2 * m_texture.Height));
            _rect = new Rectangle((int)m_position.X, (int)m_position.Y, m_texture.Width, m_texture.Height);
        }

        protected void Mouvement()
        {
            double angle = Math.Atan2(but.Y - m_position.Y, but.X - m_position.X);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            last_position = m_position;
            m_position += direction * speed;
            _rect.X = (int)m_position.X;
            _rect.Y = (int)m_position.Y;
            if (reality_offset < Math.PI / 2)
                m_position.Y -= reality_offset;
            else
                m_position.Y += reality_offset;
            distance_restante = but - m_position;
        }

        public new void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, m_position, Color.White);
        }

        public void Draw_rotation(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, _rect, null, Color.White, (float)(Math.Atan2(m_position.Y - last_position.Y, m_position.X - last_position.X)), Texture_Center, SpriteEffects.None, 0f);
        }

    }
}