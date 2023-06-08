using Chess.Utilities;
using System;
using System.Threading.Tasks;

namespace Chess.Models
{
	public class Timer : IRestart
	{
		private DateTime _startTime;
		private TimeSpan tspan1;
		private TimeSpan tspan2;
		public TimeSpan tspan3 { get; private set; }
		public TimeSpan tspan4 { get; private set; }
		private bool ft = false;
		private bool st = false;
		public void Restart()
		{
			ft = false;
			st = false;
			tspan1 = new TimeSpan(0, 5, 0);
			tspan2 = new TimeSpan(0, 5, 0);
			tspan3 = new TimeSpan(0, 5, 0);
			tspan4 = new TimeSpan(0, 5, 0);
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
				await Task.Delay(40);
				UpdateTime2();
			}
			else tspan2 = tspan4;
		}
		public string White()
		{
			return tspan3.ToString(@"hh\:mm\:ss");
		}
		public string Black()
		{
			return tspan4.ToString(@"hh\:mm\:ss");
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
	}
}
