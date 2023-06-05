using Chess.Controls;
using Chess.Models;
using System.Windows;

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
		public static Desk Desk { get; set; }
		public static GameCondition GameCondition { get; private set; }
		public static string PathStyleFigure { get; } = "glass";
		public static string PathFolderFigure { get; } = "Images";
		public static void Restart()
		{
			Desk = (App.Current.MainWindow as MainWindow).MainDesk;
			Desk.Restart();
			GameCondition = new GameCondition();
		}
	}

	#region Перечисления
	public enum SideColor
	{
		White,
		Black
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

	public enum Conditions
	{
		Stalemate,
		Check,
		Checkmate
	}
	#endregion
}
