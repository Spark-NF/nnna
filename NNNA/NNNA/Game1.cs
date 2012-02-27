using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms.Integration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Forms = System.Windows.Forms;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NNNA
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		#region Variables

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Effect _gaussianBlur;
		private Rectangle _selection = Rectangle.Empty;

		private Texture2D _backgroundDark, _flash, _night, _console;
		private Texture2D[] _backgrounds = new Texture2D[2];
		private Dictionary<string, Texture2D> _actions = new Dictionary<string, Texture2D>(), _pointers = new Dictionary<string, Texture2D>();
		private SpriteFont _fontMenu, _fontMenuTitle, _fontSmall, _fontCredits;
		private Screen _currentScreen = Screen.Title;
		private int _konami = 0, _konamiStatus = 0, _foes = 1;
		private string _currentAction = "", _language = "fr", _pointer = "pointer";
		private List<string> _lastState = new List<string>();
		private string[] _currentMenus;

		private Vector2 _screenSize = new Vector2(1680, 1050);
		private bool _fullScreen = true, _shadows = true, _healthOver, _showConsole;
		public static bool SmartHud = false;
		private float _soundGeneral = 10, _soundSFX = 10, _soundMusic = 10, a = 1.0f;
		private int _textures = 2, _sound = 2, _weather = 1;
		internal static bool FlashBool = false;
		
		private MapType _quickType = MapType.Island;
		private int _quickSize = 1, _quickResources = 1, _credits = 0, _theme = 0;
		private List<string> _currentActions = new List<string>();
		private Random _random = new Random(42);
		private Dictionary<Color, Texture2D> _colors = new Dictionary<Color, Texture2D>();
		
		// Map
		private Sprite _h, _e, _p, _t, _s, _i, _curseur;
		private Sprite[,] _matrice;
		private Map _map;
		private Minimap _minimap;
		private HUD _hud;
		private Camera2D _camera;
		private Joueur _joueur;
		private Joueur[] _enemies;
		private Building _selectedBuilding;
		private float[,] _heightMap;
		private List<ResourceMine> _resources = new List<ResourceMine>();
		private List<Unit> _selectedList = new List<Unit>();
		private List<Building> _buildings = new List<Building>();
		private List<MovibleSprite> _units = new List<MovibleSprite>();

		// Audio objects		
		private const float MusicVolume = 2.0f;
		private Sons _son = new Sons();
		private SoundEffect _debutpartie;
		private ElementHost _elementHost;
		private Technologies _techno;


		#endregion

		#region Enums

		public enum Screen
		{
			Title,
			Play,
			PlayCampain,
			PlayQuick,
			PlayMultiplayer,
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
			Island
		}

		#endregion

		public Game1()
		{
			Window.Title = "NNNA - " + GetType().Assembly.GetName().Version;

			LoadSettings();
			_showConsole = false;

			_graphics = new GraphicsDeviceManager(this)
			{
			    PreferredBackBufferWidth = (int)_screenSize.X,
			    PreferredBackBufferHeight = (int)_screenSize.Y,
			    IsFullScreen = _fullScreen
			};

			Content.RootDirectory = "Content";
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
			_joueur = new Joueur(Color.Red, "NNNNA", Content);

			 // son
			_son.Initializesons(MusicVolume, _soundMusic, _soundGeneral);
			_debutpartie = Content.Load<SoundEffect>("sounds/debutpartie");
			
			//menu technologie
			_elementHost= new ElementHost();
			_techno = new Technologies(_joueur, ref _elementHost, Content);

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
			float[,] map = IslandGenerator.Generate(width, height);
			_heightMap = map;
			Sprite[,] matrice = new Sprite[width, height];
			List<float> heights = new List<float>();

			// Calcul de la hauteur de l'eau
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{ heights.Add(map[x, y] * 255); }
			}
			
			heights.Sort();
			float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];
			// On transforme les hauteurs en sprites
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
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
			for (int x = 0; x < matrice.GetLength(0); x++)
			{
				for (int y = 0; y < matrice.GetLength(1); y++)
				{
					if (matrice[x, y] == _h)
					{
						bool waternear = (x > 0 && matrice[x - 1, y] == _e) || (y > 0 && matrice[x, y - 1] == _e) || (x < matrice.GetLength(0) - 1 && matrice[x + 1, y] == _e) || (y < matrice.GetLength(1) - 1 && matrice[x, y + 1] == _e);
						matrice[x, y] = waternear ? _p : _h;
					}
				}
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

			// Couleurs
			_colors.Add(Color.Red, CreateRectangle(1, 1, Color.Red));
			_colors.Add(Color.Green, CreateRectangle(1, 1, Color.Green));
			_colors.Add(Color.Black, CreateRectangle(1, 1, Color.Black));

			#region Actions

			#region Actions Unités
			_actions.Add("attack", Content.Load<Texture2D>("Actions/attack"));
			_actions.Add("gather", Content.Load<Texture2D>("Actions/gather"));
			_actions.Add("build", Content.Load<Texture2D>("Actions/build"));
			_actions.Add("build_hutte", Content.Load<Texture2D>("Actions/build_hutte"));
			_actions.Add("build_hutteDesChasseurs", Content.Load<Texture2D>("Actions/build_hutteDesChasseurs"));
			// m_actions.Add("build_ferme", Content.Load<Texture2D>("Actions/build_ferme"));
			#endregion Actions Unités

			#region Actions Batiments
			_actions.Add("create_peon", Content.Load<Texture2D>("Actions/create_peon"));
			_actions.Add("technologies", Content.Load<Texture2D>("Actions/technologies"));
			_actions.Add("create_guerrier", Content.Load<Texture2D>("Actions/create_guerrier"));
			#endregion Actions Batiments

			#region Actions Communes
			_actions.Add("retour", Content.Load<Texture2D>("Actions/retour"));
			#endregion Actions Communes

			#endregion Actions

			// Shaders
			_gaussianBlur = Content.Load<Effect>("Shaders/GaussianBlur");
			_gaussianBlur.CurrentTechnique = _gaussianBlur.Techniques["Blur"];

			LoadScreenSizeDependantContent();
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
			_night = CreateRectangle((int)_screenSize.X, (int)_screenSize.Y, new Color(0,0,10));
			_console = CreateRectangle(1, 1, new Color(0, 0, 0, 128));

			//Fenetre des technologies
			_elementHost.Location = new System.Drawing.Point((int) _screenSize.X/2 - 150, (int) _screenSize.Y/2 - 150);
			_elementHost.Size = new Size(300, 300);
			_elementHost.Child = _techno;
			_elementHost.BackColor = System.Drawing.Color.Transparent;
			_elementHost.BackColorTransparent = true;

			_hud = new HUD(0, ((_graphics.PreferredBackBufferHeight * 5) / 6) - 10, SmartHud, _graphics);
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
			if (_konami >= 10)
			{ _konamiStatus++; }
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
					UpdateCredits();
					break;

				case Screen.Game:
					UpdateGame(gameTime);
					break;
				case Screen.GameMenu:
					UpdateGameMenu();
					break;
			}

			//Son 
			_son.EngineMenu.Update();
			if (!_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
			{ _son.Initializesons(MusicVolume, _soundMusic, _soundGeneral); }

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
		{ _currentScreen = TestMenu(Screen.PlayQuick, Screen.Title); }
		private void UpdatePlayQuick()
		{
			Screen s = TestMenu(Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.Game, Screen.Play);
			if (s != Screen.PlayQuick)
			{
				if (s == Screen.Game)
				{
					bool ok = false;
					int[] sizes = { 50, 100, 200 };
					double[] resources = { 0.05, 0.1, 0.2 };
					var spawns = new List<Point>();
					var heights = new List<float>();
					float dist = sizes[_quickSize] / 2;

					// On regénère une carte tant qu'elle est incapable d'accueillir le bon nombre de spawns
					while (!ok)
					{
						// Génération
						_matrice = GenerateMap(_quickType, sizes[_quickSize], sizes[_quickSize]);

						// Spawns
						heights.Clear();
						for (int x = 0; x < _heightMap.GetLength(0); x++)
						{
							for (int y = 0; y < _heightMap.GetLength(1); y++)
							{ heights.Add(_heightMap[x, y] * 255); }
						}
						float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];
						var heightsOr = new List<float>(heights);
						heights.Sort((x, y) => (y.CompareTo(x)));
						spawns.Clear();
						int j = 0;

						// On génère $m_foes + 1 spawns, chacun espacés d'au moins $dist
						while (spawns.Count < _foes + 1 && j < heights.IndexOf(waterline))
						{
							var point = new Point(heightsOr.IndexOf(heights[j]) % _heightMap.GetLength(1), heightsOr.IndexOf(heights[j]) / _heightMap.GetLength(1));
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

					// Les couleurs et noms des joueurs
					Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Yellow };
					string[] names = { Environment.UserName, "Lord Lard", "Herr von Speck", "Monsieur Martin" };

					//Joueur
					_joueur = new Joueur(colors[0], names[0], Content);
					_joueur.Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 100, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, _joueur, false));
					_joueur.Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 0, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, _joueur, false));
					_joueur.Units.Add(new Peon((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 50, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, _joueur, false));
					_joueur.Buildings.Add(new GrandeHutte((int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X, (int)Matrice2Xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y, Content, _joueur));
					_camera.Position = Matrice2Xy(new Vector2(spawns[0].X + 7, spawns[0].Y + 5)) - _screenSize / 2;
					_units.AddRange(_joueur.Units);
					_buildings.AddRange(_joueur.Buildings);

					// Ennemis
					_enemies = new Joueur[_foes];
					for (int i = 0; i < _foes; i++)
					{
						_enemies[i] = new Joueur(colors[i + 1], names[i + 1], Content);
						_enemies[i].Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 100, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _joueur, false));
						_enemies[i].Units.Add(new Guerrier((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 0, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _joueur, false));
						_enemies[i].Units.Add(new Peon((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 50, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, _joueur, false));
						_enemies[i].Buildings.Add(new GrandeHutte((int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X, (int)Matrice2Xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y, Content, _joueur));
						_units.AddRange(_enemies[i].Units);
						_buildings.AddRange(_enemies[i].Buildings);
					}

					//Le reste
					_techno.Reset();
					_map.LoadContent(_matrice, Content, _minimap, _graphics.GraphicsDevice);
					_hud.LoadContent(Content, "HUD/hud2");
					_minimap.LoadContent(_map);

					//Decor
					_resources.Add(new ResourceMine((int)Matrice2Xy(new Vector2(spawns[0].X + 10, spawns[0].Y + 10)).X, (int)Matrice2Xy(new Vector2(spawns[0].X + 5, spawns[0].Y + 2)).Y, _joueur.Resource("Pierre"), _joueur.Resource("Pierre").Texture(1)));
					for (int i = 0; i < 5 + _quickResources * 5 * (_quickSize + 1); i++)
					{
						int x = _random.Next(_matrice.GetLength(0));
						int y = _random.Next(_matrice.GetLength(1));
						while ((!_matrice[y, x].Crossable))
						{
							x = _random.Next(_matrice.GetLength(0));
							y = _random.Next(_matrice.GetLength(1));
						}
						_resources.Add(new ResourceMine((int)(Matrice2Xy(new Vector2(x, y))).X - 44, (int)(Matrice2Xy(new Vector2(x, y))).Y - 152, _joueur.Resource("Bois"), Content.Load<Texture2D>("Resources/bois_1_sprite" + _random.Next(0, 3))));
						_matrice[y, x].Crossable = false;
					}

					//Le son
					_son.MusiqueMenu.Pause();
					_debutpartie.Play();
					
				}
				_currentScreen = s;
			}
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = Menu();
				switch (m)
				{
					case 0: _quickType = (MapType)Variate(0, Enum.GetValues(typeof(MapType)).Length - 1, (int)_quickType); break;
					case 1: _quickSize = Variate(0, 2, _quickSize); break;
					case 2: _quickResources = Variate(0, 2, _quickResources); break;
					case 3: _foes = Variate(1, 3, _foes); break;
					case 4: _weather = Variate(0, 2, _weather); break;
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
					case 0: _language = (_language == "en" ? "fr" : "en"); break;
					case 1: _healthOver = !_healthOver; break;
					case 2: SmartHud = !SmartHud; break;
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
						_graphics.PreferredBackBufferWidth = (int)_screenSize.X;
						_graphics.PreferredBackBufferHeight = (int)_screenSize.Y;
						LoadScreenSizeDependantContent();
						_graphics.ApplyChanges();
						break;

					case 1:
						_fullScreen = !_fullScreen;
						if (!_fullScreen)
						{ Forms.Application.EnableVisualStyles(); }
						_graphics.IsFullScreen = _fullScreen;
						_graphics.ApplyChanges();
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
			_son.MusicCategory.SetVolume(MusicVolume * _soundMusic * (_soundGeneral / 10));

		}
		private void UpdateCredits()
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ _currentScreen = Screen.Title; }
			_credits += (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl) ? 10 : 1);
		}
		float _compt;
		private void UpdateGame(GameTime gameTime)
		{
			_techno.Pre_Update(_joueur);
			Forms.Control.FromHandle(Window.Handle).Controls.Add(_elementHost);
			_techno.Post_Update(_joueur);

			//if (isbuilding)
			//{
			//	Vector2 xy = matrice2xy(xy2matrice(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
			//	Mouse.SetPosition((int) xy.X, (int) xy.Y);
			//}

			if (Clavier.Get().NewPress(Keys.Escape))
			{
				_elementHost.Visible = false;
				_currentScreen = Screen.GameMenu;
			}

			_compt = (_compt + gameTime.ElapsedGameTime.Milliseconds * 0.1f) % 100;
			_curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			_camera.Update(_curseur, _graphics);

			// Intelligence artificielle
			var rand = new Random();
			foreach (Joueur foe in _enemies)
			{
				foreach (Unit unit in foe.Units)
				{
					if (++unit.Updates == 120)
					{
						unit.Move(unit.Position + new Vector2(rand.Next(-40, 41), rand.Next(-40, 41)), _units, _buildings, _matrice);
						unit.Updates = rand.Next(0, 40);
					}
					unit.ClickMouvement(_curseur, gameTime, _camera, _hud, _units, _buildings, _matrice);
				}
			}

			// Rectangle de séléction
			if (Souris.Get().Clicked(MouseButton.Left))
			{
				Building b;
				Unit u;
				switch (_currentAction)
				{
					// Ere 1 
					case "build_hutte":
						b = new Hutte((int)(_curseur.Position.X + _camera.Position.X), (int)(_curseur.Position.Y + _camera.Position.Y), Content, _joueur, (byte)_random.Next(0,2));
						if (_joueur.Pay(b.Prix))
						{
							_joueur.Buildings.Add(b);
							_buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
							_pointer = "pointer";
							_currentAction = "";
						}
						else
						{
							MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
							_pointer = "pointer";
							_currentAction = "";
						}
						break;

					case "build_hutteDesChasseurs":
						b = new HutteDesChasseurs((int)(_curseur.Position.X + _camera.Position.X), (int)(_curseur.Position.Y + _camera.Position.Y), Content, _joueur);
						if (_joueur.Pay(b.Prix))
						{
							_joueur.Buildings.Add(b);
							_buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
							_pointer = "pointer";
							_currentAction = "";
						}
						else
						{
							MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
							_pointer = "pointer";
							_currentAction = "";
						}
						break;

					case "create_peon" :
						u = new Peon((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 5), (int)_selectedBuilding.Position.Y + 155, Content, _joueur, false);
						if (_joueur.Population + 1 > _joueur.PopulationMax && _joueur.Pay(u.Prix))
						{
							_selectedBuilding.Iterator++;
							_joueur.Units.Add(u);
							_units.Add(u);
							MessagesManager.Messages.Add(new Msg("Nouveau Peon !", Color.White, 5000));
							_currentAction = "";
						}
						else
						{
							MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
							_joueur.Population--;
							_currentAction = "";
						}
						break;

					case "create_guerrier":
						u = new Guerrier((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 3), (int)_selectedBuilding.Position.Y + 70, Content, _joueur, false);
						if (_joueur.Population + 1 > _joueur.PopulationMax && _joueur.Pay(u.Prix))
						{
							_selectedBuilding.Iterator++;
							_joueur.Units.Add(u);
							_units.Add(u);
							MessagesManager.Messages.Add(new Msg("Nouveau Guerrier !", Color.White, 5000));
							_currentAction = "";
						}
						else
						{
							MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
							_joueur.Population--;
							_currentAction = "";
						}
						break;

						// Fin Ere 1 

					/* Ere 2 
					case "build_ferme":
						b = new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, joueur);
						if (joueur.Pay(b.Prix))
						{
							joueur.buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle ferme !", Color.White, 5000));
							m_pointer = Content.Load<Texture2D>("pointer");
							isbuilding = false;
							m_currentAction = "";
						}
						else
						{
							MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
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
				foreach (Unit sprite in _joueur.Units)
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

					foreach (Building sprite in _joueur.Buildings)
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
							if (sprite.Type == "peon" && !_currentActions.Contains("build"))
							{
								_currentActions.Add("gather");
								_currentActions.Add("build");
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
						case "forum" :
							_currentActions.Add("create_peon");
							_currentActions.Add("technologies");
							break;

						case "caserne" :
							_currentActions.Add("create_guerrier");
							break;
					}
				}
				_currentAction = "";
				_lastState.Clear();
				foreach (string t in _currentActions)
				{ _lastState.Add(t); }
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
								_currentActions.Add("retour");
								break;

							case "gather":
								break;

							case "build_hutte":
								if (_joueur.Has(new Hutte().Prix))
								{
									if (_random.Next(0, 2) == 0)
									{ _pointer = "Batiments/maison1_" + _joueur.Ere.ToString(CultureInfo.CurrentCulture); }
									else
									{ _pointer = "Batiments/maison2_" + _joueur.Ere.ToString(CultureInfo.CurrentCulture); }
									_currentAction = "build_hutte";
								}
								else
								{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
								break;

							case "build_hutteDesChasseurs":
								if (_joueur.Has(new HutteDesChasseurs().Prix))
								{
									_pointer = "Batiments/caserne_" + _joueur.Ere.ToString(CultureInfo.CurrentCulture);
									_currentAction = "build_hutteDesChasseurs";
								}
								else
								{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
								break;

							case "retour":
								_currentActions.Clear();
								foreach (string t in _lastState)
								{ _currentActions.Add(t); }
								break;

							case "create_peon":
								if (_selectedBuilding != null)
								{
									var u = new Peon((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 5), (int)_selectedBuilding.Position.Y + 155, Content, _joueur);
									if (_joueur.Pay(u.Prix))
									{
										_selectedBuilding.Iterator++;
										_joueur.Units.Add(u);
										_units.Add(u);
										MessagesManager.Messages.Add(new Msg("Nouveau Peon !", Color.White, 5000));
										_currentAction = "";
									}
									else
									{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
								}
								break;

							case "technologies":
								_elementHost.Visible = true;
								break;

							case "create_guerrier":
								if (_selectedBuilding != null)
								{
									var u1 = new Guerrier((int)_selectedBuilding.Position.X + 50 * (_selectedBuilding.Iterator % 3), (int)_selectedBuilding.Position.Y + 70, Content, _joueur);
									if (_joueur.Has(u1.Prix))
									{
										_selectedBuilding.Iterator++;
										_joueur.Units.Add(u1);
										_units.Add(u1);
										MessagesManager.Messages.Add(new Msg("Nouveau Chasseur !", Color.White, 5000));
										_currentAction = "";
									}
									else
									{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
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
							{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
							break;*/
						}
					}
				}
			}
			foreach (Unit sprite in _joueur.Units)
			{ sprite.ClickMouvement(_curseur, gameTime, _camera, _hud, _units, _buildings, _matrice); }
			
			// Curseur de combat
			if (_currentAction == "" && _selectedList.Count > 0)
			{
				bool unitUnder = false;
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
								foreach (Unit uni in _joueur.Units)
								{
									float m = (uni.PositionCenter - unit.Position).Length();
									m = 1.0f - (m / (uni.LineSight + _joueur.AdditionalLineSight));
									mul = (m > 0 && m > mul) ? m : mul;
								}
								foreach (Building building in _joueur.Buildings)
								{
									float m = (building.PositionCenter - unit.Position).Length();
									m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
									mul = (m > 0 && m > mul) ? m : mul;
								}
							}
							else
							{ mul = 1.0f; }
							if (mul > 0.25f)
							{
								_pointer = "fight";
								unitUnder = true;
							}
						}
					}
				}
				if (!unitUnder)
				{ _pointer = "pointer"; }
			}

			_joueur.Units.Sort(Sprite.CompareByY);
			_joueur.Buildings.Sort(Sprite.CompareByY);
			 
			//minimap.Update(units, buildings, selectedList, joueur);
			_son.MusiqueMenu.Pause();
		}
		void UpdateGameMenu()
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{
				_pointer = "pointer";
				_currentScreen = Screen.Game;
			}
			_curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			_currentScreen = TestPauseMenu(Screen.Title, Screen.Game);
			if (!_son.MusiqueMenu.IsPlaying && !_son.MusiqueMenu.IsPaused)
			{ _son.Initializesons(MusicVolume, _soundMusic, _soundGeneral); }
			_son.MusiqueMenu.Resume();
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
				_joueur.Resource("Bois").Add(5000);
				_joueur.Resource("Pierre").Add(5000);
				_joueur.Resource("Nourriture").Add(5000);
				_konami = 0;
				/*if (m_konamiStatus < 300)
				{
					spriteBatch.DrawString(m_font_credits, "KONAMI CODE!", new Vector2(10, 10), Color.Red);
					joueur.Resource("Bois").Add(500);
					joueur.Resource("Pierre").Add(500);
					joueur.Resource("Nourriture").Add(500);
				}
				else
				{
					m_konami = 0;
					m_konamiStatus = 0;
				}*/
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
				DrawString(_spriteBatch, _fontSmall, _("Votre adresse IP est :") + " " + Réseau.GetIPaddresses(Environment.MachineName), new Vector2((_screenSize.X - _fontSmall.MeasureString(_("Votre adresse IP est :") + " " + Réseau.GetIPaddresses(Environment.MachineName)).X), _screenSize.Y - 20), Color.GhostWhite, Color.Transparent, 1);
			}

		}
		private void DrawPointer()
		{
			if (!_pointers.ContainsKey(_pointer))
			{ _pointers.Add(_pointer, Content.Load<Texture2D>(_pointer)); }
			_spriteBatch.Draw(_pointers[_pointer], new Vector2(Souris.Get().X, Souris.Get().Y), Color.White);
		}
		private void DrawTitle()
		{
			DrawCommon();
			MakeMenu("Jouer", "Options", "Crédits", "Quitter");
		}
		private void DrawPlay()
		{
			DrawCommon();
			MakeMenu("Escarmouche", "Retour");
		}
		private void DrawPlayQuick()
		{
			DrawCommon();
			string[] types = { "Île" };
			string[] tailles = { "Petite", "Moyenne", "Grande" };
			string[] ressources = { "rares", "normales", "abondantes" };
			string[] weathers = { "Ensoleillé", "Nuageux", "Pluvieux" };
			MakeMenu(types[(int)_quickType], tailles[_quickSize], _("Ressources") + " " + _(ressources[_quickResources]), _("Ennemis :") + " " + _foes.ToString(CultureInfo.CurrentCulture), weathers[_weather], "Jouer", "Retour");
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
			int y = _credits / 2;
			if (y > _screenSize.Y + 200 + (_fontCredits.MeasureString(_("Merci d'avoir joué !")).Y / 2))
			{ y = (int)(_screenSize.Y + 200 + (_fontCredits.MeasureString(_("Merci d'avoir joué !")).Y / 2)); }

			_spriteBatch.DrawString(_fontCredits, "Nicolas Allain", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Allain").X) / 2, _screenSize.Y - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Nicolas Faure", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Faure").X) / 2, _screenSize.Y + 60 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Nicolas Mouton-Besson", new Vector2((_screenSize.X - _fontCredits.MeasureString("Nicolas Mouton-Besson").X) / 2, _screenSize.Y + 120 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, "Arnaud Weiss", new Vector2((_screenSize.X - _fontCredits.MeasureString("Arnaud Weiss").X) / 2, _screenSize.Y + 180 - y), Color.White);
			_spriteBatch.DrawString(_fontCredits, _("Merci d'avoir joué !"), new Vector2((_screenSize.X - _fontCredits.MeasureString(_("Merci d'avoir joué !")).X) / 2, _screenSize.Y + (_screenSize.Y / 2) + 180 + (_fontCredits.MeasureString(_("Merci d'avoir joué !")).Y / 2) - y), Color.White);
		}
		private void DrawGame()
		{
			int index = (int)Math.Floor(_compt / 25);
			int compteur = 0;
			foreach (Sprite sprite in _matrice)
			{
				if ((sprite.Position.X - _camera.Position.X > -64
					&& sprite.Position.Y - _camera.Position.Y > -32
					&& sprite.Position.X - _camera.Position.X < _screenSize.X
					&& sprite.Position.Y - _camera.Position.Y < _screenSize.Y - (_hud.Position.Height * 4 / 5))
					|| SmartHud)
				{
					float mul = 0.0f;
					if (_weather > 0)
					{
						foreach (Unit unit in _joueur.Units)
						{
							float m = (unit.PositionCenter - sprite.PositionCenter).Length();
							m = 1.0f - (m / (unit.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in _joueur.Buildings)
						{
							float m = (building.PositionCenter - sprite.PositionCenter).Length();
							m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
					}
					else
					{ mul = 1.0f; }
					sprite.DrawMap(_spriteBatch, _camera, mul, _weather);
					compteur++;
				}
			}

			// Affichage des objets sur la carte
			foreach (Joueur foe in _enemies)
			{
				foreach (Building build in foe.Buildings)
				{
					if (_weather == 0)
					{ build.Draw(_spriteBatch, _camera, foe.ColorMovable); }
					else
					{
						float mul = 0.0f;
						foreach (Unit unit in _joueur.Units)
						{
							float m = (unit.PositionCenter - build.Position).Length();
							m = 1.0f - (m / (unit.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in _joueur.Buildings)
						{
							float m = (building.PositionCenter - build.Position).Length();
							m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						if (mul > 0)
						{ build.Draw(_spriteBatch, _camera, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255)); }
					}
				}
				foreach (Unit uni in foe.Units)
				{
					if (_weather == 0)
					{ uni.Draw(_spriteBatch, _camera, index, foe.ColorMovable); }
					else
					{
						float mul = 0.0f;
						foreach (Unit unit in _joueur.Units)
						{
							float m = (unit.PositionCenter - uni.Position).Length();
							m = 1.0f - (m / (unit.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						foreach (Building building in _joueur.Buildings)
						{
							float m = (building.PositionCenter - uni.Position).Length();
							m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
							mul = (m > 0 && m > mul) ? m : mul;
						}
						if (mul > 0f)
						{ uni.Draw(_spriteBatch, _camera, index, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255)); }
					}
				}
			}
			_joueur.Draw(_spriteBatch, _camera, index);
			foreach (ResourceMine sprite in _resources)
			{
				if (_weather == 0)
				{ sprite.Draw(_spriteBatch, 1, _camera, 1.0f, _weather); }
				else
				{
					float mul = 0.0f;
					foreach (Unit unit in _joueur.Units)
					{
						float m = (unit.PositionCenter - sprite.PositionCenter).Length();
						m = 1.0f - (m / (unit.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					foreach (Building building in _joueur.Buildings)
					{
						float m = (building.PositionCenter - sprite.PositionCenter).Length();
						m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					sprite.Draw(_spriteBatch, 1, _camera, mul, _weather);
				}
			}
			// Rectangle de séléction
			Vector2 coos = new Vector2(
				_selection.X - _camera.Position.X + (_selection.Width < 0 ? _selection.Width : 0),
				_selection.Y - _camera.Position.Y + (_selection.Height < 0 ? _selection.Height : 0)
			);
			Rectangle tex = new Rectangle(
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
				_spriteBatch.Draw(
					CreateRectangle(tex.Width, tex.Height, Color.Blue, Color.DarkBlue),
					new Vector2(tex.X, tex.Y), 
					new Color(64, 64, 64, 64)
				);
			}

			// La nuit
			//spriteBatch.Draw(m_night, Vector2.Zero, new Color(0, 0, 220, (int)(64 - 64 * Math.Cos((m_gameTime.TotalGameTime.TotalMilliseconds - m_elapsed) / 50000))));

			// Barres de vie
			foreach (Unit unit in _joueur.Units)
			{
				if (!_healthOver || unit.Selected)
				{ DrawLife(unit.Life, unit.MaxLife, unit.Position - new Vector2((26 - unit.Texture.Width / 4) / 2, 6) - _camera.Position, 28); }
			}
			foreach (Building build in _joueur.Buildings)
			{
				if (!_healthOver || build.Selected)
				{ DrawLife(build.Life, build.MaxLife, build.Position - new Vector2((98 - build.Texture.Width) / 2, 10) - _camera.Position, 100); }
			}
			foreach (Joueur foe in _enemies)
			{
				foreach (Unit unit in foe.Units)
				{
					float mul = 0.0f;
					foreach (Unit uni in _joueur.Units)
					{
						float m = (uni.PositionCenter - unit.Position).Length();
						m = 1.0f - (m / (uni.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					foreach (Building building in _joueur.Buildings)
					{
						float m = (building.PositionCenter - unit.Position).Length();
						m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					if (mul > 0.25f)
					{ DrawLife(unit.Life, unit.MaxLife, unit.Position - new Vector2((26 - unit.Texture.Width / 4) / 2, 6) - _camera.Position, 28); }
				}
				foreach (Building build in foe.Buildings)
				{
					float mul = 0.0f;
					foreach (Unit uni in _joueur.Units)
					{
						float m = (uni.PositionCenter - build.Position).Length();
						m = 1.0f - (m / (uni.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					foreach (Building building in _joueur.Buildings)
					{
						float m = (building.PositionCenter - build.Position).Length();
						m = 1.0f - (m / (building.LineSight + _joueur.AdditionalLineSight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					if (mul > 0.25f)
					{ DrawLife(build.Life, build.MaxLife, build.Position - new Vector2((98 - build.Texture.Width) / 2, 10) - _camera.Position, 100); }
				}
			}

			// Affichage du HUD
			MessagesManager.Draw(_spriteBatch, _fontSmall);
			_hud.Draw(_spriteBatch, _minimap, _joueur, _fontSmall);

			// Unités séléctionnées
			for (int i = 0; i < _selectedList.Count; i++)
			{ _selectedList[i].DrawIcon(_spriteBatch, new Vector2(356 * (_screenSize.X / 1680) + (i % 10) * 36, _screenSize.Y - _hud.Position.Height + 54 * (_screenSize.Y / 1050) + (i / 10) * 36 + _hud.SmartPos)); }

			// List des actions
			for (int i = 0; i < _currentActions.Count; i++)
			{ _spriteBatch.Draw(_actions[_currentActions[i]], new Vector2(_hud.Position.X + 20 + 40 * (i % 6), _hud.Position.Y + 20 + 40 * (i / 6) + _hud.SmartPos), Color.White); }

			// Flash de changement d'ère
			if (FlashBool && a > 0f)
			{
				_spriteBatch.Draw(_flash ,new Rectangle(0, 0, (int) _screenSize.X, (int) _screenSize.Y), new Color(0f, 0f, 0f, a));
				a -= 0.01f;
			}
			else
			{
				FlashBool = false;
				a = 1.0f;
			}

			Debug(3, _units.Count);
			Debug(4, _buildings.Count);
		}

		/// <summary>
		/// Affiche la vie d'une unité ou d'un bâtiment à l'écran.
		/// </summary>
		/// <param name="life">La vie actuelle.</param>
		/// <param name="max">La vie maximale.</param>
		/// <param name="pos">La position où afficher la barre de vie.</param>
		/// <param name="width">La longueur de la barre de vie en pixels.</param>
		private void DrawLife(int life, int max, Vector2 pos, int width)
		{
			int greenLength = (life * (width - 2)) / max;
			int redLength = (width - 2) - greenLength;
			_spriteBatch.Draw(_colors[Color.Black], new Rectangle((int)pos.X - 1, (int)pos.Y - 1, width, 5), Color.White);
			if (greenLength > 0)
			{ _spriteBatch.Draw(_colors[Color.Green], new Rectangle((int)pos.X, (int)pos.Y, greenLength, 3), Color.White); }
			if (redLength > 0)
			{ _spriteBatch.Draw(_colors[Color.Red], new Rectangle((int)pos.X + greenLength, (int)pos.Y, redLength, 3), Color.White); }
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

		// Temprs réel
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
		// Console
		private void Debug(string value)
		{
			Console.Messages.Add(new ConsoleMessage(value));
			System.Diagnostics.Debug.WriteLine(value);
		}
		private void Debug(List<Point> value)
		{
			string deb = "List<" + value.GetType().GetGenericArguments()[0].ToString() + ">(" + value.Count.ToString(CultureInfo.CurrentCulture) + ") { ";
			for (int i = 0; i < value.Count; i++)
			{ deb += value[i] + ", "; }
			Debug(deb.Substring(0, deb.Length - 2) + " }");
		}
		private void Debug(object[] value)
		{
			string deb = value.GetType() + "[" + value.Length.ToString(CultureInfo.CurrentCulture) + "] { ";
			for (int i = 0; i < value.Length; i++)
			{ deb += value[i] + ", "; }
			Debug(deb.Substring(0, deb.Length - 2) + " }");
		}
		private void Debug(bool value)
		{ Debug((value ? "true" : "false")); }
		private void Debug(object value)
		{ Debug(value.ToString()); }

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
		protected void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 coos, Color color, Color borderCol, int border = 0, string spec = "", float scale = 1)
		{
			while (font.MeasureString(text).X * scale > _screenSize.X * 0.36)
			{ font.Spacing--; }

			switch (spec)
			{
				case "Left":
					coos.X = 0;
					break;

				case "Right":
					coos.X = _screenSize.X - font.MeasureString(text).X * scale;
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

		protected void MakeMenu(params string[] args)
		{
			_currentMenus = args;
			for (int i = 0; i < args.Length; i++)
			{ DrawString(_spriteBatch, _fontMenu, _(args[i]), new Vector2(0, i * (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0))) + _screenSize.Y / 5 + (180 * (_screenSize.Y / 1050))), (Menu() == i ? Color.White : Color.Silver), Color.Black, 1, "Center", _screenSize.Y / 1050); }
		}
		protected int Menu()
		{ return Souris.Get().Y > _screenSize.Y / 5 + (180 * (_screenSize.Y / 1050)) && ((Souris.Get().Y - _screenSize.Y / 5 - (180 * (_screenSize.Y / 1050))) % (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0)))) < (_fontMenu.MeasureString("Menu").Y * _screenSize.Y) / 1050 ? (int)((Souris.Get().Y - _screenSize.Y / 5 - (180 * (_screenSize.Y / 1050))) / (_screenSize.Y / (11 + (_currentMenus.Length > 5 ? (_currentMenus.Length - 5) * 2 : 0)))) : -1; }

		protected void MakePauseMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ _spriteBatch.DrawString(_fontMenu, _(args[i]), new Vector2((668 * (_screenSize.X / 1680)), i * 80 + (180 * (_screenSize.Y / 1050))), Color.White); }
		}
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
			var list = new List<Vector2>();
			float w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			foreach (Vector2 vector in l)
			{
				// On ne propose que les résolutions ayant à peu près le même format que l'écran, sans être plus grand que lui, sauf quelques résolutions standard
				if ((Math.Round(vector.X / vector.Y, 2) == Math.Round(w / h, 2) && vector.X <= w && vector.Y <= h) || vector == new Vector2(800, 600))
				{ list.Add(vector); }
			}
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
		/// <returns>La Texture2D générée.</returns>
		private Texture2D CreateRectangle(int width, int height, Color col, Color border)
		{
			var rectangleTexture = new Texture2D(GraphicsDevice, width, height, false, SurfaceFormat.Color);
			var color = new Color[width * height];
			for (int i = 0; i < color.Length; i++)
			{ color[i] = (i < width || i >= color.Length - width || i % width == 0 || i % width == width - 1 ? border : col); }
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
			translations["en"]["Île"] = "Island";
			translations["en"]["Petite"] = "Small";
			translations["en"]["Moyenne"] = "Medium";
			translations["en"]["Grande"] = "Big";
			translations["en"]["rares"] = "rare";
			translations["en"]["normales"] = "normal";
			translations["en"]["abondantes"] = "abundant";
			translations["en"]["Ressources"] = "Resources";
			translations["en"]["Pluvieux"] = "Rainy";
			translations["en"]["Nuageux"] = "Couldy";
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
			translations["en"]["Vous etes connecte"] = "Your are connected";
			translations["en"]["Vous etes deconnecte"] = "Your are not connected";

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

		#endregion
	}
}
