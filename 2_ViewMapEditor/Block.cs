using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_ViewMapEditor
{
    class Block
    {
        private int typeBlock;
        private int x;
        private int y;
        private int breedte;
        private int hoogte;

        public int Hoogte
        {
            get { return hoogte; }
            set { hoogte = value; }
        }


        public int Breedte
        {
            get { return breedte; }
            set { breedte = value; }
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


        public int TypeBlock
        {
            get { return typeBlock; }
            set { typeBlock = value; }
        }

        

    }
}
