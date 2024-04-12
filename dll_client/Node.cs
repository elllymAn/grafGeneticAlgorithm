using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsElements
{
    public class Node
    {
        public int _x;
        public int _y;
        public Rectangle _form;
        public bool _moveStatus;
        public String _name;
        public Node(int x, int y)
        {
            _moveStatus = true;
            _x = x;
            _y = y;
            _form = new Rectangle(this._x, this._y, 50, 20);
        }
    }
}
