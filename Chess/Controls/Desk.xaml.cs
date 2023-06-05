using Chess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Desk.xaml
	/// </summary>
	public partial class Desk : UserControl, INotifyPropertyChanged
	{
		public Desk()
		{
			InitializeComponent();
			MakeDesk();
			Restart();
		}
		private string _currentTime1;
		private string _currentTime2;
		private DateTime _startTime;
		private TimeSpan tspan1;
		private TimeSpan tspan2;
		private TimeSpan tspan3;
		private TimeSpan tspan4;
		private bool ft = false;
		private bool st = false;
		public Figure WhiteKing { get; private set; }
		public Figure BlackKing { get; private set; }
		public Figure PreviousFigure { get; set; }
		public List<Figure> AllFigures { get; private set; }
		public void Restart()
		{
			ft = false;
			st = false;
			tspan1 = new TimeSpan(0, 5, 0);
			tspan2 = new TimeSpan(0, 5, 0);
			tspan3 = new TimeSpan(0, 5, 0);
			tspan4 = new TimeSpan(0, 5, 0);
			CurrentTime1 = tspan1.ToString(@"hh\:mm\:ss");
			CurrentTime2 = tspan2.ToString(@"hh\:mm\:ss");
			AllFigures = new List<Figure>();
			WhiteKing = null; BlackKing = null; PreviousFigure = null;
			foreach (var cell in Cells)
				cell.Value.ChildFigure = null;
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

			// Люди
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

			// Негры
			Cells[$"{(char)(positions[0] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[0] + 'A')}8", Type = TypesFigures.Rook, Side = SideColor.Black };
			Cells[$"{(char)(positions[1] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[1] + 'A')}8", Type = TypesFigures.Knight, Side = SideColor.Black };
			Cells[$"{(char)(positions[2] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[2] + 'A')}8", Type = TypesFigures.Bishop, Side = SideColor.Black };
			Cells[$"{(char)(positions[3] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png", Position = $"{(char)(positions[3] + 'A')}8", Type = TypesFigures.Queen, Side = SideColor.Black };
			Cells[$"{(char)(positions[4] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bK.png", Position = $"{(char)(positions[4] + 'A')}8", Type = TypesFigures.King, Side = SideColor.Black };
			Cells[$"{(char)(positions[5] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bB.png", Position = $"{(char)(positions[5] + 'A')}8", Type = TypesFigures.Bishop, Side = SideColor.Black };
			Cells[$"{(char)(positions[6] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bN.png", Position = $"{(char)(positions[6] + 'A')}8", Type = TypesFigures.Knight, Side = SideColor.Black };
			Cells[$"{(char)(positions[7] + 'A')}8"].ChildFigure = new Figure { ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bR.png", Position = $"{(char)(positions[7] + 'A')}8", Type = TypesFigures.Rook, Side = SideColor.Black };
			for (int i = 0; i < positions.Count; ++i) AllFigures.Add(Cells[$"{(char)(positions[i] + 'A')}8"].ChildFigure);

			WhiteKing = Cells[$"{(char)(positions[4] + 'A')}1"].ChildFigure;
			BlackKing = Cells[$"{(char)(positions[4] + 'A')}8"].ChildFigure;
			AllFigures.Add(WhiteKing);
			AllFigures.Add(BlackKing);
		}
		private async void UpdateTime1()
		{
			if (ft)
			{
				tspan3 = (tspan1 - (DateTime.Now - _startTime));
				CurrentTime1 = tspan3.ToString(@"hh\:mm\:ss");
				await Task.Delay(40);
				UpdateTime1();
			}
			else tspan1 = tspan3;
		}
		private async void UpdateTime2()
		{
			if (st)
			{
				tspan4 = (tspan2 - (DateTime.Now - _startTime));
				CurrentTime2 = tspan4.ToString(@"hh\:mm\:ss");
				await Task.Delay(40);
				UpdateTime2();
			}
			else tspan2 = tspan4;
		}
		public void ClearConditions()
		{
			for (int i = 0; i < AllFigures.Count; ++i)
			{
				AllFigures[i].AttackingFigures.Clear();
				AllFigures[i].PossibleMoves.Clear();
				AllFigures[i].Bound = null;
			}
		}
		public void MakeQueen(Figure selectedfigure)
		{
			selectedfigure.Type = TypesFigures.Queen;
			if(selectedfigure.Side == SideColor.White)
				selectedfigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wQ.png";
			else
				selectedfigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png";
		}

		public static readonly DependencyProperty CellsProperty = DependencyProperty.Register(nameof(Cells), typeof(Dictionary<string, Cell>), typeof(Desk));
		public string CurrentTime1
		{
			get { return _currentTime1; }
			set { _currentTime1 = value; OnPropertyChanged(nameof(CurrentTime1)); }
		}
		public string CurrentTime2
		{
			get { return _currentTime2; }
			set { _currentTime2 = value; OnPropertyChanged(nameof(CurrentTime2)); }
		}
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
		public void TimerSwitch()
		{
			_startTime = DateTime.Now;
			if (!ft && !st)
			{
				ft = true;
				UpdateTime1();
			}
			else
			{
				if (ft)
				{
					ft = false;
					st = true;
					UpdateTime2();
				}
				else
				{
					if (st)
					{
						ft = true;
						st = false;
						UpdateTime1();
					}
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		private void DeskGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			App.GameCondition.SelectedCell.IsSelected = false;
			App.GameCondition.SelectedFigure.IsSelected = false;
		}
	}
}
