using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NNNA
{
	class Resource
	{
		public static Resource Empty = new Resource("", new string[] {});

		private Texture2D[] _icons;
		public Texture2D Icon(int ere)
		{ return _icons[ere - 1]; }

		private Texture2D[] _textures;
		public Texture2D Texture(int ere)
		{ return _textures[ere - 1]; }

		private string _id;
		public string Id
		{ get { return _id; } }

		private string[] _names;
		public string Name(int ere)
		{ return _names[ere - 1]; }

		private int _count;
		public int Count
		{ get { return _count; } }
		
		public void Remove(int v)
		{ _count -= v; }
		public void Add(int v)
		{ _count += v; }

		public Resource(string id, string[] names, int count = 0)
		{
			_id = id;
			_names = names;
			_count = count;
		}

		public bool IsEmpty()
		{ return _id == ""; } 

		public void Load(ContentManager content, Random rand)
		{
			_icons = new Texture2D[4];
			_textures = new Texture2D[4];
			for (int i = 1; i <= 4; i++)
			{
				if (Name(i) != "")
				{ 
					_icons[i - 1] = content.Load<Texture2D>("Resources/" + _id + "_" + i);
					if (Name(i) == "Bois") _textures[i - 1] = content.Load<Texture2D>("Resources/" + _id + "_" + 1 + "_sprite" + (rand.Next(1000) % 3));
					else _textures[i - 1] = content.Load<Texture2D>("Resources/" + _id + "_" + 1 + "_sprite");
				}
			}
		}
	}
}
