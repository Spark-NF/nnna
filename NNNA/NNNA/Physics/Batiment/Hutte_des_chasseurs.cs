using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	class HutteDesChasseurs : ProductionBuilding
	{
		public HutteDesChasseurs(int x = 0, int y = 0)
			: base(x, y)
		{
			_type = "caserne";
			_cost.Add("Bois", 75);
		}

		public HutteDesChasseurs(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_type = "caserne";
			LoadContent(content, "Batiments/caserne_" + joueur.Ere.ToString(CultureInfo.CurrentCulture), 6);
			Texture.Animation = false;
			Life = 100;
			LineSight = 2 * 64;
			_cost.Add("Bois", 75);
		}
	}
}
