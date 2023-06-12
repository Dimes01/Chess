using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Cell.xaml
	/// </summary>
	public partial class Cell : UserControl
	{
		public Cell()
		{
			InitializeComponent();
		}
		public Dictionary<SideColor, List<Figure>> AttackingFigures { get; private set; } = new Dictionary<SideColor, List<Figure>>
		{
			{SideColor.White,new List<Figure>() },
			{SideColor.Black,new List<Figure>() }
		};

		private Figure _childFigure;
		public Figure ChildFigure
		{
			get => _childFigure;
			set
			{
				if (_childFigure == value) return;
				if (_childFigure != null) CellGrid.Children.Remove(_childFigure);
				_childFigure = value;
				if (_childFigure != null)
					CellGrid.Children.Add(_childFigure);
			}
		}

		public void RemoveFigure()
		{
			CellGrid.Children.Remove(_childFigure);
			_childFigure = null;
		}


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(string), typeof(Cell), new PropertyMetadata(""));
		public string Position
		{
			get { return (string)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}


		public static readonly DependencyProperty BackBrushProperty = DependencyProperty.Register(nameof(BackBrush), typeof(Brush), typeof(Cell));
		public Brush BackBrush
		{
			get { return (Brush)GetValue(BackBrushProperty); }
			set { SetValue(BackBrushProperty, value); }
		}


		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(Cell));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}


		public static readonly DependencyProperty MarkedProperty = DependencyProperty.Register(nameof(Marked), typeof(Visibility), typeof(Cell), new FrameworkPropertyMetadata(Visibility.Collapsed));
		public Visibility Marked
		{
			get { return (Visibility)GetValue(MarkedProperty); }
			set { SetValue(MarkedProperty, value); }
		}
		private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IsSelected = true;
			(App.Current.MainWindow as MainWindow).Game.ChangeSelectedCell(this);
		}
	}
}
