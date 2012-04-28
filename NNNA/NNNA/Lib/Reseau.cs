  using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Net;

namespace NNNA
{
    class Réseau
    {
        static bool jeuMulti;
        public bool JeuMulti
        {
            get { return jeuMulti; }
            set { jeuMulti = value; }
        }
        private List<Socket> connexions = new List<Socket>();
        /// <summary>
        /// Fonction qui donne l'adresse ip d'un ordinateur d'on le nom est placé en parametre
        /// </summary>
        /// <param name="computername">Nom du PC.</param>
        /// <returns>L'IP du PC.</returns>
        public static string GetIPaddresses(string computername)
        {
            IPHostEntry iphe =Dns.Resolve(computername);
            string ip = iphe.AddressList[0].ToString();
            return ip;
            
        }
        public static string IpWan()
         {
            string ipexterne;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Uri uri = new Uri("http://www.whatismyip.fr/raw/");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                WebResponse response = request.GetResponse();
                StreamReader read = new StreamReader(response.GetResponseStream());
                ipexterne = read.ReadToEnd();
                return ipexterne;
            }
            catch
            {
                return GetIPaddresses(Environment.MachineName);
            }
            
            
         }
        /// <summary>
        /// Fonction qui verifie si l'ordinateur est connecté a internet.
        /// </summary>
        // Déclaration de l'API
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int description, int reservedValue);

        // Utilisation de l'API
        public static bool IsConnected()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }

        public static string Connected()
        {
            if (IsConnected())
            { return "Vous êtes connecté"; }
            return "Vous êtes déconnecté";
        }
		
        public static char[,] MapInBytes(Sprite[,] _matrice)
        {
            char[,] mapInBytes = new char[_matrice.GetLength(0), _matrice.GetLength(1)];
            for (int i = 0; i < _matrice.GetLength(0); i++)
            {
                for (int j = 0; j < _matrice.GetLength(1); j++)
                {
                    mapInBytes[i, j] = _matrice[i, j].Name;
                }
            }
            return mapInBytes;
        }
        public static void Mapsender(IPAddress destinataire,int port, char[,] mapInByte)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(destinataire, port);
            if (s.Connected)
            {
                NetworkStream ns = new NetworkStream(s);
                StreamWriter sw = new StreamWriter(ns);
                sw.Write(Math.Log( (double) mapInByte.GetLength(0),(double)255));
                sw.Write(mapInByte.GetLength(0));
                sw.Write(mapInByte);
                jeuMulti = true;
                
            }
            else
            {
                MessagesManager.Messages.Add(new Msg("Error 418 I'm a Teapot",Color.Red, 5000));
            }
        }
        public static char[,] Mapreceived(IPAddress expediteur, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(expediteur, port);
                s.Listen(4);
                NetworkStream ns = new NetworkStream(s);
                StreamReader sr = new StreamReader(ns);
                char[] ligne = new char[sr.Read()];
                int nbrligne = sr.Read(ligne, 1, ligne.Length);
                char[] mapinarray = sr.ReadToEnd().ToCharArray();
                char[,] mapreceived = new char[nbrligne, mapinarray.Length / nbrligne];
                for (int i = 1; i <= mapreceived.GetLength(0); i++)
                {
                    for (int j = 0; j <= mapreceived.GetLength(1); j++)
                    {
                        mapreceived[i, j] = mapinarray[i * j];
                    }
                }
                return mapreceived;  
            }
            catch 
            {
                MessagesManager.Messages.Add(new Msg("Carte non reçut",Color.Red,5000));
                throw;
            }
        }
        public static void RecherchePartie(int nbrJoueurs)
        {
            AvailableNetworkSessionCollection partiesDisponibles = NetworkSession.Find(NetworkSessionType.Local, nbrJoueurs, null);
            partiesDisponibles.ToString();
        }
        public void CréePartie(int nbrJoueur)
        {
            NetworkSession partie = NetworkSession.Create(NetworkSessionType.Local, nbrJoueur, nbrJoueur);
        }
        public void lancerpartie(NetworkSession partie,char[] _matrice)
        {
            PacketWriter donné =new PacketWriter() ;
            donné.Write(_matrice);
            partie.LocalGamers[0].SendData(donné, SendDataOptions.ReliableInOrder);
            partie.StartGame();
        }
        public void RecevoirDonnées(NetworkSession partie)
        {
            PacketReader donné = new PacketReader();
            LocalNetworkGamer gamer = partie.LocalGamers[0];
            if (gamer.IsDataAvailable)
            {
                NetworkGamer sender;
                gamer.ReceiveData(donné, out sender);
            }
            


        }
    }
}
