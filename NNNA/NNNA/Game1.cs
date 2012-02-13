using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Forms = System.Windows.Forms;
using System.Diagnostics;

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

		private Texture2D m_background_light, m_background_dark, m_fog, m_menu, m_submenu, m_pointer, m_night, m_console, m_background;
		private Dictionary<string, Texture2D> m_actions = new Dictionary<string, Texture2D>();
		private SpriteFont m_font_menu, m_font_menu_title, m_font_small, m_font_credits;
		private Screen m_currentScreen = Screen.Title;
		private int m_konami = 0, m_konamiStatus = 0;
		private string m_currentAction = "";
		private double m_elapsed;
        List<string> last_state = new List<string>();

		private Vector2 m_screen = new Vector2(1680, 1050);
		private bool m_fullScreen = true, m_shadows = true, m_smart_hud = false, m_health_hover = false, m_showConsole = false;
		private float m_sound_general = 10, m_sound_sfx = 10, m_sound_music = 10;
		private int m_textures = 2, m_sound = 2;

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
        List<Movible_Sprite> units = new List<Movible_Sprite>();
		List<Unit> selectedList = new List<Unit>();
		List<Building> buildings = new List<Building>();
        List<Resource> resource = new List<Resource>();
		Building selectedBuilding;
		float[,] m_map;

        // Audio objects
        //private AudioEngine engine; 
        //private WaveBank musique; 
        //private SoundBank sons; 
        //private Cue piste;
        //AudioCategory musicCategory;
        float musicVolume = 2.0f;
        Sons son = new Sons();
        SoundEffect _debutpartie;
        SoundEffect _finpartie;

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
			GameMenu,
			Debug
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
			if (!File.Exists("settings.xml"))
			{ return; }

			XmlTextReader reader = new XmlTextReader("settings.xml");

			string setting = "";
			float temp;
			int tmp;
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						while (reader.MoveToNextAttribute())
						{ setting = reader.Value; }
						break;

					case XmlNodeType.Text:
						string value = reader.Value;
						switch (setting)
						{
							case "fullScreen": m_fullScreen = (value == "true" || value == "1"); break;
							case "screenWidth": float.TryParse(value, out temp); m_screen.X = temp; break;
							case "screenHeight": float.TryParse(value, out temp); m_screen.Y = temp; break;
							case "healthHover": m_health_hover = (value == "true" || value == "1"); break;
							case "smartHUD": m_smart_hud = (value == "true" || value == "1"); break;
							case "textures": int.TryParse(value, out tmp); m_textures = tmp; break;
							case "shadows": m_shadows = (value == "true" || value == "1"); break;
							case "soundGeneral": int.TryParse(value, out tmp); m_sound_general = tmp; break;
							case "soundMusic": int.TryParse(value, out tmp); m_sound_music = tmp; break;
							case "soundSFX": int.TryParse(value, out tmp); m_sound_sfx = tmp; break;
							case "sound": int.TryParse(value, out tmp); m_sound = tmp; break;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Enregistre les options dans le fichier "settings.xml".
		/// </summary>
		private void saveSettings()
		{
			StreamWriter monStreamWriter = new StreamWriter("settings.xml");
			monStreamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			monStreamWriter.WriteLine("<settings>");
			monStreamWriter.WriteLine("	<setting name=\"fullScreen\">" + (m_fullScreen ? "true" : "false") + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"screenWidth\">" + m_screen.X + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"screenHeight\">" + m_screen.Y + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"healthHover\">" + (m_health_hover ? "true" : "false") + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"smartHUD\">" + (m_smart_hud ? "true" : "false") + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"textures\">" + m_textures + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"shadows\">" + (m_shadows ? "true" : "false") + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundGeneral\">" + m_sound_general + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundMusic\">" + m_sound_music + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundSFX\">" + m_sound_sfx + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"sound\">" + m_sound + "</setting>");
			monStreamWriter.WriteLine("</settings>");
			monStreamWriter.Close();
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

			// Actions unités
			m_actions.Add("attack", Content.Load<Texture2D>("Actions/attack"));
			m_actions.Add("gather", Content.Load<Texture2D>("Actions/gather"));
			m_actions.Add("build", Content.Load<Texture2D>("Actions/build"));
			m_actions.Add("build_hutte", Content.Load<Texture2D>("Actions/build_hutte"));
			m_actions.Add("build_hutteDesChasseurs", Content.Load<Texture2D>("Actions/build_hutteDesChasseurs"));
            // m_actions.Add("build_ferme", Content.Load<Texture2D>("Actions/build_ferme"));
            
            // Action Batiment
            m_actions.Add("create_peon", Content.Load<Texture2D>("Actions/create_peon"));
            m_actions.Add("technologies", Content.Load<Texture2D>("Actions/technologies"));
            m_actions.Add("ere suivante", Content.Load<Texture2D>("Actions/evolution"));
            m_actions.Add("create_guerrier", Content.Load<Texture2D>("Actions/create_guerrier"));
            m_actions.Add("retour", Content.Load<Texture2D>("Actions/retour"));


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
			string ratio = ((int)((10 * m_screen.X) / m_screen.Y)).ToString();
			if (ratio != "13" && ratio != "16" && ratio != "18")
			{ ratio = "13"; }
			m_background = Content.Load<Texture2D>("background/" + ratio);
			m_background_light = CreateRectangle((int)(654 * (m_screen.X / 1680)), (int)m_screen.Y, Color.White);
			m_background_dark = CreateRectangle((int)m_screen.X, (int)m_screen.Y, new Color(0, 0, 0, 170));
			m_night = CreateRectangle((int)m_screen.X, (int)m_screen.Y, new Color(0,0,10));
			m_console = CreateRectangle(1, 1, new Color(0, 0, 0, 128));

			hud = new HUD(0, ((graphics.PreferredBackBufferHeight * 5) / 6) - 10, minimap, graphics);
			minimap = new Minimap((hud.Position.Width * 7) / 8 - +hud.Position.Width / 150, hud.Position.Y + hud.Position.Height / 15, (hud.Position.Height * 9) / 10, (hud.Position.Height * 9) / 10);

			MessagesManager.X = (uint)m_screen.X - 300;
		}

		/// <summary>
		/// Décharge tous les contenus du jeu.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Unload();
		}

		#endregion

		#region Updates

		/// <summary>
		/// Met à jour le jeu tous les 1/60 de secondes.
		/// </summary>
		/// <param name="gameTime">Temps courant.</param>
		protected override void Update(GameTime gameTime)
		{
			Clavier.Get().Update(Keyboard.GetState());
			Souris.Get().Update(Mouse.GetState());

			#if DEBUG
				if (Clavier.Get().NewPress(Keys.F5))
				{
					m_map = IslandGenerator.Generate(110, 70);
					m_currentScreen = Screen.Debug;
				}
				if (Clavier.Get().NewPress(Keys.F6))
				{ m_currentScreen = m_currentScreen == Screen.Debug ? Screen.Game : Screen.Debug; }
			#endif

			if (Clavier.Get().NewPress(Keys.OemQuotes))
			{ m_showConsole = !m_showConsole; }

			// Code Konami
			if (m_konami >= 10)
			{ m_konamiStatus++; }
			if (
				(Clavier.Get().NewPress(Keys.Up)    && m_konami == 0) ||
				(Clavier.Get().NewPress(Keys.Up)    && m_konami == 1) ||
				(Clavier.Get().NewPress(Keys.Down)  && m_konami == 2) ||
				(Clavier.Get().NewPress(Keys.Down)  && m_konami == 3) ||
				(Clavier.Get().NewPress(Keys.Left)  && m_konami == 4) ||
				(Clavier.Get().NewPress(Keys.Right) && m_konami == 5) ||
				(Clavier.Get().NewPress(Keys.Left)  && m_konami == 6) ||
				(Clavier.Get().NewPress(Keys.Right) && m_konami == 7) ||
				(Clavier.Get().NewPress(Keys.B)     && m_konami == 8) ||
				(Clavier.Get().NewPress(Keys.A)     && m_konami == 9)
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

				case Screen.Debug:
					UpdateDebug(gameTime);
					break;

				case Screen.Game:
					UpdateGame(gameTime);
					break;
				case Screen.GameMenu:
					UpdateGameMenu(gameTime);
					break;
			}

			//son 
            son.Engine_menu.Update();
            if (!son.Musiquemenu.IsPlaying && !son.Musiquemenu.IsPaused)
            {
                son.Initializesons(musicVolume, m_sound_music, m_sound_general);
            }

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
			Screen s = testMenu(Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.Game, Screen.Play);
			if (s != Screen.PlayQuick)
			{
				if (s == Screen.Game)
                {
                    int[] sizes = { 50, 100, 200 };
                    double[] resources = { 0.05, 0.1, 0.2 };

                    //Le reste
                    matrice = generateMap(m_quick_type, (int)(sizes[m_quick_size] * 0.6), sizes[m_quick_size], resources[m_quick_resources]);
                    map.LoadContent(matrice, Content, minimap, graphics.GraphicsDevice);
                    hud.LoadContent(Content, "HUD/hud2");
                    minimap.LoadContent(map);
                    m_elapsed = gameTime.TotalGameTime.TotalMilliseconds;

                    // Spawn
                    List<float> heights = new List<float>();
                    for (int x = 0; x < m_map.GetLength(0); x++)
                    {
                        for (int y = 0; y < m_map.GetLength(1); y++)
                        { heights.Add(m_map[x, y] * 255); }
                    }
                    float spawnHeight = heights.Max();
                    int my = heights.IndexOf(spawnHeight) % m_map.GetLength(0), mx = heights.IndexOf(spawnHeight) / m_map.GetLength(0);
                    //ConsoleManager.Messages.Add(new ConsoleMessage(mx.ToString() + "-" + my.ToString(), Color.Red, 100000));
                    mx = m_map.GetLength(1) / 2;
                    my = m_map.GetLength(0) / 2;

                    //Unites du Debut
                    joueur.Reset();
                    joueur.LoadResources(Content);
                    units.Clear();
                    units.Add(new Guerrier((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 100, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 200, Content, joueur, false));
                    units.Add(new Guerrier((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 0, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 200, Content, joueur, false));
                    units.Add(new Peon((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 50, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 200, Content, joueur, false));

                    //Batiments du Debut
                    buildings.Clear();
                    buildings.Add(new Grande_Hutte((int)matrice2xy(new Vector2(mx - 1, my - 1)).X, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y, Content, joueur));
                    camera.Position = matrice2xy(new Vector2(mx - 1, my - 1)) - m_screen / 2;

                    //Decor

                    /*Random rand = new Random();
                    for (int i = 0; i < 15; i++)
                    {
                        int x = 0;
                        int y = 0;
                        x = rand.Next(-16 * (map.Map_Width),16 * map.Map_Width);
                        y = rand.Next(0,16 *map.Map_Height);
                        while ((xy2matrice(new Vector2(x, y)).X >= 0 && xy2matrice(new Vector2(x, y)).X < matrice.GetLength(0))
                            && (xy2matrice(new Vector2(x, y)).Y >= 0 && xy2matrice(new Vector2(x, y)).Y < matrice.GetLength(1))
                            && (!matrice[(int)xy2matrice(new Vector2(x, y)).X, (int)xy2matrice(new Vector2(x, y)).Y].Crossable))
                        {
                            x = rand.Next(-16 * map.Map_Width,16 * map.Map_Width);
                            y = rand.Next(0,16 * map.Map_Height);
                        }
                        buildings.Add(new Palmier(x, y, Content));
                    }*/

                    //Le reste
                    matrice = generateMap(m_quick_type, (int)(sizes[m_quick_size] * 0.6), sizes[m_quick_size], resources[m_quick_resources]);
                    map.LoadContent(matrice, Content, minimap, graphics.GraphicsDevice);
                    hud.LoadContent(Content, "HUD/hud2");
                    minimap.LoadContent(map);
                    m_gameTime = gameTime;
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
					case 0:
						m_quick_type = (MapType)((((int)m_quick_type) + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + Enum.GetValues(typeof(MapType)).Length) % Enum.GetValues(typeof(MapType)).Length);
						break;

					case 1:
						m_quick_size = (m_quick_size + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 3) % 3;
						break;

					case 2:
						m_quick_resources = (m_quick_resources + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 3) % 3;
						break;
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
			Screen s = testMenu(Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.Options);
			if (s != Screen.OptionsGeneral)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				switch (m)
				{
					case 0:
						m_health_hover = !m_health_hover;
						break;

					case 1:
						m_smart_hud = !m_smart_hud;
						break;
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

					case 2:
						m_textures = (m_textures + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 3) % 3;
						break;

					case 3:
						m_shadows = !m_shadows;
						break;
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
					case 0:
						m_sound_general = (m_sound_general + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 11) % 11;
						break;

					case 1:
						m_sound_music = (m_sound_music + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 11) % 11;
						break;

					case 2:
						m_sound_sfx = (m_sound_sfx + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 11) % 11;
						break;

					case 3:
						m_sound = (m_sound + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 3) % 3;
						break;
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
			if (Clavier.Get().NewPress(Keys.Escape))
			{ m_currentScreen = Screen.GameMenu; }

			compt = (compt + gameTime.ElapsedGameTime.Milliseconds * 0.1f) % 100;
			curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			camera.Update(curseur, graphics);

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
							buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
							m_pointer = Content.Load<Texture2D>("pointer");
							m_currentAction = "";
						}
						else
						{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
						break;

					case "build_hutteDesChasseurs":
						b = new Hutte_des_chasseurs((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, joueur);
						if (joueur.Pay(b.Prix))
						{
							buildings.Add(b);
							MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
							m_pointer = Content.Load<Texture2D>("pointer");
							m_currentAction = "";
						}
						else
						{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
						break;

                    case "create_peon" :
                        u = new Peon((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 5), (int)selectedBuilding.Position.Y + 200, Content, joueur);
                        if (joueur.Pay(u.Prix))
						{
                            selectedBuilding.Iterator++;
							units.Add(u);
							MessagesManager.Messages.Add(new Msg("Nouveau Peon !", Color.White, 5000));
							m_currentAction = "";
						}
						else
						{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
						break;

                    case "create_guerrier":
                        u = new Guerrier((int)selectedBuilding.Position.X + 50 * (selectedBuilding.Iterator % 3), (int)selectedBuilding.Position.Y + 70, Content, joueur);
                        if (joueur.Pay(u.Prix))
                        {
                            selectedBuilding.Iterator++;
                            units.Add(u);
                            MessagesManager.Messages.Add(new Msg("Nouveau Guerrier !", Color.White, 5000));
                            m_currentAction = "";
                        }
                        else
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                        break;

                        // Fin Ere 1 

                        // Ere 2 
                    case "build_ferme":
                        b = new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), Content, joueur);
                        if (joueur.Pay(b.Prix))
                        {
                            buildings.Add(b);
                            MessagesManager.Messages.Add(new Msg("Nouvelle ferme !", Color.White, 5000));
                            m_pointer = Content.Load<Texture2D>("pointer");
                            m_currentAction = "";
                        }
                        else
                        { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                        break;
                        // Fin Ere 2

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
				foreach (Unit sprite in units)
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

					foreach (Building sprite in buildings)
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
			if (Souris.Get().Clicked(MouseButton.Left) && Souris.Get().X >= hud.Position.X + 20 && Souris.Get().Y >= hud.Position.Y + 20 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V) || Clavier.Get().NewPress(Keys.F))
			{
				int x = Souris.Get().X - hud.Position.X - 20, y = Souris.Get().Y - hud.Position.Y - 20;
                if (x % 40 < 32 && y % 40 < 32 || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V) || Clavier.Get().NewPress(Keys.F))
				{
					x /= 40;
					y /= 40;
					int pos = x + 6 * y;
                    if (pos < m_currentActions.Count || Clavier.Get().NewPress(Keys.C) || Clavier.Get().NewPress(Keys.V) || Clavier.Get().NewPress(Keys.F))
					{
						string act = "";
						if (Clavier.Get().NewPress(Keys.C))
						{ act = "build_hutte"; }
						else if (Clavier.Get().NewPress(Keys.V))
						{ act = "build_hutteDesChasseurs"; }
                        else if (Clavier.Get().NewPress(Keys.F)) 
                        { act = "build_ferme"; }
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
                                        m_pointer = Content.Load<Texture2D>("Batiments/hutte1");
                                    else
									    m_pointer = Content.Load<Texture2D>("Batiments/hutte2");
									m_currentAction = "build_hutte";
								}
								else
								{ MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
								break;

							case "build_hutteDesChasseurs":
								if (joueur.Has(new Hutte_des_chasseurs().Prix))
								{
									m_pointer = Content.Load<Texture2D>("Batiments/hutte_des_chasseurs");
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
                                if (joueur.Has(new Peon().Prix))
                                {
                                    m_currentAction = "create_peon";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                            case "technologies":
                                m_currentActions.Clear();
                                m_currentActions.Add("ere suivante");
                                m_currentActions.Add("retour");
                                break;

                            case "ere suivante":
                                m_currentActions.Clear();
                                m_currentActions.Add("create_peon");
                                m_currentActions.Add("technologies");
                                //Changements pour la deuxieme ere à effectuer ici.
                                break;

                            case "create_guerrier":
                                if (joueur.Has(new Guerrier().Prix))
                                {
                                    m_currentAction = "create_guerrier";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;

                                // Ere 2 
                            case "build_ferme":
                                if (joueur.Has(new Hutte_des_chasseurs().Prix))
                                {
                                    m_pointer = Content.Load<Texture2D>("Batiments/ferme");
                                    m_currentAction = "build_ferme";
                                }
                                else
                                { MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000)); }
                                break;
						}
					}
				}
			}

			foreach (Unit sprite in units)
			{ sprite.ClickMouvement(curseur, gameTime, camera, hud, units, buildings, matrice); }

			units.Sort(Sprite.CompareByY);
			buildings.Sort(Sprite.CompareByY);

		}
		void UpdateGameMenu(GameTime gameTime)
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ m_currentScreen = Screen.Game; }
			curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			m_currentScreen = testPauseMenu(Screen.Title, Screen.Game);
            son.Musiquemenu.Resume();

		}
		void UpdateDebug(GameTime gameTime)
		{ }

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

				case Screen.Debug:
					DrawDebug(gameTime);
					break;
			}

			// Code Konami
			if (m_konami >= 10)
			{
				joueur.Resource("Bois").Add(500);
				joueur.Resource("Pierre").Add(500);
				joueur.Resource("Nourriture").Add(500);
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
			makeMenu(types[(int)m_quick_type], tailles[m_quick_size], "Ressources " + ressources[m_quick_resources], "Jouer", "Retour");
		}
		private void DrawOptions(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Jouabilité", "Graphismes", "Son", "Retour");
		}
		private void DrawOptionsGeneral(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu((m_health_hover ? "Vie au survol" : "Vie constante"), (m_smart_hud ? "HUD intelligent" : "HUD classique"), "Retour");
		}
		private void DrawOptionsGraphics(GameTime gameTime)
		{
			DrawCommon(gameTime);
			string[] textures = { "min", "moyennes", "max" };
			makeMenu(m_screen.X + "x" + m_screen.Y, (m_fullScreen ? "Plein écran" : "Fenêtré"), "Textures " + textures[m_textures], (m_shadows ? "Ombres" : "Pas d'ombres"), "Retour");
		}
		private void DrawOptionsSound(GameTime gameTime)
		{
			DrawCommon(gameTime);
			string[] sound = { "min", "moy", "max" };
			makeMenu("Général : " + m_sound_general, "Musique : " + m_sound_music, "Effets : " + m_sound_sfx, "Qualité " + sound[m_sound], "Retour");
		}
		private void DrawCredits(GameTime gameTime)
		{
			DrawCommon(gameTime, false);

			// Crédits
			int y = m_credits / 2;
			if (y > m_screen.Y + 200 + (m_font_credits.MeasureString("Merci d'avoir joué !").Y / 2))
			{ y = (int)(m_screen.Y + 200 + (m_font_credits.MeasureString("Merci d'avoir joué !").Y / 2)); }

            spriteBatch.DrawString(m_font_credits, "Nicolas Allain", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Allain").X) / 2, m_screen.Y - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Nicolas Faure", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Faure").X) / 2, m_screen.Y + 60 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Nicolas Mouton-Besson", new Vector2((m_screen.X - m_font_credits.MeasureString("Nicolas Mouton-Besson").X) / 2, m_screen.Y + 120 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Arnaud Weiss", new Vector2((m_screen.X - m_font_credits.MeasureString("Arnaud Weiss").X) / 2, m_screen.Y + 180 - y), Color.White);
			spriteBatch.DrawString(m_font_credits, "Merci d'avoir joué !", new Vector2((m_screen.X - m_font_credits.MeasureString("Merci d'avoir joué !").X) / 2, m_screen.Y + (m_screen.Y / 2) + 180 + (m_font_credits.MeasureString("Merci d'avoir joué !").Y / 2) - y), Color.White);
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
					sprite.DrawMap(spriteBatch, camera);
					compteur++;
				}
			}

			foreach (Static_Sprite sprite in buildings)
			{ sprite.Draw(spriteBatch, camera); }

			foreach (Movible_Sprite sprite in units)
			{ sprite.Draw(spriteBatch, camera, index); }

            //foreach (Resource sprite in resource)
            //{ sprite.Draw(spriteBatch, 1, camera); }

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
			spriteBatch.Draw(m_night, Vector2.Zero, new Color(0, 0, 255, (int)(64 - 64 * Math.Cos((m_gameTime.TotalGameTime.TotalMilliseconds - m_elapsed) / 50000))));

			// Le HUD
			MessagesManager.Draw(spriteBatch, m_font_small);
			hud.Draw(spriteBatch, minimap, joueur, m_font_small);
			for (int i = 0; i < selectedList.Count; i++)
			{ selectedList[i].DrawIcon(spriteBatch, new Vector2(356 + (i % 10) * 36, m_screen.Y - hud.Position.Height + 54 + (i / 10) * 36)); }
			if (m_currentActions.Count > 0)
			{
				for (int i = 0; i < m_currentActions.Count; i++)
				{ spriteBatch.Draw(m_actions[m_currentActions[i]], new Vector2(hud.Position.X + 20 + 40 * (i % 6), hud.Position.Y + 20 + 40 * (i / 6)), Color.White); }
			}
		}
		private void DrawGameMenu(GameTime gameTime)
		{
			foreach (EffectPass pass in gaussianBlur.CurrentTechnique.Passes)
			{
				pass.Apply();
				DrawGame(gameTime);
			}
			spriteBatch.Draw(m_background_dark, Vector2.Zero, Color.White);
			makePauseMenu("Quitter", "Retour");
		}
		
		private void DrawDebug(GameTime gameTime)
		{
			List<float> heights = new List<float>();

			// Calcul de la hauteur de l'eau
			for (int x = 0; x < m_map.GetLength(0); x++)
			{
				for (int y = 0; y < m_map.GetLength(1); y++)
				{ heights.Add(m_map[x, y] * 255); }
			}
			float spawnHeight = heights.Max();
			Debug(1, heights.IndexOf(spawnHeight) % m_map.GetLength(0));
			Debug(2, heights.IndexOf(spawnHeight) / m_map.GetLength(0));
			Debug(3, spawnHeight);
			Debug(4, heights.IndexOf(spawnHeight));
			heights.Sort();
			float waterline = heights[(int)Math.Round((heights.Count - 1) * 0.6)];

			for (int x = 0; x < m_map.GetLength(0); x++)
			{
				for (int y = 0; y < m_map.GetLength(1); y++)
				{
					Color color = Color.White;
					string sprite = "";
					float m = m_map[x, y] * 255;
					if (m < waterline - 40)
					{
						sprite = "~";
						color = Color.Blue;
					}
					else if (m < waterline)
					{
						sprite = "~";
						color = Color.Cyan;
					}
					else if (m < waterline + 15)
					{
						sprite = ".";
						color = Color.Yellow;
					}
					else if (m < waterline + 35)
					{
						sprite = ".";
						color = Color.Green;
					}
					else if (m > 230)
					{
						sprite = "^";
						color = Color.Brown;
					}
					else if (m > 205)
					{
						sprite = "~";
						color = Color.Brown;
					}
					else
					{
						sprite = "*";
						color = Color.Green;
					}
					spriteBatch.DrawString(m_font_small, sprite, new Vector2(5 + x * 14, 5 + y * 14), color);
				}
			}
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
		{ Console.Messages.Add(new ConsoleMessage(value)); }
		private void Debug(List<object> value)
		{
			string deb = "List<"+value.GetType()+">(" + value.Count.ToString() + ") { ";
			for (int i = 0; i < value.Count; i++)
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
			for (int i = 0; i < args.Length; i++)
			{ DrawString(spriteBatch, m_font_menu, args[i], new Vector2(0, i * (m_screen.Y / 11) + m_screen.Y / 5 + (180 * (m_screen.Y / 1050))), (menu() == i ? Color.White : Color.Silver), Color.Black, 1, "Center", m_screen.Y / 1050); }
		}
		protected int menu()
		{ return Souris.Get().Y > m_screen.Y / 5 + (180 * (m_screen.Y / 1050)) && ((Souris.Get().Y - m_screen.Y / 5 - (180 * (m_screen.Y / 1050))) % (m_screen.Y / 11)) < (m_font_menu.MeasureString("Menu").Y * m_screen.Y) / 1050 ? (int)((Souris.Get().Y - m_screen.Y / 5 - (180 * (m_screen.Y / 1050))) / (m_screen.Y / 11)) : -1; }

		protected void makePauseMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ spriteBatch.DrawString(m_font_menu, args[i], new Vector2((668 * (m_screen.X / 1680)), i * 44 + (180 * (m_screen.Y / 1050))), Color.White); }
		}
		protected int pauseMenu()
		{ return (Souris.Get().X > (654 * (m_screen.X / 1680)) && Souris.Get().Y > (180 * (m_screen.Y / 1050))) ? (int)((Souris.Get().Y - (180 * (m_screen.Y / 1050))) / 44) : -1; }

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
		public Vector2 matrice2xy(Vector2 mouse)
		{ return new Vector2(32 * (mouse.X - mouse.Y), 16 * (mouse.X + mouse.Y)); }

		#endregion
	}
}
