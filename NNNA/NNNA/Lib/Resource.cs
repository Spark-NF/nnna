using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace NNNA
{
	class Resource
	{
		public static Resource Empty = new Resource("", new string[] {}, 0);

		private Texture2D[] m_icons;
		public Texture2D Icon(int ere)
		{ return m_icons[ere - 1]; }

		private Texture2D[] m_textures;
		public Texture2D Texture(int ere)
		{ return m_textures[ere - 1]; }

		private string m_id;
		public string Id
		{ get { return m_id; } }

		private string[] m_names;
		public string Name(int ere)
		{ return m_names[ere - 1]; }

		private int m_count;
		public int Count
		{ get { return m_count; } }
		
		public void Remove(int v)
		{ m_count -= v; }
		public void Add(int v)
		{ m_count += v; }

		public Resource(string id, string[] names, int count = 0)
		{
			m_id = id;
			m_names = names;
			m_count = count;
		}

		public bool IsEmpty()
		{ return m_id == ""; }

		public void Load(ContentManager content)
		{
			m_icons = new Texture2D[4];
			m_textures = new Texture2D[4];
			for (int i = 1; i <= 1; i++)
			{
				if (Name(i) != "")
				{
					m_icons[i - 1] = content.Load<Texture2D>("Resources/" + m_id + "_" + i);
					m_textures[i - 1] = content.Load<Texture2D>("Resources/" + m_id + "_" + i + "_sprite");
				}
			}
		}

        public void Draw(SpriteBatch spritebatch, int ere, Camera2D  camera)
        {
            spritebatch.Draw(m_textures[ere], position - camera.Position, Color.White);
        }
	}
}
