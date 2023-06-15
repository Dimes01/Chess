using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Chess.Controls;

namespace Chess.Views
{
	/// <summary>
	/// Логика взаимодействия для Menu.xaml
	/// </summary>
	public partial class Menu : Page
	{
		private MenuSettings menuSettings;
		public Menu()
		{
			InitializeComponent();
			menuSettings = new MenuSettings() { Height=MainMenu.Height};
			MainMenu.Children.Add(menuSettings);
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).Content.Content=new Game(App.timeSpans[menuSettings.SelectedTime], menuSettings.SelectedMode);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).Close();
		}
	}
}
