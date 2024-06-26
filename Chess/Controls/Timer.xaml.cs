﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chess.Controls
{
	/// <summary>
	/// Логика взаимодействия для Timer.xaml
	/// </summary>
	public partial class Timer : UserControl
	{
		public Timer(TimeSpan _time)
		{
			InitializeComponent();
			time = _time;
		}
		private TimeSpan time;
		private DateTime _startTime;
		private TimeSpan tspan1;
		private TimeSpan tspan2;
		private TimeSpan tspan3;
		private TimeSpan tspan4;
		private bool ft = false;
		private bool st = false;
		public void Restart()
		{
			ft = false;
			st = false;
			tspan1 = time;
			tspan2 = time;
			tspan3 = time;
			tspan4 = time;
			CurrentTime1 = tspan3.ToString(@"hh\:mm\:ss");
			CurrentTime2 = tspan4.ToString(@"hh\:mm\:ss");
		}
		public void Stop()
		{
			ft = false;
			st = false;
		}
		private async void UpdateTime1()
		{
			if (ft)
			{
				tspan3 = (tspan1 - (DateTime.Now - _startTime));
				CurrentTime1 = tspan3.ToString(@"hh\:mm\:ss");
				await Task.Delay(40);
				UpdateTime1();
			}
			else tspan1 = tspan3;
		}
		private async void UpdateTime2()
		{
			if (st)
			{
				tspan4 = (tspan2 - (DateTime.Now - _startTime));
				CurrentTime2 = tspan4.ToString(@"hh\:mm\:ss");
				await Task.Delay(40);
				UpdateTime2();
			}
			else tspan2 = tspan4;
		}
		public void TimerSwitch()
		{
			_startTime = DateTime.Now;
			if (!ft && !st)
			{
				ft = true;
				UpdateTime1();
			}
			else
			{
				if (ft)
				{
					ft = false;
					st = true;
					UpdateTime2();
				}
				else
				{
					if (st)
					{
						ft = true;
						st = false;
						UpdateTime1();
					}
				}
			}
		}
		private static readonly DependencyProperty _currentTime1 = DependencyProperty.Register(nameof(CurrentTime1), typeof(string), typeof(Timer));
		public string CurrentTime1
		{
			get { return (string)GetValue(_currentTime1); }
			set { SetValue(_currentTime1, value); }
		}
		private static readonly DependencyProperty _currentTime2 = DependencyProperty.Register(nameof(CurrentTime2), typeof(string), typeof(Timer));
		public string CurrentTime2
		{
			get { return (string)GetValue(_currentTime2); }
			set { SetValue(_currentTime2, value); }
		}
	}
}
