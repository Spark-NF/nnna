using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class TabItem : Control
    {
        #region ATTRIBUTS
        #endregion ATTRIBUTS

        #region GET/SET
        #endregion GET/SET

        public TabItem(Rectangle zone, string name)
            : base(zone, name) { }

        public override void Update(Souris s)
        {
            //a ajouter
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
                //a ajouter
            }
        }
    }
}
