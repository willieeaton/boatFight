using System;
using System.Collections.Generic;
namespace boatFight
{
    public class Board
    {
        private const int _boardSize = 5;
        private List<Point> _points = new List<Point>();


        public Board()
        {
            for (int i = 0; i < _boardSize; i++)
                for (int j = 0; j < _boardSize; j++)
                    _points.Add(new Point(i, j));
        }
    }
}
