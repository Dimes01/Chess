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

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для MenuSettings.xaml
	/// </summary>
	public partial class MenuSettings : UserControl
	{
		public MenuSettings()
		{
			InitializeComponent();
		}
		private static readonly DependencyProperty _selectedTime = DependencyProperty.Register(nameof(SelectedTime), typeof(int), typeof(MenuSettings));
		public int SelectedTime
		{
			get { return (int)GetValue(_selectedTime); }
			set { SetValue(_selectedTime, value); }
		}
		private static readonly DependencyProperty _selectedMode = DependencyProperty.Register(nameof(SelectedMode), typeof(int), typeof(MenuSettings));
		public int SelectedMode
		{
			get { return (int)GetValue(_selectedMode); }
			set { SetValue(_selectedMode, value); }
		}
	}
}
