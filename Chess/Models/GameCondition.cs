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
        public Figure SelectedFigure { get; private set; } = new Figure();

        public void ChangeSelectedCell(Cell cell)
        {
            SelectedCell = cell;
        }

        public void ChangeSelectedFigure(Figure figure)
        {
            SelectedFigure = figure;
            Algorithms.CalculatePossibleMoves(SelectedFigure);
            App.Desk.MarkedCells = SelectedFigure.PossibleMoves;
        }
    }
}
