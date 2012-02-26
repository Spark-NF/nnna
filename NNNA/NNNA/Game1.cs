using System;
using System.Collections.Generic;
using System.Drawing;
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

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Effect gaussianBlur;
		private Rectangle m_selection = Rectangle.Empty;
		private GameTime m_gameTime;

		private Texture2D m_background_light, m_background_dark, m_fog, m_menu, m_flash, m_submenu, m_pointer, m_night, m_console, m_background;
		private Dictionary<string, Texture2D> m_actions = new Dictionary<string, Texture2D>();
		private SpriteFont m_font_menu, m_font_menu_title, m_font_small, m_font_credits;
		private Screen m_currentScreen = Screen.Title;
		private int m_konami = 0, m_konamiStatus = 0, m_foes = 1;
		private string m_currentAction = "", m_language = "fr";
		private double m_elapsed;
		private List<string> last_state = new List<string>();
		private string[] m_currentMenus;

		private Vector2 m_screen = new Vector2(1680, 1050);
		private bool m_fullScreen = true, m_shadows = true, m_smart_hud = false, m_health_hover = false, m_showConsole = false;
		private float m_sound_general = 10, m_sound_sfx = 10, m_sound_music = 10, a = 1.0f;
		private int m_textures = 2, m_sound = 2;
        internal static bool flash_bool = false;
        
		private MapType m_quick_type = MapType.Island;
		private int m_quick_size = 1, m_quick_resources = 1, m_credits = 0;
		List<string> m_currentActions = new List<string>();
		Random random = new Random(42);
        
        // Map
		Sprite h, e, p, t, s, i, curseur;
		Sprite[,] matrice;
		Map map;
		Minimap minimap;
		HUD hud;
		Camera2D camera;
		Joueur joueur;
		Joueur[] m_enemies;
		Building selectedBuilding;
		float[,] m_map;
		List<ResourceMine> resource = new List<ResourceMine>();
		List<Unit> selectedList = new List<Unit>();

		// Audio objects		
		float musicVolume = 2.0f;
		Sons son = new Sons();
		SoundEffect _debutpartie;
		SoundEffect _finpartie;
		private ElementHost elementHost;
		private Technologies techno;


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
			Window.Title = "NNNA - " + this.GetType().Assembly.GetName().Version.ToString();

			loadSettings();

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = (int)m_screen.X;
			graphics.PreferredBackBufferHeight = (int)m_screen.Y;
			graphics.IsFullScreen = m_fullScreen;

			Content.RootDirectory = "Content";
		}

		#region Settings

		/// <summary>
		/// Charge les options depuis le fichier "settings.xml".
		/// </summary>
		private void loadSettings()
		{
			m_language = Properties.Settings.Default.Language;
			m_fullScreen = Properties.Settings.Default.FullScreen;
			m_screen.X = Properties.Settings.Default.ScreenWidth;
			m_screen.Y = Properties.Settings.Default.ScreenHeight;
			m_health_hover = Properties.Settings.Default.HealthOver;
			m_smart_hud = Properties.Settings.Default.SmartHUD;
			m_textures = Properties.Settings.Default.Textures;
			m_shadows = Properties.Settings.Default.Shadows;
			m_sound_general = Properties.Settings.Default.SoundGeneral;
			m_sound_music = Properties.Settings.Default.SoundMusic;
			m_sound_sfx = Properties.Settings.Default.SoundSFX;
			m_sound = Properties.Settings.Default.Sound;
		}

		/// <summary>
		/// Enregistre les options dans le fichier "settings.xml".
		/// </summary>
		private void saveSettings()
		{
			Properties.Settings.Default.Language = m_language;
			Properties.Settings.Default.FullScreen = m_fullScreen;
			Properties.Settings.Default.ScreenWidth = (int)m_screen.X;
			Properties.Settings.Default.ScreenHeight = (int)m_screen.Y;
			Properties.Settings.Default.HealthOver = m_health_hover;
			Properties.Settings.Default.SmartHUD = m_smart_hud;
			Properties.Settings.Default.Textures = m_textures;
			Properties.Settings.Default.Shadows = m_shadows;
			Properties.Settings.Default.SoundGeneral = (int)m_sound_general;
			Properties.Settings.Default.SoundMusic = (int)m_sound_music;
			Properties.Settings.Default.SoundSFX = (int)m_sound_sfx;
			Properties.Settings.Default.Sound = m_sound;
			Properties.Settings.Default.Save();
		}

		#endregion

		/// <summary>
		/// Fait toutes les initialisations nécéssaires pour le jeu.
		/// </summary>
		protected override void Initialize()
		{
			h = new Sprite('h');
			e = new Sprite('e');
			t = new Sprite('t');
			p = new Sprite('p');
			s = new Sprite('s');
			i = new Sprite('i');

			map = new Map();
			camera = new Camera2D(0, 0, 10);
            curseur = new Sprite(0, 0);
			joueur = new Joueur(Color.Red, "NNNNA", Content);

			 // son
			son.Initializesons(musicVolume, m_sound_music, m_sound_general);
			_debutpartie = Content.Load<SoundEffect>("sounds/debutpartie");
			_finpartie = Content.Load<SoundEffect>("sounds/sortiedejeu");
			
			//menu technologie
			elementHost= new ElementHost();
			techno = new Technologies(joueur, ref elementHost, Content);

			base.Initialize();

		}

		/// <summary>
		/// Génère une carte aléatoire.
		/// </summary>
		/// <param name="mt">Le type de carte à générer.</param>
		/// <param name="width">La largeur de la carte à générer.</param>
		/// <param name="height">La hauteur de la carte à générer.</param>
		/// <param name="resources">La proportion de resources.</param>
		/// <returns>Un tableau correspondant à la carte.</returns>
		private Sprite[,] generateMap(MapType mt, int width, int height, double resources)
		{
			float[,] map = IslandGenerator.Generate(width, height);
			m_map = map;
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
					Sprite sp = h;
					float m = map[x, y] * 255;
					if (m < waterline - 40)
					{ sp = e; }
					else if (m < waterline)
					{ sp = t; }
					else if (m < waterline + 15)
					{ sp = s; }
					else if (m < waterline + 35)
					{ sp = i; }
					else if (m > 230)
					{ sp = h; }
					else if (m > 205)
					{ sp = h; }
					else
					{ sp = h; }
					matrice[x, y] = sp;
				}
			}

			// On met du sable à côté de l'eau
			for (int x = 0; x < matrice.GetLength(0); x++)
			{
				for (int y = 0; y < matrice.GetLength(1); y++)
				{
					if (matrice[x, y] == h)
					{
						bool waternear = false;
						if (x > 0 && matrice[x - 1, y] == e)
						{ waternear = true; }
						if (y > 0 && matrice[x, y - 1] == e)
						{ waternear = true; }
						if (x < matrice.GetLength(0) - 1 && matrice[x + 1, y] == e)
						{ waternear = true; }
						if (y < matrice.GetLength(1) - 1 && matrice[x, y + 1] == e)
						{ waternear = true; }
						matrice[x, y] = waternear ? p : h;
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
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Backgrounds
			m_fog = Content.Load<Texture2D>("fog");
            m_flash = Content.Load<Texture2D>("flash");

			// Sprites
			m_menu = Content.Load<Texture2D>("menu");
			m_submenu = Content.Load<Texture2D>("submenu");
			m_pointer = Content.Load<Texture2D>("pointer");
			curseur.LoadContent(Content, "pointer");

			// Fontes
			m_font_menu = Content.Load<SpriteFont>("font_menu");
			m_font_menu_title = Content.Load<SpriteFont>("font_menu_title");
			m_font_small = Content.Load<SpriteFont>("font_small");
			m_font_credits = Content.Load<SpriteFont>("font_credits");

			#region Actions
			#region Actions Unités
			m_actions.Add("attack", Content.Load<Texture2D>("Actions/attack"));
			m_actions.Add("gather", Content.Load<Texture2D>("Actions/gather"));
			m_actions.Add("build", Content.Load<Texture2D>("Actions/build"));
			m_actions.Add("build_hutte", Content.Load<Texture2D>("Actions/build_hutte"));
			m_actions.Add("build_hutteDesChasseurs", Content.Load<Texture2D>("Actions/build_hutteDesChasseurs"));
			// m_actions.Add("build_ferme", Content.Load<Texture2D>("Actions/build_ferme"));
			#endregion Actions Unités

			#region Actions Batiments
			m_actions.Add("create_peon", Content.Load<Texture2D>("Actions/create_peon"));
			m_actions.Add("technologies", Content.Load<Texture2D>("Actions/technologies"));
			m_actions.Add("create_guerrier", Content.Load<Texture2D>("Actions/create_guerrier"));
			#endregion Actions Batiments

			#region Actions Communes
			m_actions.Add("retour", Content.Load<Texture2D>("Actions/retour"));
			#endregion Actions Communes
			#endregion Actions

			// Shaders
			gaussianBlur = Content.Load<Effect>("Shaders/GaussianBlur");
			gaussianBlur.CurrentTechnique = gaussianBlur.Techniques["Blur"];

			LoadScreenSizeDependantContent();
		}
		/// <summary>
		/// Charge tous les contenus du jeu dépendant de la résolution de l'écran.
		/// </summary>
		protected void LoadScreenSizeDependantContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			string ratio = ((int)((10 * m_screen.X) / m_screen.Y)).ToString();
			if (ratio != "13" && ratio != "16" && ratio != "18")
			{ ratio = "13"; }
			m_background = Content.Load<Texture2D>("background/" + ratio);
			m_background_light = CreateRectangle((int)(654 * (m_screen.X / 1680)), (int)m_screen.Y, Color.White);
			m_background_dark = CreateRectangle((int)m_screen.X, (int)m_screen.Y, new Color(0, 0, 0, 170));
			m_night = CreateRectangle((int)m_screen.X, (int)m_screen.Y, new Color(0,0,10));
			m_console = CreateRectangle(1, 1, new Color(0, 0, 0, 128));

			//Fenetre des technologies
			elementHost.Location = new System.Drawing.Point((int) m_screen.X/2 - 150, (int) m_screen.Y/2 - 150);
			elementHost.Size = new Size(300, 300);
			elementHost.Child = techno;
            elementHost.BackColor = System.Drawing.Color.Transparent;
            elementHost.BackColorTransparent = true;

			hud = new HUD(0, ((graphics.PreferredBackBufferHeight * 5) / 6) - 10, minimap, m_smart_hud, graphics);
			minimap = new Minimap((hud.Position.Width * 7) / 8 - +hud.Position.Width / 150, hud.Position.Y + hud.Position.Height / 15, (hud.Position.Height * 9) / 10, (hud.Position.Height * 9) / 10);

			MessagesManager.X = (uint)m_screen.X - 400;
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

			if (Clavier.Get().NewPress(Keys.OemQuotes))
			{ m_showConsole = !m_showConsole; }

			// Code Konami
			if (m_konami >= 10)
			{ m_konamiStatus++; }
			if (
				(Clavier.Get().NewPress(Keys.Up)	&& m_konami == 0) ||
				(Clavier.Get().NewPress(Keys.Up)	&& m_konami == 1) ||
				(Clavier.Get().NewPress(Keys.Down)  && m_konami == 2) ||
				(Clavier.Get().NewPress(Keys.Down)  && m_konami == 3) ||
				(Clavier.Get().NewPress(Keys.Left)  && m_konami == 4) ||
				(Clavier.Get().NewPress(Keys.Right) && m_konami == 5) ||
				(Clavier.Get().NewPress(Keys.Left)  && m_konami == 6) ||
				(Clavier.Get().NewPress(Keys.Right) && m_konami == 7) ||
				(Clavier.Get().NewPress(Keys.B)	 && m_konami == 8) ||
				(Clavier.Get().NewPress(Keys.A)	 && m_konami == 9)
			)
			{ m_konami++; }
			else if (Clavier.Get().NewPress())
			{ m_konami = 0; }

			switch (m_currentScreen)
			{
				case Screen.Title:
					UpdateTitle(gameTime);
					break;

				case Screen.Play:
					UpdatePlay(gameTime);
					break;
				case Screen.PlayQuick:
					UpdatePlayQuick(gameTime);
					break;

				case Screen.Options:
					UpdateOptions(gameTime);
					break;
				case Screen.OptionsGeneral:
					UpdateOptionsGeneral(gameTime);
					break;
				case Screen.OptionsGraphics:
					UpdateOptionsGraphics(gameTime);
					break;
				case Screen.OptionsSound:
					UpdateOptionsSound(gameTime);
					break;

				case Screen.Credits:
					UpdateCredits(gameTime);
					break;

				case Screen.Game:
					UpdateGame(gameTime);
					break;
				case Screen.GameMenu:
					UpdateGameMenu(gameTime);
					break;
			}

			//Son 
			son.Engine_menu.Update();
			if (!son.Musiquemenu.IsPlaying && !son.Musiquemenu.IsPaused)
			{ son.Initializesons(musicVolume, m_sound_music, m_sound_general); }

			base.Update(gameTime);
		}
		private void UpdateTitle(GameTime gameTime)
		{
			m_currentScreen = testMenu(Screen.Play, Screen.Options, Screen.Credits, Screen.OptionsSound);
			if (m_currentScreen == Screen.Credits)
			{ m_credits = 0; }
			if (m_currentScreen == Screen.OptionsSound)
			{ this.Exit(); }
		}
		private void UpdatePlay(GameTime gameTime)
		{ m_currentScreen = testMenu(Screen.PlayQuick, Screen.Title); }
		private void UpdatePlayQuick(GameTime gameTime)
		{
			Screen s = testMenu(Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.Game, Screen.Play);
			if (s != Screen.PlayQuick)
			{
				if (s == Screen.Game)
				{
					bool ok = false;
					int[] sizes = { 50, 100, 200 };
					double[] resources = { 0.05, 0.1, 0.2 };
					List<Point> spawns = new List<Point>();
					List<float> heights = new List<float>();
					float dist = sizes[m_quick_size] / 2;

					// On regénère une carte tant qu'elle est incapable d'accueillir le bon nombre de spawns
					while (!ok)
					{
						// Génération
						matrice = generateMap(m_quick_type, sizes[m_quick_size], sizes[m_quick_size], resources[m_quick_resources]);

						// Spawns
						heights.Clear();
						for (int x = 0; x < m_map.GetLength(0); x++)
						{
							for (int y = 0; y < m_map.GetLength(1); y++)
							{ heights.Add(m_map[x, y] * 255); }
						}
						float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];
						List<float> heightsOr = new List<float>(heights);
						heights.Sort((x, y) => (y.CompareTo(x)));
						spawns.Clear();
						int j = 0;

						// On génère $m_foes + 1 spawns, chacun espacés d'au moins $dist
						while (spawns.Count < m_foes + 1 && j < heights.IndexOf(waterline))
						{
							Point point = new Point(heightsOr.IndexOf(heights[j]) % m_map.GetLength(1), heightsOr.IndexOf(heights[j]) / m_map.GetLength(1));
							bool isNear = false;
							foreach (Point p in spawns)
							{
								if (point.distanceTo(p) <= dist)
								{ isNear = true; }
							}
							if (!isNear)
							{ spawns.Add(point); }
							j++;
						}

						if (spawns.Count == m_foes + 1)
						{ ok = true; }
					}

					// Les couleurs et noms des joueurs
					Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Yellow };
					string[] names = { Environment.UserName, "Lord Lard", "Herr von Speck", "Monsieur Martin" };

					//Joueur
					joueur = new Joueur(colors[0], names[0], Content);
					joueur.Units.Add(new Guerrier((int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 100, (int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, joueur, false));
					joueur.Units.Add(new Guerrier((int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 0, (int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, joueur, false));
					joueur.Units.Add(new Peon((int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X + 50, (int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y + 155, Content, joueur, false));
					joueur.Buildings.Add(new Grande_Hutte((int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).X, (int)matrice2xy(new Vector2(spawns[0].X - 1, spawns[0].Y - 1)).Y, Content, joueur));
					camera.Position = matrice2xy(new Vector2(spawns[0].X + 7, spawns[0].Y + 5)) - m_screen / 2;

					// Ennemis
					m_enemies = new Joueur[m_foes];
					for (int i = 0; i < m_foes; i++)
					{
						m_enemies[i] = new Joueur(colors[i + 1], names[i + 1], Content);
						m_enemies[i].Units.Add(new Guerrier((int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 100, (int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, joueur, false));
						m_enemies[i].Units.Add(new Guerrier((int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 0, (int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, joueur, false));
						m_enemies[i].Units.Add(new Peon((int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X + 50, (int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y + 155, Content, joueur, false));
						m_enemies[i].Buildings.Add(new Grande_Hutte((int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).X, (int)matrice2xy(new Vector2(spawns[i + 1].X - 1, spawns[i + 1].Y - 1)).Y, Content, joueur));
					}

					//Le reste
					techno.Reset();
					map.LoadContent(matrice, Content, minimap, graphics.GraphicsDevice);
					hud.LoadContent(Content, "HUD/hud2");
					minimap.LoadContent(map);
					m_elapsed = gameTime.TotalGameTime.TotalMilliseconds;
					m_gameTime = gameTime;

                    //Decor
                    resource.Add(new ResourceMine((int)matrice2xy(new Vector2(spawns[0].X + 10, spawns[0].Y + 10)).X, (int)matrice2xy(new Vector2(spawns[0].X + 5, spawns[0].Y + 2)).Y, Content, joueur.Resource("Pierre")));
                    for (int i = 0; i < 15; i++)
                    {
                        int x = 0;
                        int y = 0;
                        x = random.Next(matrice.GetLength(0));
                        y = random.Next(matrice.GetLength(1));
                        while ((!matrice[y, x].Crossable))
                        {
                            x = random.Next(matrice.GetLength(0));
                            y = random.Next(matrice.GetLength(1));
                        }
                        resource.Add(new ResourceMine((int)(matrice2xy(new Vector2(x, y))).X - 44, (int)(matrice2xy(new Vector2(x, y))).Y - 152, Content, joueur.Resource("Bois")));
                        matrice[y, x].Crossable = false;
                    }

                    //Le son
					son.Musiquemenu.Pause();
					_debutpartie.Play();
					
				}
				m_currentScreen = s;
			}
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				switch (m)
				{
					case 0: m_quick_type = (MapType)Variate(0, Enum.GetValues(typeof(MapType)).Length - 1, (int)m_quick_type); break;
					case 1: m_quick_size = Variate(0, 2, m_quick_size); break;
					case 2: m_quick_resources = Variate(0, 2, m_quick_resources); break;
					case 3: m_foes = Variate(1, 3, m_foes); break;
				}
			}
		}
		private void UpdateOptions(GameTime gameTime)
		{
			m_currentScreen = testMenu(Screen.OptionsGeneral, Screen.OptionsGraphics, Screen.OptionsSound, Screen.Title);
			if (m_currentScreen == Screen.Title)
			{ saveSettings(); }
		}
		private void UpdateOptionsGeneral(GameTime gameTime)
		{
			Screen s = testMenu(Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.Options);
			if (s != Screen.OptionsGeneral)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				switch (m)
				{
					case 0: m_language = (m_language == "en" ? "fr" : "en"); break;
					case 1: m_health_hover = !m_health_hover; break;
					case 2: m_smart_hud = !m_smart_hud; break;
				}
			}
		}
		private void UpdateOptionsGraphics(GameTime gameTime)
		{
			Screen s = testMenu(Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.Options);
			if (s != Screen.OptionsGraphics)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				switch (m)
				{
					case 0:
						m_screen = getNextResolution(Souris.Get().Clicked(MouseButton.Right));
						graphics.PreferredBackBufferWidth = (int)m_screen.X;
						graphics.PreferredBackBufferHeight = (int)m_screen.Y;
						LoadScreenSizeDependantContent();
						graphics.ApplyChanges();
						break;

					case 1:
						m_fullScreen = !m_fullScreen;
						if (!m_fullScreen)
						{ Forms.Application.EnableVisualStyles(); }
						graphics.IsFullScreen = m_fullScreen;
						graphics.ApplyChanges();
						break;

					case 2: m_textures = Variate(0, 2, m_textures); break;
					case 3: m_shadows = !m_shadows; break;
				}
			}
		}
		private void UpdateOptionsSound(GameTime gameTime)
		{
			Screen s = testMenu(Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.Options);
			if (s != Screen.OptionsSound)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				switch (m)
				{
					case 0: m_sound_general = Variate(0, 10, (int)m_sound_general); break;
					case 1: m_sound_music = Variate(0, 10, (int)m_sound_music); break;
					case 2: m_sound_sfx = Variate(0, 10, (int)m_sound_sfx); break;
					case 3: m_sound = Variate(0, 2, (int)m_sound); break;
				}
			}
			son.MusicCategory.SetVolume(musicVolume * m_sound_music * (m_sound_general / 10));

		}
		private void UpdateCredits(GameTime gameTime)
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ m_currentScreen = Screen.Title; }
			m_credits++;
		}
		float compt = 0;
		private void UpdateGame(GameTime gameTime)
		{
            techno.Pre_Update(joueur);
            System.Windows.Forms.Control.FromHandle(this.Window.Handle).Controls.Add(elementHost);
            techno.Post_Update(joueur);

            //if (isbuilding)
            //{
            //    Vector2 xy = matrice2xy(xy2matrice(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
            //    Mouse.SetPosition((int) xy.X, (int) xy.Y);
            //}

			if (Clavier.Get().NewPress(Keys.Escape))
			{
				elementHost.Visible = false;
				m_currentScreen = Screen.GameMenu;
			}

			compt = (compt + gameTime.ElapsedGameTime.Milliseconds * 0.1f) % 100;
			curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			camera.Update(curseur, graphics);

			// Intelligence artificielle
			foreach (Joueur foe in m_enemies)
			{
				// FAIRE TOUTES LES MISES à JOUR DE L'IA ICI
			}
			// Rectangle de séléction
			if (Souris.Get().Clicked(MouseButton.Left))
			{
				Building b;
				Unit u;
				switch (m_currentAction)
				{
						// Ere 1 
					case "build_hutte":
						b = new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, joueur, (byte)random.Next(0,2));
						if (joueur.Pay(b.Prix))
						{
							joueur.Buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
							m_pointer = Content.Load<Texture2D>("pointer");
							m_currentAction = "";
						}
						else
                        {
                            MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); m_pointer = Content.Load<Texture2D>("pointer"); m_currentAction = "";
                        }
						break;

					case "build_hutteDesChasseurs":
						b = new Hutte_des_chasseurs((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, joueur);
                        if (joueur.Pay(b.Prix))
						{
							joueur.Buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
							m_pointer = Content.Load<Texture2D>("pointer");
							m_currentAction = "";
						}
						else
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); m_pointer = Content.Load<Texture2D>("pointer"); m_currentAction = ""; }
						break;

					case "create_peon" :
						u = new Peon((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 5), (int)selectedBuilding.Position.Y + 155, Content, joueur, false);
						if (joueur.Population + 1 > joueur.Population_Max && joueur.Pay(u.Prix))
						{
							selectedBuilding.Iterator++;
							joueur.Units.Add(u);
							MessagesManager.Messages.Add(new Msg("Nouveau Peon !", Color.White, 5000));
							m_currentAction = "";
						}
						else
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); joueur.Population--; m_currentAction = ""; }
						break;

					case "create_guerrier":
						u = new Guerrier((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 3), (int)selectedBuilding.Position.Y + 70, Content, joueur, false);
                        if (joueur.Population + 1 > joueur.Population_Max && joueur.Pay(u.Prix))
						{
							selectedBuilding.Iterator++;
							joueur.Units.Add(u);
							MessagesManager.Messages.Add(new Msg("Nouveau Guerrier !", Color.White, 5000));
							m_currentAction = "";
						}
						else
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); joueur.Population--; m_currentAction = ""; }
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
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); m_pointer = Content.Load<Texture2D>("pointer"); m_currentAction = ""; }
                        break;
                    Fin Ere 2 */

                    default:
						m_selection = new Rectangle(Souris.Get().X + (int)camera.Position.X, Souris.Get().Y + (int)camera.Position.Y, 0, 0);
						m_currentAction = "select";
						break;
				}
			}
			else if (Souris.Get().Hold(MouseButton.Left) && m_currentAction == "select")
			{
				m_selection.Width = Souris.Get().X + (int)camera.Position.X - m_selection.X;
				m_selection.Height = Souris.Get().Y + (int)camera.Position.Y - m_selection.Y;
			}
			else if (Souris.Get().Hold(MouseButton.Left, ButtonState.Released))
			{ m_selection = Rectangle.Empty; }

			bool change;
			if (!m_selection.IsEmpty && Souris.Get().Released(MouseButton.Left) && (curseur.Position.Y <= hud.Position.Y + 20 || m_selection.Y - camera.Position.Y <= hud.Position.Y + 20))
			{
				// On met à jour les séléctions
				change = false;
				if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !Keyboard.GetState().IsKeyDown(Keys.RightControl))
				{
					foreach (Movible_Sprite sprite in selectedList)
					{ sprite.Selected = false; }
					selectedList.Clear();
					if (selectedBuilding != null)
					{ selectedBuilding.Selected = false; }
					selectedBuilding = null;
				}
				foreach (Unit sprite in joueur.Units)
				{
					Rectangle csel = new Rectangle((int)(m_selection.X - camera.Position.X + (m_selection.Width < 0 ? m_selection.Width : 0)), (int)(m_selection.Y - camera.Position.Y + (m_selection.Height < 0 ? m_selection.Height : 0)), (int)Math.Abs(m_selection.Width), (int)Math.Abs(m_selection.Height));
					if (!sprite.Selected && csel.Intersects(sprite.Rectangle(camera)))
					{
						sprite.Selected = true;
						selectedList.Add(sprite);
						change = true;
					}
				}
				if (!change)
				{
					foreach (Movible_Sprite sprite in selectedList)
					{ sprite.Selected = false; }
					selectedList.Clear();

					foreach (Building sprite in joueur.Buildings)
					{
						Rectangle csel = new Rectangle((int)(m_selection.X - camera.Position.X + (m_selection.Width < 0 ? m_selection.Width : 0)), (int)(m_selection.Y - camera.Position.Y + (m_selection.Height < 0 ? m_selection.Height : 0)), (int)Math.Abs(m_selection.Width), (int)Math.Abs(m_selection.Height));
						if (!sprite.Selected && csel.Intersects(sprite.Rectangle(camera)))
						{
							sprite.Selected = true;
							selectedBuilding = sprite;
							change = true;
							break;
						}
					}

					if (!change)
					{
						if (selectedBuilding != null)
						{ selectedBuilding.Selected = false; }
						selectedBuilding = null;
					}
				}

				// On met à jour les actions
				m_currentActions.Clear();
				if (selectedList.Count > 0)
				{
					bool all_same = true;
					string type = "";
					m_currentActions.Add("attack");
					foreach (Movible_Sprite sprite in selectedList)
					{
						if (sprite.Type != type)
						{
							if (type != "")
							{ all_same = false; }
							type = sprite.Type;
							if (sprite.Type == "peon" && !m_currentActions.Contains("build"))
							{
								m_currentActions.Add("gather");
								m_currentActions.Add("build");
							}
						}
					}
					if (!all_same)
					{
						m_currentActions.Clear();
						m_currentActions.Add("attack");
					}
				}
				else if (selectedBuilding != null)
				{
					switch (selectedBuilding.Type)
					{
						case "forum" :
							m_currentActions.Add("create_peon");
							m_currentActions.Add("technologies");
							break;

						case "caserne" :
							m_currentActions.Add("create_guerrier");
							break;
					}
				}
				m_currentAction = "";
				last_state.Clear();
				for (int i = 0; i < m_currentActions.Count; i++)
				{
					last_state.Add(m_currentActions[i]);
				}
			}

			// Actions
            if (Souris.Get().Clicked(MouseButton.Left) && Souris.Get().X >= hud.Position.X + 20 && Souris.Get().Y >= hud.Position.Y + 20 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
            {
                int x = Souris.Get().X - hud.Position.X - 20, y = Souris.Get().Y - hud.Position.Y - 20;
                if (x % 40 < 32 && y % 40 < 32 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
                {
                    x /= 40;
                    y /= 40;
                    int pos = x + 6 * y;
                    if (pos < m_currentActions.Count || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V))
                    {
                        string act = "";
                        if (Clavier.Get().NewPress(Keys.C))
                        { act = "build_hutte"; }
                        else if (Clavier.Get().NewPress(Keys.V))
                        { act = "build_hutteDesChasseurs"; }
                        else
                        { act = m_currentActions[pos]; }
                        Debug(act);
                        switch (act)
                        {
                            case "attack":
                                break;

                            case "build":
                                m_currentActions.Clear();
                                m_currentActions.Add("build_hutte");
                                m_currentActions.Add("build_hutteDesChasseurs");
                                m_currentActions.Add("retour");
                                break;

                            case "gather":
                                break;

                            case "build_hutte":
                                if (joueur.Has(new Hutte().Prix))
                                {
                                    if (random.Next(0, 2) == 0)
                                        m_pointer = Content.Load<Texture2D>("Batiments/maison1_" + joueur.Ere.ToString());
                                    else
                                        m_pointer = Content.Load<Texture2D>("Batiments/maison2_" + joueur.Ere.ToString());
                                    m_currentAction = "build_hutte";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                            case "build_hutteDesChasseurs":
                                if (joueur.Has(new Hutte_des_chasseurs().Prix))
                                {
                                    m_pointer = Content.Load<Texture2D>("Batiments/caserne_" + joueur.Ere.ToString());
                                    m_currentAction = "build_hutteDesChasseurs";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                            case "retour":
                                m_currentActions.Clear();
                                for (int i = 0; i < last_state.Count; i++)
                                {
                                    m_currentActions.Add(last_state[i]);
                                }
                                break;

                            case "create_peon":
                                Peon u = new Peon((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 5), (int)selectedBuilding.Position.Y + 155, Content, joueur);
                                if (joueur.Pay(u.Prix))
                                {
                                    selectedBuilding.Iterator++;
                                    joueur.Units.Add(u);
                                    MessagesManager.Messages.Add(new Msg("Nouveau Peon !", Color.White, 5000));
                                    m_currentAction = "";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                            case "technologies":
                                elementHost.Visible = true;
                                break;

                            case "create_guerrier":
                                Guerrier u1 = new Guerrier((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 3), (int)selectedBuilding.Position.Y + 70, Content, joueur);
                                if (joueur.Has(u1.Prix))
                                {
                                    selectedBuilding.Iterator++;
                                    joueur.Units.Add(u1);
                                    MessagesManager.Messages.Add(new Msg("Nouveau Chasseur !", Color.White, 5000));
                                    m_currentAction = "";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                            /* Ere 2 
                        case "build_ferme":
                            if (joueur.Has(new Ferme().Prix))
                            {
                                m_pointer = Content.Load<Texture2D>("Batiments/ferme");
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
			foreach (Unit sprite in joueur.Units)
			{ sprite.ClickMouvement(curseur, gameTime, camera, hud, joueur.Units, joueur.Buildings, matrice); }

            joueur.Units.Sort(Sprite.CompareByY);
            joueur.Buildings.Sort(Sprite.CompareByY);
             
			//minimap.Update(units, buildings, selectedList, joueur);
			son.Musiquemenu.Pause();
		}
		void UpdateGameMenu(GameTime gameTime)
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ m_currentScreen = Screen.Game; }
			curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			m_currentScreen = testPauseMenu(Screen.Title, Screen.Game);
			if (!son.Musiquemenu.IsPlaying && !son.Musiquemenu.IsPaused)
			{ son.Initializesons(musicVolume, m_sound_music, m_sound_general); }
			son.Musiquemenu.Resume();
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
			spriteBatch.Begin();

			switch (m_currentScreen)
			{
				case Screen.Title:
					DrawTitle(gameTime);
					break;

				case Screen.Play:
					DrawPlay(gameTime);
					break;
				case Screen.PlayQuick:
					DrawPlayQuick(gameTime);
					break;

				case Screen.Options:
					DrawOptions(gameTime);
					break;
				case Screen.OptionsGeneral:
					DrawOptionsGeneral(gameTime);
					break;
				case Screen.OptionsGraphics:
					DrawOptionsGraphics(gameTime);
					break;
				case Screen.OptionsSound:
					DrawOptionsSound(gameTime);
					break;

				case Screen.Credits:
					DrawCredits(gameTime);
					break;

				case Screen.Game:
					DrawGame(gameTime);
					break;
				case Screen.GameMenu:
					DrawGameMenu(gameTime);
					break;
			}

			// Code Konami
			if (m_konami >= 10)
			{
				joueur.Resource("Bois").Add(5000);
				joueur.Resource("Pierre").Add(5000);
				joueur.Resource("Nourriture").Add(5000);
				m_konami = 0;
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

			if (m_showConsole)
			{
				List<ConsoleMessage> msgs = Console.GetLast(10);
				spriteBatch.Draw(m_console, new Rectangle(0, 0, (int)m_screen.X, 26 * msgs.Count), Color.White);
				for (int i = msgs.Count; i > 0; i--)
				{ spriteBatch.DrawString(m_font_small, msgs[msgs.Count - i].Message, new Vector2(5, 26 * i - 24), Color.White); }
			}

			DrawPointer(gameTime);
			
			spriteBatch.End();
			base.Draw(gameTime);
		}

		private void DrawCommon(GameTime gameTime, bool drawText = true)
		{
			// Le fond d'écran
			Rectangle screenRectangle = new Rectangle(0, 0, (int)m_screen.X, (int)m_screen.Y);
			spriteBatch.Draw(m_background, screenRectangle, Color.White);

			if (drawText)
			{
				// Titre
				DrawString(spriteBatch, m_font_menu_title, "NNNA", new Vector2((m_screen.X - m_font_menu_title.MeasureString("NNNA").X) / 2, m_screen.Y / 13), Color.Red, Color.Black, 1);

				// Version
				spriteBatch.DrawString(m_font_small, this.GetType().Assembly.GetName().Version.ToString(), new Vector2((m_screen.X - m_font_small.MeasureString(this.GetType().Assembly.GetName().Version.ToString()).X) / 2, m_screen.Y - 50), Color.GhostWhite);
			}
		}
		private void DrawPointer(GameTime gameTime)
		{ spriteBatch.Draw(m_pointer, new Vector2(Souris.Get().X, Souris.Get().Y), Color.White); }
		private void DrawTitle(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Jouer", "Options", "Crédits", "Quitter");
		}
		private void DrawPlay(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Escarmouche", "Retour");
		}
		private void DrawPlayQuick(GameTime gameTime)
		{
			DrawCommon(gameTime);
			string[] types = { "Île" };
			string[] tailles = { "Petite", "Moyenne", "Grande" };
			string[] ressources = { "rares", "normales", "abondantes" };
			makeMenu(types[(int)m_quick_type], tailles[m_quick_size], _("Ressources")+" " + _(ressources[m_quick_resources]), _("Ennemis :")+" " + m_foes.ToString(), "Jouer", "Retour");
		}
		private void DrawOptions(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Jouabilité", "Graphismes", "Son", "Retour");
		}
		private void DrawOptionsGeneral(GameTime gameTime)
		{
			DrawCommon(gameTime);
			Dictionary<string, string> languages = new Dictionary<string, string>();
			languages["en"] = "English";
			languages["fr"] = "Français";
			makeMenu(languages[m_language], (m_health_hover ? "Vie au survol" : "Vie constante"), (m_smart_hud ? "HUD intelligent" : "HUD classique"), "Retour");
		}
		private void DrawOptionsGraphics(GameTime gameTime)
		{
			DrawCommon(gameTime);
			string[] textures = { "min", "moyennes", "max" };
			makeMenu(m_screen.X + "x" + m_screen.Y, (m_fullScreen ? "Plein écran" : "Fenêtré"), _("Textures")+" " + _(textures[m_textures]), (m_shadows ? "Ombres" : "Pas d'ombres"), "Retour");
		}
		private void DrawOptionsSound(GameTime gameTime)
		{
			DrawCommon(gameTime);
			string[] sound = { "min", "moy", "max" };
			makeMenu(_("Général :") + " " + m_sound_general, _("Musique :") + " " + m_sound_music, _("Effets :") + " " + m_sound_sfx, _("Qualité") + " " + _(sound[m_sound]), "Retour");
		}
		private void DrawCredits(GameTime gameTime)
		{
			DrawCommon(gameTime, false);

			// Crédits
			int y = m_credits / 2;
			if (y > m_screen.Y + 200 + (m_font_credits.MeasureString(_("Merci d'avoir joué !")).Y / 2))
			{ y = (int)(m_screen.Y + 200 + (m_font_credits.MeasureString(_("Merci d'avoir joué !")).Y / 2)); }

			spriteBatch.DrawString(m_font_credits, "Nicolas Allain", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Allain").X) / 2, m_screen.Y - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Nicolas Faure", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Faure").X) / 2, m_screen.Y + 60 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Nicolas Mouton-Besson", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Mouton-Besson").X) / 2, m_screen.Y + 120 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Arnaud Weiss", new Vector2((m_screen.X - m_font_credits.MeasureString("Arnaud Weiss").X) / 2, m_screen.Y + 180 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, _("Merci d'avoir joué !"), new Vector2((m_screen.X - m_font_credits.MeasureString(_("Merci d'avoir joué !")).X) / 2, m_screen.Y + (m_screen.Y / 2) + 180 + (m_font_credits.MeasureString(_("Merci d'avoir joué !")).Y / 2) - y), Color.White);
		}
		private void DrawGame(GameTime gameTime)
		{
			int index = (int)Math.Floor(compt / 25);
			int compteur = 0;
			foreach (Sprite sprite in matrice)
			{
				if (sprite.Position.X - camera.Position.X > -64
					&& sprite.Position.Y - camera.Position.Y > -32
					&& sprite.Position.X - camera.Position.X < m_screen.X
					&& sprite.Position.Y - camera.Position.Y < m_screen.Y - ((hud.Position.Height * 4) / 5))
				{
					float mul = 0.0f;
					foreach (Unit unit in joueur.Units)
					{
						float m = (unit.Position_Center - sprite.Position_Center).Length();
						m = 1.0f - (m / (unit.Line_sight + joueur.Additional_line_sight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					foreach (Building building in joueur.Buildings)
					{
						float m = (building.Position_Center - sprite.Position_Center).Length();
						m = 1.0f - (m / (building.Line_sight + joueur.Additional_line_sight));
						mul = (m > 0 && m > mul) ? m : mul;
					}
					sprite.DrawMap(spriteBatch, camera, mul);
					compteur++;
				}
			}

			// Affichage des objets sur la carte
            foreach (Joueur foe in m_enemies)
            {
                foreach (Building build in foe.Buildings)
                {
                    float mul = 0.0f;
                    foreach (Unit unit in joueur.Units)
                    {
                        float m = (unit.Position_Center - build.Position).Length();
                        m = 1.0f - (m / (unit.Line_sight + joueur.Additional_line_sight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    foreach (Building building in joueur.Buildings)
                    {
                        float m = (building.Position_Center - build.Position).Length();
                        m = 1.0f - (m / (building.Line_sight + joueur.Additional_line_sight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    build.Draw(spriteBatch, camera, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255));
                }
                foreach (Unit uni in foe.Units)
                {
                    float mul = 0.0f;
                    foreach (Unit unit in joueur.Units)
                    {
                        float m = (unit.Position_Center - uni.Position).Length();
                        m = 1.0f - (m / (unit.Line_sight + joueur.Additional_line_sight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    foreach (Building building in joueur.Buildings)
                    {
                        float m = (building.Position_Center - uni.Position).Length();
                        m = 1.0f - (m / (building.Line_sight + joueur.Additional_line_sight));
                        mul = (m > 0 && m > mul) ? m : mul;
                    }
                    uni.Draw(spriteBatch, camera, index, new Color((mul * foe.ColorMovable.R) / 255, (mul * foe.ColorMovable.G) / 255, (mul * foe.ColorMovable.B) / 255));
                }
            }
            joueur.Draw(spriteBatch, camera, index);
            foreach (ResourceMine sprite in resource)
            {
                float mul = 0.0f;
                foreach (Unit unit in joueur.Units)
                {
                    float m = (unit.Position_Center - sprite.Position_Center).Length();
                    m = 1.0f - (m / (unit.Line_sight + joueur.Additional_line_sight));
                    mul = (m > 0 && m > mul) ? m : mul;
                }
                foreach (Building building in joueur.Buildings)
                {
                    float m = (building.Position_Center - sprite.Position_Center).Length();
                    m = 1.0f - (m / (building.Line_sight + joueur.Additional_line_sight));
                    mul = (m > 0 && m > mul) ? m : mul;
                }
                sprite.Draw(spriteBatch, 1, camera, mul);
            }
			// Rectangle de séléction
			Vector2 coos = new Vector2(
				m_selection.X - camera.Position.X + (m_selection.Width < 0 ? m_selection.Width : 0),
				m_selection.Y - camera.Position.Y + (m_selection.Height < 0 ? m_selection.Height : 0)
			);
			Rectangle tex = new Rectangle(
				coos.X < 0 ? 0 : (int)coos.X,
				coos.Y < 0 ? 0 : (int)coos.Y, 
				(int)(Math.Abs(m_selection.Width) + (coos.X < 0 ? coos.X : 0)),
				(int)(Math.Abs(m_selection.Height) + (coos.Y < 0 ? coos.Y : 0))
			);
			if (tex.Width + tex.X > m_screen.X)
			{ tex.Width = (int)m_screen.X - tex.X; }
			if (tex.Height + tex.Y > m_screen.Y)
			{ tex.Height = (int)m_screen.Y - tex.Y; }
			if ((int)Math.Abs(m_selection.Width) > 0 && (int)Math.Abs(m_selection.Height) > 0)
			{
				spriteBatch.Draw(
					CreateRectangle(tex.Width, tex.Height, Color.Blue, Color.DarkBlue),
					new Vector2(tex.X, tex.Y), 
					new Color(64, 64, 64, 64)
				);
			}

			// La nuit
			//spriteBatch.Draw(m_night, Vector2.Zero, new Color(0, 0, 220, (int)(64 - 64 * Math.Cos((m_gameTime.TotalGameTime.TotalMilliseconds - m_elapsed) / 50000))));

			// Affichage du HUD
			MessagesManager.Draw(spriteBatch, m_font_small);
			hud.Draw(spriteBatch, minimap, joueur, m_font_small);

			// Unités séléctionnées
			for (int i = 0; i < selectedList.Count; i++)
			{ selectedList[i].DrawIcon(spriteBatch, new Vector2(356 * (m_screen.X / 1680) + (i % 10) * 36, m_screen.Y - hud.Position.Height + 54 * (m_screen.Y / 1050) + (i / 10) * 36)); }

			// List des actions
			for (int i = 0; i < m_currentActions.Count; i++)
			{ spriteBatch.Draw(m_actions[m_currentActions[i]], new Vector2(hud.Position.X + 20 + 40 * (i % 6), hud.Position.Y + 20 + 40 * (i / 6)), Color.White); }

            if (flash_bool && a > 0f)
            {
                spriteBatch.Draw(m_flash ,new Rectangle(0, 0, (int) m_screen.X, (int) m_screen.Y), new Color(0f, 0f, 0f, a));
                a -= 0.01f;
            }
            else
            {
                flash_bool = false;
                a = 1.0f;
            }
        }
		private void DrawGameMenu(GameTime gameTime)
		{
			foreach (EffectPass pass in gaussianBlur.CurrentTechnique.Passes)
			{
				DrawGame(gameTime);
				pass.Apply();
			}
			spriteBatch.Draw(m_background_dark, Vector2.Zero, Color.White);
			makePauseMenu("Quitter", "Retour");
		}

		#endregion

		#region Debug

		// Temprs réel
		private void Debug(int i, string value)
		{
			#if DEBUG
				spriteBatch.DrawString(m_font_small, value, new Vector2(10, 10 + i * 20), Color.White);
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
			string deb = "List<" + value.GetType().GetGenericArguments()[0].ToString() + ">(" + value.Count.ToString() + ") { ";
			for (int i = 0; i < value.Count; i++)
			{ deb += value[i].ToString() + ", "; }
			Debug(deb.Substring(0, deb.Length - 2) + " }");
		}
		private void Debug(object[] value)
		{
			string deb = value.GetType() + "[" + value.Length.ToString() + "] { ";
			for (int i = 0; i < value.Length; i++)
			{ deb += value[i].ToString() + ", "; }
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
		protected void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 coos, Color color, Color borderCol, int border = 0, string spec = "", float scale = 1)
		{
			while (font.MeasureString(text).X * scale > m_screen.X * 0.36)
			{ font.Spacing--; }

			switch (spec)
			{
				case "Left":
					coos.X = 0;
					break;

				case "Right":
					coos.X = m_screen.X - font.MeasureString(text).X * scale;
					break;

				case "Center":
					coos.X = (m_screen.X - font.MeasureString(text).X * scale) / 2;
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

		protected void makeMenu(params string[] args)
		{
			m_currentMenus = args;
			for (int i = 0; i < args.Length; i++)
			{ DrawString(spriteBatch, m_font_menu, _(args[i]), new Vector2(0, i * (m_screen.Y / (11 + (m_currentMenus.Length > 5 ? 2 : 0))) + m_screen.Y / 5 + (180 * (m_screen.Y / 1050))), (menu() == i ? Color.White : Color.Silver), Color.Black, 1, "Center", m_screen.Y / 1050); }
		}
		protected int menu()
		{ return Souris.Get().Y > m_screen.Y / 5 + (180 * (m_screen.Y / 1050)) && ((Souris.Get().Y - m_screen.Y / 5 - (180 * (m_screen.Y / 1050))) % (m_screen.Y / (11 + (m_currentMenus.Length > 5 ? 2 : 0)))) < (m_font_menu.MeasureString("Menu").Y * m_screen.Y) / 1050 ? (int)((Souris.Get().Y - m_screen.Y / 5 - (180 * (m_screen.Y / 1050))) / (m_screen.Y / (11 + (m_currentMenus.Length > 5 ? 2 : 0)))) : -1; }

		protected void makePauseMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ spriteBatch.DrawString(m_font_menu, _(args[i]), new Vector2((668 * (m_screen.X / 1680)), i * 80 + (180 * (m_screen.Y / 1050))), Color.White); }
		}
		protected int pauseMenu()
		{ return (Souris.Get().X > (654 * (m_screen.X / 1680)) && Souris.Get().Y > (180 * (m_screen.Y / 1050))) ? (int)((Souris.Get().Y - (180 * (m_screen.Y / 1050))) / 80) : -1; }

		/// <summary>
		/// Teste si un menu est cliqué.
		/// </summary>
		/// <param name="up">Le menu sur lequel revenir si on est déjà sur le menu cliqué.</param>
		/// <param name="args">La liste des menus possibles.</param>
		/// <returns>Le menu cliqué.</returns>
		protected Screen testMenu(params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				if (m >= 0 && m < args.Length)
				{ return args[m]; }
			}
			return m_currentScreen;
		}

		/// <summary>
		/// Teste si un menu pause est cliqué.
		/// </summary>
		/// <param name="args">La liste des menus possibles.</param>
		/// <returns>Le menu cliqué.</returns>
		protected Screen testPauseMenu(params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = pauseMenu();
				if (m >= 0 && m < args.Length)
				{ return args[m]; }
			}
			return m_currentScreen;
		}

		#endregion

		/// <summary>
		/// Retourne la prochaine résolution disponible.
		/// </summary>
		/// <returns>La prochaine résolution dans l'ordre croissant.</returns>
		private Vector2 getNextResolution(bool previous = false)
		{
			Vector2[] l = { // Megaliste ! Yay ! // MDR
				/*new Vector2(320, 200), Les petites résolutions servent à rien
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
			List<Vector2> list = new List<Vector2>();
			float w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			foreach (Vector2 vector in l)
			{
				// On ne propose que les résolutions ayant à peu près le même format que l'écran, sans être plus grand que lui, sauf quelques résolutions standard
				if ((Math.Round(vector.X / vector.Y, 2) == Math.Round(w / h, 2) && vector.X <= w && vector.Y <= h) || vector == new Vector2(800, 600))
				{ list.Add(vector); }
			}
			Vector2 next;
			if (list.Contains(m_screen))
			{
				int index = list.IndexOf(m_screen);
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
			Texture2D rectangleTexture = new Texture2D(GraphicsDevice, width, height, false, SurfaceFormat.Color);
			Color[] color = new Color[width * height];
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
		/// <param name="text"></param>
		/// <returns></returns>
		private string Translate(string text, string lang = "")
		{
			Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();
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
			translations["en"]["moy"] = "med";
			translations["en"]["Général :"] = "General:";
			translations["en"]["Musique :"] = "Music:";
			translations["en"]["Effets :"] = "Effects:";
			translations["en"]["Qualité"] = "Quality";
			translations["en"]["Merci d'avoir joué !"] = "Thanks for playing!";

			if (lang == "")
			{ lang = m_language; }

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
		/// <param name="text"></param>
		/// <returns></returns>
		private string _(string text, string lang = "")
		{ return Translate(text, lang);  }

		#region Conversions

		/// <summary>
		/// Convertit un booléen en entier.
		/// </summary>
		/// <param name="b">Le booléen à convertir.</param>
		/// <returns>L'entier binaire correspondant.</returns>
		protected int b2i(bool b)
		{ return b ? 1 : 0; }

		/// <summary>
		/// Transforme des coordonnées losange en coordonnées matrice.
		/// </summary>
		/// <param name="mouse">Coordonnées losange.</param>
		/// <returns>Coordonnées matrice.</returns>
		public static Vector2 xy2matrice(Vector2 mouse)
		{
			double angleDegre = -26.57; // -36.57 = -arctan(1/2)
			double angleRadian = Math.PI * angleDegre / 180;
			double sina = Math.Sin(angleRadian);
			double cosa = Math.Cos(angleRadian);
			Vector2 rot = new Vector2((float)(mouse.X * cosa - mouse.Y * sina), (float)(mouse.X * sina + mouse.Y * cosa)); // Coordonées parallélogramme
			Vector2 final = new Vector2(rot.X + (float)(0.75 * rot.Y), rot.Y); // Coordonées rectangle
			Vector2 coo = new Vector2((float)Math.Round((final.X - 35) / 35.777), (float)Math.Round((final.Y + 4) / 28.54)); // Coordonées matrice // 35.777 = sqrt(16²+32²)
			return coo;
		}

		/// <summary>
		/// Transforme des coordonnées matrice en coordonnées losange.
		/// </summary>
		/// <param name="mouse">Coordonnées matrice.</param>
		/// <returns>Coordonnées losange.</returns>
		public static Vector2 matrice2xy(Vector2 mouse)
		{ return new Vector2(32 * (mouse.X - mouse.Y), 16 * (mouse.X + mouse.Y)); }

		#endregion
	}
}
