using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NNNA
{
    class Archer : LandUnit
    {
        private int speed_tirs;
        private List<Fleche> tirs;

        public List<Fleche> Tirs
        { get { return tirs; }  set { tirs = value; } }

        public int Speed_tirs
        { get { return speed_tirs; } set { speed_tirs = value; } }

        public Archer(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Nourriture", 100);
            _cost.Add("Fer", 20);
        }
        public Archer(int x, int y, ContentManager content, Joueur joueur, bool removeResources = true)
            : base(x, y)
        {
            Joueur = joueur;
            joueur.Population++;
            _type = "archer";
            if (removeResources)
            { joueur.Resource("Nourriture").Remove(100); joueur.Resource("Fer").Remove(20); }
            Attaque = 10;
            VitesseCombat = 30;
            Life = 100;
            LineSight = 384;
            Portee = 10;
            Regeneration = 1;
            Speed = 0.05f;
            speed_tirs = 8;
            tirs = new List<Fleche>();
            SetTextures(content, "archer", 45);
            _cost.Add("Nourriture", 100);
            _cost.Add("Fer", 20);
        }

        public override void Tirer(Unit cible, ContentManager content)
        {
            tirs.Add(new Fleche(content, (int) Position.X, (int) Position.Y, speed_tirs, cible.PositionCenter));
        }
    }
}
