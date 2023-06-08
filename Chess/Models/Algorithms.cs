using Chess.Controls;
using System;
using System.Collections.Generic;

namespace Chess.Models
{
	internal static class Algorithms
	{
		private static Desk Desk;
		public static void SetData(Desk _desk)
		{
			Desk = _desk;
		}
		// начальная растановка фигур
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

		public static SideColor ColorSwitch(SideColor color) => color == SideColor.White ? SideColor.Black : SideColor.White;
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
			Figure ufigure = Desk.Cells[Str(pos0, pos1)].ChildFigure;

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
			Figure ufigure = Desk.Cells[Str(pos0, pos1)].ChildFigure;
			if (ufigure != null)
			{
				isEnemy = ufigure.Side != figure.Side;
			}
			return (ufigure == null) || isEnemy;
		}
		// Используется для проверки клетки на шах
		private static bool Check(Figure figure, int pos0, int pos1)
		{
			if (Desk.Cells[Str(pos0, pos1)].AttackingFigures[ColorSwitch(figure.Side)].Count > 0)
				return false;
			else
				return true;
		}

		// проверка на мат
		public static int CheckCheckmate()
		{
			Figure king = Desk.Kings[Desk.CurrentStep];
			int acount = Desk.Cells[king.Position].AttackingFigures[ColorSwitch(king.Side)].Count;
			if (acount == 2)
			{
				List<string> possibleMoves = new List<string>();
				CalculationsMoves[TypesFigures.King](king, possibleMoves);
				if (possibleMoves.Count == 0)
					return 1;
			}
			else
			{
				if (acount == 1)
				{
					FindProtectingCells();
					int countmoves = 0;
					foreach (var figure in Desk.AllFigures)
					{
						if (figure.Side == king.Side)
						{
							CalculatePossibleMoves(figure);
							countmoves += figure.PossibleMoves.Count;
						}
					}
					if (countmoves == 0)
					{
						return 1;
					}
				}
				else
				{
					if (acount == 0)
					{
						int countmoves = 0;
						foreach (var figure in Desk.AllFigures)
						{
							if (figure.Side == king.Side)
							{
								CalculatePossibleMoves(figure);
								countmoves += figure.PossibleMoves.Count;
							}
						}
						if (countmoves == 0)
						{
							return 2;
						}
					}
				}
			}
			return 0;
		}
		// поиск клеток, которые при шахе находятся между королём и атакающей фигурой
		private static void FindProtectingCells()
		{
			Figure king = Desk.Kings[Desk.CurrentStep];
			Figure AtFigure = Desk.Cells[king.Position].AttackingFigures[ColorSwitch(king.Side)][0];
			List<string> PossibleMoves = new List<string>();
			if (AtFigure.Type == TypesFigures.Pawn || AtFigure.Type == TypesFigures.Knight)
			{
				PossibleMoves.Add(AtFigure.Position);
				Desk.DefensiveMoves = PossibleMoves;
			}
			else
			{
				int a, b;
				bool outDesk = false, isEnemy = false, isFriend = false;
				string position = king.Position;
				// Перебор вправо вверх
				for (a = position[0] + 1, b = position[1] + 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a, ++b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор влево вверх
				for (a = position[0] - 1, b = position[1] + 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a, ++b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор Влево вниз
				for (a = position[0] - 1, b = position[1] - 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a, --b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор вправо вниз
				for (a = position[0] + 1, b = position[1] - 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a, --b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				//Перебор вправо
				for (a = position[0] + 1, b = position[1]; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор вверх
				for (a = position[0], b = position[1] + 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор влево
				for (a = position[0] - 1, b = position[1]; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --a)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();

				// Перебор вниз
				for (a = position[0], b = position[1] - 1; CheckCell(king, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); --b)
					PossibleMoves.Add(Str(a, b));
				if (!outDesk && isEnemy)
				{
					PossibleMoves.Add(Str(a, b));
					if (Str(a, b) == AtFigure.Position)
					{
						Desk.DefensiveMoves = PossibleMoves;
						return;
					}
				}
				PossibleMoves.Clear();
			}
		}
		// при шахе удаляет их списка возможных ходов фигуры все, что не являются защитными
		private static void RemoveNonDefensivMoves(Figure figure)
		{
			if (Desk.DefensiveMoves == null) return;
			for (int i = 0; i < figure.PossibleMoves.Count; i++)
			{
				if (!Desk.DefensiveMoves.Contains(figure.PossibleMoves[i]))
				{
					figure.PossibleMoves.RemoveAt(i);
					i--;
				}
			}
		}
		// проверка на то, является ли фигура привязанной
		private static void CheckCondition(Figure figure, ref int countAtack, int pos0, int pos1, List<Figure> wasAtacked)
		{
			countAtack += 1;
			Figure fig = Desk.Cells[Str(pos0, pos1)].ChildFigure;
			wasAtacked.Add(fig);
			if (fig.Type == TypesFigures.King && countAtack == 2)
				wasAtacked[0].Bound = figure;
		}
		// считаем сколько фигур атакуют клетку
		private static void UpdateCellAttackingFigures(Figure figure, int pos0, int pos1)
		{
			Desk.Cells[Str(pos0, pos1)].AttackingFigures[figure.Side].Add(figure);
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
			if (!figure.CanSelected || (Desk.Cells[Desk.Kings[Desk.CurrentStep].Position].AttackingFigures[ColorSwitch(Desk.CurrentStep)].Count == 2 && figure.Type != TypesFigures.King)) return;
			List<string> possibleMoves = new List<string>();
			CalculationsMoves[figure.Type](figure, possibleMoves);
			figure.PossibleMoves.AddRange(possibleMoves);
			if (figure.Type != TypesFigures.King)
				RemoveNonDefensivMoves(figure);
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
					if (!isEnemy && !isFriend)
						possibleMoves.Add(Str(a, b));
					b = position[1] + 2 * side;
					if (figure.CountMoves == 0 && !isEnemy && !isFriend && CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isEnemy && !isFriend)
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
							&& Desk.PreviousFigure != null
							&& Desk.PreviousFigure == Desk.Cells[Str(a, c)].ChildFigure
							&& Desk.PreviousFigure.Side != figure.Side
							&& Desk.PreviousFigure.Type is TypesFigures.Pawn
							&& Desk.PreviousFigure.CountMoves == 1
							&& ((figure.Position[1] == '4' && side == -1) || (figure.Position[1] == '5' && side == 1))
						)
						) possibleMoves.Add(Str(a, b));
					a = position[0] - 1;
					if (
						(CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy)
						|| (
							CheckCell(figure, a, c, out outDesk, out isEnemy, out isFriend)
							&& isEnemy
							&& Desk.PreviousFigure != null
							&& Desk.PreviousFigure == Desk.Cells[Str(a, c)].ChildFigure
							&& Desk.PreviousFigure.Side != figure.Side
							&& Desk.PreviousFigure.Type is TypesFigures.Pawn
							&& Desk.PreviousFigure.CountMoves == 1
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
					if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
						possibleMoves.Add(Str(a, b));
					a = position[0] - 1;
					if (CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && isEnemy && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
			bool boundremove = false;
			string position = figure.Position;

			// Перебор вправо вверх
			for (a = position[0] + 1, b = position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && (!isEnemy && !isFriend); ++a, ++b)
			{
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
				possibleMoves.Add(Str(a, b));
			}
			if (!outDesk && isEnemy)
			{
				if (figure.Bound != null && Desk.Cells[Str(a, b)].ChildFigure == figure.Bound)
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
			figure.PossibleMoves.AddRange(possibleMoves);
			PossibleMovesBishop(figure, possibleMoves);
		}

		private static void PossibleMovesKing(Figure figure, List<string> possibleMoves)
		{
			int a, b;
			string position = figure.Position;

			a = position[0] + 1; b = position[1];
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] + 1; b = position[1] + 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0]; b = position[1] + 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1] + 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1];
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] - 1; b = position[1] - 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0]; b = position[1] - 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));

			a = position[0] + 1; b = position[1] - 1;
			if (CheckCell(figure, a, b) && Check(figure, a, b)) possibleMoves.Add(Str(a, b));
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
			for (int i = 0; i < Desk.AllFigures.Count; ++i)
			{
				UpdateConds[Desk.AllFigures[i].Type](Desk.AllFigures[i]);
			}
		}

		// Главное в обновлении состояний - найти фигуры, которые атакует текущая перебираемая фигура
		private static void UpdateConditionsPawn(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int side;
			if (figure.Side == SideColor.White) side = 1;
			else side = -1;
			int a = figure.Position[0] + 1, b = figure.Position[1] + 1 * side;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);
			a = figure.Position[0] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);
		}

		// Конь атакует любые клетки
		private static void UpdateConditionsKnight(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int a = figure.Position[0] + 2, b = figure.Position[1] + 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] + 2;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] + 2;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 2; b = figure.Position[1] + 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 2; b = figure.Position[1] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] - 2;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] - 2;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] + 2; b = figure.Position[1] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);
		}


		private static void UpdateConditionsBishop(Figure figure)
		{
			int a, b, countAtack = 0;
			List<Figure> wasAtacked = new List<Figure>();
			bool outDesk = false, isEnemy = false, isFriend = false;

			for (a = figure.Position[0] + 1, b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; ++a, ++b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] + 1, b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; ++a, --b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; --a, ++b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; --a, --b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
		}

		private static void UpdateConditionsRook(Figure figure)
		{
			int a, b, countAtack = 0;
			List<Figure> wasAtacked = new List<Figure>();
			bool outDesk = false, isEnemy = false, isFriend = false;

			for (a = figure.Position[0] + 1, b = figure.Position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; ++a)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0] - 1, b = figure.Position[1]; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; --a)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0], b = figure.Position[1] + 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; ++b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
			wasAtacked.Clear();

			countAtack = 0;
			for (a = figure.Position[0], b = figure.Position[1] - 1; CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend) && !isFriend && countAtack < 2; --b)
			{
				if (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King)) UpdateCellAttackingFigures(figure, a, b);
				if (isEnemy) CheckCondition(figure, ref countAtack, a, b, wasAtacked);
			}
			if (isFriend && (countAtack < 1 || (countAtack == 1 && wasAtacked[0].Type == TypesFigures.King))) UpdateCellAttackingFigures(figure, a, b);
		}

		private static void UpdateConditionsQueen(Figure figure)
		{
			UpdateConditionsRook(figure);
			UpdateConditionsBishop(figure);
		}

		private static void UpdateConditionsKing(Figure figure)
		{
			bool outDesk = false, isEnemy = false, isFriend = false;
			int a = figure.Position[0] + 1, b = figure.Position[1];
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0]; b = figure.Position[1] + 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1];
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0]; b = figure.Position[1] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] + 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] + 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] - 1; b = figure.Position[1] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);

			a = figure.Position[0] + 1; b = figure.Position[1] - 1;
			CheckCell(figure, a, b, out outDesk, out isEnemy, out isFriend);
			if (isEnemy || isFriend || !outDesk) UpdateCellAttackingFigures(figure, a, b);
		}

		#endregion
	}
}
