using System;
using System.Collections.Generic;
using System.Linq;
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
	class Joueur
	{
		private Texture2D pop_Texture;
		public Texture2D Pop_Texture
		{
			get { return pop_Texture; }
		}
		private uint m_population = 0;
		public uint Population
		{
			get { return m_population; }
			set { m_population = value; }
		}
		private uint m_population_max = 5;
		public uint Population_Max
		{
			get { return m_population_max; }
			set { m_population_max = value; }
		}
		Color m_color;
		string m_name;

		public List<Resource> m_resources = new List<Resource>();

		public Joueur(Color couleur, string nom, ContentManager content)
		{
			m_color = couleur;
			m_name = nom;

			pop_Texture = content.Load<Texture2D>("Resources/Pop");

			Reset();
		}
		public void Reset()
		{
			m_resources.Clear();
			m_resources.Add(new Resource("Bois", new string[] { "Bois", "Bois", "Bois", "Bois" }, 1130));
			m_resources.Add(new Resource("Pierre", new string[] { "Pierre", "Pierre", "Beton", "Metonite" }));
			m_resources.Add(new Resource("Nourriture", new string[] { "Nourriture", "Nourriture", "Nourriture", "Oxygene" }, 200));
			m_resources.Add(new Resource("Or", new string[] { "", "Or", "Or", "Cristaux" }));
			m_resources.Add(new Resource("Fer", new string[] { "", "Fer", "Titane", "Tritonite" }));
			m_resources.Add(new Resource("Petrole", new string[] { "", "", "Petrole", "Tritium" }));
		}
		public void LoadResources(ContentManager content)
		{
			foreach (Resource res in m_resources)
			{ res.Load(content); }
		}

		public Resource Resource(string name)
		{
			foreach (Resource res in m_resources)
			{
				if (res.Id == name)
				{ return res; }
			}
			return NNNA.Resource.Empty;
		}

		public bool Has(Dictionary<string, int> price)
		{
			bool ok = true;
			foreach (KeyValuePair<string, int> pair in price)
			{
				if (Resource(pair.Key).Count < pair.Value)
				{ ok = false; }
			}
			return ok;
		}
		public bool Pay(Dictionary<string, int> price)
		{
			if (!Has(price))
			{ return false; }
			foreach (KeyValuePair<string, int> pair in price)
			{ Resource(pair.Key).Remove(pair.Value); }
			return true;
		}

		public List<Resource> Resources(int ere = 1)
		{
			List<Resource> r = new List<Resource>();
			foreach (Resource res in m_resources)
			{
				if (res.Name(ere) != "")
				{ r.Add(res); }
			}
			return r;
		}
	}
}
