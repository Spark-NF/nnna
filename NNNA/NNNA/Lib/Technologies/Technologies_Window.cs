using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NNNA.Form;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    delegate void Delegate();
    class Technologies_Window
    {
        private Window win;
        public bool Win_Visible
        {
            get { return win.Visible; }
            set { win.Visible = value; }
        }
        private Microsoft.Xna.Framework.Content.ContentManager _content;
        private TabControl tabcontrol;
        private TabItem tabItem1, tabItem2, tabItem3, tabItem4;
        private Button close, chasse, silex, agri, pierre_polie, irrigation, bronze, outils, feu, torche, fer, evolution1, evolution2, evolution3, evolution4, engrenage, forge, scierie, arme_de_siege, navigation, gouvernail, alchimie, bucheron, charpente, route;
		private bool _chasse, _feu, _silex, _fer, _pierrePolie, _ere1, _ere2, _ere3, _bronze, _outils, _agri, _torche, _irrigation, _engrenage, _forge, _scierie, _arme_de_siege, _navigation, _gouvernail, _alchimie, _bucheron, _charpente, _route;
        private readonly Dictionary<string, int> _prixFer = new Dictionary<string, int>(), _prixChasse = new Dictionary<string, int>(), _prixFeu = new Dictionary<string, int>(), _prixSilex = new Dictionary<string, int>(), _prixPierrePolie = new Dictionary<string, int>(), _prixBronze = new Dictionary<string, int>(), _prixOutils = new Dictionary<string, int>(), _prixAgri = new Dictionary<string, int>(), _prixTorche = new Dictionary<string, int>(), _prixIrrigation = new Dictionary<string, int>(), _prixEngrenage = new Dictionary<string, int>(), _prixForge = new Dictionary<string, int>(), _prixBucheron = new Dictionary<string, int>(), _prixNavigation = new Dictionary<string, int>(), _prixGouvernail = new Dictionary<string, int>(), _prixArme_de_siege = new Dictionary<string, int>(), _prixCharpente = new Dictionary<string, int>(), _prixRoute = new Dictionary<string, int>(), _prixAlchimie = new Dictionary<string, int>(), _prixScierie = new Dictionary<string,int>();
		
        public Technologies_Window(Rectangle zone, string name, Microsoft.Xna.Framework.Content.ContentManager content)
		{
            _content = content;
            close = new Button(new Rectangle(zone.X + zone.Width - zone.Width / 30 - zone.Height / 25, zone.Y + zone.Height / 30, zone.Height / 25, zone.Height / 25), "close", button_Close_Click, true);

            chasse = new Button(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4, zone.Height / 15, zone.Height / 15), "chasse", chasse_MouseLeftButtonDown);
            silex = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "silex", silex_MouseLeftButtonDown);
            agri = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + zone.Height / 4 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "agri", agri_MouseLeftButtonDown);
            pierre_polie = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "pierre_polie", pierre_polie_MouseLeftButtonDown);
            irrigation = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "irrigation", irrigation_MouseLeftButtonDown);
            bronze = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "bronze", bronze_MouseLeftButtonDown);
            outils = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5, zone.Height / 15, zone.Height / 15), "outils", outils_MouseLeftButtonDown);
            feu = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 4 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "feu", feu_MouseLeftButtonDown);
            torche = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "torche", torche_MouseLeftButtonDown);
            fer = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "fer", fer_MouseLeftButtonDown);
            evolution1 = new Button(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5, zone.Height / 15, zone.Height / 15), "evolution", evolution_MouseLeftButtonDown);

            close.Background = content.Load<Texture2D>(@"Technologies/button_close");
            chasse.Background = content.Load<Texture2D>(@"Technologies/chasse");
            silex.Background = content.Load<Texture2D>(@"Technologies/silex");
            agri.Background = content.Load<Texture2D>(@"Technologies/agriculture");
            pierre_polie.Background = content.Load<Texture2D>(@"Technologies/pierre_polie");
            irrigation.Background = content.Load<Texture2D>(@"Technologies/irrigation");
            bronze.Background = content.Load<Texture2D>(@"Technologies/bronze");
            outils.Background = content.Load<Texture2D>(@"Technologies/outils");
            feu.Background = content.Load<Texture2D>(@"Technologies/feu");
            torche.Background = content.Load<Texture2D>(@"Technologies/torche");
            fer.Background = content.Load<Texture2D>(@"Technologies/fer");
            evolution1.Background = content.Load<Texture2D>(@"Technologies/evolution");

            engrenage = new Button(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4, zone.Height / 15, zone.Height / 15), "engrenage", engrenage_MouseLeftButtonDown);
            forge = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "forge", forge_MouseLeftButtonDown);
            navigation = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + zone.Height / 4 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "navigation", navigation_MouseLeftButtonDown);
            scierie = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "scierie", scierie_MouseLeftButtonDown);
            gouvernail = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "gouvernail", gouvernail_MouseLeftButtonDown);
            arme_de_siege = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "arme_de_siege", arme_de_siege_MouseLeftButtonDown);
            alchimie = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 24, zone.Height / 15, zone.Height / 15), "alchimie", alchimie_MouseLeftButtonDown);
            bucheron = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "bucheron", bucheron_MouseLeftButtonDown);
            charpente = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + (zone.Height * 3) / 4, zone.Height / 15, zone.Height / 15), "charpente", charpente_MouseLeftButtonDown);
            route = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "route", route_MouseLeftButtonDown);
            evolution2 = new Button(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5, zone.Height / 15, zone.Height / 15), "evolution", evolution_MouseLeftButtonDown);

            engrenage.Background = content.Load<Texture2D>(@"Technologies/engrenage");
            forge.Background = content.Load<Texture2D>(@"Technologies/forge");
            navigation.Background = content.Load<Texture2D>(@"Technologies/navigation");
            scierie.Background = content.Load<Texture2D>(@"Technologies/scierie");
            gouvernail.Background = content.Load<Texture2D>(@"Technologies/gouvernail");
            arme_de_siege.Background = content.Load<Texture2D>(@"Technologies/arme_de_siege");
            alchimie.Background = content.Load<Texture2D>(@"Technologies/chimie");
            bucheron.Background = content.Load<Texture2D>(@"Technologies/bucheron");
            charpente.Background = content.Load<Texture2D>(@"Technologies/charpente");
            route.Background = content.Load<Texture2D>(@"Technologies/route");
            evolution2.Background = content.Load<Texture2D>(@"Technologies/evolution");
            
            tabItem1 = new TabItem(new Control[] {
                                                  chasse,
                                                  silex,
                                                  agri,
                                                  pierre_polie,
                                                  irrigation,
                                                  bronze,
                                                  outils,
                                                  feu,
                                                  torche,
                                                  fer,
                                                  evolution1
                                                  }, new Rectangle(zone.X + zone.Width / 14, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 4, zone.Height / 10), "ERE 1");

            tabItem2 = new TabItem(new Control[] {
                                                  engrenage,
                                                  forge,
                                                  navigation,
                                                  scierie,
                                                  gouvernail,
                                                  arme_de_siege,
                                                  alchimie,
                                                  bucheron,
                                                  charpente,
                                                  route,
                                                  evolution2
                                                  }, new Rectangle(zone.X + zone.Width / 14 + (zone.Width - zone.Width / 7) / 4, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 4, zone.Height / 10), "ERE 2");

            tabItem3 = new TabItem(new Control[] {
                                                  }, new Rectangle(zone.X + zone.Width / 14 + (zone.Width - zone.Width / 7) / 2, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 4, zone.Height / 10), "ERE 3");

            tabItem4 = new TabItem(new Control[] {
                                                  }, new Rectangle(zone.X + zone.Width / 14 + ((zone.Width - zone.Width / 7) * 3) / 4, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 4, zone.Height / 10), "ERE 4");

            tabItem1.Background = content.Load<Texture2D>(@"Technologies/onglet");
            tabItem2.Background = content.Load<Texture2D>(@"Technologies/onglet");
            tabItem3.Background = content.Load<Texture2D>(@"Technologies/onglet");
            tabItem4.Background = content.Load<Texture2D>(@"Technologies/onglet");

            tabcontrol = new TabControl(new TabItem[] {
                                                        tabItem1,
                                                        tabItem2,
                                                        tabItem3,
                                                        tabItem4
                                                        }, new Rectangle(zone.X + zone.Width/26, zone.Y + zone.Height / 10, zone.Width - zone.Width/13, zone.Height - zone.Height/10 - zone.Height/26),"tab Control");

            tabcontrol.Background = content.Load<Texture2D>(@"Technologies/TabControl");

            win = new Window(new Control[] {
                                            close,
                                            tabcontrol
                                            
            }, zone, name);
            win.Background = content.Load<Texture2D>(@"Technologies/window");
            win.TextColor = Color.Gainsboro;
			#region Prix

			_prixChasse.Add("Bois", 150);

			_prixFeu.Add("Bois", 200);

			_prixSilex.Add("Bois", 200);
			_prixSilex.Add("Nourriture", 50);

			_prixPierrePolie.Add("Bois", 250);
			_prixPierrePolie.Add("Nourriture", 100);

			_prixBronze.Add("Bois", 400);
			_prixBronze.Add("Nourriture", 200);
			_prixBronze.Add("Pierre", 50);

			_prixOutils.Add("Bois", 200);
			_prixOutils.Add("Nourriture", 100);
			_prixOutils.Add("Pierre", 50);

			_prixAgri.Add("Bois", 100);
			_prixAgri.Add("Nourriture", 50);

			_prixTorche.Add("Bois", 300);

			_prixIrrigation.Add("Nourriture", 500);

			_prixFer.Add("Pierre", 500);

            _prixEngrenage.Add("Bois", 700);

            _prixBucheron.Add("Bois", 100);
            _prixBucheron.Add("Fer", 300);

            _prixCharpente.Add("Bois", 800);

            _prixRoute.Add("Pierre", 1000);

            _prixArme_de_siege.Add("Bois", 1000);
            _prixArme_de_siege.Add("Fer", 500);

            _prixAlchimie.Add("Nourriture", 500);
            _prixAlchimie.Add("Or", 500);

            _prixForge.Add("Fer", 300);

            _prixGouvernail.Add("Bois", 1000);
            _prixGouvernail.Add("Fer", 100);

            _prixScierie.Add("Bois", 500);
            _prixScierie.Add("Fer", 300);

			#endregion Prix
		}

        public void Update(Souris s)
        {
            win.Update(s);
        }

        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            win.Draw(sb, sf);
        }

        public void Reset()
        {
            _ere1 = false;
            _ere2 = false;
            _ere3 = false;

            _fer = false;
            _chasse = false;
            _feu = false;
            _silex = false;
            _pierrePolie = false;
            _bronze = false;
            _outils = false;
            _agri = false;
            _torche = false;
            _irrigation = false;

            tabcontrol.Visible = true;
            tabItem1.Visible = true;
            chasse.Visible = true;
            evolution1.Visible = false;
            silex.Visible = false;
            agri.Visible = false;
            feu.Visible = false;
            torche.Visible = false;
            pierre_polie.Visible = false;
            bronze.Visible = false;
            outils.Visible = false;
            fer.Visible = false;
            irrigation.Visible = false;

            _engrenage = false;
            _forge = false;
            _navigation = false;
            _scierie = false;
            _gouvernail = false;
            _arme_de_siege = false;
            _alchimie = false;
            _bucheron = false;
            _charpente = false;
            _route = false;

            tabItem2.Visible = true;
            engrenage.Visible = false;
            forge.Visible = false;
            navigation.Visible = false;
            scierie.Visible = false;
            gouvernail.Visible = false;
            arme_de_siege.Visible = false;
            alchimie.Visible = false;
            bucheron.Visible = false;
            charpente.Visible = false;
            route.Visible = false;

            tabItem3.Visible = false;

            tabItem4.Visible = false;

            //chasse.OpacityMask = _u.OpacityMask;
            //silex.OpacityMask = _u.OpacityMask;
            //agri.OpacityMask = _u.OpacityMask;
            //feu.OpacityMask = _u.OpacityMask;
            //torche.OpacityMask = _u.OpacityMask;
            //pierre_polie.OpacityMask = _u.OpacityMask;
            //bronze.OpacityMask = _u.OpacityMask;
            //outils.OpacityMask = _u.OpacityMask;
            //fer.OpacityMask = _u.OpacityMask;
            //irrigation.OpacityMask = _u.OpacityMask;
            //evolution.OpacityMask = _u.OpacityMask;
        }

		private void button_Close_Click()
		{
			win.Visible = false;
		}

		private void chasse_MouseLeftButtonDown()
		{
			if (!_chasse && Game1.Joueur.Pay(_prixChasse))
			{
				silex.Visible = true;
				agri.Visible = true;
				feu.Visible = true;
				_chasse = true;
				//chasse.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Chasse » aquise !", Color.White, 5000));
			}
            else if (!_chasse)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void feu_MouseLeftButtonDown()
		{
			if (!_feu && Game1.Joueur.Pay(_prixFeu))
			{
				torche.Visible = true;
				Game1.Joueur.AdditionalLineSight += 128;
				_feu = true;
				//feu.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Feu » aquise !", Color.White, 5000));
			}
			else if (!_feu)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void silex_MouseLeftButtonDown()
		{
			if (!_silex && Game1.Joueur.Pay(_prixSilex))
			{
				pierre_polie.Visible = true;
				_silex = true;
				//silex.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Silex » aquise !", Color.White, 5000));
			}
			else if (!_silex)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void pierre_polie_MouseLeftButtonDown()
		{
			if (!_pierrePolie && Game1.Joueur.Pay(_prixPierrePolie))
			{
				bronze.Visible = true;
				_pierrePolie = true;
				//pierre_polie.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Pierre polie » aquise !", Color.White, 5000));
			}
			else if (!_pierrePolie)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void bronze_MouseLeftButtonDown()
		{
			if (!_bronze && Game1.Joueur.Pay(_prixBronze))
			{
				outils.Visible = true;
				_bronze = true;
				//bronze.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Bronze » aquise !", Color.White, 5000));
			}
			else if (!_bronze)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void outils_MouseLeftButtonDown()
		{
			if (!_outils && Game1.Joueur.Pay(_prixOutils))
			{
                if (_agri)
                    irrigation.Visible = true;
				fer.Visible = true;
				_outils = true;
				//outils.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Outils » aquise !", Color.White, 5000));
			}
			else if (!_outils)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void agri_MouseLeftButtonDown()
		{
			if (!_agri && Game1.Joueur.Pay(_prixAgri))
			{
                if (_outils)
                    irrigation.Visible = true;
				_agri = true;
				//agri.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Agriculture » aquise !", Color.White, 5000));
			}
			else if (!_agri)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void torche_MouseLeftButtonDown()
		{
			if (!_torche && Game1.Joueur.Pay(_prixTorche))
			{
				_torche = true;
				//torche.OpacityMask = null;
				Game1.Joueur.AdditionalLineSight += 128;
				MessagesManager.Messages.Add(new Msg("Technologie « Torche » aquise !", Color.White, 5000));
			}
			else if (!_torche)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void irrigation_MouseLeftButtonDown()
		{
			if (!_irrigation && Game1.Joueur.Pay(_prixIrrigation))
			{
                if (_fer)
                    evolution1.Visible = true;
				//irrigation.OpacityMask = null;
				_irrigation = true;
				MessagesManager.Messages.Add(new Msg("Technologie « Irrigation » aquise !", Color.White, 5000));
			}
			else if (!_irrigation)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

		private void fer_MouseLeftButtonDown()
		{
			if (!_fer && Game1.Joueur.Pay(_prixFer))
			{
                if (_irrigation)
                    evolution1.Visible = true;
				//fer.OpacityMask = null;
				_fer = true;
				MessagesManager.Messages.Add(new Msg("Technologie « Fer » aquise !", Color.White, 5000));
			}
			else if (!_fer)
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
		}

        private void engrenage_MouseLeftButtonDown()
        {
            if (!_engrenage && Game1.Joueur.Pay(_prixEngrenage))
            {
                forge.Visible = true;
                navigation.Visible = true;
                bucheron.Visible = true;
                _engrenage = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Engrenage » aquise !", Color.White, 5000));
            }
            else if (!_engrenage)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void forge_MouseLeftButtonDown()
        {
            if (!_forge && Game1.Joueur.Pay(_prixEngrenage))
            {
                scierie.Visible = true;
                _forge = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Forge » aquise !", Color.White, 5000));
            }
            else if (!_forge)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void scierie_MouseLeftButtonDown()
        {
            if (!_scierie && Game1.Joueur.Pay(_prixEngrenage))
            {
                arme_de_siege.Visible = true;
                _scierie = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Scierie » aquise !", Color.White, 5000));
            }
            else if (!_scierie)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void arme_de_siege_MouseLeftButtonDown()
        {
            if (!_arme_de_siege && Game1.Joueur.Pay(_prixEngrenage))
            {
                alchimie.Visible = true;
                _arme_de_siege = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Arme de siege » aquise !", Color.White, 5000));
            }
            else if (!_arme_de_siege)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void alchimie_MouseLeftButtonDown()
        {
            if (!_alchimie && Game1.Joueur.Pay(_prixEngrenage))
            {
                if (_gouvernail)
                {
                    evolution2.Visible = true;
                }
                _alchimie = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Alchimie » aquise !", Color.White, 5000));
            }
            else if (!_alchimie)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void navigation_MouseLeftButtonDown()
        {
            if (!_navigation && Game1.Joueur.Pay(_prixEngrenage))
            {
                gouvernail.Visible = true;
                _navigation = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Navigation » aquise !", Color.White, 5000));
            }
            else if (!_navigation)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void gouvernail_MouseLeftButtonDown()
        {
            if (!_gouvernail && Game1.Joueur.Pay(_prixEngrenage))
            {
                if (_alchimie)
                {
                    evolution2.Visible = true;
                }
                _gouvernail = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Gouvernail » aquise !", Color.White, 5000));
            }
            else if (!_gouvernail)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void bucheron_MouseLeftButtonDown()
        {
            if (!_bucheron && Game1.Joueur.Pay(_prixEngrenage))
            {
                charpente.Visible = true;
                _bucheron = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Bucheron » aquise !", Color.White, 5000));
            }
            else if (!_bucheron)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void charpente_MouseLeftButtonDown()
        {
            if (!_charpente && Game1.Joueur.Pay(_prixEngrenage))
            {
                route.Visible = true;
                _charpente = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Charpente » aquise !", Color.White, 5000));
            }
            else if (!_charpente)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void route_MouseLeftButtonDown()
        {
            if (!_route && Game1.Joueur.Pay(_prixEngrenage))
            {
                _route = true;
                //chasse.OpacityMask = null;
                MessagesManager.Messages.Add(new Msg("Technologie « Route » aquise !", Color.White, 5000));
            }
            else if (!_route)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

		private void evolution_MouseLeftButtonDown()
		{
			if (_fer && _irrigation && !_ere1)
			{
				_ere1 = true;
				//evolution.OpacityMask = null;
				Game1.Joueur.Ere = 2;
				tabItem3.Visible = true;
				Game1.FlashBool = true;
                engrenage.Visible = true;
				MessagesManager.Messages.Add(new Msg("ERE MEDIEVALE ATTEINTE !!", Color.Red, 5000));
				foreach (Building build in Game1.Joueur.Buildings)
				{
					build.UpdateEre(_content, Game1.Joueur);
				}
			}
            else if (_alchimie && _gouvernail && !_ere2)
            {
                _ere2 = true;
                Game1.Joueur.Ere = 3;
                tabItem4.Visible = true;
                Game1.FlashBool = true;
                MessagesManager.Messages.Add(new Msg("ERE IMPERIALE ATTEINTE !!", Color.Red, 5000));
                // Modif des sprites a faire içi
            }
            else if (_ere2 && !_ere3)
            {
                _ere3 = true;
                Game1.Joueur.Ere = 4;
                Game1.FlashBool = true;
                MessagesManager.Messages.Add(new Msg("ERE MODERNE ATTEINTE !!", Color.Red, 5000));
                // Modif des sprites a faire içi
            }
		}
    }
}
