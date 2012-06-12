using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NNNA
{
	[Serializable]
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
            Prix.Add("Nourriture", 100);
            Prix.Add("Fer", 20);
        }
        public Archer(int x, int y, ContentManager content, Joueur joueur, bool removeResources = true, bool add_pop = true)
            : base(x, y)
        {
            Joueur = joueur;
            Type = "archer";
            if (add_pop)
            {
                joueur.Population++;
            }
            if (removeResources)
            {
                joueur.Resource("Nourriture").Remove(100); joueur.Resource("Fer").Remove(20);
            }
            Attaque = 10;
            VitesseCombat = 90;
            Life = 100;
            LineSight = 384;
            Portee = (int)(LineSight * 0.8f) + Joueur.AdditionalUnitLineSight/2;
            Regeneration = 1;
            Speed = 0.05f + joueur.AdditionalSpeed;
            speed_tirs = 8;
            tirs = new List<Fleche>();
            SetTextures(content, "archer", 45);
            Prix.Add("Nourriture", 100);
            Prix.Add("Fer", 20);
        }

        public void Tirer(Unit cible, ContentManager content)
        {
            tirs.Add(new Fleche(content, (int) PositionCenter.X, (int) PositionCenter.Y, speed_tirs, ref cible));
        }
    }
}
