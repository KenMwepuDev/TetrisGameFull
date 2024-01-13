using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisGameFull
{
    public class Forme
    {
        #region formesType

        private static int[][][] formes;

        private static void InitializeFormes()
        {
            formes = new int[][][]
            {
            new int[][]
            {
                new int[] {1, 1},
                new int[] {1, 1},
            },
            new int[][]
            {
                new int[] {1},
                new int[] {1},
                new int[] {1},
                new int[] {1},
            },
            new int[][]
            {
                new int[] {0, 1, 1},
                new int[] {1, 1, 0},
            },
            new int[][]
            {
                new int[] {1, 1, 0},
                new int[] {0, 1, 1},
            },
            new int[][]
            {
                new int[] {1, 0, 0},
                new int[] {1, 0, 0},
                new int[] {1, 1, 0},
            },
            new int[][]
            {
                new int[] {0, 0, 1},
                new int[] {0, 0, 1},
                new int[] {0, 1, 1},
            },
            new int[][]
            {
                new int[] {1, 1, 1},
                new int[] {0, 1, 0},
            }
            };
        }
        #endregion

        private List<Block> _form;
        public bool IsStoped;
        public Form1 _caller;
        public string IdForm;
        public Board _board;

        private int[][] _currenteForm;

        public Forme(Form1 caller, int codeForme, Board board)
        {
            InitializeFormes();

            IdForm = Guid.NewGuid().ToString();
            IsStoped = false;
            _caller = caller;
            _board = board;

            DrawForme(codeForme, _board);
            AdvanceToStop();
        }

        public void DrawForme(int codeForme, Board _board, RotationDirection direction = RotationDirection.None)
        {
            IsStoped = true;
            int nbPas = direction != RotationDirection.None ? _form.FirstOrDefault().Location.Y : (new Random().Next(0, _caller._NbLigneMAx / 2));
            _form = new List<Block>();

            int[][] form = direction == RotationDirection.None ? formes[codeForme] : RotateForm(_currenteForm, direction);
            _currenteForm = form;

            if (direction != RotationDirection.None)
            {
                _board.AllBlockOnBoard.Where(x => x._forme.ToString() == this.ToString()).ToList().ForEach(b =>
                {
                    _board.Controls.Remove(b);
                    _board.AllBlockOnBoard.Remove(b);
                });
            }

            for (int i = 0; i < form.Length; i++)
            {
                for (int y = 0; y < form[i].Length; y++)
                {
                    if (form[i][y] == 1)
                        _form.Add(new Block(_caller, this, i, direction != RotationDirection.None ? nbPas : (y + nbPas), _board));
                }
            }

            GetRandomColor();

            IsStoped = false;
        }

        public async Task AdvanceToStop()
        {
            while (!IsStoped)
            {
                _form.ForEach(x =>
                {
                    x.Location = new Point(x.Location.X, x.Location.Y + _caller._SizeBlock);
                    if (!IsStoped)
                        IsStoped = x.isStoped();
                });
                await Task.Delay(1000);
            }
            _caller.PutNewFormCmd(_board);
        }

        public void AdvanceOneTime()
        {
            if (!IsStoped)
            {
                _form.ForEach(x =>
                {
                    x.Location = new Point(x.Location.X, x.Location.Y + _caller._SizeBlock);
                    if (!IsStoped)
                        IsStoped = x.isStoped();
                });
            }
            else
                _caller.PutNewFormCmd(_board);
        }

        private void GetRandomColor()
        {
            Random random = new Random();
            do
            {
                int r = random.Next(256);
                int g = random.Next(256);
                int b = random.Next(256);

                _form.ForEach(f => f.BackColor = Color.FromArgb(r, g, b));
            } while (_form[0].BackColor == Color.Black || _form[0].BackColor == Color.White);
        }

        public void GoRigth()
        {
            bool canMoveInLine = true;
            _form.ForEach(b =>
            {
                if (b.CanGoRight())
                    canMoveInLine = false;
            });

            if (canMoveInLine)
                _form.ForEach(b => b.Location = new Point(b.Location.X + _caller._SizeBlock, b.Location.Y));
        }

        public void GoLeft()
        {
            bool canMoveInLine = true;
            _form.ForEach(b =>
            {
                if (b.CanGoLeft())
                    canMoveInLine = false;
            });

            if (canMoveInLine)
                _form.ForEach(b => b.Location = new Point(b.Location.X - _caller._SizeBlock, b.Location.Y));
        }

        private int[][] RotateForm(int[][] form, RotationDirection direction)
        {
            int rows = form.Length;
            int cols = form[0].Length;

            int[][] rotatedForm = new int[cols][];
            for (int i = 0; i < cols; i++)
            {
                rotatedForm[i] = new int[rows];
                for (int j = 0; j < rows; j++)
                {
                    if (direction == RotationDirection.Right)
                        rotatedForm[i][j] = form[rows - 1 - j][i];
                    else if (direction == RotationDirection.Left)
                        rotatedForm[i][j] = form[j][cols - 1 - i];
                }
            }
            return rotatedForm;
        }

        public override string ToString() => IdForm;
    }

    public enum RotationDirection
    {
        Left,
        Right,
        None
    }
}
