using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    internal class Desk
    {
        public Figure SelectedFigure { get; private set; }
        public List<Figure> Figures { get; } = new List<Figure>();
    }
}
