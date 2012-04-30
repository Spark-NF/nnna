// #define SOUND

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if SOUND
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Media;
#endif

namespace NNNA
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		#region Variables

		public static int Frame;

		public static GraphicsDeviceManager Graphics;
		private SpriteBatch _spriteBatch;
		private Effect _gaussianBlur;
		private Rectangle _selection = Rectangle.Empty;
        private CompteurFPS fps;

		private Texture2D _backgroundDark, _flash, _console;
		private readonly Texture2D[] _backgrounds = new Texture2D[2];
		private readonly Dictionary<string, Texture2D> _actions = new Dictionary<string, Texture2D>(),  _pointers = new Dictionary<string, Texture2D>();
		private SpriteFont _fontMenu, _fontMenuTitle, _fontSmall, _fontCredits;
		private Screen _currentScreen = Screen.Title;
		private int _konami, _foes = 1;
		private string _currentAction = "", _language = "fr", _pointer = "pointer", _pointerOld = "pointer", _ipWan;
		private readonly List<string> _lastState = new List<string>();
		private string[] _currentMenus;

		private Vector2 _screenSize = new Vector2(1680, 1050);
		private bool _fullScreen = true, _shadows = true, _healthOver, _showConsole;
		public static bool SmartHud;
		private float _soundGeneral = 10, _soundSFX = 10, _soundMusic = 10, _a = 1.0f;
		private int _textures = 2, _sound = 2, _weather = 1;
		internal static bool FlashBool;
		
		private MapType _quickType = MapType.Island;
		private int _quickSize = 1, _quickResources = 1, _credits, _theme;
		private readonly List<string> _currentActions = new List<string>();
		private readonly Random _random = new Random();
        private readonly Dictionary<Color, Texture2D> _colors = new Dictionary<Color, Texture2D>();
		
		// Map
		private Sprite _h, _e, _p, _t, _s, _i, _curseur;
		private Sprite[,] _matrice;
		private Map _map;
		private Minimap _minimap;
		private HUD _hud;
		private Camera2D _camera;
		internal static Joueur Joueur;
		private Joueur[] _enemies;
		private Building _selectedBuilding;
		private float[,] _heightMap;
		private readonly List<ResourceMine> _resources = new List<ResourceMine>();
		private readonly List<Unit> _selectedList = new List<Unit>();
		private readonly List<Building> _buildings = new List<Building>();
		private readonly List<MovibleSprite> _units = new List<MovibleSprite>();
        private List<Sprite> _drawList = new List<Sprite>();
		private Vector2 _dimensions;
		private int[] _players;
		private Color[] _playersColors;

		// Audio objects		
		#if SOUND
			private const float MusicVolume = 2.0f;
			private Sons _son = new Sons();
		#endif
		// private SoundEffect _debutpartie;
        private Technologies_Window _techno;
		#endregion

		#region Enums

		public enum Screen
		{
			Title,
			Play,
			PlayCampain,
			PlayQuick,
			PlayQuick2,
			PlayMultiplayer,
            Local,
            Internet,
            OptionsReseau,
			Options,
			OptionsGraphics,
			OptionsSound,
			OptionsGeneral,
			Credits,
			Game,
			GameMenu
		};
		public enum MapType
		{
			Island,
			Flat
		}

		#endregion

        public Game1()
        {
            Window.Title = "NNNA - " + GetType().Assembly.GetName().Version;

            LoadSettings();
            _showConsole = false;

			Graphics = new GraphicsDeviceManager(this)
			{
                PreferredBackBufferWidth = (int)_screenSize.X,
                PreferredBackBufferHeight = (int)_screenSize.Y,
                IsFullScreen = _fullScreen,
				SynchronizeWithVerticalRetrace = false
			};
			IsFixedTimeStep = true;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            fps = new CompteurFPS(this);
            Components.Add(fps);

            // Dossier Utilisateur
			Components.Add(new GamerServicesComponent(this));
            
        }

		#region Settings

		/// <summary>
		/// Charge les options depuis le fichier "settings.xml".
		/// </summary>
		private void LoadSettings()
		{
			_language = Properties.Settings.Default.Language;
			_fullScreen = Properties.Settings.Default.FullScreen;
			_screenSize.X = Properties.Settings.Default.ScreenWidth;
			_screenSize.Y = Properties.Settings.Default.ScreenHeight;
			_healthOver = Properties.Settings.Default.HealthOver;
			SmartHud = Properties.Settings.Default.SmartHUD;
			_textures = Properties.Settings.Default.Textures;
			_shadows = Properties.Settings.Default.Shadows;
			_soundGeneral = Properties.Settings.Default.SoundGeneral;
			_soundMusic = Properties.Settings.Default.SoundMusic;
			_soundSFX = Properties.Settings.Default.SoundSFX;
			_sound = Properties.Settings.Default.Sound;
			_theme = Properties.Settings.Default.Theme;
		}

		/// <summary>
		/// Enregistre les options dans le fichier "settings.xml".
		/// </summary>
		private void SaveSettings()
		{
			Properties.Settings.Default.Language = _language;
			Properties.Settings.Default.FullScreen = _fullScreen;
			Properties.Settings.Default.ScreenWidth = (int)_screenSize.X;
			Properties.Settings.Default.ScreenHeight = (int)_screenSize.Y;
			Properties.Settings.Default.HealthOver = _healthOver;
			Properties.Settings.Default.SmartHUD = SmartHud;
			Properties.Settings.Default.Textures = _textures;
			Properties.Settings.Default.Shadows = _shadows;
			Properties.Settings.Default.SoundGeneral = (int)_soundGeneral;
			Properties.Settings.Default.SoundMusic = (int)_soundMusic;
			Properties.Settings.Default.SoundSFX = (int)_soundSFX;
			Properties.Settings.Default.Sound = _sound;
			Properties.Settings.Default.Theme = _theme;
			Properties.Settings.Default.Save();
		}

		#endregion

		/// <summary>
		/// Fait toutes les initialisations nécéssaires pour le jeu.
		/// </summary>
		protected override void Initialize()
		{
			_h = new Sprite('h');
			_e = new Sprite('e');
			_t = new Sprite('t');
			_p = new Sprite('p');
			_s = new Sprite('s');
			_i = new Sprite('i');

			_map = new Map();
			_camera = new Camera2D(0, 0);
			_curseur = new Sprite(0, 0);
			Joueur = new Joueur(Color.Red, "NNNA", Content);
			_players = new[] { 2, 1, 0, 0 };
			_playersColors = new[] { Color.Blue, Color.Red, Color.Green, Color.Yellow };

            // réseau
            _ipWan = Réseau.IpWan();

			base.Initialize();
		}
        /// <summary>
		/// Génère une carte aléatoire.
		/// </summary>
		/// <param name="mt">Le type de carte à générer.</param>
		/// <param name="width">La largeur de la carte à générer.</param>
		/// <param name="height">La hauteur de la carte à générer.</param>
		/// <returns>Un tableau correspondant à la carte.</returns>
		private Sprite[,] GenerateMap(MapType mt, int width, int height)
		{
			var matrice = new Sprite[width, height];

			switch (mt)
			{
				case MapType.Island:

					float[,] map = IslandGenerator.Generate(width, height);
					_heightMap = map;
					var heights = new List<float>();

					// Calcul de la hauteur de l'eau
					for (int x = 0; x < map.GetLength(0); x++)
					{
						for (int y = 0; y < map.GetLength(1); y++)
						{ heights.Add(map[x, y] * 255); }
					}
			
					heights.Sort();
					float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];
					// On transforme les hauteurs en sprites
					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							Sprite sp = _h;
							float m = map[x, y] * 255;
							if (m < waterline - 40)
							{ sp = _e; }
							else if (m < waterline)
							{ sp = _t; }
							else if (m < waterline + 15)
							{ sp = _s; }
							else if (m < waterline + 35)
							{ sp = _i; }
							matrice[x, y] = sp;
						}
					}

					// On met du sable à côté de l'eau
					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							if (matrice[x, y] == _h)
							{
								bool waternear = (x > 0 && matrice[x - 1, y] == _e) || (y > 0 && matrice[x, y - 1] == _e) || (x < matrice.GetLength(0) - 1 && matrice[x + 1, y] == _e) || (y < matrice.GetLength(1) - 1 && matrice[x, y + 1] == _e);
								matrice[x, y] = waternear ? _p : _h;
							}
						}
					}
					break;

				case MapType.Flat:
					_heightMap = new float[width, height];
					var rand = new Random();
					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							_heightMap[x, y] = (float)rand.NextDouble() / 10.0f;
							matrice[x, y] = _h;
						}
					}
					break;
			}

			return matrice;
		}

		#region Content

		/// <summary>
		/// Charge touts les contenus du jeu.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Fonds
			_flash = Content.Load<Texture2D>("flash");

			// Sprites
			_curseur.LoadContent(Content, "pointer");

			// Fontes
			_fontMenu = Content.Load<SpriteFont>("font_menu");
			_fontMenuTitle = Content.Load<SpriteFont>("font_menu_title");
			_fontSmall = Content.Load<SpriteFont>("font_small");
			_fontCredits = Content.Load<SpriteFont>("font_credits");

			#region Actions

			#region Actions Unités
			_actions.Add("attack", Content.Load<Texture2D>("Actions/attack"));
			_actions.Add("mine", Content.Load<Texture2D>("Actions/gather"));
			_actions.Add("poches", Content.Load<Texture2D>("Actions/poches"));
			_actions.Add("build", Content.Load<Texture2D>("Actions/build"));
			_actions.Add("build_hutte", Content.Load<Texture2D>("Actions/build_hutte"));
			_actions.Add("build_hutteDesChasseurs", Content.Load<Texture2D>("Actions/build_hutteDesChasseurs"));
            _actions.Add("build_tour", Content.Load<Texture2D>("Actions/build_tour"));
            _actions.Add("build_ecurie", Content.Load<Texture2D>("Actions/build_ecurie"));
            _actions.Add("build_ferme", Content.Load<Texture2D>("Actions/build_ferme"));
            _actions.Add("build_archerie",Content.Load<Texture2D>("Actions/build_archerie"));
            _actions.Add("build_forge", Content.Load<Texture2D>("Actions/build_usine"));
			// m_actions.Add("build_ferme", Content.Load<Texture2D>("Actions/build_ferme"));
			#endregion Actions Unités

			#region Actions Batiments
			_actions.Add("create_peon", Content.Load<Texture2D>("Actions/create_peon"));
			_actions.Add("technologies", Content.Load<Texture2D>("Actions/technologies"));
			_actions.Add("create_guerrier", Content.Load<Texture2D>("Actions/create_guerrier"));
            _actions.Add("create_archer", Content.Load<Texture2D>("Actions/create_archer"));
			#endregion Actions Batiments

			#region Actions Communes
			_actions.Add("retour", Content.Load<Texture2D>("Actions/retour"));
			#endregion Actions Communes

			#endregion Actions

			// Shaders
			_gaussianBlur = Content.Load<Effect>("Shaders/GaussianBlur");
			_gaussianBlur.CurrentTechnique = _gaussianBlur.Techniques["Blur"];
			LoadScreenSizeDependantContent();

            // Sons
			// _debutpartie = Content.Load<SoundEffect>("sounds/debutpartie3");
			// _debutpartie.Play();
            
		}
		/// <summary>
		/// Charge tous les contenus du jeu dépendant de la résolution de l'écran.
		/// </summary>
		protected void LoadScreenSizeDependantContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			string ratio = ((int)((10 * _screenSize.X) / _screenSize.Y)).ToString(CultureInfo.CurrentCulture);
			if (ratio != "13" && ratio != "16" && ratio != "18")
			{ ratio = "13"; }
			_backgrounds[0] = Content.Load<Texture2D>("background/" + ratio + "_0");
			_backgrounds[1] = Content.Load<Texture2D>("background/" + ratio + "_1");
			_backgroundDark = CreateRectangle((int)_screenSize.X, (int)_screenSize.Y, new Color(0, 0, 0, 170));
			_console = CreateRectangle(1, 1, new Color(0, 0, 0, 128));

			//Fenetre des technologies
            _techno = new Technologies_Window(new Rectangle((int)(_screenSize.X / 4), (int)(_screenSize.Y / 4), (int)(_screenSize.X / 2), (int)(_screenSize.Y / 2)), "Technologies", Content);

			_hud = new HUD(0, ((Graphics.PreferredBackBufferHeight * 5) / 6) - 10, SmartHud, Graphics);
			_minimap = new Minimap((_hud.Position.Width * 7) / 8 - _hud.Position.Width / 150, _hud.Position.Y + _hud.Position.Height / 15, (_hud.Position.Height * 9) / 10, (_hud.Position.Height * 9) / 10);

			MessagesManager.X = (uint)_screenSize.X - 400;
		}

		/// <summary>
		/// Décharge tous les contenus du jeu.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Unload();
		}

#endregion Content

		#region Updates

		/// <summary>
		/// Fait varier une valeur entre deux autres en fonction du clic de la souris.
		/// </summary>
		/// <param name="from">Valeur minimale.</param>
		/// <param name="to">Valeur maximale.</param>
		/// <param name="current">Valeur actuelle.</param>
		/// <returns>La valeur changée en fonction du clic de la souris.</returns>
		private int Variate(int from, int to, int current)
		{ return (current + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + to - 2 * from + 1) % (to - from + 1) + from; }

		/// <summary>
		/// Met à jour le jeu tous les 1/60 de secondes.
		/// </summary>
		/// <param name="gameTime">Temps courant.</param>
		protected override void Update(GameTime gameTime)
		{

			Frame++;

			Clavier.Get().Update(Keyboard.GetState());
			Souris.Get().Update(Mouse.GetState());

			// Console de debug
			if (Clavier.Get().NewPress(Keys.OemQuotes))
			{ _showConsole = !_showConsole; }

			// Screenshots
			if (Clavier.Get().NewPress(Keys.PrintScreen))
			{
				Draw(gameTime);

				var backBuffer = new int[(int)_screenSize.X * (int)_screenSize.Y];
				GraphicsDevice.GetBackBufferData(backBuffer);

				var texture = new Texture2D(GraphicsDevice, (int)_screenSize.X, (int)_screenSize.Y, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
				texture.SetData(backBuffer);

				Stream stream = File.OpenWrite("screenshot_" + Guid.NewGuid().ToString() + ".png");
				texture.SaveAsPng(stream, (int)_screenSize.X, (int)_screenSize.Y);
				stream.Close(); 
			}

			// Code Konami
			if (
				(Clavier.Get().NewPress(Keys.Up)	&& _konami == 0) ||
				(Clavier.Get().NewPress(Keys.Up)	&& _konami == 1) ||
				(Clavier.Get().NewPress(Keys.Down)  && _konami == 2) ||
				(Clavier.Get().NewPress(Keys.Down)  && _konami == 3) ||
				(Clavier.Get().NewPress(Keys.Left)  && _konami == 4) ||
				(Clavier.Get().NewPress(Keys.Right) && _konami == 5) ||
				(Clavier.Get().NewPress(Keys.Left)  && _konami == 6) ||
				(Clavier.Get().NewPress(Keys.Right) && _konami == 7) ||
				(Clavier.Get().NewPress(Keys.B)	 && _konami == 8) ||
				(Clavier.Get().NewPress(Keys.A)	 && _konami == 9)
			)
			{ _konami++; }
			else if (Clavier.Get().NewPress())
			{ _konami = 0; }

			switch (_currentScreen)
			{
				case Screen.Title:
					UpdateTitle();
					break;

				case Screen.Play:
					UpdatePlay();
					break;
				case Screen.PlayQuick:
					UpdatePlayQuick();
					break;
				case Screen.PlayQuick2:
					UpdatePlayQuick2();
					break;
                case Screen.PlayMultiplayer:
                    UpdateMulti();
                    break;
				case Screen.Options:
					UpdateOptions();
					break;
				case Screen.OptionsGeneral:
					UpdateOptionsGeneral();
					break;
				case Screen.OptionsGraphics:
					UpdateOptionsGraphics();
					break;
				case Screen.OptionsSound:
					UpdateOptionsSound();
					break;

				case Screen.Credits:
					UpdateCredits(gameTime);
					break;

				case Screen.Game:
					UpdateGame(gameTime);
					break;
				case Screen.GameMenu:
					UpdateGameMenu();
					break;
			}

			//Son 
			#if SOUND
				_son.EngineMenu.Update();
				if (!_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
				{ _son.Initializesons(MusicVolume, _soundMusic, _soundGeneral); }
			#endif

            // MediaPlayer
            if (Clavier.Get().NewPress(Keys.Space))
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();
            }

			base.Update(gameTime);


		}
		private void UpdateTitle()
		{
			_currentScreen = TestMenu(Screen.Play, Screen.Options, Screen.Credits, Screen.OptionsSound);
			if (_currentScreen == Screen.Credits)
			{ _credits = 0; }
			if (_currentScreen == Screen.OptionsSound)
			{ Exit(); }
		}
		private void UpdatePlay()
        { _currentScreen = TestMenu(Screen.PlayQuick, Screen.PlayMultiplayer, Screen.Title); }
        private void UpdateMultiplayer()
		{ _currentScreen = TestMenu(Screen.PlayMultiplayer, Screen.PlayMultiplayer, Screen.PlayMultiplayer, Screen.Play); }
		private void UpdatePlayQuick()
		{
			Screen s = TestMenu(Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick2, Screen.Play);
			if (s != Screen.PlayQuick)
			{ _currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				switch (m)
				{
					case 0: _quickType = (MapType)Variate(0, Enum.GetValues(typeof(MapType)).Length - 1, (int)_quickType); break;
					case 1: _quickSize = Variate(0, 2, _quickSize); break;
					case 2: _quickResources = Variate(0, 2, _quickResources); break;
					case 3: _weather = Variate(0, 2, _weather); break;
				}
			}
		}
		private void UpdatePlayQuick2()
		{
			Screen s = TestMenu(Screen.PlayQuick2, Screen.PlayQuick2, Screen.PlayQuick2, Screen.PlayQuick2, Screen.Game, Screen.PlayQuick);
			if (s != Screen.PlayQuick2)
			{
				if (s == Screen.Game)
				{
					bool ok = false;
					int[] sizes = { 50, 100, 200 };
					var spawns = new List<Point>();
					var heights = new List<float>();
					var dist = (float)Math.Round((double)sizes[_quickSize] / 2);

					_buildings.Clear();
					_units.Clear();

					// Les couleurs, noms et compte des joueurs
					var colors = new List<Color> { _playersColors[0] };
					string[] names = { Environment.UserName, "Lord Lard", "Herr von Speck", "Monsieur Martin" };
					//int users = _players.Count(t => t > 0);
					_foes = 0;
					for (int i = 1; i < _players.Length; i++)
					{
						if (_players[i] > 0)
						{
							colors.Add(_playersColors[i]);
							_foes++;
						}
					}

					// On regénère une carte tant qu'elle est incapable d'accueillir le bon nombre de spawns
					while (!ok)
					{
						// Génération
						_matrice = GenerateMap(_quickType, sizes[_quickSize], sizes[_quickSize]);
						_minimap.Dimensions = new Vector2(sizes[_quickSize], sizes[_quickSize]);

						// Spawns
						heights.Clear();
						for (int x = 0; x < _heightMap.GetLength(0); x++)
						{
							for (int y = 0; y < _heightMap.GetLength(1); y++)
							{ heights.Add(_heightMap[x, y] * 255); }
						}
						float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];
						int last = heights.IndexOf(waterline) > 0 ? heights.IndexOf(waterline) : heights.Count;
						var heightsOr = new List<float>(heights);
						heights.Sort((x, y) => (y.CompareTo(x)));
						spawns.Clear();
						int j = 0;

						// On génère autant de spawns qu'il y aura de joueurs, chacun espacés d'au moins $dist
						while (spawns.Count < _foes + 1 && j < last)
						{
							int index = heightsOr.IndexOf(heights[j]);
							heightsOr[index] = -1;
							var point = new Point(index % _heightMap.GetLength(1), index / _heightMap.GetLength(1));
							bool isNear = false;
							foreach (Point p in spawns)
							{
								if (point.DistanceTo(p) <= dist)
								{ isNear = true; }
							}
							if (!isNear)
							{ spawns.Add(point); }
							j++;
						}

						if (spawns.Count == _foes + 1)
						{ ok = true; }
					}

					/*
					 * envoie de la map si partie multi
					 */

					//Joueur
					Joueur = new Joueur(colors[0], names[0], Content);
					var hutte = new GrandeHutte((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y, Content, Joueur);
					Joueur.Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 100, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, Joueur, false));
					Joueur.Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 0, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, Joueur, false));
					Joueur.Units.Add(new Peon((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 50, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, Joueur, hutte, false));
					Joueur.Buildings.Add(hutte);
					_camera.Position = Matrice2Xy(new Vector2(spawns[0].X + 7, spawns[0].Y + 5)) - _screenSize / 2;
					_units.AddRange(Joueur.Units);
					_buildings.AddRange(Joueur.Buildings);

					// Ennemis
					_enemies = new Joueur[_foes];
					for (int i = 0; i < _foes; i++)
					{
						_enemies[i] = new Joueur(colors[i + 1], names[i + 1], Content);
						hutte = new GrandeHutte((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y, Content, _enemies[i]);
						_enemies[i].Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 100, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _enemies[i], false));
						_enemies[i].Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 0, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _enemies[i], false));
						_enemies[i].Units.Add(new Peon((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 50, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _enemies[i], hutte, false));
						_enemies[i].Buildings.Add(hutte);
						_units.AddRange(_enemies[i].Units);
						_buildings.AddRange(_enemies[i].Buildings);
					}

					//Le reste
					_techno.Reset();
					_map.LoadContent(_matrice, Content, _minimap, Graphics.GraphicsDevice);
					_hud.LoadContent(Content, "HUD/hud2");
					_minimap.LoadContent(_map);

					//Decor
					_resources.Clear();
					for (int i = 0; i < 5 * (_quickResources + _quickSize + 1); i++)
					{
						int x = _random.Next(_matrice.GetLength(0));
						int y = _random.Next(_matrice.GetLength(1));
						while ((!_matrice[y, x].Crossable))
						{
							x = _random.Next(_matrice.GetLength(0));
							y = _random.Next(_matrice.GetLength(1));
						}
						_resources.Add(new ResourceMine((int)(Matrice2Xy(new Vector2(x, y))).X - 44, (int)(Matrice2Xy(new Vector2(x, y))).Y - 152, Joueur.Resource("Bois"), 1000, new Image(Content, "Resources/bois_1_sprite" + _random.Next(0, 3))));
					}
                    _resources.Sort(Sprite.CompareByY);

					//Le son
					#if SOUND
						if (_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
						{ _son.MusiqueMenu.Pause(); }
						//_debutpartie.Play();
					#endif
				}
				_currentScreen = s;
			}
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = MenuFoes();
				switch (m)
				{
					case 0:
					case 1:
					case 2:
					case 3:
						var colors = new List<Color>(new[] { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Gray, Color.DeepPink, Color.Lime, Color.DarkOrange, Color.SaddleBrown, Color.Cyan });
						float v = ((Souris.Get().Y - (_screenSize.Y / 5) - (180 * (_screenSize.Y / 1050))) % (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0))));
						if (Souris.Get().X >= _screenSize.X * 3 / 7 - 60 && Souris.Get().X <= _screenSize.X * 3 / 7 - 20 && Souris.Get().Y > (_screenSize.Y / 5) + (180 * (_screenSize.Y / 1050)) &&  v >= 21 && v <= 61)
						{ _playersColors[m] = colors[(colors.IndexOf(_playersColors[m]) + (Souris.Get().Clicked(MouseButton.Left) ? 1 : -1) + colors.Count) % colors.Count]; }
						else
						{ _players[m] = Variate(0, 2, _players[m]); }
						break;
				}
			}
		}
		private void UpdateOptions()
		{
			_currentScreen = TestMenu(Screen.OptionsGeneral, Screen.OptionsGraphics, Screen.OptionsSound, Screen.Title);
			if (_currentScreen == Screen.Title)
			{ SaveSettings(); }
		}
		private void UpdateOptionsGeneral()
		{
			Screen s = TestMenu(Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.Options);
			if (s != Screen.OptionsGeneral)
			{ _currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				switch (m)
				{
					case 0: _language = (_language == "en" ? "es" : (_language == "es" ? "fr" : "en")); break;
					case 1: _healthOver = !_healthOver; break;

					case 2:
						SmartHud = !SmartHud;
						_hud = new HUD(0, ((Graphics.PreferredBackBufferHeight * 5) / 6) - 10, SmartHud, Graphics);
						break;
				}
			}
		}
		private void UpdateOptionsGraphics()
		{
			Screen s = TestMenu(Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.Options);
			if (s != Screen.OptionsGraphics)
			{ _currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				switch (m)
				{
					case 0:
						_screenSize = GetNextResolution(Souris.Get().Clicked(MouseButton.Right));
						Graphics.PreferredBackBufferWidth = (int)_screenSize.X;
						Graphics.PreferredBackBufferHeight = (int)_screenSize.Y;
						LoadScreenSizeDependantContent();
						Graphics.ApplyChanges();
						break;

					case 1:
						_fullScreen = !_fullScreen;
						Graphics.IsFullScreen = _fullScreen;
						Graphics.ApplyChanges();
						break;

					case 2: _textures = Variate(0, 2, _textures); break;
					case 3: _shadows = !_shadows; break;
					case 4: _theme = Variate(0, 1, _theme); break;
				}
			}
		}
		private void UpdateOptionsSound()
		{
			Screen s = TestMenu(Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.Options);
			if (s != Screen.OptionsSound)
			{ _currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				switch (m)
				{
					case 0: _soundGeneral = Variate(0, 10, (int)_soundGeneral); break;
					case 1: _soundMusic = Variate(0, 10, (int)_soundMusic); break;
					case 2: _soundSFX = Variate(0, 10, (int)_soundSFX); break;
					case 3: _sound = Variate(0, 2, _sound); break;
				}
			}
			#if SOUND
				_son.MusicCategory.SetVolume(MusicVolume * _soundMusic * (_soundGeneral / 10));
			#endif
		}
		private void UpdateCredits(GameTime gameTime)
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ _currentScreen = Screen.Title; }
			_credits += (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl) ? 10 : 1) * gameTime.ElapsedGameTime.Milliseconds;
		}
		float _compt;

		/// <summary>
		/// Met à jour la liste des actions des unités. C'est ici que doivent être mises toutes les actions du menu de gauche dans le HUD.
		/// </summary>
		public void UpdateActions()
		{
			_currentActions.Clear();
			if (_selectedList.Count > 0)
			{
				bool allSame = true;
				string type = "";
				_currentActions.Add("attack");
				foreach (Unit sprite in _selectedList)
				{
					if (sprite.Type != type)
					{
						if (type != "")
						{ allSame = false; }
						type = sprite.Type;
						if (type == "peon" && !_currentActions.Contains("build"))
						{
							_currentActions.Add("mine");
							_currentActions.Add("build");
							_currentActions.Add("poches");
						}
					}
				}
				if (!allSame)
				{
					_currentActions.Clear();
					_currentActions.Add("attack");
				}
			}
			else if (_selectedBuilding != null)
			{
				switch (_selectedBuilding.Type)
				{
					case "forum":
						_currentActions.Add("create_peon");
						_currentActions.Add("technologies");
						break;

					case "caserne":
						_currentActions.Add("create_guerrier");
                        if (Joueur.Ere > 1)
                            _currentActions.Add("create_archer");
						break;
				}
			}
			_lastState.Clear();
			foreach (string t in _currentActions)
			{ _lastState.Add(t); }
		}

		private void UpdateGame(GameTime gameTime)
		{
            _techno.Update(Souris.Get());

			if (Clavier.Get().NewPress(Keys.Escape))
			{
				_pointer = "pointer";
				if (_currentAction.StartsWith("build_"))
				{ _currentAction = ""; }
				else if (_techno.Win_Visible)
				{ _techno.Win_Visible = false; }
				else
				{
					_pointerOld = _pointer;
					_currentScreen = Screen.GameMenu;
					return;
				}
			}
            if ((Clavier.Get().Pressed(Keys.LeftControl) || Clavier.Get().Pressed(Keys.RightControl)) && Clavier.Get().Pressed(Keys.T))
            { _techno.Win_Visible = true; }

			_compt = (_compt + gameTime.ElapsedGameTime.Milliseconds * 0.1f) % 100;
			_curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			_camera.Update(_curseur, Graphics);

			// On vire les ressources vides
			_resources.RemoveAll(res => res.Quantity <= 0);

			// Intelligence artificielle
			var rand = new Random();
			foreach (Joueur foe in _enemies)
			{
				for (int i = 0; i < foe.Units.Count; i++)
				{
					var unit = (Unit)foe.Units[i];
					if (unit.Life <= 0)
					{
						foe.Units.Remove(unit);
						_units.Remove(unit);
					}
					else
					{
						if (++unit.Updates == 120)
						{
							unit.Move(unit.Position + new Vector2(rand.Next(-40, 41), rand.Next(-40, 41)), _units, _buildings, _matrice);
							unit.Updates = rand.Next(0, 40);
						}
						unit.ClickMouvement(_curseur, gameTime, _camera, _hud, _units, _buildings, _resources, _matrice, Content);
					}
				}
			}

			// Rectangle de séléction
            if (!_techno.Win_Visible)
            {
                if (Souris.Get().Clicked(MouseButton.Left))
                {
                    Building b;
                    Vector2 pos;
                    switch (_currentAction)
                    {
                        // Ere 1 
                        case "build_hutte":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Hutte((int)position.X, (int)position.Y, Content, Joueur, (byte)_random.Next(0, 2));
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle hutte !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;

                        case "build_hutteDesChasseurs":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new HutteDesChasseurs((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle hutte des chasseurs !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;

                        case "build_tour":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Tour((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle tour !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;
                        case "build_ecurie":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Ecurie((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle écurie !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;
                        case "build_ferme":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Ferme((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle ferme !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;
                        case "build_archerie":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Archerie((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle archerie !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;
                        case "build_forge":
                            pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
                            if (ValidSpawn(pos, _dimensions))
                            {
                                var position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16);
                                b = new Forge((int)position.X, (int)position.Y, Content, Joueur);
                                if (Joueur.Pay(b.Prix))
                                {
                                    _selectedList[0].Build(b);
                                    _buildings.Add(b);
                                    MessagesManager.Messages.Add(new Msg(_("Nouvelle Forge !"), Color.White, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                                else
                                {
                                    MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                    _pointer = "pointer";
                                    _currentAction = "";
                                }
                            }
                            else
                            { MessagesManager.Messages.Add(new Msg(_("Vous ne pouvez pas construire ici."), Color.Red, 5000)); }
                            break;

                        // Fin Ere 1 

                        /* Ere 2 
                        case "build_ferme":
                            b = new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, _joueur);
                            if (joueur.Pay(b.Prix))
                            {
                                joueur.buildings.Add(b);
                                MessagesManager.Messages.Add(new Msg(_("Nouvelle ferme !"), Color.White, 5000));
                                m_pointer = Content.Load<Texture2D>("pointer");
                                isbuilding = false;
                                m_currentAction = "";
                            }
                            else
                            {
                                MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000));
                                m_pointer = Content.Load<Texture2D>("pointer");
                                m_currentAction = "";
                            }
                            break;
                        Fin Ere 2 */

                        default:
                            _selection = new Rectangle(Souris.Get().X + (int)_camera.Position.X, Souris.Get().Y + (int)_camera.Position.Y, 0, 0);
                            _currentAction = "select";
                            break;
                    }
                }
                else if (Souris.Get().Hold(MouseButton.Left) && _currentAction == "select")
                {
                    _selection.Width = Souris.Get().X + (int)_camera.Position.X - _selection.X;
                    _selection.Height = Souris.Get().Y + (int)_camera.Position.Y - _selection.Y;
                }
                else if (Souris.Get().Hold(MouseButton.Left, ButtonState.Released))
                { _selection = Rectangle.Empty; }

                if (!_selection.IsEmpty && Souris.Get().Released(MouseButton.Left) && (_curseur.Position.Y <= _hud.Position.Y + 20 || _selection.Y - _camera.Position.Y <= _hud.Position.Y + 20))
                {
                    // On met à jour les séléctions
                    bool change = false;
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !Keyboard.GetState().IsKeyDown(Keys.RightControl))
                    {
                        foreach (Unit sprite in _selectedList)
                        { sprite.Selected = false; }
                        _selectedList.Clear();
                        if (_selectedBuilding != null)
                        { _selectedBuilding.Selected = false; }
                        _selectedBuilding = null;
                    }
                    foreach (Unit sprite in Joueur.Units)
                    {
                        var csel = new Rectangle((int)(_selection.X - _camera.Position.X + (_selection.Width < 0 ? _selection.Width : 0)), (int)(_selection.Y - _camera.Position.Y + (_selection.Height < 0 ? _selection.Height : 0)), Math.Abs(_selection.Width), Math.Abs(_selection.Height));
                        if (!sprite.Selected && csel.Intersects(sprite.Rectangle(_camera)))
                        {
                            sprite.Selected = true;
                            _selectedList.Add(sprite);
                            change = true;
                        }
                    }
                    if (!change)
                    {
                        foreach (Unit sprite in _selectedList)
                        { sprite.Selected = false; }
                        _selectedList.Clear();

                        foreach (Building sprite in Joueur.Buildings)
                        {
                            var csel = new Rectangle((int)(_selection.X - _camera.Position.X + (_selection.Width < 0 ? _selection.Width : 0)), (int)(_selection.Y - _camera.Position.Y + (_selection.Height < 0 ? _selection.Height : 0)), Math.Abs(_selection.Width), Math.Abs(_selection.Height));
                            if (!sprite.Selected && csel.Intersects(sprite.Rectangle(_camera)))
                            {
                                sprite.Selected = true;
                                _selectedBuilding = sprite;
                                change = true;
                                break;
                            }
                        }
                   

                        if (!change)
                        {
                            if (_selectedBuilding != null)
                            { _selectedBuilding.Selected = false; }
                            _selectedBuilding = null;
                        }
                    }

                    // On met à jour les actions
                    UpdateActions();
                    _currentAction = "";
                }

                // Actions
                if (Souris.Get().Clicked(MouseButton.Left) && Souris.Get().X >= _hud.Position.X + 20 && Souris.Get().Y >= _hud.Position.Y + 20 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
                {
                    int x = Souris.Get().X - _hud.Position.X - 20, y = Souris.Get().Y - _hud.Position.Y - 20;
                    if (x % 40 < 32 && y % 40 < 32 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
                    {
                        x /= 40;
                        y /= 40;
                        int pos = x + 6 * y;
                        if (pos < _currentActions.Count || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
                        {
                            string act;
                            if (Clavier.Get().NewPress(Keys.C))
                            { act = "build_hutte"; }
                            else if (Clavier.Get().NewPress(Keys.V))
                            { act = "build_hutteDesChasseurs"; }
                            else
                            { act = _currentActions[pos]; }
                            Debug(act);
                            switch (act)
                            {
                                case "attack":
                                    break;

                                case "build":
                                    _currentActions.Clear();
                                    _currentActions.Add("build_hutte");
                                    _currentActions.Add("build_hutteDesChasseurs");
                                    if (Joueur.Ere > 1)
                                    {
                                        _currentActions.Add("build_archerie");
                                        _currentActions.Add("build_tour");
                                        _currentActions.Add("build_ferme");
                                        _currentActions.Add("build_ecurie");

                                    }
                                    _currentActions.Add("retour");
                                    break;

                                case "mine":
                                    break;

                                case "poches":
                                    break;

                                case "build_hutte":
                                    _pointer = "Batiments/maison" + _random.Next(1, 3).ToString(CultureInfo.CurrentCulture) + "_" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_hutte";
                                    _dimensions = new Vector2(4, 4);
                                    break;

                                case "build_hutteDesChasseurs":
                                    _pointer = "Batiments/caserne_" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_hutteDesChasseurs";
                                    _dimensions = new Vector2(2, 2);
                                    break;
                                case "build_archerie":
                                    _pointer = "Batiments/archerie" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_archerie";
                                    _dimensions = new Vector2(2, 2);
                                    break;

                                case "build_tour":
                                    _pointer = "Batiments/tour" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_tour";
                                    _dimensions = new Vector2(2, 2);
                                    break;

                                case "build_ferme":
                                    _pointer = "Batiments/ferme" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_ferme";
                                    _dimensions = new Vector2(2, 2);
                                    break;

                                case "build_ecurie":
                                    _pointer = "Batiments/ecurie" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_ecurie";
                                    _dimensions = new Vector2(2, 2);
                                    break;

                                case "build_forge": 
                                    _pointer = "Batiments/forge" + Joueur.Ere.ToString(CultureInfo.CurrentCulture) + "_c";
                                    _currentAction = "build_forge";
                                    _dimensions = new Vector2(2, 2);
                                    break;


                                case "retour":
                                    _currentActions.Clear();
                                    foreach (string t in _lastState)
                                    { _currentActions.Add(t); }
                                    break;

                                case "create_peon":
                                    if (_selectedBuilding != null)
                                    {
                                        var u = new Peon((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 5), (int)_selectedBuilding.Position.Y + 155, Content, Joueur, _selectedBuilding, false);
                                        if (Joueur.Pay(u.Prix))
                                        {
                                            _selectedBuilding.Iterator++;
                                            Joueur.Units.Add(u);
                                            _units.Add(u);
                                            MessagesManager.Messages.Add(new Msg(_("Nouveau peon !"), Color.White, 5000));
                                            _currentAction = "";
                                        }
                                        else
                                        { MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000)); }
                                    }
                                    break;

                                case "technologies":
                                    _techno.Win_Visible = true;
                                    break;

                                case "create_guerrier":
                                    if (_selectedBuilding != null)
                                    {
                                        var u1 = new Guerrier((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 3), (int)_selectedBuilding.Position.Y + 70, Content, Joueur, false);
                                        if (Joueur.Has(u1.Prix))
                                        {
                                            _selectedBuilding.Iterator++;
                                            Joueur.Units.Add(u1);
                                            _units.Add(u1);
                                            MessagesManager.Messages.Add(new Msg(_("Nouveau chasseur !"), Color.White, 5000));
                                            _currentAction = "";
                                        }
                                        else
                                        { MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000)); }
                                    }
                                    break;
                                case "create_archer":
                                    if (_selectedBuilding != null)
                                    {
                                        var u1 = new Archer((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 3), (int)_selectedBuilding.Position.Y + 70, Content, Joueur, false);
                                        if (Joueur.Has(u1.Prix))
                                        {
                                            _selectedBuilding.Iterator++;
                                            Joueur.Units.Add(u1);
                                            _units.Add(u1);
                                            MessagesManager.Messages.Add(new Msg(_("Nouvel archer !"), Color.White, 5000));
                                            _currentAction = "";
                                        }
                                        else
                                        { MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000)); }
                                    }
                                    break;

                                /* Ere 2 
                            case "build_ferme":
                                if (joueur.Has(new Ferme().Prix))
                                {
                                    m_pointer = "Batiments/ferme";
                                    isbuilding = true;
                                    m_currentAction = "build_ferme";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg(_("Vous n'avez pas assez de ressources."), Color.Red, 5000)); }
                                break;*/
                            }
                        }
                    }
                }
            }
			foreach (Unit sprite in Joueur.Units)
			{ sprite.ClickMouvement(_curseur, gameTime, _camera, _hud, _units, _buildings, _resources, _matrice, Content); }

			// Curseur de combat
			Unit unitUnder = null;
            if (!_techno.Win_Visible)
            {
                if (_selectedList.Count > 0)
                {
                    foreach (Joueur foe in _enemies)
                    {
                        foreach (Unit unit in foe.Units)
                        {
                            // Si une unité ennemie se trouve sous le curseur
                            if (unit.Rectangle(_camera).Intersects(new Rectangle(Souris.Get().X, Souris.Get().Y, 1, 1)))
                            {
                                float mul = 0.0f;
                                if (_weather > 0)
                                {
                                    foreach (Unit uni in Joueur.Units)
                                    {
                                        float m = (uni.PositionCenter - unit.Position).Length();
                                        m = 1.0f - (m / (uni.LineSight + Joueur.AdditionalLineSight));
                                        mul = (m > 0 && m > mul) ? m : mul;
                                    }
                                    foreach (Building building in Joueur.Buildings)
                                    {
                                        float m = (building.PositionCenter - unit.Position).Length();
                                        m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
                                        mul = (m > 0 && m > mul) ? m : mul;
                                    }
                                }
                                else
                                { mul = 1.0f; }
                                if (mul > 0.25f)
                                {
                                    _pointer = "fight";
                                    unitUnder = unit;
                                }
                            }
                        }
                    }
                }
                if (_pointer == "fight" && unitUnder == null)
                { _pointer = "pointer"; }

                // Curseur de minage
                ResourceMine resourceUnder = null;
                if (_selectedList.Count > 0 && (_currentActions.Contains("mine") || _currentActions.Contains("retour")))
                {
                    foreach (ResourceMine resource in _resources)
                    {
                        // Si une unité ennemie se trouve sous le curseur
                        if (resource.Rectangle(_camera).Intersects(new Rectangle(Souris.Get().X, Souris.Get().Y, 1, 1)))
                        {
                            float mul = 0.0f;
                            if (_weather > 0)
                            {
                                foreach (Unit uni in Joueur.Units)
                                {
                                    float m = (uni.PositionCenter - resource.Position).Length();
                                    m = 1.0f - (m / (uni.LineSight + Joueur.AdditionalLineSight));
                                    mul = (m > 0 && m > mul) ? m : mul;
                                }
                                foreach (Building building in Joueur.Buildings)
                                {
                                    float m = (building.PositionCenter - resource.Position).Length();
                                    m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
                                    mul = (m > 0 && m > mul) ? m : mul;
                                }
                            }
                            else
                            { mul = 1.0f; }
                            if (mul > 0.25f)
                            {
                                _pointer = "mine";
                                resourceUnder = resource;
                            }
                        }
                    }
                }
                if (_pointer == "mine" && resourceUnder == null)
                { _pointer = "pointer"; }

                // Vidage de poches
                Building buildingUnder = null;
                if (_selectedList.Count > 0 && (_currentActions.Contains("poches") || _currentActions.Contains("retour")))
                {
                    foreach (Unit unit in _selectedList)
                    {
                        // Si le bâtiment affilié se trouve sous le curseur
                        if (unit.Poches > 0 && unit.Affiliate.Rectangle(_camera).Intersects(new Rectangle(Souris.Get().X, Souris.Get().Y, 1, 1)))
                        {
                            _pointer = "poches";
                            buildingUnder = unit.Affiliate;
                            // Si l'utilisateur a fait un clic droit, on lance la commande de vidage de poches
                            if (Souris.Get().Clicked(MouseButton.Right))
                            {
                                unit.Will = "poches";
                                unit.DestinationBuilding = unit.Affiliate;
                                unit.Move(unit.DestinationBuilding.Position + new Vector2((float)Math.Round((double)unit.DestinationBuilding.Texture.Width / 2), 0)); //, sprites, buildings, matrice);
                            }
                        }
                    }
                }
                if (_pointer == "poches" && buildingUnder == null)
                { _pointer = "pointer"; }

                // Combat
                if (unitUnder != null && Souris.Get().Clicked(MouseButton.Right))
                {
                    foreach (Unit unit in _selectedList)
                    { unit.Attack(unitUnder); }
                }
                // Minage
                else if (resourceUnder != null && Souris.Get().Clicked(MouseButton.Right))
                {
                    foreach (Unit unit in _selectedList)
                    { unit.Mine(resourceUnder); }
                }
            }
			Joueur.Units.Sort(Sprite.CompareByY);
			Joueur.Buildings.Sort(Sprite.CompareByY);
			 
			//minimap.Update(units, buildings, selectedList, joueur);

			#if SOUND
				if (_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
				{ _son.MusiqueMenu.Pause(); }
			#endif
		}

		void UpdateGameMenu()
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ _currentScreen = Screen.Game; }
			_curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			_currentScreen = TestPauseMenu(Screen.Title, Screen.Game);
			if (_currentScreen == Screen.Game)
			{ _pointer = _pointerOld; }

			#if SOUND
				if (!_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
				{_son.Initializesons(MusicVolume, _soundMusic, _soundGeneral); }
				_son.MusiqueMenu.Resume(); 
			#endif
		}

		#endregion

		#region Draws

		/// <summary>
		/// Affichage de tous les éléments du jeu.
		/// </summary>
		/// <param name="gameTime">Temps courant.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin();

			switch (_currentScreen)
			{
				case Screen.Title:
					DrawTitle();
					break;

				case Screen.Play:
					DrawPlay();
					break;
				case Screen.PlayQuick:
					DrawPlayQuick();
					break;
				case Screen.PlayQuick2:
					DrawPlayQuick2();
					break;

				case Screen.Options:
					DrawOptions();
					break;
				case Screen.OptionsGeneral:
					DrawOptionsGeneral();
					break;
				case Screen.OptionsGraphics:
					DrawOptionsGraphics();
					break;
				case Screen.OptionsSound:
					DrawOptionsSound();
					break;

				case Screen.Credits:
					DrawCredits();
					break;

				case Screen.Game:
					DrawGame();
					break;
				case Screen.GameMenu:
					DrawGameMenu();
					break;
			}

			// Code Konami
			if (_konami >= 10)
			{
				Joueur.Resource("Bois").Add(10000);
				Joueur.Resource("Pierre").Add(10000);
				Joueur.Resource("Nourriture").Add(10000);
                Joueur.Resource("Fer").Add(10000);
                Joueur.Resource("Or").Add(10000);
				_konami = 0;
			}

			if (_showConsole)
			{
				List<ConsoleMessage> msgs = Console.GetLast();
				_spriteBatch.Draw(_console, new Rectangle(0, 0, (int)_screenSize.X, 26 * msgs.Count), Color.White);
				for (int i = msgs.Count; i > 0; i--)
				{ _spriteBatch.DrawString(_fontSmall, msgs[msgs.Count - i].Message, new Vector2(5, 26 * i - 24), Color.White); }
			}

			DrawPointer();
			
			_spriteBatch.End();
			base.Draw(gameTime);
		}

		private void DrawCommon(bool drawText = true)
		{
			// Le fond d'écran
			var screenRectangle = new Rectangle(0, 0, (int)_screenSize.X, (int)_screenSize.Y);
			_spriteBatch.Draw(_backgrounds[_theme], screenRectangle, Color.White);

			if (drawText)
			{
				// Titre
				DrawString(_spriteBatch, _fontMenuTitle, "NNNA", new Vector2((_screenSize.X - _fontMenuTitle.MeasureString("NNNA").X) / 2, _screenSize.Y / 13), new Color(200, 0, 0), Color.Black, 1);

				// Version
				_spriteBatch.DrawString(_fontSmall, GetType().Assembly.GetName().Version.ToString(), new Vector2((_screenSize.X - _fontSmall.MeasureString(GetType().Assembly.GetName().Version.ToString()).X) / 2, _screenSize.Y - 50), Color.GhostWhite);
			   
				// réseau
				DrawString(_spriteBatch, _fontSmall, _(Réseau.Connected()), new Vector2(5, _screenSize.Y -20),Color.GhostWhite,Color.Transparent,1);
				DrawString(_spriteBatch, _fontSmall, _("Votre adresse IP est :") + " " + _ipWan, new Vector2((_screenSize.X - _fontSmall.MeasureString(_("Votre adresse IP est :") + " " + _ipWan).X), _screenSize.Y - 20), Color.GhostWhite, Color.Transparent, 1);
			}

		}

		/// <summary>
		/// Affiche le pointeur actuel à l'écran, et le charge en mémoire si ce n'est pas déjà fait.
		/// </summary>
		private void DrawPointer()
		{
			if (!_pointers.ContainsKey(_pointer))
			{ _pointers.Add(_pointer, Content.Load<Texture2D>(_pointer)); }

			Color color = Color.White;
			var position = new Vector2(Souris.Get().X, Souris.Get().Y);

			if (_currentAction.StartsWith("build_"))
			{
				Vector2 pos = Xy2Matrice(Souris.Get().Position + _camera.Position + new Vector2(0, _dimensions.Y * 16));
				position = new Vector2((pos.X - pos.Y) * 32, (pos.X + pos.Y - _dimensions.Y) * 16) - _camera.Position;
				color = ValidSpawn(pos, _dimensions) ? new Color(80, 255, 80) : new Color(255, 80, 80);
			}

			_spriteBatch.Draw(_pointers[_pointer], position, color);
		}
		private bool ValidSpawn(Vector2 pos, Vector2 dimensions)
		{
			if (pos.X < 0 || pos.Y < 0 || pos.X >= _matrice.GetLength(0) || pos.Y >= _matrice.GetLength(1))
			{ return false; }
			/*
			!_matrice[(int)pos.Y, (int)pos.X].Liquid &&
			!_matrice[(int)pos.Y, (int)pos.X + 1].Liquid &&
			!_matrice[(int)pos.Y - 1, (int)pos.X].Liquid &&
			!_matrice[(int)pos.Y - 1, (int)pos.X + 1].Liquid;
			*/
			for (int x = 0; x < dimensions.X; x++)
			{
				for (int y = 0; y < dimensions.Y; y++)
				{
					if (_matrice[(int)pos.Y - y, (int)pos.X + x].Liquid)
					{ return false; }
				}
			}
			return true;
		}

		private void DrawTitle()
		{
			DrawCommon();
			MakeMenu("Jouer", "Options", "Crédits", "Quitter");
		}
		private void DrawPlay()
		{
			DrawCommon();
			MakeMenu("Escarmouche","Multijoueur", "Retour");
		}
		private void DrawPlayQuick()
		{
			DrawCommon();
			string[] types = { "Île", "Plaine" };
			string[] tailles = { "Petite", "Moyenne", "Grande" };
			string[] ressources = { "rares", "normales", "abondantes" };
			string[] weathers = { "Ensoleillé", "Nuageux", "Pluvieux" };
			MakeMenu(types[(int)_quickType], tailles[_quickSize], _("Ressources") + " " + _(ressources[_quickResources]), weathers[_weather], "Suite", "Retour");
		}
		private void DrawPlayQuick2()
		{
			DrawCommon();
			string[] texts = { "N/A", "Ordinateur", "Humain" };
			MakeMenuFoes(_playersColors, texts[_players[0]], texts[_players[1]], texts[_players[2]], texts[_players[3]], "Jouer", "Retour");
		}
        private void DrawMultiplayer()
        {
            DrawCommon();
            MakeMenu("Créer un partie", "Rejoindre une partie", "Options", "Retour");
        }
		private void DrawOptions()
		{
			DrawCommon();
			MakeMenu("Jouabilité", "Graphismes", "Son", "Retour");
		}
		private void DrawOptionsGeneral()
		{
			DrawCommon();
			var languages = new Dictionary<string, string>();
			languages["en"] = "English";
			languages["es"] = "Español";
			languages["fr"] = "Français";
			MakeMenu(languages[_language], (_healthOver ? "Vie au survol" : "Vie constante"), (SmartHud ? "HUD intelligent" : "HUD classique"), "Retour");
		}
		private void DrawOptionsGraphics()
		{
			DrawCommon();
			string[] textures = { "min", "moyennes", "max" };
			MakeMenu(_screenSize.X + " x " + _screenSize.Y, (_fullScreen ? "Plein écran" : "Fenêtré"), _("Textures")+" " + _(textures[_textures]), (_shadows ? "Ombres" : "Pas d'ombres"), _("Thème")+" "+(_theme + 1), "Retour");
		}
		private void DrawOptionsSound()
		{
			DrawCommon();
			string[] sound = { "min", "moy", "max" };
			MakeMenu(_("Général :") + " " + _soundGeneral, _("Musique :") + " " + _soundMusic, _("Effets :") + " " + _soundSFX, _("Qualité") + " " + _(sound[_sound]), "Retour");
		}
		private void DrawCredits()
		{
			DrawCommon(false);

			// Crédits
			int y = _credits / 20;
			if (y > _screenSize.Y + 240 + (_fontCredits.MeasureString(_("Merci d'avoir joué !")).Y / 2))
			{ y = (int)(_screenSize.Y + 240 + (_fontCredits.MeasureString(_("Merci d'avoir joué !")).Y / 2)); }

			_spriteBatch.DrawString(_fontCredits, "Nicolas Allain", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Allain").X) / 2, _screenSize.Y - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Nicolas Faure", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Faure").X) / 2, _screenSize.Y + 60 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Nicolas Mouton-Besson", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Mouton-Besson").X) / 2, _screenSize.Y + 120 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Arnaud Weiss", new Vector2((_screenSize.X - _fontCredits.MeasureString("Arnaud Weiss").X) / 2, _screenSize.Y + 180 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, _("Merci d'avoir joué !"), new Vector2((_screenSize.X - _fontCredits.MeasureString(_("Merci d'avoir joué !")).X) / 2, (_screenSize.Y * 3 + _fontCredits.MeasureString(_("Merci d'avoir joué !")).Y) / 2 + 180 - y), Color.White);
		}
		private void DrawGame()
		{
			var index = (int)Math.Floor(_compt / 25);
			bool bui = _currentAction.StartsWith("build_");
			foreach (Sprite sprite in _matrice)
			{
				if (sprite.Position.X - _camera.Position.X > -64 && 
					sprite.Position.Y - _camera.Position.Y > -32 && 
					sprite.Position.X - _camera.Position.X < _screenSize.X &&
					(sprite.Position.Y - _camera.Position.Y < _screenSize.Y - Math.Round((double)_hud.Position.Height * 4 / 5) || 
					sprite.Position.Y - _camera.Position.Y < _screenSize.Y && SmartHud))
				{
					float mul = 0.0f;
					if (_weather > 0)
					{
						foreach (Unit unit in Joueur.Units)
						{
							float m = (unit.PositionCenter - sprite.PositionCenter).Length();
							m = 1.0f - (m / (unit.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in Joueur.Buildings)
						{
							float m = (building.PositionCenter - sprite.PositionCenter).Length();
							m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
					}
					else
					{ mul = 1.0f; }
					sprite.DrawMap(_spriteBatch, _camera, mul, _weather, bui ? (sprite.Liquid ? new Color(255, 80, 80) : new Color(80, 255, 80)) : Color.Transparent);
				}
			}

			// Affichage des objets sur la carte
            foreach (Joueur foe in _enemies)
            {
                foreach (Building build in foe.Buildings)
                {
                    if (_weather == 0)
                    {
                        build.Visible = true;
                        build.Draw(_spriteBatch, _camera, foe.ColorMovable);
                    }
                    else
                    {
                        float mul = 0.0f;
                        foreach (Unit unit in Joueur.Units)
                        {
                            float m = (unit.PositionCenter - build.Position).Length();
                            m = 1.0f - (m / (unit.LineSight + Joueur.AdditionalLineSight));
                            mul = (m > 0 && m > mul) ? m : mul;
                        }
                        foreach (Building building in Joueur.Buildings)
                        {
                            float m = (building.PositionCenter - build.Position).Length();
                            m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
                            mul = (m > 0 && m > mul) ? m : mul;
                        }
                        build.Visible = mul > 0;
                        if (mul > 0)
                        { build.Draw(_spriteBatch, _camera, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255)); }
                    }
                }
                foreach (Unit uni in foe.Units)
                {
                    if (_weather == 0)
                    {
                        uni.Visible = true;
                        uni.Draw(_spriteBatch, _camera, index, foe.ColorMovable);
                    }
                    else
                    {
                        float mul = 0.0f;
                        foreach (Unit unit in Joueur.Units)
                        {
                            float m = (unit.PositionCenter - uni.Position).Length();
                            m = 1.0f - (m / (unit.LineSight + Joueur.AdditionalLineSight));
                            mul = (m > 0 && m > mul) ? m : mul;
                        }
                        foreach (Building building in Joueur.Buildings)
                        {
                            float m = (building.PositionCenter - uni.Position).Length();
                            m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
                            mul = (m > 0 && m > mul) ? m : mul;
                        }
                        uni.Visible = mul > 0;
                        if (mul > 0f)
                        { uni.Draw(_spriteBatch, _camera, index, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255)); }
                    }
                }
            }
            //_drawList.Clear();
            //foreach (MovibleSprite sprite in _units)
            //{
            //    _drawList.Add(sprite);
            //}
            //foreach (Building sprite in _buildings)
            //{
            //    _drawList.Add(sprite);
            //}
            //foreach (ResourceMine sprite in _resources)
            //{
            //    _drawList.Add(sprite);
            //}
            //_drawList.Sort(Sprite.CompareByY);
            //foreach (Sprite sprite in _drawList)
            //{
            //    sprite.DrawMap(_spriteBatch, _camera, 1.0f, _weather);
            //}

            Joueur.Draw(_spriteBatch, _camera, index);
            foreach (ResourceMine sprite in _resources)
            {
                if (_weather == 0)
                {
                    sprite.Visible = true;
                    sprite.Draw(_spriteBatch, 1, _camera, 1.0f, _weather);
                }
                else
                {
                    float mul = 0.0f;
                    foreach (Unit unit in Joueur.Units)
                    {
                        float m = (unit.PositionCenter - sprite.PositionCenter).Length();
                        m = 1.0f - (m / (unit.LineSight + Joueur.AdditionalLineSight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    foreach (Building building in Joueur.Buildings)
                    {
                        float m = (building.PositionCenter - sprite.PositionCenter).Length();
                        m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    sprite.Visible = mul > 0;
                    if (mul > 0f)
                    { sprite.Draw(_spriteBatch, 1, _camera, mul, _weather); }
                }
            }

			// Rectangle de séléction
			var coos = new Vector2(
				_selection.X - _camera.Position.X + (_selection.Width < 0 ? _selection.Width : 0),
				_selection.Y - _camera.Position.Y + (_selection.Height < 0 ? _selection.Height : 0)
			);
			var tex = new Rectangle(
				coos.X < 0 ? 0 : (int)coos.X,
				coos.Y < 0 ? 0 : (int)coos.Y, 
				(int)(Math.Abs(_selection.Width) + (coos.X < 0 ? coos.X : 0)),
				(int)(Math.Abs(_selection.Height) + (coos.Y < 0 ? coos.Y : 0))
			);
			if (tex.Width + tex.X > _screenSize.X)
			{ tex.Width = (int)_screenSize.X - tex.X; }
			if (tex.Height + tex.Y > _screenSize.Y)
			{ tex.Height = (int)_screenSize.Y - tex.Y; }
			if (Math.Abs(_selection.Width) > 0 && Math.Abs(_selection.Height) > 0)
			{
				_spriteBatch.Draw(Color2Texture2D(Color.DarkBlue), tex, new Color(64, 64, 64, 64));
				_spriteBatch.Draw(Color2Texture2D(Color.Blue), new Rectangle(tex.X + 1, tex.Y + 1, tex.Width - 2, tex.Height - 2), new Color(64, 64, 64, 64));
			}

			// Barres de vie et de poches
			foreach (Unit unit in Joueur.Units)
			{
				if (!_healthOver || unit.Selected)
				{
					DrawBar(unit.Life, unit.MaxLife, unit.Position - new Vector2(13 - (float)Math.Round((double)unit.Texture.Width / 2), 6) - _camera.Position, 28, Color.Green, Color.Red);
					if (unit.Poches > 0)
					{ DrawBar(unit.Poches, unit.PochesMax, unit.Position - new Vector2(13 - (float)Math.Round((double)unit.Texture.Width / 2), 10) - _camera.Position, 28, Color.Cyan, Color.DarkBlue); }
				}
			}
			foreach (Building build in Joueur.Buildings)
			{
				if (!_healthOver || build.Selected)
				{ DrawBar(build.Life, build.MaxLife, build.Position - new Vector2(49 - (float)Math.Round((double)build.Texture.Width / 2), 10) - _camera.Position, 100, Color.Green, Color.Red); }
			}
			foreach (Joueur foe in _enemies)
			{
				foreach (Unit unit in foe.Units)
				{
					float mul = 0.0f;
					if (_weather > 0)
					{
						foreach (Unit uni in Joueur.Units)
						{
							float m = (uni.PositionCenter - unit.Position).Length();
							m = 1.0f - (m / (uni.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in Joueur.Buildings)
						{
							float m = (building.PositionCenter - unit.Position).Length();
							m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
					}
					else
					{ mul = 1.0f; }
					if (mul > 0.25f)
					{ DrawBar(unit.Life, unit.MaxLife, unit.Position - new Vector2(13 - (float)Math.Round((double)unit.Texture.Width / 2), 6) - _camera.Position, 28, Color.Green, Color.Red); }
				}
				foreach (Building build in foe.Buildings)
				{
					float mul = 0.0f;
					if (_weather > 0)
					{
						foreach (Unit uni in Joueur.Units)
						{
							float m = (uni.PositionCenter - build.Position).Length();
							m = 1.0f - (m / (uni.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in Joueur.Buildings)
						{
							float m = (building.PositionCenter - build.Position).Length();
							m = 1.0f - (m / (building.LineSight + Joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
					}
					else
					{ mul = 1.0f; }
					if (mul > 0.25f)
					{ DrawBar(build.Life, build.MaxLife, build.Position - new Vector2(49 - (float)Math.Round((double)build.Texture.Width / 2), 10) - _camera.Position, 100, Color.Green, Color.Red); }
				}
			}

			// Affichage du HUD
			MessagesManager.Draw(_spriteBatch, _fontSmall);
			_hud.Draw(_spriteBatch, _minimap, _units, _buildings, Joueur, _camera.Position + _screenSize / 2, _fontSmall);

			// Unités séléctionnées
			for (int i = 0; i < _selectedList.Count; i++)
			{
				var pos = new Vector2(356*(_screenSize.X/1680) + (i%10)*36, _screenSize.Y - _hud.Position.Height + 54*(_screenSize.Y/1050) + (i/10)*36 + _hud.SmartPos);
				_selectedList[i].DrawIcon(_spriteBatch, pos);
				DrawBar(_selectedList[i].Life, _selectedList[i].MaxLife, pos + new Vector2(0, 28), 33, Color.Green, Color.Red);
			}

			// List des actions
			for (int i = 0; i < _currentActions.Count; i++)
			{ _spriteBatch.Draw(_actions[_currentActions[i]], new Vector2(_hud.Position.X + 20 + 40 * (i % 6), _hud.Position.Y + 20 + 40 * (i / 6) + _hud.SmartPos), Color.White); }

			// Flash de changement d'ère
			if (FlashBool && _a > 0f)
			{
				_spriteBatch.Draw(_flash ,new Rectangle(0, 0, (int) _screenSize.X, (int) _screenSize.Y), new Color(0f, 0f, 0f, _a));
				_a -= 0.01f;
			}
			else
			{
				FlashBool = false;
				_a = 1.0f;
			}

            // Affichage les fps
            //Debug(4, Math.Truncate(fps.FPS));

            _techno.Draw(_spriteBatch, _fontSmall);
		}

		/// <summary>
		/// Affiche d'une barre colorée à l'écran.
		/// </summary>
		/// <param name="life">La valeur actuelle.</param>
		/// <param name="max">La valeur maximale.</param>
		/// <param name="pos">La position où afficher la barre.</param>
		/// <param name="width">La longueur de la barre en pixels.</param>
		/// <param name="c1">La couleur de la partie de la barre qui est remplie.</param>
		/// <param name="c2">La couleur de la partie de la barre qui est vide.</param>
		private void DrawBar(int life, int max, Vector2 pos, int width, Color c1, Color c2)
		{
			int greenLength = (Math.Max(life, 0) * (width - 2)) / max;
			int redLength = (width - 2) - greenLength;
			_spriteBatch.Draw(Color2Texture2D(Color.Black), new Rectangle((int)pos.X - 1, (int)pos.Y - 1, width, 5), Color.White);
			if (greenLength > 0)
			{ _spriteBatch.Draw(Color2Texture2D(c1), new Rectangle((int)pos.X, (int)pos.Y, greenLength, 3), Color.White); }
			if (redLength > 0)
			{ _spriteBatch.Draw(Color2Texture2D(c2), new Rectangle((int)pos.X + greenLength, (int)pos.Y, redLength, 3), Color.White); }
		}
		private void DrawGameMenu()
		{
			foreach (EffectPass pass in _gaussianBlur.CurrentTechnique.Passes)
			{
				DrawGame();
				pass.Apply();
			}
			_spriteBatch.Draw(_backgroundDark, Vector2.Zero, Color.White);
			MakePauseMenu("Quitter", "Retour");
		}

		#endregion

		#region Debug
        
		#region Temps Réel
		private void Debug(int i, string value)
		{
			#if DEBUG
				_spriteBatch.DrawString(_fontSmall, value, new Vector2(10, 10 + i * 20), Color.White);
			#endif
		}
		private void Debug(int i, bool value)
		{ Debug(i, (value ? "true" : "false")); }
		private void Debug(int i, object value)
		{ Debug(i, value.ToString()); }
		#endregion

		#region Console
		private void Debug(string value)
		{
			Console.Messages.Add(new ConsoleMessage(value));
			#if DEBUG
				System.Diagnostics.Debug.WriteLine(value);
			#endif
		}
		private void Debug(List<Point> value)
		{
			string deb = "List<" + value.GetType().GetGenericArguments()[0] + ">(" + value.Count.ToString(CultureInfo.CurrentCulture) + ") { ";
			deb = value.Aggregate(deb, (current, t) => current + (t + ", "));
			Debug(deb.Substring(0, deb.Length - 2) + " }");
		}
		private void Debug(object[] value)
		{
			string deb = value.GetType() + "[" + value.Length.ToString(CultureInfo.CurrentCulture) + "] { ";
			deb = value.Aggregate(deb, (current, t) => current + (t + ", "));
			Debug(deb.Substring(0, deb.Length - 2) + " }");
		}
		private void Debug(bool value)
		{ Debug((value ? "true" : "false")); }
		private void Debug(object value)
		{ Debug(value.ToString()); }
		#endregion

		#endregion

		#region Menus

		/// <summary>
		/// Affiche un texte avec bordures à l'écran.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="font">La police à utiliser pour afficher le texte.</param>
		/// <param name="text">Le texte à afficher.</param>
		/// <param name="coos">Les coordonnées du texte.</param>
		/// <param name="color">La couleur du texte.</param>
		/// <param name="borderCol">La couleur de la bordure.</param>
		/// <param name="border">La taille de la bordure.</param>
		/// <param name="spec">L'alignement du text. Valeurs possibles : "Left", "Right", "Center". Laissez vide pour ne rien changer.</param>
		/// <param name="scale">La déformation en taille du texte.</param>
		/// <param name="origin">Origine pour l'alignement à droite et à gauche.</param>
		protected void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 coos, Color color, Color borderCol, int border, string spec, float scale, Vector2 origin)
		{
			while (font.MeasureString(text).X * scale > _screenSize.X * 0.36)
			{ font.Spacing--; }

			switch (spec)
			{
				case "Left":
					coos.X = origin.X;
					break;

				case "Right":
					coos.X = _screenSize.X - font.MeasureString(text).X * scale - origin.X;
					break;

				case "Center":
					coos.X = (_screenSize.X - font.MeasureString(text).X * scale) / 2;
					break;
			}

			if (border > 0)
			{
				spriteBatch.DrawString(font, text, coos - new Vector2(border, 0), borderCol, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, text, coos + new Vector2(border, 0), borderCol, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, text, coos - new Vector2(0, border), borderCol, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.DrawString(font, text, coos + new Vector2(0, border), borderCol, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
			}

			spriteBatch.DrawString(font, text, coos, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

			font.Spacing = 0;
		}
		protected void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 coos, Color color, Color borderCol, int border = 0, string spec = "", float scale = 1)
		{ DrawString(spriteBatch, font, text, coos, color, borderCol, border, spec, scale, Vector2.Zero); }

		/// <summary>
		/// Affiche un menu à l'écran.
		/// </summary>
		/// <param name="args">Les options du menu.</param>
		protected void MakeMenu(params string[] args)
		{
			_currentMenus = args;
			for (int i = 0; i < args.Length; i++)
			{ DrawString(_spriteBatch, _fontMenu, _(args[i]), new Vector2(0, i * (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0))) + _screenSize.Y / 5 + (180 * (_screenSize.Y / 1050))), (Menu() == i ? Color.White : Color.Silver), Color.Black, 1, "Center", _screenSize.Y / 1050); }
		}

		/// <summary>
		/// Affiche un menu avec un choix de couleur à l'écran.
		/// </summary>
		/// <param name="colors">Les couleurs des joueurs.</param>
		/// <param name="args">Les options du menu.</param>
		protected void MakeMenuFoes(Color[] colors, params string[] args)
		{
			_currentMenus = args;
			for (int i = 0; i < Math.Min(args.Length, 4); i++)
			{
				float y = i * (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0))) + _screenSize.Y / 5 + (180 * (_screenSize.Y / 1050));
				_spriteBatch.Draw(CreateRectangle(40, 40, colors[i], new Color(24, 24, 24), 3), new Vector2(_screenSize.X * 3 / 7 - 60, y + 21), Color.White);
				DrawString(_spriteBatch, _fontMenu, _(args[i]), new Vector2(0, y), (MenuFoes() == i ? Color.White : Color.Silver), Color.Black, 1, "Left", _screenSize.Y / 1050, new Vector2(_screenSize.X * 3 / 7, 0));
			}
			for (int i = 4; i < args.Length; i++)
			{ DrawString(_spriteBatch, _fontMenu, _(args[i]), new Vector2(0, i * (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0))) + _screenSize.Y / 5 + (180 * (_screenSize.Y / 1050))), (Menu() == i ? Color.White : Color.Silver), Color.Black, 1, "Center", _screenSize.Y / 1050); }
		}

		/// <summary>
		/// Retourne le menu actuellement séléctionné.
		/// </summary>
		/// <returns>Le menu actuellement séléctionné</returns>
		protected int Menu()
		{
			float span = _screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0));
			int y = Souris.Get().Y > (_screenSize.Y / 5) + (180 * (_screenSize.Y / 1050)) && ((Souris.Get().Y - (_screenSize.Y / 5) - (180 * (_screenSize.Y / 1050))) % span) < (_fontMenu.MeasureString("Menu").Y * _screenSize.Y) / 1050 ? (int)((Souris.Get().Y - _screenSize.Y / 5 - (180 * (_screenSize.Y / 1050))) / span) : -1;
			return y >= 0 && 
				   y < _currentMenus.Length &&
			       Souris.Get().X >= (_screenSize.X - _fontMenu.MeasureString(_(_currentMenus[y])).X)/2 &&
			       Souris.Get().X <= (_screenSize.X + _fontMenu.MeasureString(_(_currentMenus[y])).X)/2
			       	? y
			       	: -1;
		}
		protected int MenuFoes()
		{
			float span = _screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0));
			int y = Souris.Get().Y > (_screenSize.Y / 5) + (180 * (_screenSize.Y / 1050)) && ((Souris.Get().Y - (_screenSize.Y / 5) - (180 * (_screenSize.Y / 1050))) % span) < (_fontMenu.MeasureString("Menu").Y * _screenSize.Y) / 1050 ? (int)((Souris.Get().Y - _screenSize.Y / 5 - (180 * (_screenSize.Y / 1050))) / span) : -1;
			return y >= 0 &&
			       y < _currentMenus.Length &&
				   ((y < 4 && Souris.Get().X >= _screenSize.X * 3 / 7 &&
				     Souris.Get().X - (_screenSize.X * 3 / 7) <= _fontMenu.MeasureString(_(_currentMenus[y])).X) ||
			        (Souris.Get().X >= (_screenSize.X - _fontMenu.MeasureString(_(_currentMenus[y])).X)/2 &&
			         Souris.Get().X <= (_screenSize.X + _fontMenu.MeasureString(_(_currentMenus[y])).X)/2))
			       	? y
			       	: -1;
		}

		/// <summary>
		/// Affiche un menu de pause à l'écran.
		/// </summary>
		/// <param name="args">Les options du menu.</param>
		protected void MakePauseMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ _spriteBatch.DrawString(_fontMenu, _(args[i]), new Vector2((668 * (_screenSize.X / 1680)), i * 80 + (180 * (_screenSize.Y / 1050))), Color.White); }
		}

		/// <summary>
		/// Retourne le menu de pause actuellement séléctionné.
		/// </summary>
		/// <returns>Le menu de pause actuellement séléctionné</returns>
		protected int PauseMenu()
		{ return (Souris.Get().X > (654 * (_screenSize.X / 1680)) && Souris.Get().Y > (180 * (_screenSize.Y / 1050))) ? (int)((Souris.Get().Y - (180 * (_screenSize.Y / 1050))) / 80) : -1; }

		/// <summary>
		/// Teste si un menu est cliqué.
		/// </summary>
		/// <param name="args">La liste des menus possibles.</param>
		/// <returns>Le menu cliqué.</returns>
		protected Screen TestMenu(params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				if (m >= 0 && m < args.Length)
				{ return args[m]; }
			}
			return _currentScreen;
		}

		/// <summary>
		/// Teste si un menu pause est cliqué.
		/// </summary>
		/// <param name="args">La liste des menus possibles.</param>
		/// <returns>Le menu cliqué.</returns>
		protected Screen TestPauseMenu(params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = PauseMenu();
				if (m >= 0 && m < args.Length)
				{ return args[m]; }
			}
			return _currentScreen;
		}

		#endregion

		/// <summary>
		/// Retourne la prochaine résolution disponible.
		/// </summary>
		/// <returns>La prochaine résolution dans l'ordre croissant.</returns>
		private Vector2 GetNextResolution(bool previous = false)
		{
			Vector2[] l = {
				/*new Vector2(320, 200), 
				new Vector2(320, 240), 
				new Vector2(640, 480), 
				new Vector2(800, 480), */
				new Vector2(800, 600), 
				new Vector2(854, 480), 
				new Vector2(1024, 768), 
				new Vector2(1152, 768), 
				new Vector2(1280, 720), 
				new Vector2(1280, 768), 
				new Vector2(1280, 800), 
				new Vector2(1280, 854), 
				new Vector2(1280, 960), 
				new Vector2(1280, 1024), 
				new Vector2(1366, 768), 
				new Vector2(1400, 1050), 
				new Vector2(1440, 900), 
				new Vector2(1440, 960), 
				new Vector2(1600, 1200), 
				new Vector2(1680, 1050), 
				new Vector2(1920, 1080), 
				new Vector2(1920, 1200), 
				new Vector2(2048, 1536), 
				new Vector2(2048, 1080),
				new Vector2(2560, 1600), 
				new Vector2(2560, 2048)
			};
			float w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			var list = l.Where(vector => (Math.Abs(Math.Round(vector.X/vector.Y, 2) - Math.Round(w/h, 2)) < 0.1 && vector.X <= w && vector.Y <= h) || vector == new Vector2(800, 600)).ToList();
			Vector2 next;
			if (list.Contains(_screenSize))
			{
				int index = list.IndexOf(_screenSize);
				next = list.ElementAt((index + (previous ? -1 : 1) + list.Count) % list.Count);
			}
			else
			{ next = list.ElementAt(0); }
			return next;
		}

		/// <summary>
		/// Génère un rectangle d'une certaine couleur de fond et de bordure à la volée.
		/// </summary>
		/// <param name="width">La largeur du rectangle.</param>
		/// <param name="height">La hauteur du rectangle.</param>
		/// <param name="col">La couleur du rectangle (peut avoir un canal alpha).</param>
		/// <param name="border">La couleur de la bordure du rectangle (peut avoir un canal alpha).</param>
		/// <param name="size">La taille de la bordure à générer.</param>
		/// <returns>La Texture2D générée.</returns>
		private Texture2D CreateRectangle(int width, int height, Color col, Color border, int size = 1)
		{
			var rectangleTexture = new Texture2D(GraphicsDevice, width, height, false, SurfaceFormat.Color);
			var color = new Color[width * height];
			for (int i = 0; i < color.Length; i++)
			{ color[i] = (i < width * size || i >= color.Length - width * size || i % width < size || i % width >= width - size ? border : col); }
			rectangleTexture.SetData(color);
			return rectangleTexture;
		}
		/// <summary>
		/// Génère un rectangle d'une certaine couleur de fond à la volée.
		/// </summary>
		/// <param name="width">La largeur du rectangle.</param>
		/// <param name="height">La hauteur du rectangle.</param>
		/// <param name="col">La couleur du rectangle (peut avoir un canal alpha).</param>
		/// <returns>La Texture2D générée.</returns>
		private Texture2D CreateRectangle(int width, int height, Color col)
		{ return CreateRectangle(width, height, col, col); }

		/// <summary>
		/// Traduit la chaîne passée en argument en fonction de la langue choisie.
		/// </summary>
		/// <param name="text">Le texte à traduire.</param>
		/// <param name="lang">Langue de destination.</param>
		/// <returns>Le texte traduit.</returns>
		private string Translate(string text, string lang = "")
		{
			var translations = new Dictionary<string, Dictionary<string, string>>();
			translations["en"] = new Dictionary<string, string>();
			translations["en"]["Jouer"] = "Play";
			translations["en"]["Options"] = "Settings";
			translations["en"]["Crédits"] = "Credits";
			translations["en"]["Quitter"] = "Quit";
			translations["en"]["Escarmouche"] = "Skirmish";
			translations["en"]["Retour"] = "Back";
            translations["en"]["Multijoueur"] = "MultiPlayer";
            translations["en"]["Local"] = "Local";
			translations["en"]["Internet"] = "Internet";
			translations["en"]["Île"] = "Island";
			translations["en"]["Plaine"] = "Flat";
			translations["en"]["Petite"] = "Small";
			translations["en"]["Moyenne"] = "Medium";
			translations["en"]["Grande"] = "Big";
			translations["en"]["rares"] = "rare";
			translations["en"]["normales"] = "normal";
			translations["en"]["abondantes"] = "abundant";
			translations["en"]["Ressources"] = "Resources";
			translations["en"]["Pluvieux"] = "Rainy";
			translations["en"]["Nuageux"] = "Cloudy";
			translations["en"]["Ensoleillé"] = "Sunny";
			translations["en"]["Ennemis :"] = "Enemies:";
			translations["en"]["Jouabilité"] = "Playability";
			translations["en"]["Graphismes"] = "Graphics";
			translations["en"]["Son"] = "Sound";
			translations["en"]["Vie au survol"] = "Over life";
			translations["en"]["Vie constante"] = "Always life";
			translations["en"]["HUD intelligent"] = "Smart HUD";
			translations["en"]["HUD classique"] = "Classical HUD";
			translations["en"]["min"] = "min";
			translations["en"]["moyennes"] = "medium";
			translations["en"]["max"] = "max";
			translations["en"]["Plein écran"] = "Full screen";
			translations["en"]["Fenêtré"] = "Windowed";
			translations["en"]["Textures"] = "Textures";
			translations["en"]["Ombres"] = "Shadows";
			translations["en"]["Pas d'ombres"] = "No shadows";
			translations["en"]["Thème"] = "Theme";
			translations["en"]["moy"] = "med";
			translations["en"]["Général :"] = "General:";
			translations["en"]["Musique :"] = "Music:";
			translations["en"]["Effets :"] = "Effects:";
			translations["en"]["Qualité"] = "Quality";
			translations["en"]["Merci d'avoir joué !"] = "Thanks for playing!";
			translations["en"]["Votre adresse IP est :"] = "Your IP adress is:";
			translations["en"]["Vous êtes connecté"] = "Your are connected";
			translations["en"]["Vous êtes déconnecté"] = "Your are not connected";
			translations["en"]["Vous n'avez pas assez de ressources."] = "You do not have enough resources.";
			translations["en"]["Vous ne pouvez pas construire ici"] = "You cannot build here.";
			translations["en"]["Nouvelle hutte !"] = "New hut!";
			translations["en"]["Nouvelle hutte des chasseurs !"] = "New hunters' hut!";
			translations["en"]["Nouveau peon !"] = "New peon!";
			translations["en"]["Nouveau guerrier !"] = "New fighter!";
			translations["en"]["Nouveau chasseur !"] = "New hunter!";
            translations["en"]["Ordinateur"] = "Computer";
            translations["en"]["Humain"] = "Human";
            translations["en"]["Suite"] = "Next";
            translations["en"]["Nouvelle tour !"] = "New tower!";
            translations["en"]["Nouvelle écurie !"] = "New stable!";

			translations["es"] = new Dictionary<string, string>();
			translations["es"]["Jouer"] = "Jugar";
			translations["es"]["Options"] = "Opciones";
			translations["es"]["Crédits"] = "Créditos";
			translations["es"]["Quitter"] = "Salir";
			translations["es"]["Escarmouche"] = "Escaramuza";
			translations["es"]["Retour"] = "Vuelta";
			translations["es"]["Île"] = "Isla";
			translations["en"]["Plaine"] = "Llanura";
			translations["es"]["Petite"] = "Pequeño";
			translations["es"]["Moyenne"] = "Medio";
			translations["es"]["Grande"] = "Grande";
			translations["es"]["rares"] = "raras";
			translations["es"]["normales"] = "normal";
			translations["es"]["abondantes"] = "abundante";
			translations["es"]["Ressources"] = "Recursos";
			translations["es"]["Pluvieux"] = "lluvioso";
			translations["es"]["Nuageux"] = "Nublado";
			translations["es"]["Ensoleillé"] = "Soleado";
			translations["es"]["Ennemis :"] = "Enemigos:";
			translations["es"]["Jouabilité"] = "Jugabilidad";
			translations["es"]["Graphismes"] = "Gráficos";
			translations["es"]["Son"] = "Sonido";
			translations["es"]["Vie au survol"] = "Sobre la Vida";
			translations["es"]["Vie constante"] = "Siempre la vida";
			translations["es"]["HUD intelligent"] = "Inteligente HUD";
			translations["es"]["HUD classique"] = "Clásica de HUD";
			translations["es"]["min"] = "min";
			translations["es"]["moyennes"] = "medio";
			translations["es"]["max"] = "max";
			translations["es"]["Plein écran"] = "Pantalla completa";
			translations["es"]["Fenêtré"] = "Ventanad";
			translations["es"]["Textures"] = "Texturas";
			translations["es"]["Ombres"] = "Sombras";
			translations["es"]["Pas d'ombres"] = "No sombras";
			translations["es"]["Thème"] = "Tema";
			translations["es"]["moy"] = "med";
			translations["es"]["Général :"] = "General:";
			translations["es"]["Musique :"] = "Música:";
			translations["es"]["Effets :"] = "Efectos:";
			translations["es"]["Qualité"] = "Calidad";
			translations["es"]["Merci d'avoir joué !"] = "Gracias por jugar!";
			translations["es"]["Votre adresse IP est :"] = "Tu IP dirección es:";
			translations["es"]["Vous êtes connecté"] = "Usted están conectado";
			translations["es"]["Vous êtes déconnecté"] = "Usted no están conectado";
			translations["es"]["Vous n'avez pas assez de ressources."] = "Usted no tiene suficientes recursos.";
			translations["es"]["Nouvelle hutte !"] = "Nueva cabaña!";
			translations["es"]["Nouvelle hutte des chasseurs !"] = "Nueva cabaña de cazadores!";
			translations["es"]["Nouveau peon !"] = "Nueva peón!";
			translations["es"]["Nouveau guerrier !"] = "Nueva guerrero!";
			translations["es"]["Nouveau chasseur !"] = "Nuevo caza!";
            translations["es"]["Ordinateur"] = "Ordenador";
            translations["es"]["Humain"] = "Humano";
            translations["es"]["Suite"] = "Continuación";
            translations["en"]["Nouvelle tour !"] = "Nueva torre!";
            translations["en"]["Nouvelle écurie !"] = "Nuevo Establo!";

			if (lang == "")
			{ lang = _language; }

			if (translations.ContainsKey(lang))
			{
				if (translations[lang].ContainsKey(text))
				{ return translations[lang][text]; }
			}
			return text;
		}

		/// <summary>
		/// Traduit la chaîne passée en argument en fonction de la langue choisie.
		/// </summary>
		/// <param name="text">Le texte à traduire.</param>
		/// <param name="lang">Langue de destination.</param>
		/// <returns>Le texte traduit.</returns>
		private string _(string text, string lang = "")
		{ return Translate(text, lang);  }

		#region Conversions

		/// <summary>
		/// Convertit un booléen en entier.
		/// </summary>
		/// <param name="b">Le booléen à convertir.</param>
		/// <returns>L'entier binaire correspondant.</returns>
		protected int B2I(bool b)
		{ return b ? 1 : 0; }

		/// <summary>
		/// Transforme des coordonnées losange en coordonnées matrice.
		/// </summary>
		/// <param name="mouse">Coordonnées losange.</param>
		/// <returns>Coordonnées matrice.</returns>
		public static Vector2 Xy2Matrice(Vector2 mouse)
		{
			const double angleDegre = -26.57; // -36.57 = -arctan(1/2)
			const double angleRadian = Math.PI * angleDegre / 180;
			double sina = Math.Sin(angleRadian);
			double cosa = Math.Cos(angleRadian);
			var rot = new Vector2((float)(mouse.X * cosa - mouse.Y * sina), (float)(mouse.X * sina + mouse.Y * cosa)); // Coordonées parallélogramme
			var final = new Vector2(rot.X + (float)(0.75 * rot.Y), rot.Y); // Coordonées rectangle
			var coo = new Vector2((float)Math.Round((final.X - 35) / 35.777), (float)Math.Round((final.Y + 4) / 28.54)); // Coordonées matrice // 35.777 = sqrt(16²+32²)
			return coo;
		}

		/// <summary>
		/// Transforme des coordonnées matrice en coordonnées losange.
		/// </summary>
		/// <param name="mouse">Coordonnées matrice.</param>
		/// <returns>Coordonnées losange.</returns>
		public static Vector2 Matrice2Xy(Vector2 mouse)
		{ return new Vector2(32 * (mouse.X - mouse.Y), 16 * (mouse.X + mouse.Y)); }

		private Texture2D Color2Texture2D(Color color)
		{
			if (!_colors.ContainsKey(color))
			{ _colors.Add(color, CreateRectangle(1, 1, color)); }
			return _colors[color];
		}

		#endregion
	}
}
