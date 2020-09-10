/*
    Код написан Сражовым Мирасом для задания EPAM .Net Training
    31/08/2020
*/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace Gomoku
{
    public class GomokuPlayer
    {
        public readonly List<Point> Occupation_;
        public readonly List<Line> Lines;

        public char Symbol { get; }

        public GomokuPlayer(char symbol)
        {
            Symbol = symbol;
            Occupation_ = new List<Point>();
            Lines = new List<Line>();
        }

        //Выбрасывает исключение если точка уже существует
        //Добавляет в список Point координаты новой точки
        //Начинает проверять соседние клетки на наличие уже зарегистрированных точек
        //Если такова есть, то создает новый (Или изменяет) отрезок
        public void MakeMove(int X, int Y)
        {
            Point Current = new Point(X, Y);
            if (Occupation_.Contains(Current))
                throw new ArgumentException();
            Occupation_.Add(Current);

            //Парсинг ближайших элементов новой точки
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if ((x == 0 && y == 0))
                        continue;

                    Point temp = new Point(Current.X + x, Current.Y + y);
                    if (Occupation_.Contains(temp))
                    {
                        //Создает новый отрезок (Новая точка, ближайшая точка)
                        Line TempLine = new Line(temp, Current);

                        for (int i = 0; i < Lines.Count; i++)
                        {
                            if (Lines[i].JointPoint(Current))
                            {
                                List<Line> ToDelete = new List<Line>();
                                foreach (var line in Lines)
                                    if (Lines[i].JointLine(line))
                                        ToDelete.Add(line);

                                foreach (var deleteds in ToDelete)
                                    Lines.Remove(deleteds);
                            }
                            if (TempLine != null && Lines[i].JointLine(TempLine))
                                TempLine = null;
                        }
                        if (TempLine != null)
                            Lines.Add(TempLine);
                    }
                }
        }
        public bool DidWin()
        {
            foreach (var Line in Lines)
                if (Line.Lenght == 5)
                    return true;
            return false;
        }
    }
}