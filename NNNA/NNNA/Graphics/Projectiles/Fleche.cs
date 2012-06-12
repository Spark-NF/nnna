using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Fleche : Projectile
    {
        public Unit Cible { get; private set; }
        public Fleche(ContentManager content, int x, int y, int speed, Unit cible, string assetName = "Projectiles/fleche")
            : base(content, x, y, speed, cible.PositionCenter, assetName)
        {
            Cible = cible;
        }

        public void Update()
        {
            _realityOffset = (float)(1.5f * (Math.Sin((_distanceRestante.Length() * Math.PI / _distanceIni) - Math.PI / 6)));
            Mouvement();
        }

    }
}
