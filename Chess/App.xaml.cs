using Chess.Controls;
using Chess.Models;
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
    /// 
    /// НАСЛАЖДАЙТЕСЬ!!!
    /// 
    /// </summary>
    public partial class App : Application
    {
        public static Desk Desk { get; set; }
        public static GameCondition GameCondition { get; set; } = new GameCondition();
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

    public enum Conditions
    {
        Stalemate,
        Check,
        Checkmate
    }
    #endregion
}
