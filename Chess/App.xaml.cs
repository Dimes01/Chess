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
        public static CurrentStep CurrentStep { get; set; } = CurrentStep.White;
        public static Cell SelectedCell { get; set; } = new Cell();
        public static Figure SelectedFigure { get; set; } = new Figure();
    }

    public enum CurrentStep
    {
        White,
        Black
    }
}
