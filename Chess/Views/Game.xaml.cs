using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Chess.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Views
{
	/// <summary>
	/// Логика взаимодействия для Game.xaml
	/// </summary>
	public partial class Game : Page
	{
		public Game(TimeSpan time, int mode)
		{
			InitializeComponent();
			GameCondition gameCondition = new GameCondition(time, mode) { Height = MainGrid.Height, Width = MainGrid.Width };
			MainGrid.Children.Add(gameCondition);
			App.GameCondition = gameCondition;
		}

		private void ToMenu(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).Content.Navigate(App.menu);
		}
	}
}
