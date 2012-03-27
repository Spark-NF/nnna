using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    abstract class Containing : Control
    {
        #region ATTRIBUTS
        private Control[] _children;
        #endregion ATTRIBUTS

        #region GET/SET
        public Control[] Children
        { get { return _children; } set { _children = value; } }
        #endregion GET/SET

        public Containing(Control[] children, Rectangle zone, string name)
            : base(zone, name)
        {
            _children = children;
        }
    }
}
