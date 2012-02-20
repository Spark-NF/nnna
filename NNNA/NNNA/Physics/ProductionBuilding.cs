using Microsoft.Xna.Framework;

namespace NNNA
{
	/// <summary>
	/// ProductionBuilding représente un bâtiment qui permet de produire des unités.
	/// </summary>
	class ProductionBuilding : Building
	{
		private Vector2 m_destination;

		public override void RightClick(Vector2 coos, Camera2D camera)
		{ m_destination = coos; }

		public ProductionBuilding(int x, int y)
            : base(x, y)
        { }
	}
}
