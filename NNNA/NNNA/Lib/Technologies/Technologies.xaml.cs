using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	/// <summary>
	/// Interaction logic for Technologies.xaml
	/// </summary>
	public partial class Technologies : UserControl
	{
		private UIElement _u;
		private new ContentManager _content;
		internal Joueur Joueur;
		private ElementHost _elementHost;
		private bool _chasse, _feu, _silex, _fer, _pierrePolie, _ere1, _bronze, _outils, _agri, _torche, _irrigation;
		private Dictionary<string, int> _prixFer = new Dictionary<string, int>(), _prixChasse = new Dictionary<string, int>(), _prixFeu = new Dictionary<string, int>(), _prixSilex = new Dictionary<string, int>(), _prixPierrePolie = new Dictionary<string, int>(), _prixBronze = new Dictionary<string, int>(), _prixOutils = new Dictionary<string, int>(), _prixAgri = new Dictionary<string, int>(), _prixTorche = new Dictionary<string, int>(), _prixIrrigation = new Dictionary<string, int>();
		internal Technologies(Joueur joueur, ref ElementHost elementHost, ContentManager content)
		{
			_content = content;
			InitializeComponent();
			Joueur = joueur;
			_elementHost = elementHost;
			elementHost.Visible = false;
			evolution.Visibility = Visibility.Visible;
			_u = new UIElement { OpacityMask = chasse.OpacityMask };

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

		internal void Pre_Update(Joueur joueur)
		{
			Joueur._resources = joueur._resources;
			Joueur.Buildings = joueur.Buildings;
			Joueur.Units = joueur.Units;
		}

		internal void Post_Update(Joueur joueur)
		{
			joueur._resources = Joueur._resources;
			joueur.AdditionalLineSight = Joueur.AdditionalLineSight;
			joueur.Ere = Joueur.Ere;
			joueur.Buildings = Joueur.Buildings;
			joueur.Units = Joueur.Units;
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
			silex.Visibility = Visibility.Hidden;
			agri.Visibility = Visibility.Hidden;
			feu.Visibility = Visibility.Hidden;
			torche.Visibility = Visibility.Hidden;
			pierre_polie.Visibility = Visibility.Hidden;
			bronze.Visibility = Visibility.Hidden;
			outils.Visibility = Visibility.Hidden;
			fer.Visibility = Visibility.Hidden;
			irrigation.Visibility = Visibility.Hidden;
			tabItem2.Visibility = Visibility.Hidden;
			chasse.OpacityMask = _u.OpacityMask;
			silex.OpacityMask = _u.OpacityMask;
			agri.OpacityMask = _u.OpacityMask;
			feu.OpacityMask = _u.OpacityMask;
			torche.OpacityMask = _u.OpacityMask;
			pierre_polie.OpacityMask = _u.OpacityMask;
			bronze.OpacityMask = _u.OpacityMask;
			outils.OpacityMask = _u.OpacityMask;
			fer.OpacityMask = _u.OpacityMask;
			irrigation.OpacityMask = _u.OpacityMask;
			evolution.OpacityMask = _u.OpacityMask;
		}

		private void button_Close_Click(object sender, RoutedEventArgs e)
		{
			_elementHost.Visible = false;
		}

		private void chasse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_chasse && Joueur.Pay(_prixChasse))
			{
				silex.Visibility = Visibility.Visible;
				agri.Visibility = Visibility.Visible;
				feu.Visibility = Visibility.Visible;
				_chasse = true;
				chasse.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Chasse » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixChasse))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void feu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_feu && Joueur.Pay(_prixFeu))
			{
				torche.Visibility = Visibility.Visible;
				this.Joueur.AdditionalLineSight += 128;
				_feu = true;
				feu.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Feu » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixFeu))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void silex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_silex && Joueur.Pay(_prixSilex))
			{
				pierre_polie.Visibility = Visibility.Visible;
				_silex = true;
				silex.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Silex » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixSilex))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void pierre_polie_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_pierrePolie && Joueur.Pay(_prixPierrePolie))
			{
				bronze.Visibility = Visibility.Visible;
				_pierrePolie = true;
				pierre_polie.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Pierre polie » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixPierrePolie))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void bronze_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_bronze && Joueur.Pay(_prixBronze))
			{
				outils.Visibility = Visibility.Visible;
				_bronze = true;
				bronze.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Bronze » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixBronze))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void outils_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_outils && Joueur.Pay(_prixOutils))
			{
				fer.Visibility = Visibility.Visible;
				_outils = true;
				outils.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Outils » aquise !", Color.White, 5000));
			}
			if (_agri)
				irrigation.Visibility = Visibility.Visible;
			else if (!Joueur.Pay(_prixOutils))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void agri_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_outils)
				irrigation.Visibility = Visibility.Visible;
			if (!_agri && Joueur.Pay(_prixAgri))
			{
				_agri = true;
				agri.OpacityMask = null;
				MessagesManager.Messages.Add(new Msg("Technologie « Agriculture » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixAgri))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void torche_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_torche && Joueur.Pay(_prixTorche))
			{
				_torche = true;
				torche.OpacityMask = null;
				this.Joueur.AdditionalLineSight += 128;
				MessagesManager.Messages.Add(new Msg("Technologie « Torche » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixTorche))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void irrigation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!_irrigation && Joueur.Pay(_prixIrrigation))
			{
				irrigation.OpacityMask = null;
				_irrigation = true;
				MessagesManager.Messages.Add(new Msg("Technologie « Irrigation » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixIrrigation))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void fer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_outils && Joueur.Pay(_prixFer))
			{
				fer.OpacityMask = null;
				_fer = true;
				MessagesManager.Messages.Add(new Msg("Technologie « Fer » aquise !", Color.White, 5000));
			}
			else if (!Joueur.Pay(_prixFer))
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
			}
		}

		private void evolution_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_fer && _irrigation && !_ere1)
			{
				_ere1 = true;
				evolution.OpacityMask = null;
				Joueur.Ere = 2;
				tabItem2.Visibility = Visibility.Visible;
				Game1.FlashBool = true;
				MessagesManager.Messages.Add(new Msg("ERE MEDIEVALE ATTEINTE !!", Color.Red, 5000));
				foreach (Building build in Joueur.Buildings)
				{
					build.UpdateEre(_content, Joueur);
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
