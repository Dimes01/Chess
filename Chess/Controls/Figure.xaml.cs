﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Figure.xaml
    /// </summary>
    public partial class Figure : UserControl
    {
        public Figure()
        {
            InitializeComponent();
        }


        public delegate void CalcMove();

        public static readonly DependencyProperty MoveProperty = DependencyProperty.Register(nameof(Move), typeof(CalcMove), typeof(Figure));
        public CalcMove Move
        {
            get { return (CalcMove)GetValue(MoveProperty); }
            set { SetValue(MoveProperty, value); }
        }


        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(string), typeof(Figure));
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(string), typeof(Figure));
        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }


        private SolidColorBrush SelectedBrush = Brushes.Green;
        public static readonly DependencyProperty BrushBorderProperty = DependencyProperty.Register(nameof(BrushBorder), typeof(SolidColorBrush), typeof(Figure), 
            new PropertyMetadata(Brushes.Transparent));
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
            IsSelected = true;
            App.SelectedFigure = this;
        }
    }
}