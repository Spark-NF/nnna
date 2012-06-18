using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
	class Program
	{
		public static List<TcpClient> Users;
		public static List<String> Pseudos;
		public static List<Thread> Threads;
		public static List<int> ReceivedPackets;

		public static bool IsRunning = true;
		public static TcpListener Server;
		public static Thread ThreadWait;

		public static String Minimap = "", Players = "", Resources = "";

		static void Main()
		{
			Users = new List<TcpClient>();
			Pseudos = new List<String>();
			Threads = new List<Thread>();
			ReceivedPackets = new List<int>();

			Console.Write("Setting up server...");
			Server = new TcpListener(IPAddress.Any, 25666);
			Server.Start();
			Console.WriteLine(" Done.");

			ThreadWait = new Thread(Wait);
			ThreadWait.Start();
		}

		public static void Wait()
		{
			while (IsRunning)
			{
				Console.WriteLine("\r\nWaiting for users to join.");
				Console.WriteLine("Users connected: {0}", Users.Count);

				TcpClient client = Server.AcceptTcpClient();
				Users.Add(client);
				Pseudos.Add("");
				ReceivedPackets.Add(0);

				Console.WriteLine("Connected!");

				Console.Write("Starting updating thread for user {0}...", Users.Count);
				var thread = new Thread(Update);
				thread.Start();
				Threads.Add(thread);
				Console.WriteLine(" Done.");
			}

			for (int i = 0; i < Users.Count; i++)
			{
				Users[i].GetStream().Close();
				Users[i].Close();
				Threads[i].Abort();
			}

			Console.WriteLine("\r\nServer closed.");
		}

		public static void Update()
		{
			int u = Users.Count - 1;
			var user = Users[u];
			var stream = user.GetStream();

			while (IsRunning)
			{
				// On lit les données envoyées par l'utilisateur
				String data = "";
				var bit = new Byte[1];
				try
				{
					int i;
					while ((i = stream.Read(bit, 0, 1)) != 0 && bit[0] != 0)
					{
						data += System.Text.Encoding.ASCII.GetString(bit, 0, i);
						if (bit[0] == 0)
						{ break; }
					}
				}
				catch (IOException)
				{
					Console.WriteLine("User #{0} \"{1}\" aborted connection.", u, Pseudos[u]);
					user.Close();
					Users.RemoveAt(u);
					return;
				}

				if (data != "")
				{
					if (Pseudos[u] == "")
					{ Pseudos[u] = data; }
					Console.WriteLine("Received from user {0}: {1} ({2})", Pseudos[u], data.GetType(), data.Length);

					// Si le joueur vient d'arriver, on lui envoie les informations du serveur
					if (ReceivedPackets[u] == 0)
					{
						Console.WriteLine("Received pseudo from user {0}", Pseudos[u]);
						if (u > 0)
						{
							Console.WriteLine("Sending terrain data to user {0}", Pseudos[u]);
							SendToUser(stream, u, Minimap);
							SendToUser(stream, u, u.ToString(CultureInfo.InvariantCulture));
							SendToUser(stream, u, Players);
							SendToUser(stream, u, Resources);
							SendToAllButOne(user, "join "+u+" "+Pseudos[u]);
						}
					}
					// Réception des informations du serveur depuis l'hôte
					else if (u == 0 && ReceivedPackets[u] == 1)
					{
						Minimap = data;
						Console.WriteLine("Received terrain data from user {0}", Pseudos[u]);
					}
					else if (u == 0 && ReceivedPackets[u] == 2)
					{
						Players = data;
						Console.WriteLine("Received players data from user {0}", Pseudos[u]);
					}
					else if (u == 0 && ReceivedPackets[u] == 3)
					{
						Resources = data;
						Console.WriteLine("Received resources data from user {0}", Pseudos[u]);
					}
					// Sinon c'est une action que l'on renvoie le message à tous les autres utilisateurs
					else
					{ SendToAllButOne(user, data); }

					// En cas de déconnexion du joueur
					if (data == "close")
					{
						Console.WriteLine("User {0} left server.", Pseudos[u]);
						user.Close();
						Users.RemoveAt(u);
						return;
					}

					ReceivedPackets[u]++;
				}
			}
		}

		public static void SendToUser(NetworkStream stream, int u, String message)
		{
			Byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
			Array.Resize(ref msg, msg.Length + 1);
			stream.Write(msg, 0, msg.Length);
			Console.WriteLine("Sent to user #{0} {1}: {2} ({3})", u, Pseudos[u], message.GetType(), message.Length);
		}

		/// <summary>
		/// Envoie un message à tous les autres joueurs sauf un.
		/// </summary>
		/// <param name="user">Le joueur à qui ne pas envoyer le message.</param>
		/// <param name="message">Le message à envoyer.</param>
		public static void SendToAllButOne(TcpClient user, String message)
		{
			for (int j = 0; j < Users.Count; j++)
			{
				var other = Users[j];
				if (other != user)
				{
					NetworkStream send = other.GetStream();
					SendToUser(send, j, message);
				}
			}
		}
	}
}
