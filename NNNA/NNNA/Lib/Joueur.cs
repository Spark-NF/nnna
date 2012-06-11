﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Joueur
	{
		private Texture2D _popTexture;
		public Texture2D PopTexture
		{
			get { return _popTexture; }
		}
		private uint _population;
		public uint Population
		{
			get { return _population; }
			set { _population = value; }
		}

        public bool Caserne { get; set; }
        public bool Ferme { get; set; }

        private int _additionalAttack;
        public int AdditionalAttack
        {
            get { return _additionalAttack; }
            set { _additionalAttack = value; }
        }
        private float _additionalSpeed;
        public float AdditionalSpeed
        {
            get { return _additionalSpeed; }
            set { _additionalSpeed = value; }
        }
        private int _additionalLife;
        public int AdditionalLife
        {
            get { return _additionalLife; }
            set { _additionalLife = value; }
        }

		private int _additionalUnitLineSight;
		public int AdditionalUnitLineSight
		{
			get { return _additionalUnitLineSight; }
			set { _additionalUnitLineSight = value; }
		}
        private int _additionalBuildingLineSight;
        public int AdditionalBuildingLineSight
        {
            get { return _additionalBuildingLineSight; }
            set { _additionalBuildingLineSight = value; }
        }

		private Random _rand = new Random();
		private uint _populationMax;
		public uint PopulationMax
		{
			get { return _populationMax; }
			set { _populationMax = value; }
		}
		private Color _color, _colorMovable;
		public Color Color
		{
			get { return _color; }
			set { _color = value; }
		}
		public Color ColorMovable
		{
			get { return _colorMovable; }
			set { _colorMovable = value; }
		}

		private int _ere;
		internal int Ere
		{
			get { return _ere; }
			set { _ere = value; }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		private List<MovibleSprite> _units = new List<MovibleSprite>();
		public List<MovibleSprite> Units
		{
			get { return _units; }
			set { _units = value; }
		}
		private List<Building> _buildings = new List<Building>();
		public List<Building> Buildings
		{
			get { return _buildings; }
			set { _buildings = value; }
		}
		public List<Resource> _resources = new List<Resource>();

		public Joueur(Color couleur, string nom, ContentManager content)
		{
			_ere = 1;
			_color = couleur;
			_colorMovable = new Color(couleur.R + ((255 - couleur.R) / 2), couleur.G + ((255 - couleur.G) / 2), couleur.B + ((255 - couleur.B) / 2));
			_name = nom;
			_population = 0;
			_populationMax = 5;
			_additionalUnitLineSight = 0;
            _additionalBuildingLineSight = 0;
		    _additionalAttack = 0;
            _additionalLife = 0;
		    Caserne = false;
            Ferme = false;

			_resources.Add(new Resource("Bois", new[] { "Bois", "Bois", "Bois", "Bois" }, 500));
			_resources.Add(new Resource("Pierre", new[] { "Pierre", "Pierre", "Beton", "Metonite" }, 500));
			_resources.Add(new Resource("Nourriture", new[] { "Nourriture", "Nourriture", "Nourriture", "Oxygene" }, 500));
			_resources.Add(new Resource("Or", new[] { "", "Or", "Or", "Cristaux" }));
			_resources.Add(new Resource("Fer", new[] { "", "Fer", "Titane", "Tritonite" }));
			_resources.Add(new Resource("Petrole", new[] { "", "", "Petrole", "Tritium" }));

			_popTexture = content.Load<Texture2D>("Resources/Pop");

			foreach (Resource res in _resources)
			{ res.Load(content, _rand); }
		}



		/// <summary>
		/// Récupère une ressource du joueur en fonction de son identifiant.
		/// </summary>
		/// <param name="id">L'identifiant de la ressource.</param>
		/// <returns>La ressource correspondant à cet identifiant.</returns>
		public Resource Resource(string id)
		{
			foreach (Resource res in _resources)
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
		{ return _resources.Where(res => res.Name(ere) != "").ToList(); }

		/// <summary>
		/// Affiche les bâtiments et unités du joueur sur la carte.
		/// </summary>
		/// <param name="sb">Le SpriteBatch à utiliser pour afficher les textures.</param>
		/// <param name="camera">La caméra courante.</param>
		/// <param name="index">L'index, pour les unités.</param>
		public void Draw(SpriteBatch sb, Camera2D camera, int index)
		{
			foreach (Building sprite in _buildings)
			{
				sprite.Visible = true;
				sprite.Draw(sb, camera, _colorMovable);
			}
			foreach (Unit sprite in _units)
			{
				sprite.Visible = true;
				sprite.Draw(sb, camera, index, _colorMovable);
			}
		}
	}
}
