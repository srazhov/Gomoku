/*
    Код написан Сражовым Мирасом для задания EPAM .Net Training
    12/09/2020
*/

using System.Drawing;
using System.Collections.Generic;
using System;

namespace Gomoku
{
    class AI : GomokuPlayer
    {
        public AI(char symbol, int size)
        {
            Symbol = symbol;
            MaxSize = size;
        }

        public override int[] ChooseMove(GomokuPlayer Opponent)
        {
            List<Point> excepts = new List<Point>();
            excepts.AddRange(Occupation_);
            excepts.AddRange(Opponent.Occupation_);

            Point result = new Point();
            Random rand = new Random();
            int MostDangerous = 0;

            while (excepts.Contains(result))
                result = new Point(rand.Next(0, MaxSize - 1), rand.Next(0, MaxSize - 1));

            GetBest(Lines.ToArray());
            GetBest(Opponent.Lines.ToArray());

            if(MostDangerous == 0)
            {
                foreach (var poi in Occupation_)
                {
                    Point temp = new Point(poi.X + rand.Next(-1, 1), poi.Y + rand.Next(-1, 1));
                    if (CanBePointed(temp))
                    {
                        result = temp;
                        break;
                    }
                }
            }

            return new int[] { result.X, result.Y };


            //Нахождение самого опасного в данный момент отрезка
            void GetBest(Line[] linesArray)
            {
                foreach(Line line in linesArray)
                {
                    if(line.Lenght >= MostDangerous)
                    {
                        Point point1 = line.GetNext(true);
                        if (!CanBePointed(point1))
                        {
                            point1 = line.GetNext(false);
                            if (CanBePointed(point1))
                            {
                                result = point1;
                                MostDangerous = line.Lenght;
                            }
                        }
                        else
                        {
                            result = point1;
                            MostDangerous = line.Lenght;
                        }
                    }
                }
            }

            bool CanBePointed(Point point1)
            {
                return !excepts.Contains(point1) && point1.X >= 0 && point1.Y >= 0 
                    && point1.X < MaxSize && point1.Y < MaxSize;
            }
        }
    }
}
