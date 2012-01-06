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
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Effect gaussianBlur;
		private Rectangle m_selection = Rectangle.Empty;
		private GameTime m_gameTime;

		private Texture2D m_background_light, m_background_dark, m_fog, m_light, m_menu, m_submenu, m_pointer, m_night;
		private Texture2D[] m_backgrounds = new Texture2D[2];
		private SpriteFont m_font_menu, m_font_small, m_font_credits;
		private Screen m_currentScreen = Screen.Title;
		private int m_konami = 0, m_konamiStatus = 0;

		// Settings
		private Vector2 m_screen = new Vector2(1680, 1050);
		private bool m_fullScreen = true, m_shadows = true, m_smart_hud = false, m_health_hover = false;
		private float m_sound_general = 10, m_sound_sfx = 10, m_sound_music = 10;
		private int m_textures = 2, m_sound = 2, m_theme = 0;

		// Vars
		private MapType m_quick_type = MapType.Island;
		private int m_quick_size = 1, m_quick_resources = 1, m_credits = 0;

		// Map
		Sprite h, e, p, t, s, i, curseur;
		Sprite[,] matrice;
		Map map;
        Minimap minimap;
        HUD hud;
		Camera2D camera;
		Joueur joueur;
        List<Movible_Sprite> units = new List<Movible_Sprite>();
        List<Movible_Sprite> selectedList = new List<Movible_Sprite>();
		List<Static_Sprite> buildings = new List<Static_Sprite>();
		float[,] m_map;

        // Audio objects
        private AudioEngine engine; 
        private WaveBank musique; 
        private SoundBank sons; 
        private Cue piste;
        AudioCategory musicCategory;
        float musicVolume = 2.0f;



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

		public Game1()
		{
			Window.Title = "NNNA - " + this.GetType().Assembly.GetName().Version.ToString();

			loadSettings();

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = (int)m_screen.X;
			graphics.PreferredBackBufferHeight = (int)m_screen.Y;
			graphics.IsFullScreen = m_fullScreen;

			Content.RootDirectory = "Content";

            // son 
            AudioCategory musicCategory;
		}

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
							case "theme": int.TryParse(value, out tmp); m_theme = tmp; break;
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
			monStreamWriter.WriteLine("	<setting name=\"theme\">" + m_theme + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundGeneral\">" + m_sound_general + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundMusic\">" + m_sound_music + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"soundSFX\">" + m_sound_sfx + "</setting>");
			monStreamWriter.WriteLine("	<setting name=\"sound\">" + m_sound + "</setting>");
			monStreamWriter.WriteLine("</settings>");
			monStreamWriter.Close();
		}

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
            engine = new AudioEngine("Content/sounds/son projet.xgs");
            musique = new WaveBank(engine, "Content/sounds/Wave Bank.xwb");
			sons = new SoundBank(engine, "Content/sounds/sound_menu.xsb");
			piste = sons.GetCue("sonmenu");
			piste.Play();
            engine.Update();
			musicCategory = engine.GetCategory("Music");
            musicCategory.SetVolume(musicVolume * m_sound_music * (m_sound_general / 10));

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

		/// <summary>
		/// Charge touts les contenus du jeu.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Backgrounds
			m_fog = Content.Load<Texture2D>("fog");
			m_light = Content.Load<Texture2D>("light");
			m_backgrounds[0] = Content.Load<Texture2D>("background1");
			m_backgrounds[1] = Content.Load<Texture2D>("background2");

			// Sprites
			m_menu = Content.Load<Texture2D>("menu");
			m_submenu = Content.Load<Texture2D>("submenu");
			m_pointer = Content.Load<Texture2D>("pointer");
			curseur.LoadContent(Content, "pointer");

			// Fontes
			m_font_menu = Content.Load<SpriteFont>("font_menu");
			m_font_small = Content.Load<SpriteFont>("font_small");
			m_font_credits = Content.Load<SpriteFont>("font_credits");

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
			m_background_light = CreateRectangle((int)(654 * (m_screen.X / 1680)), (int)m_screen.Y, Color.White);
			m_background_dark = CreateRectangle((int)m_screen.X, (int)m_screen.Y, new Color(0, 0, 0, 170));
			m_night = CreateRectangle((int)m_screen.X, (int)m_screen.Y, Color.Black);

			hud = new HUD(0, ((graphics.PreferredBackBufferHeight * 5) / 6) - 10, minimap, graphics);
			minimap = new Minimap((hud.Position.Width * 7) / 8 - +hud.Position.Width / 150, hud.Position.Y + hud.Position.Height / 15, (hud.Position.Height * 9) / 10, (hud.Position.Height * 9) / 10);

			ConsoleManager.X = (uint)m_screen.X - 300;
		}

		/// <summary>
		/// Décharge tous les contenus du jeu.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Unload();
		}

		/// <summary>
		/// Met à jour le jeu tous les 1/60 de secondes.
		/// </summary>
		/// <param name="gameTime">Temps courant.</param>
		protected override void Update(GameTime gameTime)
		{
			Clavier.Get().Update(Keyboard.GetState());
			Souris.Get().Update(Mouse.GetState());

			if (Clavier.Get().NewPress(Keys.F5))
			{
				m_map = IslandGenerator.Generate(110, 70);
				m_currentScreen = Screen.Debug;
			}
			if (Clavier.Get().NewPress(Keys.F6))
			{ m_currentScreen = m_currentScreen == Screen.Debug ? Screen.Game : Screen.Debug; }

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
			engine.Update();
			if (!piste.IsPlaying)
			{
				piste = sons.GetCue("sonmenu");
				piste.Play();
			}

			base.Update(gameTime);
		}
		private void UpdateTitle(GameTime gameTime)
		{
			m_currentScreen = testMenu(Screen.Title, Screen.Play, Screen.Options, Screen.Credits, Screen.OptionsSound);
			if (m_currentScreen == Screen.Credits)
			{ m_credits = 0; }
			if (m_currentScreen == Screen.OptionsSound)
			{ this.Exit(); }
		}
		private void UpdatePlay(GameTime gameTime)
		{
			m_currentScreen = testMenu(Screen.Play, Screen.PlayQuick, Screen.Title);
		}
		private void UpdatePlayQuick(GameTime gameTime)
		{
			UpdatePlay(gameTime);
			Screen s = testSubMenu(Screen.PlayQuick, Screen.PlayQuick, Screen.PlayQuick, Screen.Game, Screen.Play);
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
					units.Add(new Guerrier((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 100, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 100, Content, joueur, "sans_ressources"));
					units.Add(new Guerrier((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 0, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 100, Content, joueur, "sans_ressources"));
					units.Add(new Peon((int)matrice2xy(new Vector2(mx - 1, my - 1)).X + 50, (int)matrice2xy(new Vector2(mx - 1, my - 1)).Y + 100, Content, joueur, "sans_ressources"));

					//Batiments du Debut
					buildings.Clear();
					buildings.Add(new Grande_Hutte((int)matrice2xy(new Vector2(mx-1, my-1)).X, (int)matrice2xy(new Vector2(mx-1, my-1)).Y, Content, joueur, "sans_ressources"));
					camera.Position = matrice2xy(new Vector2(mx-1, my-1)) - m_screen / 2;

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
				}
				m_currentScreen = s;
			}
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = subMenu();
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
			m_currentScreen = testMenu(Screen.Options, Screen.OptionsGeneral, Screen.OptionsGraphics, Screen.OptionsSound, Screen.Title);
			if (m_currentScreen == Screen.Title)
			{ saveSettings(); }
		}
		private void UpdateOptionsGeneral(GameTime gameTime)
		{
			UpdateOptions(gameTime);
			Screen s = testSubMenu(Screen.OptionsGeneral, Screen.OptionsGeneral, Screen.Options);
			if (s != Screen.OptionsGeneral)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left))
			{
				int m = subMenu();
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
			UpdateOptions(gameTime);
			Screen s = testSubMenu(Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.OptionsGraphics, Screen.Options);
			if (s != Screen.OptionsGraphics)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = subMenu();
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

                    case 4:
						m_theme = (m_theme + (Souris.Get().Clicked(MouseButton.Right) ? -1 : 1) + 2) % 2;
                        break;
				}
			}
		}
		private void UpdateOptionsSound(GameTime gameTime)
		{
			UpdateOptions(gameTime);
			Screen s = testSubMenu(Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.OptionsSound, Screen.Options);
			if (s != Screen.OptionsSound)
			{ m_currentScreen = s; }
			else if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = subMenu();
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
            musicCategory.SetVolume(musicVolume * m_sound_music * (m_sound_general / 10));

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
			{ m_selection = new Rectangle(Souris.Get().X + (int)camera.Position.X, Souris.Get().Y + (int)camera.Position.Y, 0, 0); }
			else if (Souris.Get().Hold(MouseButton.Left))
			{
				m_selection.Width = Souris.Get().X + (int)camera.Position.X - m_selection.X;
				m_selection.Height = Souris.Get().Y + (int)camera.Position.Y - m_selection.Y;
			}
			else if (Souris.Get().Hold(MouseButton.Left, ButtonState.Released))
			{ m_selection = Rectangle.Empty; }

			// SelectMovible n'a rien à faire dans la classe Sprite, sa place est cependant idéale ici (même si c'est un gros pavé :D). D'ailleurs la classe Sprite est complètement foireuse. Un Sprite n'a pas de vie, ni d'attaque ou autre. C'est une classe Unit qu'il faut utiliser pour faire ça.
			bool change;
			if (!m_selection.IsEmpty && Souris.Get().Released(MouseButton.Left) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5))
			{
				change = false;
				if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !Keyboard.GetState().IsKeyDown(Keys.RightControl))
				{
					foreach (Movible_Sprite sprites in selectedList)
					{ sprites.Selected = false; }
					selectedList.Clear();
				}
				foreach (Movible_Sprite sprite in units)
				{
					Rectangle csel = new Rectangle((int)(m_selection.X - camera.Position.X + (m_selection.Width < 0 ? m_selection.Width : 0)), (int)(m_selection.Y - camera.Position.Y + (m_selection.Height < 0 ? m_selection.Height : 0)), (int)Math.Abs(m_selection.Width), (int)Math.Abs(m_selection.Height));
					if (sprite.Selected == false && csel.Intersects(sprite.Rectangle(camera)))
					{
						sprite.Selected = true;
						selectedList.Add(sprite);
						change = true;
					}
				}
				if (change == false)
				{
					foreach (Movible_Sprite sprite in selectedList)
					{ sprite.Selected = false; }
					selectedList.Clear();
				}
			}

            foreach (Movible_Sprite sprite in selectedList)
            {
                if (sprite.Type == "peon")
                {
                    if (Clavier.Get().NewPress(Keys.C))
                    { sprite.Create_Maison(curseur, buildings, Content, joueur, camera); }
                    else if (Clavier.Get().NewPress(Keys.V))
                    { sprite.Create_Hutte_Chasseurs(curseur, buildings, Content, joueur, camera); }
                }
            }
            foreach (Movible_Sprite sprite in units)
            {
                if (sprite.Type == "peon")
                {
                    if (sprite.Create_maison)
                    { sprite.Create_Maison(curseur, buildings, Content, joueur, camera); }
                    else if (sprite.Create_hutte_chasseurs)
                    { sprite.Create_Hutte_Chasseurs(curseur, buildings, Content, joueur, camera); }
                }
            }

			foreach (Movible_Sprite sprite in units)
			{
				sprite.ClickMouvement(curseur, gameTime, camera, hud, units, buildings, matrice);
			}

			units.Sort(Sprite.CompareByY);
			buildings.Sort(Sprite.CompareByY);

		}
		void UpdateGameMenu(GameTime gameTime)
		{
			if (Clavier.Get().NewPress(Keys.Escape))
			{ m_currentScreen = Screen.Game; }
			curseur.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			m_currentScreen = testPauseMenu(Screen.Title, Screen.Game);
		}
		void UpdateDebug(GameTime gameTime)
		{ }

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

			spriteBatch.End();
			base.Draw(gameTime);
		}
		private void DrawCommon(GameTime gameTime)
		{
			// Le fond d'écran
			Rectangle screenRectangle = new Rectangle(0, 0, (int)m_screen.X, (int)m_screen.Y);
			spriteBatch.Draw(m_backgrounds[m_theme], screenRectangle, Color.White);
			spriteBatch.Draw(m_background_light, Vector2.Zero, new Color(26, 26, 26, 26));

			// Le brouillard
			int pos = (int)(gameTime.TotalGameTime.TotalMilliseconds / 80) % 1680;
			spriteBatch.Draw(m_fog, new Vector2(pos - 1680, 0), new Color(32, 32, 32, 32));
			spriteBatch.Draw(m_fog, new Vector2(pos, 0), new Color(32, 32, 32, 32));

			// Version
			spriteBatch.DrawString(m_font_small, this.GetType().Assembly.GetName().Version.ToString(), new Vector2(34, m_screen.Y - 50), Color.GhostWhite);
		}
		private void DrawPointer(GameTime gameTime)
		{ spriteBatch.Draw(m_pointer, new Vector2(Souris.Get().X, Souris.Get().Y), Color.White); }
		private void DrawTitle(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Jouer", "Options", "Crédits", "Quitter");
			DrawPointer(gameTime);
		}
		private void DrawPlay(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Escarmouche", "Retour");
			DrawPointer(gameTime);
		}
		private void DrawPlayQuick(GameTime gameTime)
		{
			DrawPlay(gameTime);

			string[] types = { "Île" };
			string[] tailles = { "Petite", "Moyenne", "Grande" };
			string[] ressources = { "Rares", "Moyennes", "Abondantes" };
			makeSubMenu("Type : " + types[(int)m_quick_type], "Taille : " + tailles[m_quick_size], "Ressources : " + ressources[m_quick_resources], "Jouer", "Retour");
		}
		private void DrawOptions(GameTime gameTime)
		{
			DrawCommon(gameTime);
			makeMenu("Jouabilité", "Graphismes", "Son", "Retour");
			DrawPointer(gameTime);
		}
		private void DrawOptionsGeneral(GameTime gameTime)
		{
			DrawOptions(gameTime);
			makeSubMenu("Vie au survol : " + (m_health_hover ? "Oui" : "Non"), "HUD intelligent : " + (m_smart_hud ? "Oui" : "Non"), "Retour");
		}
		private void DrawOptionsGraphics(GameTime gameTime)
		{
			DrawOptions(gameTime);
			string[] textures = { "Min", "Moyen", "Max" };
			makeSubMenu("Résolution : " + m_screen.X + "x" + m_screen.Y, "Mode : " + (m_fullScreen ? "Plein écran" : "Fenêtré"), "Textures : " + textures[m_textures], "Ombres : " + (m_shadows ? "Oui" : "Non"), "Thème : " + (m_theme + 1), "Retour");
		}
		private void DrawOptionsSound(GameTime gameTime)
		{
			DrawOptions(gameTime);
			string[] sound = { "Min", "Moyen", "Max" };
			makeSubMenu("Général : " + m_sound_general, "Musique : " + m_sound_music, "Effets : " + m_sound_sfx, "Qualité : " + sound[m_sound], "Retour");
		}
		private void DrawCredits(GameTime gameTime)
		{
			// Le fond d'écran
			Rectangle screenRectangle = new Rectangle(0, 0, (int)m_screen.X, (int)m_screen.Y);
			spriteBatch.Draw(m_backgrounds[m_theme], screenRectangle, Color.White);

			// Le brouillard
			int pos = (int)(gameTime.TotalGameTime.TotalMilliseconds / 80) % 1680;
			spriteBatch.Draw(m_fog, new Vector2(pos - 1680, 0), new Color(32, 32, 32, 32));
			spriteBatch.Draw(m_fog, new Vector2(pos, 0), new Color(32, 32, 32, 32));

			// Crédits
			int y = m_credits / 2;
			if (y > m_screen.Y + 180 + (m_font_credits.MeasureString("Merci d'avoir joué !").Y / 2))
			{ y = (int)(m_screen.Y + 180 + (m_font_credits.MeasureString("Merci d'avoir joué !").Y / 2)); }
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
			{ sprite.DrawMap(spriteBatch, camera); }

			foreach (Movible_Sprite sprite in units)
			{ sprite.Draw(spriteBatch, camera, index); }

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

			// Le brouillard
			spriteBatch.Draw(m_night, Vector2.Zero, new Color(255, 255, 255, (int)(64 - 64 * Math.Cos(m_gameTime.TotalGameTime.TotalMilliseconds / 12000))));

			ConsoleManager.Draw(spriteBatch, m_font_small);

			hud.Draw(spriteBatch, minimap, joueur, m_font_small);
			curseur.Draw(spriteBatch);

			// Debug
			Vector2 mouse = new Vector2(Souris.Get().X + camera.Position.X, Souris.Get().Y + camera.Position.Y);
			Debug(3, mouse);
			Debug(4, xy2matrice(mouse));
			Debug(5, matrice2xy(xy2matrice(mouse)));
		}
		private void DrawGameMenu(GameTime gameTime)
		{
			foreach (EffectPass pass in gaussianBlur.CurrentTechnique.Passes)
			{
				pass.Apply();
				DrawGame(gameTime);
			}
			spriteBatch.Draw(m_background_dark, Vector2.Zero, Color.White);
			curseur.Draw(spriteBatch);
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

		// Fonctions de debug
		private void Debug(int i, string value)
		{
			#if DEBUG
				spriteBatch.DrawString(m_font_small, value, new Vector2(10, 10 + i * 20), Color.White);
			#endif
		}
		private void Debug(int i, int value)
		{ Debug(i, value.ToString()); }
		private void Debug(int i, float value)
		{ Debug(i, value.ToString()); }
		private void Debug(int i, double value)
		{ Debug(i, value.ToString()); }
		private void Debug(int i, bool value)
		{ Debug(i, (value ? "true" : "false")); }
		private void Debug(int i, Vector2 value)
		{ Debug(i, value.ToString()); }
		private void Debug(int i, Rectangle value)
		{ Debug(i, value.ToString()); }

		protected void makeMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ spriteBatch.DrawString(m_font_menu, args[i], new Vector2((640 * (m_screen.X / 1680)) - m_font_menu.MeasureString(args[i]).X, i * 44 + (180 * (m_screen.Y / 1050))), Color.GhostWhite); }
			if (menu() >= 0 && menu() < args.Length)
			{ spriteBatch.Draw(m_menu, new Vector2((654 * (m_screen.X / 1680)) - m_menu.Width, menu() * 44 + (180 * (m_screen.Y / 1050))), new Color(64, 64, 64,64)); }
		}
		protected int menu()
		{ return (Souris.Get().X < (654 * (m_screen.X / 1680)) && Souris.Get().Y > (180 * (m_screen.Y / 1050)) && Souris.Get().X > (654 * (m_screen.X / 1680)) - m_menu.Width) ? (int)((Souris.Get().Y - (180 * (m_screen.Y / 1050))) / 44) : -1; }

		protected void makeSubMenu(params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{ spriteBatch.DrawString(m_font_menu, args[i], new Vector2((668 * (m_screen.X / 1680)), i * 44 + (180 * (m_screen.Y / 1050))), Color.GhostWhite); }
			if (subMenu() >= 0 && subMenu() < args.Length)
			{ spriteBatch.Draw(m_submenu, new Vector2((654 * (m_screen.X / 1680)), subMenu() * 44 + (180 * (m_screen.Y / 1050))), new Color(32, 32, 32, 32)); }
		}
		protected int subMenu()
		{ return (Souris.Get().X > (654 * (m_screen.X / 1680)) && Souris.Get().Y > (180 * (m_screen.Y / 1050))) ? (int)((Souris.Get().Y - (180 * (m_screen.Y / 1050))) / 44) : -1; }

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
		protected Screen testMenu(Screen up, params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = menu();
				if (m >= 0 && m < args.Length)
				{
					return m_currentScreen == args[m] ? up : args[m];
				}
			}
			return m_currentScreen;
		}

		/// <summary>
		/// Teste si un sous-menu est cliqué.
		/// </summary>
		/// <param name="args">La liste des sous-menus possibles.</param>
		/// <returns>Le sous-menu cliqué.</returns>
		protected Screen testSubMenu(params Screen[] args)
		{
			if (Souris.Get().Clicked(MouseButton.Left) || Souris.Get().Clicked(MouseButton.Right))
			{
				int m = subMenu();
				if (m >= 0 && m < args.Length)
				{
					return args[m];
				}
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

		/// <summary>
		/// Convertit un booléen en entier.
		/// </summary>
		/// <param name="b">Le booléen à convertir.</param>
		/// <returns>L'entier binaire correspondant.</returns>
		protected int b2i(bool b)
		{ return b ? 1 : 0; }

		/// <summary>
		/// Retourne la prochaine résolution disponible.
		/// </summary>
		/// <returns>La prochaine résolution dans l'ordre croissant.</returns>
		private Vector2 getNextResolution(bool previous = false)
		{
			Vector2[] l = { // Megaliste ! Yay !
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
	}
}
