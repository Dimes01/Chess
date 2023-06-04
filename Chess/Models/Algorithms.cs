using Chess.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
	internal static class Algorithms
	{
		public static List<int> FisherRandom(List<string> strings)
		{
			List<string> list = new List<string>(strings);
			Random random = new Random();
			string firstBishop = strings[2], secondBishop = strings[5];
			string firstRook = strings[0], secondRook = strings[7];
			string temp, king = strings[4];
			int j, posFirstBishop, posSecondBishop, posKing, posFirstRook, posSecondRook;
			bool flagForBishops = false, flagForRooks = false;
			while (!flagForBishops || !flagForRooks)
			{
				list = new List<string>(strings);
				flagForBishops = false;
				flagForRooks = false;
				for (int i = list.Count - 1; i >= 1; --i)
				{
					j = random.Next(i + 1);
					temp = list[j];
					list[j] = list[i];
					list[i] = temp;
				}
				posFirstBishop = list.IndexOf(firstBishop);
				posSecondBishop = list.IndexOf(secondBishop);
				posKing = list.IndexOf(king);
				posFirstRook = list.IndexOf(firstRook);
				posSecondRook = list.IndexOf(secondRook);
				if ((posFirstBishop + posSecondBishop) % 2 == 1)
					flagForBishops = true;
				if ((posFirstRook < posKing && posKing < posSecondRook) || (posFirstRook > posKing && posKing > posSecondRook))
					flagForRooks = true;
			}
			List<int> nums = new List<int>();
			for (int i = 0; i < strings.Count; ++i) nums.Add(list.IndexOf(strings[i]));
			return nums;
		}

		// Упрощение форматирования строки
		private static string Str(int pos0, int pos1) => $"{(char)pos0}{(char)pos1}";

		// Проверка клетки на возможность хода на неё.
		// outDesk = true означает, что клетка вне доски
		// isEnemy = true означает, что на клетке вражеская фигура
		// isFriend = true означает, что на клетке дружеская фигура
		private static bool CheckCell(Figure figure, int pos0, int pos1, out bool outDesk, out bool isEnemy, out bool isFriend)
		{
			outDesk = false; isEnemy = false; isFriend = false;
			if ((pos0 < 'A') || (pos0 > 'H') || (pos1 < '1') || (pos1 > '8'))
			{
				outDesk = true;
				return false;
			}
			Figure ufigure = App.Desk.Cells[Str(pos0, pos1)].ChildFigure;
			if (ufigure != null)
			{
				isEnemy = ufigure.Side != figure.Side;
				isFriend = !isEnemy;
			}
			return (ufigure == null) || isEnemy;
		}
		private static bool CheckCell(Figure figure, int pos0, int pos1)
		{
			bool isEnemy = false;
			if ((pos0 < 'A') || (pos0 > 'H') || (pos1 < '1') || (pos1 > '8'))
			{
				return false;
			}
			Figure ufigure = App.Desk.Cells[Str(pos0, pos1)].ChildFigure;
			if (ufigure != null)
			{
				isEnemy = ufigure.Side != figure.Side;
			}
			return (ufigure == null) || isEnemy;
		}

		// Используется для недопущения нахождения королей рядом друг с другом
		private static bool CheckConflictKings(Figure figure, int pos0, int pos1)
		{
			Figure conflictFigure;
			if (figure == App.Desk.WhiteKing) conflictFigure = App.Desk.BlackKing;
			else conflictFigure = App.Desk.WhiteKing;

			return Math.Abs(conflictFigure.Position[0] - pos0) >= 2 || Math.Abs(conflictFigure.Position[1] - pos1) >= 2;
		}

		// недописаная проверка на мат, которая, по идее, должна быть после обновления состояний всех фигур
		private static bool CheckCheckmate()
		{
			return false;
		}

		// нейминг говно. Метод используется тогда, когда во время обновления состояний на проверяемой клетке оказалась вражеская фигура.
		// метод добавляет информацию атакуемой фигуре о том, кто её атакует, и, если атакуемая фигура связана, то запрещает ей ходить
		private static void CheckCondition(Figure figure, ref int countAtack, int a, int b, List<Figure> wasAtacked = null)
		{
			if(wasAtacked==null)
			{
				wasAtacked= new List<Figure>();
			}
			countAtack += 1;
			Figure fig = App.Desk.Cells[Str(a, b)].ChildFigure;
			fig.AttackingFigures.Add(figure);
			wasAtacked.Add(fig);
			if (fig.Type == TypesFigures.King && countAtack == 2)
				wasAtacked[0].Bound = figure;
		}

		#region Подсчёт возможных ходов

		private delegate void CalcPossibleMove(Figure figure, List<string> possibleMoves);
		private static Dictionary<TypesFigures, CalcPossibleMove> CalculationsMoves = new Dictionary<TypesFigures, CalcPossibleMove>
		{
			{ TypesFigures.Pawn,     PossibleMovesPawn   },
			{ TypesFigures.Knight,   PossibleMovesKnight },
			{ TypesFigures.Bishop,   PossibleMovesBishop },
			{ TypesFigures.Rook,     PossibleMovesRook   },
			{ TypesFigures.Queen,    PossibleMovesQueen  },
			{ TypesFigures.King,     PossibleMovesKing   },
		};

		public static void CalculatePossibleMoves(Figure figure)
		{
			if (!figure.CanMove) return;
			List<string> possibleMoves = new List<string>();
			CalculationsMoves[figure.Type](figure, possibleMoves);
			figure.PossibleMoves = possibleMoves;
		}

		private static void PossibleMovesPawn(Figure figure, List<string> possibleMoves)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			string position = figure.Position;
			int side;
			if (figure.Side == SideColor.White) side = 1;
			else side = -1;
			int a = position[0], b = position[1] + 1 * side;
			if (figure.Bound == null)
			{
				if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend))
				{
					if (!isEnemy && !isFriend) possibleMoves.Add(Str(a, b));
					b = position[1] + 2 * side;
					if (figure.CountMoves == 0 && CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend))
						possibleMoves.Add(Str(a, b));
					a = position[0] + 1; b = position[1] + 1 * side;
					int c;
					if (side == 1) c = b - 1;
					else c = b + 1;
					if (
						(CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy)
						|| (
							CheckCell(figure, a, c, out outDesk, out isEnemy, out isFriend)
							&& isEnemy
							&& App.Desk.PreviousFigure != null
							&& App.Desk.PreviousFigure == App.Desk.Cells[Str(a, c)].ChildFigure
							&& App.Desk.PreviousFigure.Side != figure.Side
							&& App.Desk.PreviousFigure.Type is TypesFigures.Pawn
							&& App.Desk.PreviousFigure.CountMoves == 1
							&& ((figure.Position[1] == '4' && side == -1) || (figure.Position[1] == '5' && side == 1))
						)
						) possibleMoves.Add(Str(a, b));
					a = position[0] - 1;
					if (
						(CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy)
						|| (
							CheckCell(figure, a, c, out outDesk, out isEnemy, out isFriend)
							&& isEnemy
							&& App.Desk.PreviousFigure != null
							&& App.Desk.PreviousFigure == App.Desk.Cells[Str(a, c)].ChildFigure
							&& App.Desk.PreviousFigure.Side != figure.Side
							&& App.Desk.PreviousFigure.Type is TypesFigures.Pawn
							&& App.Desk.PreviousFigure.CountMoves == 1
							&& ((figure.Position[1] == '4' && side == -1) || (figure.Position[1] == '5' && side == 1))
						)
						) possibleMoves.Add(Str(a, b));
				}
			}
			else
			{
				if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend))
				{
					a = position[0] + 1; b = position[1] + 1 * side;
					int c;
					if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
						possibleMoves.Add(Str(a, b));
					a = position[0] - 1;
					if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
						possibleMoves.Add(Str(a, b));
				}
			}
		}

		private static void PossibleMovesKnight(Figure figure, List<string> possibleMoves)
		{
			if (figure.Bound == null)
			{
				string position = figure.Position;
				int a = position[0] + 2, b = position[1] + 1;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] + 1; b = position[1] + 2;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] - 1; b = position[1] + 2;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] - 2; b = position[1] + 1;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] - 2; b = position[1] - 1;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] - 1; b = position[1] - 2;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] + 1; b = position[1] - 2;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));

				a = position[0] + 2; b = position[1] - 1;
				if (CheckCell(figure, a, b)) possibleMoves.Add(Str(a, b));
			}
		}

		private static void PossibleMovesBishop(Figure figure, List<string> possibleMoves)
		{
			int a, b;
			bool outDesk = false, isEnemy = false, isFriend = false;
			bool boundremove=false;
			string position = figure.Position;

			// Перебор вправо вверх
			for (a = position[0] + 1, b = position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a, ++b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy) 
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
				boundremove = true;
					possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();
			// Перебор влево вверх
			for (a = position[0] - 1, b = position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a, ++b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}

			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();

			// Перебор Влево вниз
			for (a = position[0] - 1, b = position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a, --b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();

			// Перебор вправо вниз
			for (a = position[0] + 1, b = position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a, --b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();
		}

		private static void PossibleMovesRook(Figure figure, List<string> possibleMoves)
		{
			int a, b;
			bool outDesk = false, isEnemy = false, isFriend = false;
			string position = figure.Position;
			bool boundremove = false;
			// Перебор вправо
			for (a = position[0] + 1, b = position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();

			// Перебор вверх
			for (a = position[0], b = position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();

			// Перебор влево
			for (a = position[0] - 1, b = position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();

			// Перебор вниз
			for (a = position[0], b = position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --b)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && App.Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
					boundremove = true;
				possibleMoves.Add(Str(a, b));
			}
			if (figure.Bound != null)
				if (boundremove)
					return;
				else
					possibleMoves.Clear();
		}

		private static void PossibleMovesQueen(Figure figure, List<string> possibleMoves)
		{
			PossibleMovesRook(figure, possibleMoves);
			PossibleMovesBishop(figure, possibleMoves);
		}

		private static void PossibleMovesKing(Figure figure, List<string> possibleMoves)
		{
			int a, b;
			string position = figure.Position;

			a = position[0] + 1; b = position[1];
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] + 1; b = position[1] + 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0]; b = position[1] + 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1] + 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1];
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1] - 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0]; b = position[1] - 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] + 1; b = position[1] - 1;
			if (CheckCell(figure, a, b) && CheckConflictKings(figure, a, b)) possibleMoves.Add(Str(a, b));
		}

		#endregion
		#region Обновление состояний фигур

		private delegate void UpdateConditions(Figure figure);
		private static Dictionary<TypesFigures, UpdateConditions> UpdateConds = new Dictionary<TypesFigures, UpdateConditions>
		{
			{ TypesFigures.Pawn,     UpdateConditionsPawn   },
			{ TypesFigures.Knight,   UpdateConditionsKnight },
			{ TypesFigures.Bishop,   UpdateConditionsBishop },
			{ TypesFigures.Rook,     UpdateConditionsRook   },
			{ TypesFigures.Queen,    UpdateConditionsQueen  },
			{ TypesFigures.King,     UpdateConditionsKing   },
		};

		public static void UpdateConditionFigures()
		{
			for (int i = 0; i < App.Desk.AllFigures.Count; ++i)
			{
				UpdateConds[App.Desk.AllFigures[i].Type](App.Desk.AllFigures[i]);
			}
			CheckCheckmate();
		}

		// Главное в обновлении состояний - найти фигуры, которые атакует текущая перебираемая фигура
		private static void UpdateConditionsPawn(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int side, countAtack = 0;
			if (figure.Side == SideColor.White) side = 1;
			else side = -1;
			int a = figure.Position[0] + 1, b = figure.Position[1] + 1 * side;
			if (CheckCell(figure, a, b,	out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);
			a = figure.Position[0] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);
		}

		// Конь атакует любые клетки
		private static void UpdateConditionsKnight(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int a = figure.Position[0] + 2, b = figure.Position[1] + 1, countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] + 2; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] + 2; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 2; b = figure.Position[1] + 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 2; b = figure.Position[1] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] - 2; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] - 2; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] + 2; b = figure.Position[1] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy) CheckCondition(figure, ref countAtack, a, b);
		}


		private static void UpdateConditionsBishop(Figure figure)
		{
			int a, b, countAtack = 0;
			List<Figure> wasAtacked = new List<Figure>();
			bool outDesk = false, isEnemy = false, isFriend = false;

			// Логичнее добавить условие перебора до тех пор, пока countAtack <= 2
			for (a = figure.Position[0] + 1, b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; ++a, ++b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] + 1, b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; ++a, --b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; --a, ++b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; --a, --b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
		}

		private static void UpdateConditionsRook(Figure figure)
		{
			int a, b, countAtack = 0;
			List<Figure> wasAtacked = new List<Figure>();
			bool outDesk = false, isEnemy = false, isFriend = false;

			// Логичнее добавить условие перебора до тех пор, пока countAtack <= 2
			for (a = figure.Position[0] + 1, b = figure.Position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; ++a)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; --a)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0], b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; ++b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0], b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !outDesk && !isFriend && countAtack < 2; --b)
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
		}

		private static void UpdateConditionsQueen(Figure figure)
		{
			UpdateConditionsRook(figure);
			UpdateConditionsBishop(figure);
		}

		// Недописано для короля. Нужен самый блять тупой перебор
		private static void UpdateConditionsKing(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int a = figure.Position[0] + 1, b = figure.Position[1], countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0]; b = figure.Position[1] + 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1]; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0]; b = figure.Position[1] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] + 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] + 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] - 1; countAtack = 0;
			if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && !outDesk && isEnemy) CheckCondition(figure, ref countAtack, a, b);
		}

		#endregion
	}
}
