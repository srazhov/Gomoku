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
            Gomoku Game = new Gomoku(15);
            Game.MakeGrid(false);

            while (true)
            {
                string[] line = Console.ReadLine().Split('/');
                if (line.Length != 2 || !int.TryParse(line[0], out int PlaceA) || !int.TryParse(line[1], out int PlaceB) || 
                    PlaceA > 15 || PlaceB > 15)
                    continue;

                Game.MakeMove(PlaceA, PlaceB);
                Game.MakeGrid(true);

                Console.WriteLine("Make move in format (A/B), Max is 15: ");
            }
        }
    }

    class Gomoku
    {
        private char[,] Field_;

        //false - 0, true - 1
        bool currentPlayer;

        public readonly GomokuPlayer[] Players;
        public int Size { get; }

        public Gomoku(int size)
        {
            Size = size;
            Field_ = new char[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int b = 0; b < Size; b++)
                    Field_[i, b] = '_';

            Players = new GomokuPlayer[] { new GomokuPlayer('X'), new GomokuPlayer('O') };
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
        }
        
        //Делает ход за определенного игрока и записывает его в Field
        public void MakeMove(int X, int Y) 
        {
            if (Field_[X, Y] != '_')
                throw new ArgumentException();

            Players[Convert.ToInt32(currentPlayer)].MakeMove(X, Y);
            Field_[X, Y] = Players[Convert.ToInt32(currentPlayer)].Symbol;

            currentPlayer = !currentPlayer;
        }
    }
}
