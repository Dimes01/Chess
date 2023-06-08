using System.Windows;

namespace Chess
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			App.Start();
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			App.GameCondition.Restart();
		}
	}
}
