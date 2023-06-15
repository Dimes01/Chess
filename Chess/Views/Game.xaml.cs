using Chess.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

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
			(App.Current.MainWindow as MainWindow).Content.Content = new Menu();
		}
	}
}
