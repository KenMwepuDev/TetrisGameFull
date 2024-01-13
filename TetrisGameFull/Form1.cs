using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisGameFull
{
    public partial class Form1 : Form
    {
        public int _SizeBlock = 20;
        public int _NbPasMax = 20;
        public int _NbLigneMAx = 12;
        public SalonCommunication Communicator;

        public List<Board> _boards;

        public Board InitializeCmd()
        {
            Board board = new Board()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                BorderStyle = BorderStyle.Fixed3D,
                Size = new Size(_SizeBlock * _NbLigneMAx, _SizeBlock * (_NbPasMax + 1)),
                Location = new Point(10, 10),
                BackColor = Color.Black,
            };

            FLP_BoardsContener.Controls.Add(board);
            return board;
        }

        public void PutNewFormCmd(Board board)
        {
            for (int i = 0; i <= _NbPasMax; i++)
            {
                for (int y = 0; y < _NbLigneMAx; y++)
                {
                    if (board.AllBlockOnBoard.FirstOrDefault(b => b.Location == new Point(_SizeBlock * y, _SizeBlock * i)) == null)
                        break;

                    if (y + 1 == _NbLigneMAx)
                    {
                        var blocksToDelete = board.AllBlockOnBoard.Where(x => x.Location.Y == _SizeBlock * i && x.Location.X <= _SizeBlock * y).ToList();
                        blocksToDelete.ForEach(b =>
                        {
                            board.Controls.Remove(b);
                            board.AllBlockOnBoard.Remove(b);
                        });
                    }
                }
            }
            board.CurrentForme = new Forme(this, new Random().Next(0, 7), board);
        }

        public void AdvanceForme(Board board)
        {
            board.CurrentForme.AdvanceOneTime();
        }

        public Form1()
        {
            InitializeComponent();
            Communicator = new SalonCommunication(this);

            _boards = new List<Board>();

            _boards.Add(InitializeCmd());


            PutNewFormCmd(_boards[0]);

            KeyDown += Form1_KeyDown;

            listBox1.DataSource = Communicator.GetAllIpAdress();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    //_CurrentForme.DrawForme(-1, _board, RotationDirection.Right);
                    break;
                case Keys.Down:
                    //_CurrentForme.DrawForme(-1, _board, RotationDirection.Left);
                    break;
                case Keys.Left:
                    _boards[0].CurrentForme.GoLeft();
                    break;
                case Keys.Right:
                    _boards[0].CurrentForme.GoRigth();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LB_IpAdressServeur.Text = Communicator.CreateSalon((IPAddress)listBox1.SelectedItem).ToString();

            //_boards[0].CurrentForme.AdvanceToStop();
            panel1.Enabled = false;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Communicator.CloseSalon();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Communicator.ConnectToSalon(TXB_AddressClient.Text);
            //_boards[0].CurrentForme.AdvanceToStop();

            panel1.Enabled = false;

        }
    }
}
