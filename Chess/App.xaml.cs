using Chess.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chess
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SideColor CurrentStep { get; set; } = SideColor.White;
        public static Cell SelectedCell { get; set; } = new Cell();
        public static Figure SelectedFigure { get; set; } = new Figure();
        public static string PathStyleFigure { get; } = "glass";
        public static string PathFolderFigure { get; } = "Images";

    }

    #region Перечисления
    public enum SideColor
    {
        White,
        Black
    }

    public enum TypesFigures
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }
    #endregion
}
