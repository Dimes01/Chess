using System;
using System.Windows;
using Chess.Views;
using System.Collections.Generic;
using Chess.Controls;

namespace Chess
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// 
	/// НАСЛАЖДАЙТЕСЬ!!!
	/// 
	/// </summary>
	public partial class App : Application
	{
		public static GameCondition GameCondition { get; set; }
		public static Menu menu { get; set; }
		public static string PathStyleFigure { get; } = "glass";
		public static string PathFolderFigure { get; } = "Images";
		public static List<TimeSpan> timeSpans { get; } = new List<TimeSpan> { new TimeSpan(0,1,0),new TimeSpan(0,5,0), new TimeSpan(0,30,0), new TimeSpan(1,0,0)};
	}

	#region Перечисления
	public enum SideColor
	{
		White,
		Black,
		None
	}

	public enum TypesFigures
	{
		Pawn,
		Knight,
		Bishop,
		Rook,
		Queen,
		King
	}
	#endregion
}
