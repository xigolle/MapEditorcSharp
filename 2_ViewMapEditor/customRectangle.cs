using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_ViewMapEditor
{
    class customRectangle
    {
        private int x;
        private int y;
        private int typeBlock;
        private int width;
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int TypeBlock
        {
            get { return typeBlock; }
            set { typeBlock = value; }
        }


        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

    }
}
