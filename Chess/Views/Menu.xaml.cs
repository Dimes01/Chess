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
using System.Windows.Shapes;

namespace Chess.Views
{
	/// <summary>
	/// Логика взаимодействия для Menu.xaml
	/// </summary>
	public partial class Menu : Page
	{
		public Menu()
		{
			InitializeComponent();
			App.menu = this;
		}
		private readonly DependencyProperty _selectedTime = DependencyProperty.Register(nameof(SelectedTime), typeof(int), typeof(Menu));
		public int SelectedTime
		{
			get { return (int)GetValue(_selectedTime); }
			set { SetValue(_selectedTime, value); }
		}
		private readonly DependencyProperty _selectedMode = DependencyProperty.Register(nameof(SelectedMode), typeof(int), typeof(Menu));
		public int SelectedMode
		{
			get { return (int)GetValue(_selectedMode); }
			set { SetValue(_selectedMode, value); }
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).Content.Content=new Game(App.timeSpans[SelectedTime],SelectedMode);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).Close();
		}
	}
}
