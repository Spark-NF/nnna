using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Fleche : Projectile
    {
        public Fleche(ContentManager content, int x, int y, int speed, Vector2 but, string assetName = "Projectiles/fleche")
            : base(content, x, y, speed, but, assetName) { }

        public void Update()
        {
            reality_offset = (float)(1.5f * (Math.Sin((distance_restante.Length() * Math.PI / distance_ini) - Math.PI / 6)));
            Mouvement();
        }

    }
}
