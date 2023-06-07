using Chess.Controls;
using System.Collections.Generic;

namespace Chess.Models
{
	public class GameCondition
	{

		public bool IsAtacked = false;
		public SideColor CurrentStep { get; set; } = SideColor.White;
		public Cell SelectedCell { get; private set; } = new Cell();
		private Cell CellWithSelectedFigure { get;  set; }
		public Figure SelectedFigure { get; private set; } = new Figure();

		/// <summary>
		/// СОБЫТИЕ клика мыши на доске ТУННЕЛЬНОЕ (НЕ ПУЗЫРЬКОВОЕ)
		/// Сначала срабатывает обработчик на Desk, потом на Cell и только потом на конкретной Figure
		/// </summary>
		public void ChangeSelectedCell(Cell cell)
		{
			if (cell.ChildFigure != null && cell.ChildFigure.Side == CurrentStep) CellWithSelectedFigure = cell;
			SelectedCell = cell;
			MakeMove();
			App.Desk.MarkedCells = new List<string>();
		}

		public void ChangeSelectedFigure(Figure figure)
		{
			SelectedFigure = figure;
			Algorithms.CalculatePossibleMoves(SelectedFigure);
			App.Desk.MarkedCells = new List<string>(SelectedFigure.PossibleMoves);
		}

		private void MakeMove()
		{
			if (App.Desk.MarkedCells == null || App.Desk.MarkedCells.Count == 0) return;
			if (App.Desk.MarkedCells.Contains(SelectedCell.Position))
			{
				SelectedFigure.CountMoves++;
				// Сначала чистим состояния всех фигур
				App.Desk.ClearConditions();
				CellWithSelectedFigure.RemoveFigure();
				SelectedFigure.Position = SelectedCell.Position;
				if (SelectedCell.ChildFigure != null)
				{
					SelectedCell.RemoveFigure();
					IsAtacked = true;
				}
				// если пешка дошла до края доски, меняем её на ферзя
				if ((SelectedCell.Position[1] == '1' && SelectedFigure.Type == TypesFigures.Pawn) || (SelectedCell.Position[1] == '8' && SelectedFigure.Type == TypesFigures.Pawn))
				{
					App.Desk.MakeQueen();
				}
				SelectedCell.ChildFigure = SelectedFigure;
				
				// После того, как ход сделан, обновляем состояния всех фигур.
				Algorithms.UpdateConditionFigures();
				// меняем сторону
				App.Desk.TimerSwitch();
				CurrentStep = Algorithms.ColorSwitch(CurrentStep);
				App.Desk.PreviousFigure = SelectedFigure;
				// Ищем шахи, маты
				if(Algorithms.CheckCheckmate())
				{
					App.Desk.Win(Algorithms.ColorSwitch(CurrentStep));
				}
			}
		}
	}
}
