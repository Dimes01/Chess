using Chess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Desk.xaml
	/// </summary>
	public partial class Desk : UserControl
	{
		public GameCondition GameCondition { get; private set; }
		public Desk()
		{
			InitializeComponent();
			MakeDesk();
			Restart();
		}
		public Dictionary<SideColor, Figure> Kings { get; private set; }
		public Figure PreviousFigure { get; set; }
		public List<Figure> AllFigures { get; private set; } = new List<Figure>();
		public List<string> DefensiveMoves { get; set; }
		public void Restart()
		{
			GameCondition = new GameCondition();
			TimerRestart();
			WinRestart();
			AllFigures.Clear();
			DefensiveMoves = null;
			Kings = null; PreviousFigure = null;
			MarkedCells = new List<string>();
			foreach (var cell in Cells)
			{
				cell.Value.RemoveFigure();
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
			GameCondition.SelectedFigure.Type = TypesFigures.Queen;
			if (GameCondition.SelectedFigure.Side == SideColor.White)
				GameCondition.SelectedFigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/wQ.png";
			else
				GameCondition.SelectedFigure.ImageSource = $"pack://application:,,,/{App.PathFolderFigure}/{App.PathStyleFigure}/bQ.png";
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
			GameCondition.SelectedCell.IsSelected = false;
			GameCondition.SelectedFigure.IsSelected = false;
		}
		#region timer
		private DateTime _startTime;
		private TimeSpan tspan1;
		private TimeSpan tspan2;
		private TimeSpan tspan3;
		private TimeSpan tspan4;
		private bool ft = false;
		private bool st = false;
		private void TimerRestart()
		{
			ft = false;
			st = false;
			tspan1 = new TimeSpan(0, 5, 0);
			tspan2 = new TimeSpan(0, 5, 0);
			tspan3 = new TimeSpan(0, 5, 0);
			tspan4 = new TimeSpan(0, 5, 0);
			CurrentTime1 = tspan1.ToString(@"hh\:mm\:ss");
			CurrentTime2 = tspan2.ToString(@"hh\:mm\:ss");
		}
		private static readonly DependencyProperty _currentTime1 = DependencyProperty.Register(nameof(CurrentTime1), typeof(string), typeof(Desk));
		public string CurrentTime1
		{
			get { return (string)GetValue(_currentTime1); }
			private set { SetValue(_currentTime1, value); }
		}
		private static readonly DependencyProperty _currentTime2 = DependencyProperty.Register(nameof(CurrentTime2), typeof(string), typeof(Desk));
		public string CurrentTime2
		{
			get { return (string)GetValue(_currentTime2); }
			private set { SetValue(_currentTime2, value); }
		}
		private async void UpdateTime1()
		{
			if (ft)
			{
				tspan3 = (tspan1 - (DateTime.Now - _startTime));
				CurrentTime1 = tspan3.ToString(@"hh\:mm\:ss");
				if (tspan3.Hours == 0 && tspan3.Minutes == 0 && tspan3.Seconds == 0)
				{
					Win(Algorithms.ColorSwitch(GameCondition.CurrentStep));
				}
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
				if (tspan4.Hours == 0 && tspan4.Minutes == 0 && tspan4.Seconds == 0)
				{
					Win(Algorithms.ColorSwitch(GameCondition.CurrentStep));
				}
				await Task.Delay(40);
				UpdateTime2();
			}
			else tspan2 = tspan4;
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
		#endregion
		#region победа
		private static readonly DependencyProperty _textBlock1_Text = DependencyProperty.Register(nameof(TextBlock1_Text), typeof(string), typeof(Desk));
		public string TextBlock1_Text
		{
			get { return (string)GetValue(_textBlock1_Text); }
			private set { SetValue(_textBlock1_Text, value); }
		}
		private static readonly DependencyProperty _textBlock2_Text = DependencyProperty.Register(nameof(TextBlock2_Text), typeof(string), typeof(Desk));
		public string TextBlock2_Text
		{
			get { return (string)GetValue(_textBlock2_Text); }
			private set { SetValue(_textBlock2_Text, value); }
		}
		private static readonly DependencyProperty _textBlock1_Color = DependencyProperty.Register(nameof(TextBlock1_Color), typeof(string), typeof(Desk));
		public string TextBlock1_Color
		{
			get { return (string)GetValue(_textBlock1_Color); }
			private set { SetValue(_textBlock1_Color, value); }
		}
		private static readonly DependencyProperty _textBlock2_Color = DependencyProperty.Register(nameof(TextBlock2_Color), typeof(string), typeof(Desk));
		public string TextBlock2_Color
		{
			get { return (string)GetValue(_textBlock2_Color); }
			private set { SetValue(_textBlock2_Color, value); }
		}
		private void WinRestart()
		{
			TextBlock2_Text = null;
			TextBlock2_Color = null;
			TextBlock1_Text = null;
			TextBlock1_Color = null;
		}
		public void Win(SideColor wincolor)
		{
			foreach (var figure in AllFigures)
			{
				figure.CanSelected = false;
			}
			ft = false;
			st = false;
			//текстблоки
			if (wincolor == SideColor.White)
			{
				TextBlock2_Text = "Победа";
				TextBlock2_Color = "#FF00E800";
				TextBlock1_Text = "Поражение";
				TextBlock1_Color = "#FFF50000";
			}
			else
			{
				if (wincolor == SideColor.Black)
				{
					TextBlock1_Text = "Победа";
					TextBlock1_Color = "#FF00E800";
					TextBlock2_Text = "Поражение";
					TextBlock2_Color = "#FFF50000";
				}
				else
				{
					TextBlock1_Text = "Ничья";
					TextBlock1_Color = "#FF00E800";
					TextBlock2_Text = "Ничья";
					TextBlock2_Color = "#FF00E800";
				}
			}
		}
		#endregion
	}
}
