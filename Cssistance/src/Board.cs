using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cssistance.src
{
    internal class Board
    {
        public static SortedDictionary<int, int> Coords = new SortedDictionary<int, int>();
        public static string CurrentFEN = null;
        public static string BestMove = null;
        public static int ThinkTime = 3000;
        public static string ToFen(SortedDictionary<int, int> Coordination)
        {
            string FEN = "";
            string FEN8 = "";
            string FEN7 = "";
            string FEN6 = "";
            string FEN5 = "";
            string FEN4 = "";
            string FEN3 = "";
            string FEN2 = "";
            string FEN1 = "";
            int[] AlterNumber = new int[9];

            // FEN ~= nrnkrbbq/pppppppp/8/8/3N4/8/PPPPPPPP/NR1KRBBQ b - - 0 1
            foreach (var pos in Board.Coords)
            {
                int rank = pos.Key;

                if (rank < 20)
                {
                    int file = rank - 10;
                    int gap = file - AlterNumber[8] - 1;

                    if (gap != 0)
                    {
                        FEN8 += gap.ToString();
                        AlterNumber[8] += gap;
                    }
                    FEN8 += Piece.FEN[pos.Value];
                    AlterNumber[8] += 1;

                }
                else if (rank < 30)
                {
                    int file = rank - 20;
                    int gap = file - AlterNumber[7] - 1;

                    if (gap != 0)
                    {
                        FEN7 += gap.ToString();
                        AlterNumber[7] += gap;
                    }
                    FEN7 += Piece.FEN[pos.Value];
                    AlterNumber[7] += 1;
                }
                else if (rank < 40)
                {
                    int file = rank - 30;
                    int gap = file - AlterNumber[6] - 1;

                    if (gap != 0)
                    {
                        FEN6 += gap.ToString();
                        AlterNumber[6] += gap;
                    }
                    FEN6 += Piece.FEN[pos.Value];
                    AlterNumber[6] += 1;
                }
                else if (rank < 50)
                {
                    int file = rank - 40;
                    int gap = file - AlterNumber[5] - 1;

                    if (gap != 0)
                    {
                        FEN5 += gap.ToString();
                        AlterNumber[5] += gap;
                    }
                    FEN5 += Piece.FEN[pos.Value];
                    AlterNumber[5] += 1;
                }
                else if (rank < 60)
                {
                    int file = rank - 50;
                    int gap = file - AlterNumber[4] - 1;

                    if (gap != 0)
                    {
                        FEN4 += gap.ToString();
                        AlterNumber[4] += gap;
                    }
                    FEN4 += Piece.FEN[pos.Value];
                    AlterNumber[4] += 1;
                }
                else if (rank < 70)
                {
                    int file = rank - 60;
                    int gap = file - AlterNumber[3] - 1;

                    if (gap != 0)
                    {
                        FEN3 += gap.ToString();
                        AlterNumber[3] += gap;
                    }
                    FEN3 += Piece.FEN[pos.Value];
                    AlterNumber[3] += 1;
                }
                else if (rank < 80)
                {
                    int file = rank - 70;
                    int gap = file - AlterNumber[2] - 1;

                    if (gap != 0)
                    {
                        FEN2 += gap.ToString();
                        AlterNumber[2] += gap;
                    }
                    FEN2 += Piece.FEN[pos.Value];
                    AlterNumber[2] += 1;
                }
                else 
                {
                    int file = rank - 80;
                    int gap = file - AlterNumber[1] - 1;

                    if (gap != 0)
                    {
                        FEN1 += gap.ToString();
                        AlterNumber[1] += gap;
                    }
                    FEN1 += Piece.FEN[pos.Value];
                    AlterNumber[1] += 1;
                }
            }

            Dictionary<int, object> FENvarMap = new Dictionary<int, object>
            {
                { 1, FEN1 },
                { 2, FEN2 },
                { 3, FEN3 },
                { 4, FEN4 },
                { 5, FEN5 },
                { 6, FEN6 },
                { 7, FEN7 },
                { 8, FEN8 }
            };

            for (int i = 1; i < 9; i++)
            {
                int CurNum = AlterNumber[i];

                if (CurNum < 8) { 
                    Console.WriteLine($"Found That {i} is small: {CurNum} FEN: {FENvarMap[i]}");
                    FENvarMap[i] += (8 - CurNum).ToString();
                }
            }

            char MySide = 'w';
            if (BoardManager.GameStatus == "Black")
            {
                MySide = 'b';
            }
            FEN += $"{FENvarMap[1]}/{FENvarMap[2]}/{FENvarMap[3]}/{FENvarMap[4]}/{FENvarMap[5]}/{FENvarMap[6]}/{FENvarMap[7]}/{FENvarMap[8]} {MySide} KQkq - 0 1";

            return FEN;
        }
    }
}
