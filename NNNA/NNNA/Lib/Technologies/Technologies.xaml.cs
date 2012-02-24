using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms.Integration;
using Microsoft.Xna.Framework;

namespace NNNA
{
    /// <summary>
    /// Interaction logic for Technologies.xaml
    /// </summary>
    public partial class Technologies : UserControl
    {
        internal Joueur joueur;
        private ElementHost elementHost;
        private bool m_chasse, m_feu, m_silex, m_fer, m_pierre_polie, m_ere1, m_ere2, m_bronze, m_outils, m_agri, m_torche, m_irrigation;
        private Dictionary<string, int> Prix_fer = new Dictionary<string, int>(), Prix_chasse = new Dictionary<string, int>(), Prix_feu = new Dictionary<string, int>(), Prix_silex = new Dictionary<string, int>(), Prix_pierre_polie = new Dictionary<string, int>(), Prix_bronze = new Dictionary<string, int>(), Prix_outils = new Dictionary<string, int>(), Prix_agri = new Dictionary<string, int>(), Prix_torche = new Dictionary<string, int>(), Prix_irrigation = new Dictionary<string, int>();
        internal Technologies(Joueur joueur, ref ElementHost elementHost)
        {
            InitializeComponent();
            this.joueur = joueur;
            this.elementHost = elementHost;
            elementHost.Visible = false;
            #region Prix
            Prix_chasse.Add("Bois", 150);
            Prix_feu.Add("Bois", 200);
            Prix_silex.Add("Bois", 200);
            Prix_silex.Add("Nourriture", 50);
            Prix_pierre_polie.Add("Bois", 250);
            Prix_pierre_polie.Add("Nourriture", 100);
            Prix_bronze.Add("Bois", 400);
            Prix_bronze.Add("Nourriture", 200);
            Prix_bronze.Add("Pierre", 50);
            Prix_outils.Add("Bois", 200);
            Prix_outils.Add("Nourriture", 100);
            Prix_outils.Add("Pierre", 50);
            Prix_agri.Add("Bois", 100);
            Prix_agri.Add("Nourriture", 50);
            Prix_torche.Add("Bois", 300);
            Prix_irrigation.Add("Nourriture", 500);
            Prix_fer.Add("Pierre", 500);
            #endregion Prix
        }

        internal void Pre_Update(Joueur joueur)
        {
            this.joueur.m_resources = joueur.m_resources;
            this.joueur.Additional_line_sight = joueur.Additional_line_sight;
            this.joueur.Ere = joueur.Ere;
        }

        internal void Post_Update(Joueur joueur)
        {
            joueur.m_resources = this.joueur.m_resources;
            joueur.Additional_line_sight = this.joueur.Additional_line_sight;
            joueur.Ere = this.joueur.Ere;
        }

        public void Reset()
        {
            m_ere1 = false;
            m_ere2 = false;
            m_fer = false;
            m_chasse = false;
            m_feu = false;
            m_silex = false;
            m_pierre_polie = false;
            m_bronze = false;
            m_outils = false;
            m_agri = false;
            m_torche = false;
            m_irrigation = false;
            silex.Visibility = Visibility.Hidden;
            agri.Visibility = Visibility.Hidden;
            feu.Visibility = Visibility.Hidden;
            torche.Visibility = Visibility.Hidden;
            pierre_polie.Visibility = Visibility.Hidden;
            bronze.Visibility = Visibility.Hidden;
            outils.Visibility = Visibility.Hidden;
            fer.Visibility = Visibility.Hidden;
            irrigation.Visibility = Visibility.Hidden;
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            elementHost.Visible = false;
        }

        private void chasse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_chasse && joueur.Pay(Prix_chasse))
            {
                silex.Visibility = Visibility.Visible;
                agri.Visibility = Visibility.Visible;
                feu.Visibility = Visibility.Visible;
                m_chasse = true;
            }
            if (!joueur.Pay(Prix_chasse))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void feu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_feu && joueur.Pay(Prix_feu))
            {
                torche.Visibility = Visibility.Visible;
                this.joueur.Additional_line_sight += 128;
                m_feu = true;
            }
            if (!joueur.Pay(Prix_feu))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void silex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_silex && joueur.Pay(Prix_silex))
            {
                pierre_polie.Visibility = Visibility.Visible;
                m_silex = true;
            }
            if (!joueur.Pay(Prix_silex))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void pierre_polie_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_pierre_polie && joueur.Pay(Prix_pierre_polie))
            {
                bronze.Visibility = Visibility.Visible;
                m_pierre_polie = true;
            }
            if (!joueur.Pay(Prix_pierre_polie))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void bronze_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_bronze && joueur.Pay(Prix_bronze))
            {
                outils.Visibility = Visibility.Visible;
                m_bronze = true;
            }
            if (!joueur.Pay(Prix_bronze))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void outils_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_outils && joueur.Pay(Prix_outils))
            {
                fer.Visibility = Visibility.Visible;
                m_outils = true;
            }
            if (m_agri)
                irrigation.Visibility = Visibility.Visible;
            if (!joueur.Pay(Prix_outils))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void agri_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_outils)
                irrigation.Visibility = Visibility.Visible;
            if (!m_agri && joueur.Pay(Prix_agri))
                m_agri = true;
            if (!joueur.Pay(Prix_agri))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void torche_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_torche && joueur.Pay(Prix_torche))
            {
                m_torche = true;
                this.joueur.Additional_line_sight += 128;
            }
            if (!joueur.Pay(Prix_torche))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void irrigation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_irrigation && joueur.Pay(Prix_irrigation))
                m_irrigation = true;
            if (!joueur.Pay(Prix_irrigation))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void fer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_outils && joueur.Pay(Prix_fer))
                m_fer = true;
            if (!joueur.Pay(Prix_fer))
            {
                MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de ressources.", Color.Red, 5000));
            }
        }

        private void evolution_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_fer && m_irrigation && !m_ere1)
            {
                m_ere1 = true;
                joueur.Ere = 2;
                MessagesManager.Messages.Add(new Msg("ERE MEDIEVALE ATTEINTE !!", Color.Red, 5000));
            }
            else if (!m_ere2)
            {
                m_ere2 = true;
                joueur.Ere = 3;
                MessagesManager.Messages.Add(new Msg("ERE IMPERIALE ATTEINTE !!", Color.Red, 5000));
            }
        }
    }
}
