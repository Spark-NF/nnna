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
        private TabItem tabItem1, tabItem2;
        private Button close, chasse, silex, agri, pierre_polie, irrigation, bronze, outils, feu, torche, fer, evolution;
		private bool _chasse, _feu, _silex, _fer, _pierrePolie, _ere1, _bronze, _outils, _agri, _torche, _irrigation;
		private readonly Dictionary<string, int> _prixFer = new Dictionary<string, int>(), _prixChasse = new Dictionary<string, int>(), _prixFeu = new Dictionary<string, int>(), _prixSilex = new Dictionary<string, int>(), _prixPierrePolie = new Dictionary<string, int>(), _prixBronze = new Dictionary<string, int>(), _prixOutils = new Dictionary<string, int>(), _prixAgri = new Dictionary<string, int>(), _prixTorche = new Dictionary<string, int>(), _prixIrrigation = new Dictionary<string, int>();
		
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
            evolution = new Button(new Rectangle(zone.X + (zone.Width * 5 / 12), zone.Y + (zone.Height * 4) / 5, zone.Height / 15, zone.Height / 15), "evolution", evolution_MouseLeftButtonDown);

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
            evolution.Background = content.Load<Texture2D>(@"Technologies/evolution");

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
                                                  evolution
                                                  }, new Rectangle(zone.X + zone.Width / 26, zone.Y + zone.Height / 10, (zone.Width - zone.Width / 13) / 4, zone.Height / 10), "ERE 1");

            tabItem2 = new TabItem(new Control[] {
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown),
                                                  new Button(new Rectangle(), "chasse", chasse_MouseLeftButtonDown)
                                                  }, new Rectangle(zone.X + zone.Width / 26 + (zone.Width - zone.Width / 13) / 4, zone.Y + zone.Height / 10, (zone.Width - zone.Width / 13) / 4, zone.Height / 10), "ERE 2");

            tabcontrol = new TabControl(new TabItem[] {
                                                        tabItem1,
                                                        tabItem2
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
            evolution.Visible = false;
			silex.Visible = false;
			agri.Visible = false;
			feu.Visible = false;
			torche.Visible = false;
			pierre_polie.Visible = false;
			bronze.Visible = false;
			outils.Visible = false;
			fer.Visible = false;
			irrigation.Visible = false;
			tabItem2.Visible = false;

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
			if (!_chasse && Game1._joueur.Pay(_prixChasse))
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
			if (!_feu && Game1._joueur.Pay(_prixFeu))
			{
				torche.Visible = true;
				Game1._joueur.AdditionalLineSight += 128;
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
			if (!_silex && Game1._joueur.Pay(_prixSilex))
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
			if (!_pierrePolie && Game1._joueur.Pay(_prixPierrePolie))
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
			if (!_bronze && Game1._joueur.Pay(_prixBronze))
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
			if (!_outils && Game1._joueur.Pay(_prixOutils))
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
			if (!_agri && Game1._joueur.Pay(_prixAgri))
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
			if (!_torche && Game1._joueur.Pay(_prixTorche))
			{
				_torche = true;
				//torche.OpacityMask = null;
				Game1._joueur.AdditionalLineSight += 128;
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
			if (!_irrigation && Game1._joueur.Pay(_prixIrrigation))
			{
                if (_fer)
                    evolution.Visible = true;
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
			if (_outils && Game1._joueur.Pay(_prixFer))
			{
                if (_irrigation)
                    evolution.Visible = true;
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

		private void evolution_MouseLeftButtonDown()
		{
			if (_fer && _irrigation && !_ere1)
			{
				_ere1 = true;
				//evolution.OpacityMask = null;
				Game1._joueur.Ere = 2;
				tabItem2.Visible = true;
				Game1.FlashBool = true;
				MessagesManager.Messages.Add(new Msg("ERE MEDIEVALE ATTEINTE !!", Color.Red, 5000));
				foreach (Building build in Game1._joueur.Buildings)
				{
					build.UpdateEre(_content, Game1._joueur);
				}
			}
			//else if (!m_ere2)
			//{
			//    m_ere2 = true;
			//    joueur.Ere = 3;
			//    MessagesManager.Messages.Add(new Msg("ERE IMPERIALE ATTEINTE !!", Color.Red, 5000));
			//}
		}
    }
}
