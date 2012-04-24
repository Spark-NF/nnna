using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;

namespace NNNA
{
    class Réseau
    {
        /// <summary>
        /// Fonction qui donne l'adresse ip d'un ordinateur d'on le nom est pcé en parametre
        /// </summary>
        /// <param name="computername">Nom du PC.</param>
        /// <returns>L'IP du PC.</returns>
        public static string GetIPaddresses(string computername)
        {
            IPHostEntry iphe = Dns.GetHostEntry(computername);//Dns.Resolve(computername);
            string ip = iphe.AddressList[0].ToString();
            return ip;
        }
        /*public string IpWan()
         {
            string ip;
            string ipexterne;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect("http://whatismyipaddress.com/",80);
            Stream st = new NetworkStream(s);
            
            //<a href="/ip/
              //  ">
            return ipexterne;
         }*/
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
            { return "Vous etes connecte"; }
            return "Vous etes deconnecte";
        }
        public static void Mapsender(IPAddress ipAdress, char[,] mapInByte)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            NetworkStream ns = new NetworkStream(s);
            StreamWriter sw = new StreamWriter(ns);
            sw.Write(mapInByte);


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
    }
}
