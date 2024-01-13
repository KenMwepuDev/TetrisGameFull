using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TetrisGameFull
{
    public class Block : Panel
    {
        public Form1 _caller;
        public Forme _forme;
        public Board _board;

        public Block(Form1 caller, Forme forme, int pasStart, int ligneStart, Board board)
        {
            _caller = caller;
            _forme = forme;
            _board = board;

            Size = new Size(_caller._SizeBlock, _caller._SizeBlock);
            Location = new Point(ligneStart * 20, pasStart * 20);
            BorderStyle = BorderStyle.FixedSingle;

            _board.AllBlockOnBoard.Add(this);
            _board.Controls.Add(this);

        }

        public bool isStoped() => Location.Y >= _board.Size.Height - _caller._SizeBlock || _board.AllBlockOnBoard.Where(x => x.Location == new Point(Location.X, Location.Y + _caller._SizeBlock) && x._forme.ToString() != _forme.ToString()).Count() != 0;

        public override string ToString() => $"Location:{Location} - Pas:{Location.Y / _caller._SizeBlock}, Stoped : {isStoped()}";

        public bool CanGoLeft()
        {
            return (Location.X - _caller._NbLigneMAx <= 0 || _board.AllBlockOnBoard.Where(b => b.Location == new Point(Location.X - _caller._SizeBlock, Location.Y) && b._forme.ToString() != _forme.ToString()).Count() > 0);
        }

        public bool CanGoRight()
        {
            return (Location.X + (_caller._NbLigneMAx * 2) >= (_caller._SizeBlock * _caller._NbLigneMAx) || _board.AllBlockOnBoard.Where(b => b.Location == new Point(Location.X + _caller._SizeBlock, Location.Y) && b._forme.ToString() != _forme.ToString()).Count() > 0);
        }
    }
}
