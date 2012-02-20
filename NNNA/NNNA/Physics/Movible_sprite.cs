using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Movible_Sprite : Sprite
	{
		protected Dictionary<int, Texture2D> m_textures = new Dictionary<int, Texture2D>();
		public Rectangle Rectangle(Camera2D cam)
		{
			return new Rectangle(
				(int)(Position.X - cam.Position.X),
				(int)(Position.Y - cam.Position.Y),
				Texture.Width / 4,
				Texture.Height
			);
		}

		protected Texture2D m_icon;
		public Texture2D Icon
		{
			get { return m_icon; }
			set { m_icon = value; }
		}

        private byte a = 0;

		private int dec = 90;
		protected bool create_maison;
		public bool Create_maison
		{
			get { return create_maison; }
		}
        protected bool create_hutte_chasseurs;
        public bool Create_hutte_chasseurs
        {
            get { return create_hutte_chasseurs; }
        }
		protected string type;
		public string Type
		{
			get { return type; }
		}

		private Vector2 m_positionIni;
		public Vector2 PositionIni
		{
			get { return m_positionIni; }
			set { m_positionIni = value; }
		}
		private Vector2 m_cparcourir;
		public Vector2 Cparcourir
		{
			get { return m_cparcourir; }
			set { m_cparcourir = value; }
		}
		private Vector2 m_cparcouru;
		public Vector2 Cparcouru
		{
			get { return m_cparcouru; }
			set { m_cparcouru = value; }
		}
		double m_angle = 90;
		public double Angle
		{
			get { return m_angle; }
			set { m_angle = value; }
		}
		private Vector2 m_direction;
		public Vector2 Direction
		{
			get { return m_direction; }
			set { m_direction = value; }
		}
		protected float m_speed = 0.1f;
		public float Speed
		{
			get { return m_speed; }
			set { m_speed = value; }
		}

        protected Dictionary<string, int> m_cost = new Dictionary<string, int>();
        public Dictionary<string, int> Prix
        { get { return m_cost; } }

		private bool m_click = false;
		public bool Click
		{
			get { return m_click; }
			set { m_click = value; }
		}
		private Vector2 m_clickPosition;
		public Vector2 ClickPosition
		{
			get { return m_clickPosition; }
			set { m_clickPosition = value; }
		}
        private bool m_selected = false;
		public bool Selected
		{
            get { return m_selected; }
            set { m_selected = value; }
		}

		public Movible_Sprite(int x, int y)
			: base(x, y)
		{ }
		public void ClickMouvement(Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<Movible_Sprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (m_click == true || m_selected == true)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (m_selected == true || m_click == false))
				{
                    m_click = true;
                    m_clickPosition = curseur.Position + camera.Position - new Vector2(Texture.Width / 8, (Texture.Height * 4) / 5);
                    m_angle = Math.Atan2(m_clickPosition.Y - m_position.Y, m_clickPosition.X - m_position.X);
                    m_direction = new Vector2((float)Math.Cos(m_angle), (float)Math.Sin(m_angle));
                    m_cparcourir = new Vector2(m_clickPosition.X - m_position.X, m_clickPosition.Y - m_position.Y);
                    m_cparcouru = Vector2.Zero;
                    m_positionIni = m_position;
                }
                if (m_click == true)
                {
                    if (Math.Abs(m_cparcouru.X) >= Math.Abs(m_cparcourir.X) && Math.Abs(m_cparcouru.Y) >= Math.Abs(m_cparcourir.Y))
                    {
                        m_click = false;
                    }
                    else
                    {
                        m_cparcouru = m_position - m_positionIni;
						Vector2 translation = m_direction * gameTime.ElapsedGameTime.Milliseconds * m_speed;
                        Update(translation);
						if (Collides(sprites, buildings, matrice))
						{ m_position -= translation; }
                    }
                }
            }
        }
		public void Create_Maison(Sprite curseur, List<Static_Sprite> Static_Sprite_List, ContentManager content, Joueur joueur, Camera2D camera, Random random)
		{
			if (joueur.Resource("Bois").Count >= 50)
			{
				if (create_maison == false)
				{
                    if (random.Next(0, 2) == 1)
                    {
                        curseur.Texture = content.Load<Texture2D>("Batiments/hutte1");
                        a = 0;
                    }
                    else
                    {
                        curseur.Texture = content.Load<Texture2D>("Batiments/hutte2");
                        a = 1;
                    }
					create_maison = true;
				}
				if (Souris.Get().Clicked(MouseButton.Right))
				{
					create_maison = false;
					curseur.Texture = content.Load<Texture2D>("pointer");
				}
				else if (Souris.Get().Clicked(MouseButton.Left))
				{/*
				Click = true;
				ClickPosition = curseur.Position + camera.Position;
				Angle = Math.Atan2(ClickPosition.Y - m_position.Y, ClickPosition.X - m_position.X);
				Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
				Cparcourir = new Vector2(ClickPosition.X - m_position.X, ClickPosition.Y - m_position.Y);
				Cparcouru = Vector2.Zero;
				PositionIni = m_position;
				curseur.Texture = content.Load<Texture2D>("pointer");
			}
			else if (create_maison == true && Click == false)
			{*/
					curseur.Texture = content.Load<Texture2D>("pointer");
					create_maison = false;
					Static_Sprite_List.Add(new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), content, joueur, a));
					MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
				}
			}
			else
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
				create_maison = false;
			}
		}
        public void Create_Hutte_Chasseurs(Sprite curseur, List<Static_Sprite> Static_Sprite_List, ContentManager content, Joueur joueur, Camera2D camera)
        {
            if (joueur.Resource("Bois").Count >= 75)
            {
                if (create_hutte_chasseurs == false)
                {
                    curseur.Texture = content.Load<Texture2D>("Batiments/hutte_des_chasseurs");
                    create_hutte_chasseurs = true;
                }
                if (Souris.Get().Clicked(MouseButton.Right))
                {
                    create_hutte_chasseurs = false;
                    curseur.Texture = content.Load<Texture2D>("pointer");
                }
                else if (Souris.Get().Clicked(MouseButton.Left))
                {/*
				Click = true;
				ClickPosition = curseur.Position + camera.Position;
				Angle = Math.Atan2(ClickPosition.Y - m_position.Y, ClickPosition.X - m_position.X);
				Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
				Cparcourir = new Vector2(ClickPosition.X - m_position.X, ClickPosition.Y - m_position.Y);
				Cparcouru = Vector2.Zero;
				PositionIni = m_position;
				curseur.Texture = content.Load<Texture2D>("pointer");
			}
			else if (create_maison == true && Click == false)
			{*/
                    curseur.Texture = content.Load<Texture2D>("pointer");
                    create_hutte_chasseurs = false;
                    Static_Sprite_List.Add(new Hutte_des_chasseurs((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), content, joueur));
                    MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
                }
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
                create_hutte_chasseurs = false;
            }
        }
		public void Draw(SpriteBatch spriteBatch, Camera2D camera, int index, Color col)
		{
			int tex = 0;

			//MODE 8 ANGLES
			if (dec == 45)
			{
				if (m_angle > 1 * (Math.PI / 8) && m_angle <= 3 * (Math.PI / 8))
				{ tex = 45; }
				else if (m_angle > 3 * (Math.PI / 8) && m_angle <= 5 * (Math.PI / 8))
				{ tex = 90; }
				else if (m_angle > 5 * (Math.PI / 8) && m_angle <= 7 * (Math.PI / 8))
				{ tex = 135; }
				else if (m_angle > 7 * (Math.PI / 8) || m_angle <= -7 * (Math.PI / 8))
				{ tex = 180; }
				else if (m_angle > -7 * (Math.PI / 8) && m_angle <= -5 * (Math.PI / 8))
				{ tex = 225; }
				else if (m_angle > -5 * (Math.PI / 8) && m_angle <= -3 * (Math.PI / 8))
				{ tex = 270; }
				else if (m_angle > -3 * (Math.PI / 8) && m_angle <= -1 * (Math.PI / 8))
				{ tex = 315; }
			}

			//MODE 4 ANGLES
			else
			{
				if (m_angle > 1 * (Math.PI / 4) && m_angle <= 3 * (Math.PI / 4))
				{ tex = 90; }
				else if (m_angle > 3 * (Math.PI / 4) || m_angle <= -3 * (Math.PI / 4))
				{ tex = 180; }
				else if (m_angle > -3 * (Math.PI / 4) && m_angle <= -1 * (Math.PI / 4))
				{ tex = 270; }
			}

			m_texture = m_textures[tex];
			if (Selected)
			{
				if (Click)
				{
					int distance = (int)Math.Sqrt(Math.Pow((ClickPosition.X/* + m_go.Width / 2*/) - (Position.X/* + m_texture.Width / 8*/), 2) + Math.Pow((ClickPosition.Y/* + m_go.Height / 2*/) - (Position.Y/* + (m_texture.Height * 4) / 5*/), 2));
					for (int i = 0; i < distance; i += 4)
					{ spriteBatch.Draw(m_dots, ClickPosition - camera.Position + new Vector2(m_go.Width, m_texture.Height - (m_go.Height / 2) - 1) - new Vector2((float)(i * Math.Cos(Angle)), (float)(i * Math.Sin(Angle))), Color.White); }
					spriteBatch.Draw(m_go, ClickPosition - camera.Position + new Vector2(m_go.Width / 2, m_texture.Height - (m_go.Height)), Color.White);
				}
				spriteBatch.Draw(m_texture, m_position - camera.Position, new Rectangle(m_click ? index * 32 : 0, 0, 32, 48), Color.Peru);
			}
			else
			{ spriteBatch.Draw(m_texture, m_position - camera.Position, new Rectangle(m_click ? index * 32 : 0, 0, 32, 48), col); }
		}
		public void DrawIcon(SpriteBatch spriteBatch, Vector2 position)
		{ spriteBatch.Draw(m_icon, position, new Rectangle(0, 0, m_icon.Width, m_icon.Height), Color.White); }
		public void SetTextures(ContentManager content, string name, int dec = 90)
		{
			this.dec = dec;
			for (int i = 0; i <= 315; i += dec)
			{ m_textures.Add(i, content.Load<Texture2D>("Units/" + name + "/" + name + "_" + i.ToString())); }
			m_texture = m_textures[0];
			m_go = content.Load<Texture2D>("go");
			m_dots = content.Load<Texture2D>("dots");
			m_icon = content.Load<Texture2D>("Units/" + name + "/" + name + "_icon");
		}
	}
}
