using System;
using System.Runtime.InteropServices;
using System.Net;

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
			IPHostEntry iphe = Dns.Resolve(computername);
			string ip = iphe.AddressList[0].ToString();
			return ip;
		}
		/// <summary>
		/// Fonction qui verifie si l'ordinateur est connecté a internet.
		/// </summary>
		// Déclaration de l'API
		[DllImport("wininet.dll")]
		public extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

		// Utilisation de l'API
		public static bool IsConnected()
		{
			int Desc;
			return InternetGetConnectedState(out Desc, 0);
		}

		public static string Connected()
	    {
		    if (IsConnected())
		    { return "Vous etes connecte"; }
		    else
		    { return "Vous etes deconnecte"; }
	    }
	}
}
