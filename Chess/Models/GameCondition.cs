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
        public List<string> PossibleMoves { get; private set; } = new List<string>();
        private List<string> AllMovesFigure { get; set; } = new List<string>();
        private List<string> AllMoves { get; set; } = new List<string>();

        public void ChangeSelectedCell(Cell cell)
        {
            SelectedCell = cell;
        }

        public void ChangeSelectedFigure(Figure figure)
        {
            SelectedFigure = figure;

        }

        private void UpdatePossibleMoves()
        {
            UpdateAllMoves(SelectedFigure);


        }

        private void UpdateAllMoves(Figure figure)
        {
            Algorithms.CalculateAllMoves(figure, AllMovesFigure);
        }
    }
}
