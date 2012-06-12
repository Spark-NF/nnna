using System;

namespace NNNA
{
	/// <summary>
	/// ProductionBuilding représente un bâtiment qui permet de produire des unités.
	/// </summary>
	[Serializable]
	class ProductionBuilding : Building
	{
		public ProductionBuilding(int x, int y)
            : base(x, y)
        { }
	}
}
