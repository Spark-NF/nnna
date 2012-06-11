using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class TabControl : Containing
    {
        #region ATTRIBUTS
        private int selectedTab;
        private TabItem[] children;
        #endregion ATTRIBUTS

        #region GET/SET
        #endregion GET/SET

        public TabControl(TabItem[] children, Rectangle zone, string name)
            : base(children, zone, name) 
        {
            this.children = children;
            selectedTab = 0;
            children[selectedTab].Selected = true;
        }

        public override void Update(Souris s)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if (children[i].Select(s))
                {
                    children[selectedTab].Selected = false;
                    selectedTab = i;
                    children[selectedTab].Selected = true;
                }
                children[i].Update(s);
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Draw(sb, sf);
                }
                // dessine les enfants de l'onglet sélectionné après avoir avoir dessiné tous les autres onglets.
                for (int j = 0; j < children[selectedTab].Children.Length; j++)
                {
                    children[selectedTab].Children[j].Draw(sb, sf);
                }
                // dessine les info bulles du boutton survolé
                for (int j = 0; j < children[selectedTab].Children.Length; j++)
                {
                    if (children[selectedTab].Children[j] is Button)
                    {
                        var button = (children[selectedTab].Children[j] as Button);
                        button.Draw_Info(sb, sf);
                    }
                }
            }
        }
    }
}
