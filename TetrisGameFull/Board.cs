using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisGameFull
{
    public class Board : Panel
    {
        public Forme CurrentForme { get; set; }
        public List<Block> AllBlockOnBoard { get; set; }

        public Board() => AllBlockOnBoard = new List<Block>();
    }
}
