using Chess.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chess.Models
{
	public class GameCondition
	{
		public GameCondition(Desk d, RemovedFigures rf)
		{
			Desk = d;
			Desk.SetRemoved(rf);
			Timer = new Timer();
			Timer.Restart();
			Algorithms.SetData(Desk);
			CheckTime();
		}
		private Timer Timer { get; set; } = new Timer();
		private bool Go = true;
		public bool IsAtacked = false;
		private Desk Desk { get; set; }
		public void Restart()
		{
			Go = true;
			Desk.Restart();
			Timer.Restart();
			Desk.CurrentTime1 = Timer.White();
			Desk.CurrentTime2 = Timer.Black();
			foreach (var f in Desk.AllFigures)
				if (f.Side != Desk.CurrentStep)
					f.CanSelected = false;
			CheckTime();
		}
		/// <summary>
		/// СОБЫТИЕ клика мыши на доске ТУННЕЛЬНОЕ (НЕ ПУЗЫРЬКОВОЕ)
		/// Сначала срабатывает обработчик на Desk, потом на Cell и только потом на конкретной Figure
		/// </summary>
		public void ChangeSelectedCell(Cell cell)
		{
			if (cell.ChildFigure != null && cell.ChildFigure.Side == Desk.CurrentStep) Desk.CellWithSelectedFigure = cell;
			Desk.SelectedCell = cell;
			MakeMove();
			Desk.MarkedCells = new List<string>();
		}

		public void ChangeSelectedFigure(Figure figure)
		{
			Desk.SelectedFigure = figure;
			Algorithms.CalculatePossibleMoves(Desk.SelectedFigure);
			Desk.MarkedCells = new List<string>(Desk.SelectedFigure.PossibleMoves);
		}
		private async void CheckTime()
		{
			if (Go)
			{
				if (Timer.White() == "00:00:00")
				{
					Timer.Stop();
					Desk.CurrentTime1 = Timer.White();
					Desk.Win(SideColor.White);
				}
				else
				{
					if (Timer.Black() == "00:00:00")
					{
						Timer.Stop();
						Desk.CurrentTime2 = Timer.Black();
						Desk.Win(SideColor.Black);
					}
					else
					{
						Desk.CurrentTime1 = Timer.White();
						Desk.CurrentTime2 = Timer.Black();
						await Task.Delay(30);
						CheckTime();
					}
				}
			}
			else
			{
				Timer.Stop();
			}
		}
		private void MakeMove()
		{
			if (Desk.MarkedCells == null || Desk.MarkedCells.Count == 0) return;
			if (Desk.MarkedCells.Contains(Desk.SelectedCell.Position))
			{
				Desk.SelectedFigure.CountMoves++;
				// Сначала чистим состояния всех фигур
				Desk.ClearConditions();
				Desk.RemoveFigure(Desk.CellWithSelectedFigure);
				Desk.SelectedFigure.Position = Desk.SelectedCell.Position;
				if (Desk.SelectedCell.ChildFigure != null)
				{
					IsAtacked = true;
					Desk.RemoveFigure(Desk.SelectedCell, IsAtacked);
				}
				// если пешка дошла до края доски, меняем её на ферзя
				if ((Desk.SelectedCell.Position[1] == '1' && Desk.SelectedFigure.Type == TypesFigures.Pawn) || (Desk.SelectedCell.Position[1] == '8' && Desk.SelectedFigure.Type == TypesFigures.Pawn))
				{
					Desk.MakeQueen();
				}
				Desk.SelectedCell.ChildFigure = Desk.SelectedFigure;

				// После того, как ход сделан, обновляем состояния всех фигур.
				Algorithms.UpdateConditionFigures();
				// меняем сторону
				Timer.TimerSwitch();
				Desk.CurrentStep = Algorithms.ColorSwitch(Desk.CurrentStep);
				Desk.PreviousFigure = Desk.SelectedFigure;
				// Ищем шахи, маты
				int ishod = Algorithms.CheckCheckmate();
				if (ishod == 1)
				{
					Go = false;
					Desk.Win(Algorithms.ColorSwitch(Desk.CurrentStep));
				}
				else
				{
					if (ishod == 2)
					{
						Go = false;
						Desk.Win(SideColor.None);
					}
				}
				foreach (var f in Desk.AllFigures)
					if (f.Side != Desk.CurrentStep)
						f.CanSelected = false;
			}
		}
	}
}
