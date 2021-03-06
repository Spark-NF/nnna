﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NNNA.Form;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    class TechnologiesWindow
    {
        private readonly Window win;
        public bool WinVisible
        {
            get { return win.Visible; }
            set { win.Visible = value; }
        }
        private Microsoft.Xna.Framework.Content.ContentManager _content;
        private readonly TabControl tabcontrol;
        private readonly TabItem tabItem1;
        private readonly TabItem tabItem2;
        private readonly TabItem tabItem3;
        #region BUTTONS
        private readonly Button close;
        private readonly Button chasse;
        private readonly Button silex;
        private readonly Button agri;
        private readonly Button pierre_polie;
        private readonly Button irrigation;
        private readonly Button bronze;
        private readonly Button outils;
        private readonly Button feu;
        private readonly Button torche;
        private readonly Button fer;
        private readonly Button evolution1;
        private readonly Button evolution2;
        private readonly Button engrenage;
        private readonly Button forge;
        private readonly Button scierie;
        private readonly Button arme_de_siege;
        private readonly Button navigation;
        private readonly Button gouvernail;
        private readonly Button alchimie;
        private readonly Button bucheron;
        private readonly Button charpente;
        private readonly Button route;
        #endregion
        #region BOOLEENS
        private bool _chasse;
        private bool _feu;
        private bool _silex;
        private bool _fer;
        private bool _pierrePolie;
        private bool _ere1;
        private bool _ere2;
        private bool _bronze;
        private bool _outils;
        private bool _agri;
        private bool _torche;
        private bool _irrigation;
        private bool _engrenage;
        private bool _forge;
        private bool _scierie;
        private bool _armeDeSiege;
        private bool _navigation;
        private bool _gouvernail;
        private bool _alchimie;
        private bool _bucheron;
        private bool _charpente;
        private bool _route;
        #endregion
        #region PRIX
        private readonly Dictionary<string, int> _prixFer = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixChasse = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixFeu = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixSilex = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixPierrePolie = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixBronze = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixOutils = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixAgri = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixTorche = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixIrrigation = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixEngrenage = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixForge = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixBucheron = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixNavigation = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixGouvernail = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixArmeDeSiege = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixCharpente = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixRoute = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixAlchimie = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _prixScierie = new Dictionary<string,int>();
        #endregion

        public TechnologiesWindow(Rectangle zone, string name, Microsoft.Xna.Framework.Content.ContentManager content)
		{
            _content = content;

            var ichasse = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4 - zone.Height / 3, (int) (zone.Width / 2.085f), zone.Height / 3), "chasse");
            var isilex = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "silex");
            var iagri = new InfoBulle(new Rectangle(zone.X + zone.Width / 2, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "agri");
            var ipierrePolie = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 12, (int)(zone.Width / 2.085f), zone.Height / 3), "pierre_polie");
            var iirrigation = new InfoBulle(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "irrigation");
            var ibronze = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "bronze");
            var ioutils = new InfoBulle(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "outils");
            var ifeu = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "feu");
            var itorche = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "torche");
            var ifer = new InfoBulle(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "fer");
            var ievolution1 = new InfoBulle(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "evolution1");
            
            ichasse.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/chasse");
            isilex.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/silex");
            iagri.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/agri");
            ipierrePolie.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/pierre_polie");
            iirrigation.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/irrigation");
            ibronze.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/bronze");
            ioutils.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/outils");
            ifeu.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/feu");
            itorche.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/torche");
            ifer.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/fer");
            ievolution1.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/evolution1");

            
            close = new Button(new Rectangle(zone.X + zone.Width - zone.Width / 30 - zone.Height / 25, zone.Y + zone.Height / 30, zone.Height / 25, zone.Height / 25), "close", button_Close_Click, true);

            chasse = new Button(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4, zone.Height / 15, zone.Height / 15), "chasse", chasse_MouseLeftButtonDown, false, ichasse);
            silex = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "silex", silex_MouseLeftButtonDown, false, isilex);
            agri = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "agri", agri_MouseLeftButtonDown, false, iagri);
            pierre_polie = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "pierre_polie", pierre_polie_MouseLeftButtonDown, false, ipierrePolie);
            irrigation = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "irrigation", irrigation_MouseLeftButtonDown, false, iirrigation);
            bronze = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "bronze", bronze_MouseLeftButtonDown, false, ibronze);
            outils = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5, zone.Height / 15, zone.Height / 15), "outils", outils_MouseLeftButtonDown, false, ioutils);
            feu = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "feu", feu_MouseLeftButtonDown, false, ifeu);
            torche = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "torche", torche_MouseLeftButtonDown, false, itorche);
            fer = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "fer", fer_MouseLeftButtonDown, false, ifer);
            evolution1 = new Button(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5, zone.Height / 15, zone.Height / 15), "evolution", evolution_MouseLeftButtonDown, false, ievolution1);

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

            var iengrenage = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "engrenage");
            var iforge = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "forge");
            var inavigation = new InfoBulle(new Rectangle(zone.X + zone.Width / 2, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "navigation");
            var iscierie = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 12, (int)(zone.Width / 2.085f), zone.Height / 3), "scierie");
            var igouvernail = new InfoBulle(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "gouvernail");
            var iarmeDeSiege = new InfoBulle(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "arme_de_siege");
            var ialchimie = new InfoBulle(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "alchimie");
            var ibucheron = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y, (int)(zone.Width / 2.085f), zone.Height / 3), "bucheron");
            var icharpente = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 3 + zone.Height / 12 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "fer");
            var iroute = new InfoBulle(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "fer");
            var ievolution2 = new InfoBulle(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5 - zone.Height / 3, (int)(zone.Width / 2.085f), zone.Height / 3), "evolution2");

            iengrenage.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/engrenage");
            iforge.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/forge");
            inavigation.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/navigation");
            iscierie.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/scierie");
            igouvernail.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/gouvernail");
            iarmeDeSiege.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/armes_de_siege");
            ialchimie.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/alchimie");
            ibucheron.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/bucheron");
            icharpente.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/charpente");
            iroute.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/route");
            ievolution2.Background = content.Load<Texture2D>(@"Technologies/Info Bulles/evolution2");

            engrenage = new Button(new Rectangle(zone.X + (zone.Width * 3 / 8), zone.Y + zone.Height / 4, zone.Height / 15, zone.Height / 15), "engrenage", engrenage_MouseLeftButtonDown, false, iengrenage);
            forge = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "forge", forge_MouseLeftButtonDown, false, iforge);
            navigation = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + zone.Height / 4 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "navigation", navigation_MouseLeftButtonDown, false, inavigation);
            scierie = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 3 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "scierie", scierie_MouseLeftButtonDown, false, iscierie);
            gouvernail = new Button(new Rectangle(zone.X + zone.Width / 2, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "gouvernail", gouvernail_MouseLeftButtonDown, false, igouvernail);
            arme_de_siege = new Button(new Rectangle(zone.X + zone.Width / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "arme_de_siege", arme_de_siege_MouseLeftButtonDown, false, iarmeDeSiege);
            alchimie = new Button(new Rectangle(zone.X + zone.Width / 3, zone.Y + (zone.Height * 3) / 5 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "alchimie", alchimie_MouseLeftButtonDown, false, ialchimie);
            bucheron = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 3, zone.Height / 15, zone.Height / 15), "bucheron", bucheron_MouseLeftButtonDown, false, ibucheron);
            charpente = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 3 + zone.Height / 12, zone.Height / 15, zone.Height / 15), "charpente", charpente_MouseLeftButtonDown, false, icharpente);
            route = new Button(new Rectangle(zone.X + (zone.Width * 3) / 4, zone.Y + zone.Height / 2, zone.Height / 15, zone.Height / 15), "route", route_MouseLeftButtonDown, false, iroute);
            evolution2 = new Button(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5, zone.Height / 15, zone.Height / 15), "evolution", evolution_MouseLeftButtonDown, false, ievolution2);

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
                                                  }, new Rectangle(zone.X + zone.Width / 14, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 3, zone.Height / 10), "ERE 1");

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
                                                  }, new Rectangle(zone.X + zone.Width / 14 + (zone.Width - zone.Width / 7) / 3, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 3, zone.Height / 10), "ERE 2");

            tabItem3 = new TabItem(new Control[] {
                                                  }, new Rectangle(zone.X + zone.Width / 14 + (zone.Width - zone.Width / 7)*2 / 3, zone.Y + zone.Height / 8, (zone.Width - zone.Width / 7) / 3, zone.Height / 10), "ERE 3");

            tabItem1.Background = content.Load<Texture2D>(@"Technologies/onglet");
            tabItem2.Background = content.Load<Texture2D>(@"Technologies/onglet");
            tabItem3.Background = content.Load<Texture2D>(@"Technologies/onglet");

            tabcontrol = new TabControl(new[] {
                                                    tabItem1,
                                                    tabItem2,
                                                    tabItem3,
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

            _prixArmeDeSiege.Add("Bois", 1000);
            _prixArmeDeSiege.Add("Fer", 500);

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
            _armeDeSiege = false;
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
        }

		private void button_Close_Click()
		{
			win.Visible = false;
		}

        private void chasse_MouseLeftButtonDown() // + 1 Attaque
		{
			if (!_chasse && Game1.Joueur.Pay(_prixChasse))
			{
				silex.Visible = true;
				agri.Visible = true;
				feu.Visible = true;
				_chasse = true;
			    Game1.Joueur.AdditionalAttack += 1;
			    Game1.Joueur.Caserne = true;
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

        private void feu_MouseLeftButtonDown() // + De luminosité 
		{
			if (!_feu && Game1.Joueur.Pay(_prixFeu))
			{
				torche.Visible = true;
				Game1.Joueur.AdditionalBuildingLineSight += 128;
				_feu = true;
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

        private void silex_MouseLeftButtonDown() // + 5 Attaque 
		{
			if (!_silex && Game1.Joueur.Pay(_prixSilex))
			{
				pierre_polie.Visible = true;
                Game1.Joueur.AdditionalAttack += 1;
				_silex = true;
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

        private void pierre_polie_MouseLeftButtonDown() // + 5 Attaque
		{
			if (!_pierrePolie && Game1.Joueur.Pay(_prixPierrePolie))
			{
				bronze.Visible = true;
                Game1.Joueur.AdditionalAttack += 1;
				_pierrePolie = true;
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

		private void bronze_MouseLeftButtonDown() // + 10 Attaque 
		{
			if (!_bronze && Game1.Joueur.Pay(_prixBronze))
			{
				outils.Visible = true;
                Game1.Joueur.AdditionalAttack += 1;
				_bronze = true;
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

        private void outils_MouseLeftButtonDown() // Construction Batiment + Rapide ( A FAIRE ! )
		{
			if (!_outils && Game1.Joueur.Pay(_prixOutils))
			{
                if (_agri)
                    irrigation.Visible = true;
				fer.Visible = true;
				_outils = true;
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

        private void agri_MouseLeftButtonDown()// Creation Ferme
		{
			if (!_agri && Game1.Joueur.Pay(_prixAgri))
			{
                if (_outils)
                    irrigation.Visible = true;
                Game1.Joueur.Ferme = true;
				_agri = true;
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

		private void torche_MouseLeftButtonDown() // + 128 LineSight
		{
			if (!_torche && Game1.Joueur.Pay(_prixTorche))
			{
				_torche = true;
				Game1.Joueur.AdditionalUnitLineSight += 128;
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

		private void irrigation_MouseLeftButtonDown() // Augmente la vitesse de colte de la nourriture 
		{
			if (!_irrigation && Game1.Joueur.Pay(_prixIrrigation))
			{
                if (_fer)
                    evolution1.Visible = true;
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

		private void fer_MouseLeftButtonDown() // + 1 Attaque
		{
			if (!_fer && Game1.Joueur.Pay(_prixFer))
			{
                if (_irrigation)
                    evolution1.Visible = true;
                Game1.Joueur.AdditionalAttack += 1;
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

        //ERE 2

        private void engrenage_MouseLeftButtonDown() // Debloque Techno
        {
            if (!_engrenage && Game1.Joueur.Pay(_prixEngrenage))
            {
                forge.Visible = true;
                navigation.Visible = true;
                bucheron.Visible = true;
                _engrenage = true;
                Game1.Joueur.Moulin = true;
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

        private void forge_MouseLeftButtonDown() // + 20 de vie
        {
            if (!_forge && Game1.Joueur.Pay(_prixForge))
            {
                scierie.Visible = true;
                Game1.Joueur.AdditionalLife += 20;
                _forge = true;
                Game1.Joueur.Forge = true;
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

        private void scierie_MouseLeftButtonDown()// Ya pas de camp de bucheron pour linstant
        {
            if (!_scierie && Game1.Joueur.Pay(_prixScierie))
            {
                arme_de_siege.Visible = true;
                _scierie = true;
                Game1.Joueur.Scierie = true;
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

        private void arme_de_siege_MouseLeftButtonDown() // Debloque techno
        {
            if (!_armeDeSiege && Game1.Joueur.Pay(_prixArmeDeSiege))
            {
                alchimie.Visible = true;
                _armeDeSiege = true;
                Game1.Joueur.ArmeDeSiege = true;
                MessagesManager.Messages.Add(new Msg("Technologie « Arme de siege » aquise !", Color.White, 5000));
            }
            else if (!_armeDeSiege)
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Technologie deja aquise.", Color.Red, 5000));
            }
        }

        private void alchimie_MouseLeftButtonDown()// Debloque Techno
        {
            if (!_alchimie && Game1.Joueur.Pay(_prixAlchimie))
            {
                if (_gouvernail)
                {
                    evolution2.Visible = true;
                }
                _alchimie = true;
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

        private void navigation_MouseLeftButtonDown()// BULLSHIT
        {
            if (!_navigation && Game1.Joueur.Pay(_prixNavigation))
            {
                gouvernail.Visible = true;
                _navigation = true;
                Game1.Joueur.Port = true;
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

        private void gouvernail_MouseLeftButtonDown() // Pas de bateaux -_- 
        {
            if (!_gouvernail && Game1.Joueur.Pay(_prixGouvernail))
            {
                if (_alchimie)
                {
                    evolution2.Visible = true;
                }
                _gouvernail = true;
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

        private void bucheron_MouseLeftButtonDown() // augmente les poches
        {
            if (!_bucheron && Game1.Joueur.Pay(_prixBucheron))
            {
                charpente.Visible = true;
                Game1.Joueur.AdditionalPoches += 20;
                _bucheron = true;
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

        private void charpente_MouseLeftButtonDown() // augmente les pv des batiments
        {
            if (!_charpente && Game1.Joueur.Pay(_prixCharpente))
            {
                route.Visible = true;
                Game1.Joueur.AdditionalBuildingLife += 100;
                _charpente = true;
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

        private void route_MouseLeftButtonDown() // 2x plus vite unité
        {
            if (!_route && Game1.Joueur.Pay(_prixRoute))
            {
                _route = true;
                Game1.Joueur.AdditionalSpeed += 0.02f;
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
                Game1.FlashBool = true;
                MessagesManager.Messages.Add(new Msg("ERE IMPERIALE ATTEINTE !!", Color.Red, 5000));
            }
		}
    }
}
