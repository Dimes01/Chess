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

        private static string Str(int pos0, int pos1) => $"{(char)pos0}{(char)pos1}";
        private static bool CheckCell(int pos0, int pos1, bool outDesk = false, bool isEnemy = false, bool isFriend = false)
        {
            outDesk = false; isEnemy = false; isFriend = false;
            if ((pos0 < 'A') || (pos0 > 'H') || (pos1 < '1') || (pos1 > '8'))
            {
                outDesk = true;
                return false;
            }
            Figure figure = App.Desk.Cells[Str(pos0, pos1)].ChildFigure;
            if (figure != null)
            {
                isEnemy = figure.Type != App.GameCondition.SelectedFigure.Type;
                isFriend = !isEnemy;
            }
            return (figure == null) || isEnemy;
        }
        private static bool CheckConflictKings(Figure figure, int pos0, int pos1)
        {
            Figure conflictFigure;
            if (figure == App.Desk.WhiteKing) conflictFigure = App.Desk.BlackKing;
            else conflictFigure = App.Desk.WhiteKing;

            return Math.Abs(conflictFigure.Position[0] - pos0) >= 2 && Math.Abs(conflictFigure.Position[1] - pos1) >= 2;
        }

        #region Подсчёт всевозможных ходов

        private delegate void CalcPossibleMove(Figure figure, List<string> possibleMoves);
        private static Dictionary<TypesFigures, CalcPossibleMove> CalculationsMoves = new Dictionary<TypesFigures, CalcPossibleMove>
        {
            { TypesFigures.Pawn,     PossibleMovesPawn   },
            { TypesFigures.Knight,   PossibleMovesKnight },
            { TypesFigures.Bishop,   PossibleMovesBishop },
            { TypesFigures.Rook,     PossibleMovesRook   },
            { TypesFigures.Queen,    PossibleMovesQueen  },
            { TypesFigures.King,     PossibleMovesKing   },
        };

        public static void CalculatePossibleMoves(Figure figure)
        {
            if (!figure.CanMove) return;
            List<string> possibleMoves = new List<string>();
            CalculationsMoves[figure.Type](figure, possibleMoves);
            figure.PossibleMoves = possibleMoves;
        }

        private static void PossibleMovesPawn(Figure figure, List<string> possibleMoves)
        {
            bool outDesk = false, isEnemy = false, isFriend = false;
            string position = figure.Position;
            if (CheckCell(position[0], position[1] + 1))
            {
                possibleMoves.Add(Str(position[0], position[1] + 1));
                if (figure.CountMoves == 0 && CheckCell(position[0], position[1] + 2)) 
                    possibleMoves.Add(Str(position[0], position[1] + 2));
                if (CheckCell(position[0] + 1, position[1] + 1, outDesk, isEnemy, isFriend) && isEnemy) possibleMoves.Add(Str(position[0] + 1, position[1] + 1));
                if (CheckCell(position[0] - 1, position[1] + 1, outDesk, isEnemy, isFriend) && isEnemy) possibleMoves.Add(Str(position[0] - 1, position[1] + 1));
            }
        }

        private static void PossibleMovesKnight(Figure figure, List<string> possibleMoves)
        {
            string position = figure.Position;
            int a = position[0] + 2, b = position[1] + 1;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] + 1; b = position[1] + 2;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 1; b = position[1] + 2;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 2; b = position[1] + 1;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 2; b = position[1] - 1;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 1; b = position[1] - 2;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] + 1; b = position[1] - 2;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] + 2; b = position[1] - 1;
            if (CheckCell(a, b)) possibleMoves.Add(Str(a, b));
        }

        private static void PossibleMovesBishop(Figure figure, List<string> possibleMoves)
        {
            int a, b;
            bool outDesk = false, isEnemy = false, isFriend = false;
            string position = figure.Position;
            for (a = position[0] + 1, b = position[1] + 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); ++a, ++b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0] - 1, b = position[1] + 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); --a, ++b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0] - 1, b = position[1] - 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); --a, --b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0] + 1, b = position[1] - 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); ++a, --b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));
        }

        private static void PossibleMovesRook(Figure figure, List<string> possibleMoves)
        {
            int a, b;
            bool outDesk = false, isEnemy = false, isFriend = false;
            string position = figure.Position;
            for (a = position[0] + 1, b = position[1]; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); ++a) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0], b = position[1] + 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); ++b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0] - 1, b = position[1]; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); --a) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));

            outDesk = false; isEnemy = false; isFriend = false;
            for (a = position[0], b = position[1] - 1; CheckCell(a, b, outDesk, isEnemy, isFriend) && (!isEnemy || !isFriend); --b) possibleMoves.Add(Str(a, b));
            if (!outDesk && isEnemy) possibleMoves.Add(Str(a, b));
        }

        private static void PossibleMovesQueen(Figure figure, List<string> possibleMoves)
        {
            PossibleMovesRook(figure, possibleMoves);
            PossibleMovesBishop(figure, possibleMoves);
        }

        private static void PossibleMovesKing(Figure figure, List<string> possibleMoves)
        {
            int a, b;
            string position = figure.Position;

            a = position[0] + 1; b = position[1];
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] + 1; b = position[1] + 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0]; b = position[1] + 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 1; b = position[1] + 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 1; b = position[1];
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] - 1; b = position[1] - 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0]; b = position[1] - 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

            a = position[0] + 1; b = position[1] - 1;
            if (CheckCell(a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));
        }

        #endregion
    }
}
