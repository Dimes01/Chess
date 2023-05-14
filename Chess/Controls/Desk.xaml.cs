using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Controls
{
    /// <summary>
    /// Логика взаимодействия для Desk.xaml
    /// </summary>
    public partial class Desk : UserControl
    {
        public Desk()
        {
            InitializeComponent();
            MakeDesk();
            MakeFigures();
        }


        private void MakeDesk()
        {
            if (Cells == null) Cells = new Dictionary<string, Cell>();
            for (char i = 'A'; i <= 'H'; ++i)
            {
                for (char j = '1'; j <= '8'; ++j)
                {
                    Cell tempCell = new Cell();
                    if ((i + j) % 2 == 1) tempCell.Background = BlackBrush;
                    else tempCell.Background = WhiteBrush;
                    tempCell.Position = $"{i}{j}";
                    Cells.Add(tempCell.Position, tempCell);
                    Grid.SetRow(tempCell, 7 - j + 49);
                    Grid.SetColumn(tempCell, i - 65);
                    DeskGrid.Children.Add(tempCell);
                }
            }
        }

        private void MakeFigures()
        {
            for (char i = 'A'; i <= 'H'; ++i)
            {
                Cells[$"{i}2"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wP.png", Position = $"{i}2" });
                Cells[$"{i}7"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bP.png", Position = $"{i}7" });
            }
            // Белые
            Cells["A1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wR.png", Position = "A1" });
            Cells["B1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wN.png", Position = "B1" });
            Cells["C1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wB.png", Position = "C1" });
            Cells["D1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wQ.png", Position = "D1" });
            Cells["E1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wK.png", Position = "E1" });
            Cells["F1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wB.png", Position = "F1" });
            Cells["G1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wN.png", Position = "G1" });
            Cells["H1"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/wR.png", Position = "H1" });
            // Негры
            Cells["A8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bR.png", Position = "A8" });
            Cells["B8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bN.png", Position = "B8" });
            Cells["C8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bB.png", Position = "C8" });
            Cells["D8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bQ.png", Position = "D8" });
            Cells["E8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bK.png", Position = "E8" });
            Cells["F8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bB.png", Position = "F8" });
            Cells["G8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bN.png", Position = "G8" });
            Cells["H8"].AddChild(new Figure { ImageSource = "pack://application:,,,/Images/dash/bR.png", Position = "H8" });
        }


        public static readonly DependencyProperty CellsProperty = DependencyProperty.Register(nameof(Cells), typeof(Dictionary<string, Cell>), typeof(Desk));
        public Dictionary<string, Cell> Cells
        {
            get { return (Dictionary<string, Cell>)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }


        public static readonly DependencyProperty WhiteBrushProperty = DependencyProperty.Register(nameof(WhiteBrush), typeof(Brush), typeof(Desk),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnWhiteBrushChanged)));
        public Brush WhiteBrush
        {
            get { return (Brush)GetValue(WhiteBrushProperty); }
            set { SetValue(WhiteBrushProperty, value); }
        }
        private static void OnWhiteBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Desk desk = (Desk)d;
            if (desk.Cells == null) return;
            for (char i = 'A'; i <= 'H'; ++i)
                for (char j = '1'; j <= '8'; ++j)
                    if ((i + j) % 2 == 1) desk.Cells[$"{i}{j}"].BackBrush = (Brush)e.NewValue;
        }


        public static readonly DependencyProperty BlackBrushProperty = DependencyProperty.Register(nameof(BlackBrush), typeof(Brush), typeof(Desk),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnBlackBrushChanged)));
        public Brush BlackBrush
        {
            get { return (Brush)GetValue(BlackBrushProperty); }
            set { SetValue(BlackBrushProperty, value); }
        }
        private static void OnBlackBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Desk desk = (Desk)d;
            if (desk.Cells == null) return;
            for (char i = 'A'; i <= 'H'; ++i)
                for (char j = '1'; j <= '8'; ++j)
                    if ((i + j) % 2 == 0) desk.Cells[$"{i}{j}"].BackBrush = (Brush)e.NewValue;
        }


        private void DeskGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.SelectedCell.IsSelected = false;
            App.SelectedFigure.IsSelected = false;
        }
    }
}
