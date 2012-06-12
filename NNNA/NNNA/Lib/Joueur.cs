using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	[Serializable]
	public abstract class Joueur
	{
		public uint Population { get; set; }
		public bool Caserne { get; set; }
        public bool Ferme { get; set; }
		public int AdditionalAttack { get; set; }
		public float AdditionalSpeed { get; set; }
		public int AdditionalLife { get; set; }
		public int AdditionalUnitLineSight { get; set; }
		public int AdditionalBuildingLineSight { get; set; }
		public uint PopulationMax { get; set; }
		public Color Color { get; set; }
		public Color ColorMovable { get; set; }
		internal int Ere { get; set; }
		public string Name { get; set; }
		public List<MovibleSprite> Units { get; set; }
		public List<Building> Buildings { get; set; }
		public List<Resource> _Resources { get; set; }
		public string Type { protected set; get; }

		protected readonly Random _rand = new Random();

		[field: NonSerialized]
		protected readonly ContentManager _content;

		[field: NonSerialized]
		private Texture2D _popTexture;
		public Texture2D PopTexture
		{
			get { return _popTexture; }
			private set { _popTexture = value; }
		}

		protected Joueur(Color couleur, string nom, ContentManager content, string type = "")
		{
			Type = type;
			_content = content;
			Buildings = new List<Building>();
			Units = new List<MovibleSprite>();
			_Resources = new List<Resource>();
			Ere = 1;
			Color = couleur;
			ColorMovable = new Color(couleur.R + ((255 - couleur.R) / 2), couleur.G + ((255 - couleur.G) / 2), couleur.B + ((255 - couleur.B) / 2));
			Name = nom;
			Population = 0;
			PopulationMax = 5;
			AdditionalUnitLineSight = 0;
            AdditionalBuildingLineSight = 0;
		    AdditionalAttack = 0;
            AdditionalLife = 0;
		    Caserne = false;
            Ferme = false;

			_Resources.Add(new Resource("Bois", new[] { "Bois", "Bois", "Bois", "Bois" }, 500));
			_Resources.Add(new Resource("Pierre", new[] { "Pierre", "Pierre", "Beton", "Metonite" }, 500));
			_Resources.Add(new Resource("Nourriture", new[] { "Nourriture", "Nourriture", "Nourriture", "Oxygene" }, 500));
			_Resources.Add(new Resource("Or", new[] { "", "Or", "Or", "Cristaux" }));
			_Resources.Add(new Resource("Fer", new[] { "", "Fer", "Titane", "Tritonite" }));
			_Resources.Add(new Resource("Petrole", new[] { "", "", "Petrole", "Tritium" }));

			PopTexture = content.Load<Texture2D>("Resources/Pop");

			foreach (Resource res in _Resources)
			{ res.Load(content, _rand); }
		}

		public abstract void Update(GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> units, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, List<Sprite> toDraw);



		/// <summary>
		/// Récupère une ressource du joueur en fonction de son identifiant.
		/// </summary>
		/// <param name="id">L'identifiant de la ressource.</param>
		/// <returns>La ressource correspondant à cet identifiant.</returns>
		public Resource Resource(string id)
		{
			foreach (Resource res in _Resources)
			{
				if (res.Id == id)
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
		{ return _Resources.Where(res => res.Name(ere) != "").ToList(); }

		/// <summary>
		/// Affiche les bâtiments et unités du joueur sur la carte.
		/// </summary>
		/// <param name="sb">Le SpriteBatch à utiliser pour afficher les textures.</param>
		/// <param name="camera">La caméra courante.</param>
		/// <param name="index">L'index, pour les unités.</param>
		public void Draw(SpriteBatch sb, Camera2D camera, int index)
		{
			foreach (Building sprite in Buildings)
			{
				sprite.Visible = true;
				sprite.Draw(sb, camera, ColorMovable);
			}
			foreach (Unit sprite in Units)
			{
				sprite.Visible = true;
				sprite.Draw(sb, camera, ColorMovable);
			}
		}
	}
}
