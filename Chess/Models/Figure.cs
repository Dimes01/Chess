using Chess.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chess.Models
{
    internal class Figure
    {
        public Figure (TypesFigures type, Image image, bool isWhite)
        {
            Type = type;
            Image = image;
            IsWhite = isWhite;
        }
        public TypesFigures Type { get; private set; }
        public Image Image { get; private set; }
        public bool IsWhite { get; private set; }
        public char Vertical { get; set; }
        public char Horizontal { get; set; }
        public string Position
        {
            get => $"{Vertical}{Horizontal}";
            set
            {
                Vertical = value[0];
                Horizontal = value[1];
            }
        }
        public int CountMove { get; private set; }
        public List<string> AllMoves { get; private set; } = new List<string>();
        public List<string> PossibleMoves { get; private set; } = new List<string>();


        public delegate void MoveFigure(string position, List<string> allMoves, int countMove);
        public MoveFigure CalculateAllMoves { get; set; }
        public bool CanMove => PossibleMoves.Count > 0;
        public void Move(string position)
        {
            ++CountMove;
            Position = position;
            CalculateAllMoves(position, AllMoves, CountMove);
        }
    }

    public enum TypesFigures
    {
        Pawn,
        Knight,
        Bishop,
        Ladle,
        Queen,
        King
    }
}
