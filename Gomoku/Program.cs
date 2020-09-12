/*
    Код написан Сражовым Мирасом для задания EPAM .Net Training
    31/08/2020
*/

using System;

namespace Gomoku
{
    class Program
    {
        static void Main()
        {
            Gomoku Game = new Gomoku(15, new GomokuPlayer[] { new AI('X', 15), new AI('O', 15) });
            Game.MakeGrid(false);

            while (!Game.GameOver)
            {
                int[] places;
                try { places = Game.Players[Convert.ToInt32(Game.currentPlayer)].ChooseMove(Game.Players[Convert.ToInt32(!Game.currentPlayer)]); }
                catch { continue; }

                Game.MakeMove(places[0], places[1]);
                Game.MakeGrid(false);

                if (!Game.GameOver)
                    Console.WriteLine("Make move in format (X/Y), 0 - 14: ");

                Console.ReadKey();
            }
        }
    }

    class Gomoku
    {
        private char[,] Field_;

        //false - 0, true - 1
        public bool currentPlayer;

        public readonly GomokuPlayer[] Players;
        public int Size { get; }
        public bool GameOver { get; private set; }

        public Gomoku(int size, GomokuPlayer[] players)
        {
            Size = size;
            Field_ = new char[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int b = 0; b < Size; b++)
                    Field_[i, b] = '_';

            Players = players;
        }

        //Создает сетку в консоли с обозначениями игроков и пустых клеток
        //По бокам сетки рисует обозначение координат X и Y и выравнивает клетки
        public void MakeGrid(bool ShowLines)
        {
            string Margin = " | ";

            Console.Clear();

            Console.Write('-');
            for (char i = (char)('A' - 1); i < 'A' + Size; i++)
                Console.Write(i + Margin);

            for (int i = 0; i < Size; i++)
            {
                Console.WriteLine();
                string ToWrite = i < 10 ? '-' + i.ToString() : i.ToString();
                Console.Write(ToWrite + Margin);

                for (int k = 0; k < Size; k++)
                    Console.Write(Field_[i, k] + Margin);
            }

            Console.WriteLine();
            if (ShowLines)
            {
                foreach (var player in Players)
                {
                    Console.WriteLine("Player {0}", player.Symbol);
                    foreach (var line in player.Lines)
                        Console.WriteLine("({0}) -:- ({1} : {2}) - ({3} : {4}) + Lenght: {5} + Type: {6}", player.Symbol, line.Start.X, line.Start.Y, line.End.X, line.End.Y, line.Lenght, line.LineType);
                }
            }
            if (GameOver)
            {
                Console.WriteLine();
                Console.WriteLine("Player ({0}) has won!!!", Players[Convert.ToInt32(currentPlayer)].Symbol);
                foreach (Line TempLine in Players[Convert.ToInt32(currentPlayer)].Lines)
                    if (TempLine.Lenght == 5)
                    {
                        Console.WriteLine("Winning Line (X/Y) is -:- ({0} : {1}) - ({2} : {3})", 
                            TempLine.Start.X, TempLine.Start.Y, TempLine.End.X, TempLine.End.Y);
                        break;
                    }

            }
        }

        //Делает ход за определенного игрока и записывает его в Field
        public void MakeMove(int X, int Y)
        {
            int current = Convert.ToInt32(currentPlayer);

            Players[current].MakeMove(X, Y);
            Field_[X, Y] = Players[current].Symbol;

            if (Players[current].DidWin())
                GameOver = true;
            else
                currentPlayer = !currentPlayer;
        }
    }
}
