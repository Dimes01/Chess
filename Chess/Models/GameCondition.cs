using Chess.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
	public class GameCondition
	{
		public SideColor CurrentStep { get; private set; } = SideColor.White;
		public Cell SelectedCell { get; private set; } = new Cell();
		public Cell CellWithSelectedFigure { get; private set; }
		public Figure SelectedFigure { get; private set; } = new Figure();

		/// <summary>
		/// ВНИМАНИЕ БЛЯТЬ!!!
		/// СОБЫТИЕ клика мыши на доске ТУННЕЛЬНОЕ (НЕ ПУЗЫРЬКОВОЕ)
		/// Сначала срабатывает обработчик на Desk, потом на Cell и только потом на конкретной Figure
		/// </summary>
		public void ChangeSelectedCell(Cell cell)
		{
			if (cell.ChildFigure != null) CellWithSelectedFigure = cell;
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

		// ЕГОР, ВОТ ЗДЕСЬ ТВОЕ МЕСТО!!! Тут нужно прописывать логику!
		private void MakeMove()
		{
			if (App.Desk.MarkedCells == null || App.Desk.MarkedCells.Count == 0) return;
			if (App.Desk.MarkedCells.Contains(SelectedCell.Position))
			{
				SelectedFigure.CountMoves++;
				// Сначала чистим состояния всех фигур. Это нужно на случай атаки на короля с нескольких сторон
				App.Desk.ClearConditions();
				CellWithSelectedFigure.RemoveFigure();
				SelectedFigure.Position = SelectedCell.Position;
				SelectedCell.ChildFigure = SelectedFigure;
				// После того, как ход сделан, обновляем состояния всех фигур. Ищем шахи, маты и прочую хуйню
				Algorithms.UpdateConditionFigures();
				if (CurrentStep == SideColor.White) CurrentStep = SideColor.Black;
				else CurrentStep = SideColor.White;
			}
		}
	}
}
