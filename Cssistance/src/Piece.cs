using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cssistance.src
{
    internal class Piece
    {
        public static class Black
        {
            public const int Pawn = 0;
            public const int Bishop = 1;
            public const int Knight = 2;
            public const int Rook = 3;
            public const int Queen = 4;
            public const int King = 5;
        }

        public class White
        {
            public const int Pawn = 6;
            public const int Bishop = 7;
            public const int Knight = 8;
            public const int Rook = 9;
            public const int Queen = 10;
            public const int King = 11;
        }

        public static Dictionary<int, string> Symbol = new Dictionary<int, string>
        {
            { Black.Pawn, "♟" },
            { Black.Bishop, "♝" },
            { Black.Knight, "♞" },
            { Black.Rook, "♜" },
            { Black.Queen, "♛" },
            { Black.King, "♚" },
            { White.Pawn, "♙" },
            { White.Bishop, "♗" },
            { White.Knight, "♘" },
            { White.Rook, "♖" },
            { White.Queen, "♕" },
            { White.King, "♔" }
        };

        public static Dictionary<int, string> FEN = new Dictionary<int, string>
        {
            { Black.Pawn, "p" },
            { Black.Bishop, "b" },
            { Black.Knight, "n" },
            { Black.Rook, "r" },
            { Black.Queen, "q" },
            { Black.King, "k" },
            { White.Pawn, "P" },
            { White.Bishop, "B" },
            { White.Knight, "N" },
            { White.Rook, "R" },
            { White.Queen, "Q" },
            { White.King, "K" }
        };
    }
}
