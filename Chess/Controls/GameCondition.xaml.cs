using Chess.Models;
using Chess.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для GameCondition.xaml
	/// </summary>
	public partial class GameCondition : UserControl, IRestart
	{
		private Timer timer { get; set; }
		private Desk desk { get; set; }
		private RemovedFigures removedFigures { get; set; }
		public GameCondition(TimeSpan time, int mode)
		{
			InitializeComponent();
			desk = new Desk(mode) { Height = DeskGrid.Height };
			timer = new Timer(time) { Height = TimerGrid.Height - 70, Width = TimerGrid.Width, Margin = new Thickness { Top = 35, Bottom = 35 } };
			removedFigures = new RemovedFigures { Height = RemovedGrid.Height, Width = RemovedGrid.Width };
			timer.DataContext = timer;
			DeskGrid.Children.Add(desk);
			TimerGrid.Children.Add(timer);
			RemovedGrid.Children.Add(removedFigures);
			timer.Restart();
			Algorithms.SetData(desk);
			CheckTime();
		}

		private bool Go = true;
		public bool IsAtacked = false;
		public void Restart()
		{
			Go = true;
			desk.Restart();
			timer.Restart();
			removedFigures.Restart();
			WinRestart();
			foreach (var f in desk.AllFigures)
				if (f.Side != desk.CurrentStep)
					f.CanSelected = false;
			CheckTime();
		}
		// <summary>
		// СОБЫТИЕ клика мыши на доске ТУННЕЛЬНОЕ(НЕ ПУЗЫРЬКОВОЕ)
		// Сначала срабатывает обработчик на Desk, потом на Cell и только потом на конкретной Figure
		// </summary>
		public void ChangeSelectedCell(Cell cell)
		{
			if (cell.ChildFigure != null && cell.ChildFigure.Side == desk.CurrentStep) desk.CellWithSelectedFigure = cell;
			desk.SelectedCell = cell;
			MakeMove();
			desk.MarkedCells = new List<string>();
		}

		public void ChangeSelectedFigure(Figure figure)
		{
			desk.SelectedFigure = figure;
			Algorithms.CalculatePossibleMoves(desk.SelectedFigure);
			desk.MarkedCells = new List<string>(desk.SelectedFigure.PossibleMoves);
		}

		private void MakeMove()
		{
			if (desk.MarkedCells == null || desk.MarkedCells.Count == 0) return;
			if (desk.MarkedCells.Contains(desk.SelectedCell.Position))
			{
				desk.SelectedFigure.CountMoves++;
				//Сначала чистим состояния всех фигур
				desk.ClearConditions();
				RemoveFigure();
				desk.SelectedFigure.Position = desk.SelectedCell.Position;
				if (desk.SelectedCell.ChildFigure != null)
				{
					IsAtacked = true;
					RemoveFigure();
				}
				//если пешка дошла до края доски, меняем её на ферзя
				if ((desk.SelectedCell.Position[1] == '1' && desk.SelectedFigure.Type == TypesFigures.Pawn) || (desk.SelectedCell.Position[1] == '8' && desk.SelectedFigure.Type == TypesFigures.Pawn))
				{
					desk.MakeQueen();
				}
				desk.SelectedCell.ChildFigure = desk.SelectedFigure;

				//После того, как ход сделан, обновляем состояния всех фигур.
				Algorithms.UpdateConditionFigures();
				// меняем сторону
				timer.TimerSwitch();
				desk.CurrentStep = Algorithms.ColorSwitch(desk.CurrentStep);
				desk.PreviousFigure = desk.SelectedFigure;
				// Ищем шахи, маты
				int ishod = Algorithms.CheckCheckmate();
				if (ishod == 1)
				{
					Go = false;
					Win(Algorithms.ColorSwitch(desk.CurrentStep));
				}
				else
				{
					if (ishod == 2)
					{
						Go = false;
						Win(SideColor.None);
					}
				}
				foreach (var f in desk.AllFigures)
					if (f.Side != desk.CurrentStep)
						f.CanSelected = false;
			}
		}
		private void RemoveFigure()
		{
			if (IsAtacked)
			{
				Figure figure = desk.SelectedCell.ChildFigure;
				desk.RemoveFigure(desk.SelectedCell);
				removedFigures.AddRemoved(figure);
			}
			else
			{
				desk.CellWithSelectedFigure.RemoveFigure();
			}
		}
		private async void CheckTime()
		{
			if (Go)
			{
				if (timer.CurrentTime1 == "00:00:00")
				{
					timer.Stop();
					Win(SideColor.White);
				}
				else
				{
					if (timer.CurrentTime2 == "00:00:00")
					{
						timer.Stop();
						Win(SideColor.Black);
					}
					else
					{
						await Task.Delay(30);
						CheckTime();
					}
				}
			}
			else
			{
				timer.Stop();
			}
		}
		#region победа
		private static readonly DependencyProperty _textBlock1_Text = DependencyProperty.Register(nameof(TextBlock1_Text), typeof(string), typeof(GameCondition));
		public string TextBlock1_Text
		{
			get { return (string)GetValue(_textBlock1_Text); }
			set { SetValue(_textBlock1_Text, value); }
		}
		private static readonly DependencyProperty _textBlock2_Text = DependencyProperty.Register(nameof(TextBlock2_Text), typeof(string), typeof(GameCondition));
		public string TextBlock2_Text
		{
			get { return (string)GetValue(_textBlock2_Text); }
			private set { SetValue(_textBlock2_Text, value); }
		}
		private static readonly DependencyProperty _textBlock1_Color = DependencyProperty.Register(nameof(TextBlock1_Color), typeof(string), typeof(GameCondition));
		public string TextBlock1_Color
		{
			get { return (string)GetValue(_textBlock1_Color); }
			private set { SetValue(_textBlock1_Color, value); }
		}
		private static readonly DependencyProperty _textBlock2_Color = DependencyProperty.Register(nameof(TextBlock2_Color), typeof(string), typeof(GameCondition));
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
			foreach (var figure in desk.AllFigures)
			{
				figure.CanSelected = false;
			}
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
		private void Restart(object sender, RoutedEventArgs e)
		{
			this.Restart();
		}
	}
}
