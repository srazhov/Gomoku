/*
    Код написан Сражовым Мирасом для задания EPAM .Net Training
    09/09/2020
*/

using System;
using System.Drawing;

namespace Gomoku
{
    //Класс для хранения успешно построенных линии матрицы

    public class Line
    {
        public enum LineTypes { horizontal, vertical, diagonalLeft, diagonalRight }

        private Point start;
        private Point end;
        private int StartNumb { get { if (LineType == LineTypes.horizontal) return Start.Y; return Start.X; } }
        private int EndNumb { get { if (LineType == LineTypes.horizontal) return End.Y; return End.X; } }

        public Point Start { get { return start; } }
        public Point End { get { return end; } }

        public int Lenght { get { return Math.Abs(EndNumb - StartNumb) + 1; ; } }
        public LineTypes LineType { get; }

        public Line(Point start, Point end)
        {
            //Исключение если начало и конец отрезка находятся в одной точке
            if (start == end)
                throw new ArgumentException();

            this.start = start;
            this.end = end;

            //Определение типа отрезка
            if (Start.X == End.X)
                LineType = LineTypes.horizontal;
            else if (Start.Y == End.Y)
                LineType = LineTypes.vertical;
            else
                LineType = LineTypes.diagonalRight;

            //Определить где должно быть начало и конец
            if (StartNumb > EndNumb)
            {
                Point temp = start;
                this.start = this.end;
                this.end = temp;
            }

            if(LineType == LineTypes.diagonalRight)
                for(int i = -Lenght; i <= Lenght; i++)
                    if (Start.X + i == End.X && Start.Y + i == End.Y)
                    {
                        LineType = LineTypes.diagonalLeft;
                        break;
                    }
        }

        //Проверяет, возможно ли присоединение точки к отрезку, сравниваются разные типы и если можно, добавляет точку в конец или начало
        public bool JointPoint(Point target)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (GetCondition(x, y, true, false))
                    {
                        if (Start == new Point(target.X + x, target.Y + y))
                        {
                            start = target;
                            return true;
                        }
                    }
                    if (GetCondition(x, y, false, false))
                    {
                        if (End == new Point(target.X + x, target.Y + y))
                        {
                            end = target;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //Соединяет два отрезка, если возможно, затем определяет новые Start и End
        public bool JointLine(Line line2)
        {
            if (LineType != line2.LineType || this == line2)
                return false;

            for (int i = -1; i <= 1; i++)
                for (int k = -1; k <= 1; k++)
                {
                    if (GetCondition(i, k, true, true))
                    {
                        Point temp1 = new Point(Start.X + i, Start.Y + k);
                        Point temp2 = new Point(End.X + i, End.Y + k);
                        if (line2.Start == temp1 || line2.End == temp1
                            || line2.Start == temp2 || line2.End == temp2)
                        {
                            if (StartNumb > line2.StartNumb)
                                start = line2.Start;
                            if (EndNumb < line2.EndNumb)
                                end = line2.End;
                            return true;
                        }
                    }
                }
            return false;
        }
        //Пояснение: у разных типов разные условия, при которых возможно соединение точки с отрезком
        //Кроме общих точек, различные условии есть и у начальных и конечных точек
        private bool GetCondition(int x, int y, bool Start, bool Single)
        {
            switch (LineType)
            {
                //Начальное_Условие && Исключение && (Если_не_нужно_разделять || Условие_для_Старта_И_Конца)
                case LineTypes.vertical:
                    return y == 0 && x != 0 && (Single || (Start ? x != -1 : x != 1));
                case LineTypes.horizontal:
                    return x == 0 && y != 0 && (Single || (Start ? y != -1 : y != 1));
                case LineTypes.diagonalLeft:
                    return x == y && x != 0 && (Single || (Start ? y != -1 : y != 1));
                case LineTypes.diagonalRight:
                    return x != y && x != 0 && y != 0 && (Single || (Start ? y != 1 : y != -1));
                default:
                    return false;
            }
        }
    }
}
