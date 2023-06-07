using System.Windows.Controls;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для RemovedFigures.xaml
	/// </summary>
	public partial class RemovedFigures : UserControl
	{
		private int iW = 0;
		private int jW = 0;
		private int iB = 0;
		private int jB = 0;
		public RemovedFigures()
		{
			InitializeComponent();
		}
		public void AddRemoved(Figure figure)
		{
			figure.CanSelected = false;


			if (figure.Side == SideColor.White)
			{
				Grid.SetColumn(figure, iW);
				Grid.SetRow(figure, jW);
				BlackGrid.Children.Add(figure);
				iW++;
				if (iW == 8)
				{
					jW++;
					iW = 0;
				}
			}
			else
			{
				Grid.SetColumn(figure, iB);
				Grid.SetRow(figure, jB);
				WhiteGrid.Children.Add(figure);
				iB++;
				if (iB == 8)
				{
					jB++;
					iB = 0;
				}
			}
		}
		public void Clear()
		{
			jW = 0; iW = 0; jB = 0; iB = 0;
			WhiteGrid.Children.Clear();
			BlackGrid.Children.Clear();
		}
	}
}
