using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    class Joueur
	{
		private Texture2D pop_Texture;
		public Texture2D Pop_Texture
		{
			get { return pop_Texture; }
		}
		private uint m_population;
		public uint Population
		{
			get { return m_population; }
			set { m_population = value; }
		}
		private uint m_population_max;
		public uint Population_Max
		{
			get { return m_population_max; }
			set { m_population_max = value; }
		}
		private Color m_color, m_colorMovable;
		public Color Color
		{
			get { return m_color; }
			set { m_color = value; }
		}
		private string m_name;
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}
		private List<Movible_Sprite> m_units = new List<Movible_Sprite>();
		public List<Movible_Sprite> Units
		{
			get { return m_units; }
			set { m_units = value; }
		}
		private List<Building> m_buildings = new List<Building>();
		public List<Building> Buildings
		{
			get { return m_buildings; }
			set { m_buildings = value; }
		}
		private List<Resource> m_resources = new List<Resource>();



		public Joueur(Color couleur, string nom, ContentManager content)
		{
			m_color = couleur;
			m_colorMovable = new Color(couleur.R + ((255 - couleur.R) / 2), couleur.G + ((255 - couleur.G) / 2), couleur.B + ((255 - couleur.B) / 2));
			m_name = nom;
			m_population = 0;
			m_population_max = 5;

			m_resources.Add(new Resource("Bois", new string[] { "Bois", "Bois", "Bois", "Bois" }, 500));
			m_resources.Add(new Resource("Pierre", new string[] { "Pierre", "Pierre", "Beton", "Metonite" }, 500));
			m_resources.Add(new Resource("Nourriture", new string[] { "Nourriture", "Nourriture", "Nourriture", "Oxygene" }, 500));
			m_resources.Add(new Resource("Or", new string[] { "", "Or", "Or", "Cristaux" }));
			m_resources.Add(new Resource("Fer", new string[] { "", "Fer", "Titane", "Tritonite" }));
			m_resources.Add(new Resource("Petrole", new string[] { "", "", "Petrole", "Tritium" }));

			pop_Texture = content.Load<Texture2D>("Resources/Pop");

			foreach (Resource res in m_resources)
			{ res.Load(content); }
		}



		/// <summary>
		/// Récupère une ressource du joueur en fonction de son identifiant.
		/// </summary>
		/// <param name="name">L'identifiant de la ressource.</param>
		/// <returns>La ressource correspondant à cet identifiant.</returns>
		public Resource Resource(string name)
		{
			foreach (Resource res in m_resources)
			{
				if (res.Id == name)
				{ return res; }
			}
			return NNNA.Resource.Empty;
		}

		/// <summary>
		/// Vérifie que le joueur a entièrement les moyens de payer quelque chose.
		/// </summary>
		/// <param name="price">Le prix à vérifier.</param>
		/// <returns>Un booléen indiquant s'il peut payer.</returns>
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

		/// <summary>
		/// Fait payer au joueur un prix donné, uniquement s'il en a entièrement les moyens.
		/// </summary>
		/// <param name="price">Le prix à payer.</param>
		/// <returns>Un booléen indiquant s'il a pu payer le prix.</returns>
		public bool Pay(Dictionary<string, int> price)
		{
			if (!Has(price))
			{ return false; }

			foreach (KeyValuePair<string, int> pair in price)
			{ Resource(pair.Key).Remove(pair.Value); }
			return true;
		}

		/// <summary>
		/// Retourne la list des ressources du joueur à l'ère donnée.
		/// </summary>
		/// <param name="ere">L'ère à laquelle chercher les ressources.</param>
		/// <returns>La liste des ressources.</returns>
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

		/// <summary>
		/// Affiche les bâtiments et unités du joueur sur la carte.
		/// </summary>
		/// <param name="sb">Le SpriteBatch à utiliser pour afficher les textures.</param>
		/// <param name="camera">La caméra courante.</param>
		/// <param name="index">L'index, pour les unités.</param>
		public void Draw(SpriteBatch sb, Camera2D camera, int index)
		{
			foreach (Static_Sprite sprite in m_buildings)
			{ sprite.Draw(sb, camera, m_colorMovable); }
			foreach (Movible_Sprite sprite in m_units)
			{ sprite.Draw(sb, camera, index, m_colorMovable); }
		}
	}
}
