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

        private void MakeMove()
        {
            if (App.Desk.MarkedCells == null || App.Desk.MarkedCells.Count == 0) return;
            if (App.Desk.MarkedCells.Contains(SelectedCell.Position))
            {
                App.Desk.ClearConditions();
                CellWithSelectedFigure.RemoveFigure();
                SelectedFigure.Position = SelectedCell.Position;
                SelectedCell.ChildFigure = SelectedFigure;
                Algorithms.UpdateConditionFigures();
                if (CurrentStep == SideColor.White) CurrentStep = SideColor.Black;
                else CurrentStep = SideColor.White;
            }
        }
    }
}
