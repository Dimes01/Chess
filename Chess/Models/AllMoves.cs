using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    internal static class AllMoves
    {
        public static void AllMovesPawn(string position, List<string> allMoves, int countMove)
        {
            allMoves.Clear();
            if (position[1] <= '7')
            {
                allMoves.Add($"{position[0]}{position[1] + 1}");
                if (countMove == 0) allMoves.Add($"{position[0]}{position[1] + 2}");
            }
            if (position[0] <= 'G') allMoves.Add($"{position[0] + 1}{position[1] + 1}");
            if (position[0] >= 'B') allMoves.Add($"{position[0] - 1}{position[1] + 1}");
        }

        public static void AllMovesKnight(string position, List<string> allMoves, int countMove)
        {
            allMoves.Clear();
            if (position[0] + 2 <= 'H' && position[1] + 1 <= '8') allMoves.Add($"{position[0] + 2}{position[1] + 1}");
            if (position[0] + 1 <= 'H' && position[1] + 2 <= '8') allMoves.Add($"{position[0] + 1}{position[1] + 2}");
            if (position[0] - 1 >= 'A' && position[1] + 2 <= '8') allMoves.Add($"{position[0] - 1}{position[1] + 2}");
            if (position[0] - 2 >= 'A' && position[1] + 1 <= '8') allMoves.Add($"{position[0] - 2}{position[1] + 1}");
            if (position[0] - 2 >= 'A' && position[1] - 1 >= '1') allMoves.Add($"{position[0] - 2}{position[1] - 1}");
            if (position[0] - 1 >= 'A' && position[1] - 2 >= '1') allMoves.Add($"{position[0] - 1}{position[1] - 2}");
            if (position[0] + 1 <= 'H' && position[1] - 2 >= '1') allMoves.Add($"{position[0] + 1}{position[1] - 2}");
            if (position[0] + 2 <= 'H' && position[1] - 1 >= '1') allMoves.Add($"{position[0] + 2}{position[1] - 1}");
        }

        public static void AllMovesBishop(string position, List<string> allMoves, int countMove)
        {
            allMoves.Clear();
            char c = position[0], s = position[1];
            for (int i = 1; c < 'H'; ++i)
            {
                c = (char)(position[0] + i);
                s = (char)(position[1] + i);
                if (s <= '8') allMoves.Add($"{c}{s}");
                s = (char)(position[1] - i);
                if (s >= '1') allMoves.Add($"{c}{s}");
            }
            for (c = (char)(position[0] - 1); c >= 'A'; --c)
            {
                for (s = (char)(position[1] + 1); s <= '8'; ++s) allMoves.Add($"{c}{s}");
                for (s = (char)(position[1] - 1); s >= '1'; --s) allMoves.Add($"{c}{s}");
            }
        }
    }
}
