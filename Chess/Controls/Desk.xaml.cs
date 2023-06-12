using Chess.Models;
using Chess.Utilities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Desk.xaml
	/// </summary>
	public partial class Desk : UserControl, IRestart
	{
		public Desk()
		{
			InitializeComponent();
			MakeDesk();
			Restart();
		}
		public Dictionary<SideColor, Figure> Kings { get; private set; }
		public List<Figure> AllFigures { get; private set; } = new List<Figure>();
		public List<string> DefensiveMoves { get; set; }
		public Cell SelectedCell { get; set; } = new Cell();
		public Figure SelectedFigure { get; set; } = new Figure();
		public SideColor CurrentStep { get; set; } = SideColor.White;
		public Cell CellWithSelectedFigure { get; set; }
		public Figure PreviousFigure { get; set; }

		public void Restart()
		{
			AllFigures.Clear();
			DefensiveMoves = null;
			Kings = null;
			MarkedCells = new List<string>();
			CurrentStep = SideColor.White;
			SelectedCell = new Cell();
			SelectedFigure = new Figure();
			PreviousFigure = new Figure();
			CellWithSelectedFigure = null;
			foreach (var cell in Cells)
			{
				RemoveFigure(cell.Value);
			}
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
				Cells[$"{i}2"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wP.png", Position = $"{i}2", Type = TypesFigures.Pawn, Side = SideColor.White };
				Cells[$"{i}7"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bP.png", Position = $"{i}7", Type = TypesFigures.Pawn, Side = SideColor.Black };
				AllFigures.Add(Cells[$"{i}2"].ChildFigure);
				AllFigures.Add(Cells[$"{i}7"].ChildFigure);
			}

			// Белые
			List<int> positions = Algorithms.FisherRandom(new List<string> { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1" });
			Cells[$"{(char)(positions[0] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wR.png", Position = $"{(char)(positions[0] + 'A')}1", Type = TypesFigures.Rook, Side = SideColor.White };
			Cells[$"{(char)(positions[1] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wN.png", Position = $"{(char)(positions[1] + 'A')}1", Type = TypesFigures.Knight, Side = SideColor.White };
			Cells[$"{(char)(positions[2] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wB.png", Position = $"{(char)(positions[2] + 'A')}1", Type = TypesFigures.Bishop, Side = SideColor.White };
			Cells[$"{(char)(positions[3] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wQ.png", Position = $"{(char)(positions[3] + 'A')}1", Type = TypesFigures.Queen, Side = SideColor.White };
			Cells[$"{(char)(positions[4] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wK.png", Position = $"{(char)(positions[4] + 'A')}1", Type = TypesFigures.King, Side = SideColor.White };
			Cells[$"{(char)(positions[5] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wB.png", Position = $"{(char)(positions[5] + 'A')}1", Type = TypesFigures.Bishop, Side = SideColor.White };
			Cells[$"{(char)(positions[6] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wN.png", Position = $"{(char)(positions[6] + 'A')}1", Type = TypesFigures.Knight, Side = SideColor.White };
			Cells[$"{(char)(positions[7] + 'A')}1"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wR.png", Position = $"{(char)(positions[7] + 'A')}1", Type = TypesFigures.Rook, Side = SideColor.White };
			for (int i = 0; i < positions.Count; ++i) AllFigures.Add(Cells[$"{(char)(positions[i] + 'A')}1"].ChildFigure);

			// Чёрные
			Cells[$"{(char)(positions[0] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[0] + 'A')}8", Type = TypesFigures.Rook, Side = SideColor.Black };
			Cells[$"{(char)(positions[1] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[1] + 'A')}8", Type = TypesFigures.Knight, Side = SideColor.Black };
			Cells[$"{(char)(positions[2] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[2] + 'A')}8", Type = TypesFigures.Bishop, Side = SideColor.Black };
			Cells[$"{(char)(positions[3] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png", Position = $"{(char)(positions[3] + 'A')}8", Type = TypesFigures.Queen, Side = SideColor.Black };
			Cells[$"{(char)(positions[4] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bK.png", Position = $"{(char)(positions[4] + 'A')}8", Type = TypesFigures.King, Side = SideColor.Black };
			Cells[$"{(char)(positions[5] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[5] + 'A')}8", Type = TypesFigures.Bishop, Side = SideColor.Black };
			Cells[$"{(char)(positions[6] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[6] + 'A')}8", Type = TypesFigures.Knight, Side = SideColor.Black };
			Cells[$"{(char)(positions[7] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[7] + 'A')}8", Type = TypesFigures.Rook, Side = SideColor.Black };
			for (int i = 0; i < positions.Count; ++i) AllFigures.Add(Cells[$"{(char)(positions[i] + 'A')}8"].ChildFigure);

			Kings = new Dictionary<SideColor, Figure>
			{
				{ SideColor.White, Cells[$"{(char)(positions[4] + 'A')}1"].ChildFigure },
				{ SideColor.Black, Cells[$"{(char)(positions[4] + 'A')}8"].ChildFigure }
			};
			AllFigures.Add(Kings[SideColor.White]);
			AllFigures.Add(Kings[SideColor.Black]);
		}

		public void ClearConditions()
		{
			DefensiveMoves = null;
			for (int i = 0; i < AllFigures.Count; ++i)
			{
				AllFigures[i].PossibleMoves.Clear();
				AllFigures[i].CanSelected = true;
				AllFigures[i].Bound = null;
			}
			foreach (var key in Cells)
			{
				key.Value.AttackingFigures[SideColor.White].Clear();
				key.Value.AttackingFigures[SideColor.Black].Clear();
			}
		}
		public void MakeQueen()
		{
			SelectedFigure.Type = TypesFigures.Queen;
			if (SelectedFigure.Side == SideColor.White)
				SelectedFigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wQ.png";
			else
				SelectedFigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png";
		}
		public void RemoveFigure(Cell _cell)
		{
			if (_cell.ChildFigure == null) return;
			AllFigures.Remove(_cell.ChildFigure);
			_cell.RemoveFigure();
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


		public static readonly DependencyProperty MarkedCellsProperty = DependencyProperty.Register(nameof(MarkedCells), typeof(List<string>), typeof(Desk),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMarkedCellsChanged)));
		public List<string> MarkedCells
		{
			get { return (List<string>)GetValue(MarkedCellsProperty); }
			set { SetValue(MarkedCellsProperty, value); }
		}
		private static void OnMarkedCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Desk desk = d as Desk;
			List<string> list = e.OldValue as List<string>;
			if (list != null)
				for (int i = 0; i < list.Count; ++i) desk.Cells[list[i]].Marked = Visibility.Collapsed;
			list = e.NewValue as List<string>;
			for (int i = 0; i < list.Count; ++i) desk.Cells[list[i]].Marked = Visibility.Visible;
		}
		private void DeskGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			SelectedCell.IsSelected = false;
			SelectedFigure.IsSelected = false;
		}
		
	}
}
