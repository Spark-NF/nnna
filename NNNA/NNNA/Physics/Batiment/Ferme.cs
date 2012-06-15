using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// Ere 2
	[Serializable]
	class Ferme : ResourceMine
	{
		public Ferme(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y, joueur.Resource("Nourriture"), 500, new Image(content, "Batiments/ferme2"))
		{ }
    }
}
