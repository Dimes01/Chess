using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Figure.xaml
	/// </summary>
	public partial class Figure : UserControl
	{
		public Figure()
		{
			InitializeComponent();
		}

		public bool CanMove { get; set; } = true;
		public Figure Bound { get; set; } = null;
		public int CountMoves { get; set; }
		public List<string> PossibleMoves { get; set; } = new List<string>();
		public List<Figure> AttackingFigures { get; set; } = new List<Figure>();

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(string), typeof(Figure));
		public string ImageSource
		{
			get { return (string)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}


		public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(TypesFigures), typeof(Figure));
		public TypesFigures Type
		{
			get { return (TypesFigures)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}


		public static readonly DependencyProperty SideProperty = DependencyProperty.Register(nameof(Side), typeof(SideColor), typeof(Figure));
		public SideColor Side
		{
			get { return (SideColor)GetValue(SideProperty); }
			set { SetValue(SideProperty, value); }
		}


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(string), typeof(Figure));
		public string Position
		{
			get { return (string)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}


		private SolidColorBrush SelectedBrush = Brushes.Green;

		public static readonly DependencyProperty BrushBorderProperty = DependencyProperty.Register(nameof(BrushBorder), typeof(SolidColorBrush), typeof(Figure), new PropertyMetadata(Brushes.Transparent));
		public SolidColorBrush BrushBorder
		{
			get { return (SolidColorBrush)GetValue(BrushBorderProperty); }
			set { SetValue(BrushBorderProperty, value); }
		}


		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(Figure),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsSelectedChanged)));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Figure figure = (Figure)d;
			SolidColorBrush brush = new SolidColorBrush(figure.BrushBorder.Color);
			figure.BrushBorder = figure.SelectedBrush;
			figure.SelectedBrush = brush;
		}


		private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (Side == App.GameCondition.CurrentStep && !App.GameCondition.IsAtacked)
			{
				IsSelected = true;
				App.GameCondition.ChangeSelectedFigure(this);
			}
			App.GameCondition.IsAtacked = false;
		}
	}
}