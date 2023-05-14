using Chess.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    internal static class Algorithms
    {
        public static List<int> FisherRandom(List<string> strings)
        {
            List<string> list = new List<string>(strings);
            Random random = new Random();
            string firstBishop = strings[2], secondBishop = strings[5];
            string firstRook = strings[0], secondRook = strings[7];
            string temp, king = strings[4];
            int j, posFirstBishop, posSecondBishop, posKing, posFirstRook, posSecondRook;
            bool flagForBishops = false, flagForRooks = false;
            while (!flagForBishops || !flagForRooks)
            {
                list = new List<string>(strings);
                flagForBishops = false;
                flagForRooks = false;
                for (int i = list.Count - 1; i >= 1; --i)
                {
                    j = random.Next(i + 1);
                    temp = list[j];
                    list[j] = list[i];
                    list[i] = temp;
                }
                posFirstBishop = list.IndexOf(firstBishop);
                posSecondBishop = list.IndexOf(secondBishop);
                posKing = list.IndexOf(king);
                posFirstRook = list.IndexOf(firstRook);
                posSecondRook = list.IndexOf(secondRook);
                if ((posFirstBishop + posSecondBishop) % 2 == 1)
                    flagForBishops = true;
                if ((posFirstRook < posKing && posKing < posSecondRook) || (posFirstRook > posKing && posKing > posSecondRook))
                    flagForRooks = true;
            }
            List<int> nums = new List<int>();
            for (int i = 0; i < strings.Count; ++i) nums.Add(list.IndexOf(strings[i]));
            return nums;
        }

        #region Подсчёт всевозможных ходов

        private delegate void CalcAllMove(string position, List<string> allMoves, int countMove);
        private static Dictionary<TypesFigures, CalcAllMove> CalculationsAllMoves = new Dictionary<TypesFigures, CalcAllMove>
        {
            { TypesFigures.Pawn,     AllMovesPawn   },
            { TypesFigures.Knight,   AllMovesKnight },
            { TypesFigures.Bishop,   AllMovesBishop },
            { TypesFigures.Rook,     AllMovesRook   },
            { TypesFigures.Queen,    AllMovesQueen  },
            { TypesFigures.King,     AllMovesKing   },
        };

        public static void CalculateAllMoves(Figure figure, List<string> allMoves)
        {
            allMoves.Clear();
            CalculationsAllMoves[figure.Type](figure.Position, allMoves, figure.CountMoves);
        }

        private static void AllMovesPawn(string position, List<string> allMoves, int countMove)
        {
            if (position[1] <= '7')
            {
                allMoves.Add($"{position[0]}{position[1] + 1}");
                if (countMove == 0) allMoves.Add($"{position[0]}{position[1] + 2}");
            }
            if (position[0] <= 'G') allMoves.Add($"{position[0] + 1}{position[1] + 1}");
            if (position[0] >= 'B') allMoves.Add($"{position[0] - 1}{position[1] + 1}");
        }

        private static void AllMovesKnight(string position, List<string> allMoves, int countMove)
        {
            if (position[0] + 2 <= 'H' && position[1] + 1 <= '8') allMoves.Add($"{position[0] + 2}{position[1] + 1}");
            if (position[0] + 1 <= 'H' && position[1] + 2 <= '8') allMoves.Add($"{position[0] + 1}{position[1] + 2}");
            if (position[0] - 1 >= 'A' && position[1] + 2 <= '8') allMoves.Add($"{position[0] - 1}{position[1] + 2}");
            if (position[0] - 2 >= 'A' && position[1] + 1 <= '8') allMoves.Add($"{position[0] - 2}{position[1] + 1}");
            if (position[0] - 2 >= 'A' && position[1] - 1 >= '1') allMoves.Add($"{position[0] - 2}{position[1] - 1}");
            if (position[0] - 1 >= 'A' && position[1] - 2 >= '1') allMoves.Add($"{position[0] - 1}{position[1] - 2}");
            if (position[0] + 1 <= 'H' && position[1] - 2 >= '1') allMoves.Add($"{position[0] + 1}{position[1] - 2}");
            if (position[0] + 2 <= 'H' && position[1] - 1 >= '1') allMoves.Add($"{position[0] + 2}{position[1] - 1}");
        }

        private static void AllMovesBishop(string position, List<string> allMoves, int countMove)
        {
            char c, s;
            for (c = (char)(position[0] + 1), s = (char)(position[1] + 1); c <= 'H' && s <= '8'; ++c, ++s) allMoves.Add($"{c}{s}");
            for (c = (char)(position[0] - 1), s = (char)(position[1] + 1); c >= 'A' && s <= '8'; --c, ++s) allMoves.Add($"{c}{s}");
            for (c = (char)(position[0] - 1), s = (char)(position[1] - 1); c >= 'A' && s >= '1'; --c, --s) allMoves.Add($"{c}{s}");
            for (c = (char)(position[0] + 1), s = (char)(position[1] - 1); c <= 'H' && s >= '1'; ++c, --s) allMoves.Add($"{c}{s}");
        }

        private static void AllMovesRook(string position, List<string> allMoves, int countMove)
        {
            for (char i = position[0]; i <= 'H'; ++i) allMoves.Add($"{i}{position[1]}");
            for (char i = position[1]; i <= '8'; ++i) allMoves.Add($"{position[1]}{i}");
            for (char i = position[0]; i >= 'A'; --i) allMoves.Add($"{i}{position[1]}");
            for (char i = position[1]; i >= '1'; --i) allMoves.Add($"{position[1]}{i}");
        }

        private static void AllMovesQueen(string position, List<string> allMoves, int countMove)
        {
            AllMovesRook(position, allMoves, countMove);
            AllMovesBishop(position, allMoves, countMove);
        }

        private static void AllMovesKing(string position, List<string> allMoves, int countMove)
        {
            if (position[0] + 1 <= 'H') allMoves.Add($"{(char)(position[0] + 1)}{position[1]}");
            if (position[0] + 1 <= 'H' && position[1] + 1 <= '8') allMoves.Add($"{(char)(position[0] + 1)}{(char)(position[1] + 1)}");
            if (position[1] + 1 <= '8') allMoves.Add($"{position[0]}{(char)(position[1] + 1)}");
            if (position[0] - 1 >= 'A' && position[1] + 1 <= '8') allMoves.Add($"{(char)(position[0] - 1)}{(char)(position[1] + 1)}");
            if (position[0] - 1 >= 'A') allMoves.Add($"{(char)(position[0] - 1)}{position[1]}");
            if (position[0] - 1 >= 'A' && position[1] - 1 >= '1') allMoves.Add($"{(char)(position[0] - 1)}{(char)(position[1] - 1)}");
            if (position[1] - 1 >= '1') allMoves.Add($"{position[0]}{(char)(position[1] - 1)}");
            if (position[0] + 1 <= 'H' && position[1] - 1 >= '1') allMoves.Add($"{(char)(position[0] + 1)}{(char)(position[1] - 1)}");
        }

        #endregion

        #region Подсчёт возможных на данный момент ходов

        private delegate void CalcPossibleMove(Figure figure, List<string> allMoves, List<Cell> cells);
        private static Dictionary<TypesFigures, CalcPossibleMove> CalculationsPossibleMoves = new Dictionary<TypesFigures, CalcPossibleMove>
        {

        };

        public static void CalculatePossibleMove()
        {

        }

        #endregion
    }
}
