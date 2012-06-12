using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Fleche : Projectile
    {
        public Unit Cible { get; private set; }
        public Fleche(ContentManager content, int x, int y, int speed, ref Unit cible, string assetName = "Projectiles/fleche")
            : base(content, x, y, speed, cible.PositionCenter, assetName)
        {
            Cible = cible;
        }

        public void Update()
        {
            _realityOffset = (float)(1.5f * (Math.Sin((_distanceRestante.Length() * Math.PI / _distanceIni) - Math.PI / 6)));
            Mouvement();
            Touche = Touche || _rect.Intersects(new Rectangle((int)Cible.Position.X, (int)Cible.Position.Y, Cible.Texture.Width, Cible.Texture.Height));
        }

    }
}
