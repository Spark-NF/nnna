using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Peon : LandUnit
	{
        //SoundEffect creationPeon;
        public Peon(int x = 0, int y = 0)
            : base(x, y)
        {
            m_cost.Add("Nourriture", 50);
        }

		public Peon(int x, int y, ContentManager content, Joueur joueur, bool remove_resources = true)
			: base(x, y)
		{
			create_maison = false;
			m_joueur = joueur;
            joueur.Population++;
			type = "peon";
			if (remove_resources)
			{ joueur.Resource("Nourriture").Remove(50); }
			m_attaque = 2;
			m_life = 30;
            m_line_sight = 512;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.06f;
			SetTextures(content, "peon", 90);
            m_cost.Add("Nourriture", 50);
            //creationPeon = content.Load<SoundEffect>("sounds/creationpeon");
           // creationPeon.Play();
		}
	}
}
