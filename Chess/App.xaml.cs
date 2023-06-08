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
		public static GameCondition GameCondition { get; private set; }
		public static string PathStyleFigure { get; } = "glass";
		public static string PathFolderFigure { get; } = "Images";
		public static void Start()
		{
			GameCondition = new GameCondition((App.Current.MainWindow as MainWindow).MainDesk, (App.Current.MainWindow as MainWindow).MainRemoved);
		}
		public static void Restart()
		{
			GameCondition.Restart();
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
