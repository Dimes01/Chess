using Chess.Controls;
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
		public static RemovedFigures RemovedFigures { get; set; }
		public static string PathStyleFigure { get; } = "glass";
		public static string PathFolderFigure { get; } = "Images";
		public static void Start()
		{
			Desk = (App.Current.MainWindow as MainWindow).MainDesk;
			RemovedFigures = (App.Current.MainWindow as MainWindow).MainRemoved;
		}
		public static void Restart()
		{
			Desk.Restart();
			RemovedFigures.Clear();
		}
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

	public enum Conditions
	{
		Stalemate,
		Check,
		Checkmate
	}
	#endregion
}
