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
                Cells[$"{i}2"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wP.png", Position = $"{i}2", Type = TypesFigures.Pawn, Side = SideColor.White });
                Cells[$"{i}7"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bP.png", Position = $"{i}7", Type = TypesFigures.Pawn, Side = SideColor.Black });
            }

            // Белые
            List<int> positions = FisherRandom(new List<string> { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1" });
            Cells[$"{(char)(positions[0] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wR.png", Position = $"{(char)(positions[0] + 'A')}1", Type = TypesFigures.Rook,    Side = SideColor.White });
            Cells[$"{(char)(positions[1] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wN.png", Position = $"{(char)(positions[1] + 'A')}1", Type = TypesFigures.Knight,  Side = SideColor.White });
            Cells[$"{(char)(positions[2] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wB.png", Position = $"{(char)(positions[2] + 'A')}1", Type = TypesFigures.Bishop,  Side = SideColor.White });
            Cells[$"{(char)(positions[3] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wQ.png", Position = $"{(char)(positions[3] + 'A')}1", Type = TypesFigures.Queen,   Side = SideColor.White });
            Cells[$"{(char)(positions[4] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wK.png", Position = $"{(char)(positions[4] + 'A')}1", Type = TypesFigures.King,    Side = SideColor.White });
            Cells[$"{(char)(positions[5] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wB.png", Position = $"{(char)(positions[5] + 'A')}1", Type = TypesFigures.Bishop,  Side = SideColor.White });
            Cells[$"{(char)(positions[6] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wN.png", Position = $"{(char)(positions[6] + 'A')}1", Type = TypesFigures.Knight,  Side = SideColor.White });
            Cells[$"{(char)(positions[7] + 'A')}1"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wR.png", Position = $"{(char)(positions[7] + 'A')}1", Type = TypesFigures.Rook,    Side = SideColor.White });
            // Негры
            Cells[$"{(char)(positions[0] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[0] + 'A')}8", Type = TypesFigures.Rook,    Side = SideColor.Black });
            Cells[$"{(char)(positions[1] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[1] + 'A')}8", Type = TypesFigures.Knight,  Side = SideColor.Black });
            Cells[$"{(char)(positions[2] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[2] + 'A')}8", Type = TypesFigures.Bishop,  Side = SideColor.Black });
            Cells[$"{(char)(positions[3] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png", Position = $"{(char)(positions[3] + 'A')}8", Type = TypesFigures.Queen,   Side = SideColor.Black });
            Cells[$"{(char)(positions[4] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bK.png", Position = $"{(char)(positions[4] + 'A')}8", Type = TypesFigures.King,    Side = SideColor.Black });
            Cells[$"{(char)(positions[5] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[5] + 'A')}8", Type = TypesFigures.Bishop,  Side = SideColor.Black });
            Cells[$"{(char)(positions[6] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[6] + 'A')}8", Type = TypesFigures.Knight,  Side = SideColor.Black });
            Cells[$"{(char)(positions[7] + 'A')}8"].AddChild(new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[7] + 'A')}8", Type = TypesFigures.Rook,    Side = SideColor.Black });
        }

        private List<int> FisherRandom(List<string> strings)
        {
            List<string> list = new List<string>(strings);
            Random random = new Random();
            string firstBishop = strings[2], secondBishop = strings[5];
            string firstRook = strings[0], secondRook = strings[7];
            string temp, king = strings[4];
            int j, posFirstBishop, posSecondBishop, posKing, posFirstRook, posSecondRook;
            bool flagForBishops = false, flagForRooks = false;
            while(!flagForBishops || !flagForRooks)
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
