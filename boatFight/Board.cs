using System;
using System.Collections.Generic;
using System.Linq;

namespace boatFight
{
    public class Board
    {
        private List<Point> _points = new List<Point>();

        public int BoardSize { get; set; }

        public bool CellExists (int x, int y)
        {
            var pointQuery =
                from pt in _points
                where (pt.X == x) && (pt.Y == y)
                select pt;

            return pointQuery.ToArray<Point>().Length > 0;
        }

        public Point LocatePoint(int x, int y)
        {
            var pointQuery =
                from pt in _points
                where (pt.X == x) && (pt.Y == y)
                select pt;

            foreach (Point pt in pointQuery)
            {
                return pt;
            }

            return new Point(0, 0);
    }

        public void ShotMapDisplay()
        {
            //display the board, with the following rules:
            //coordinates
            //'.' for unshot-at location
            //'*' for hits
            //'o' for misses
            //this is subject to change but sounds good in my head

            /* alike so:
             *   A B C D E
             * 1|. . . . .
             * 2|* * . . o
             * 3|o o . . .
             * 4|. . o . *
             * 5|o . . . *
             * */            
        }

        public Board(int boardSize)
        {
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    _points.Add(new Point(i, j));

            BoardSize = boardSize;
        }
    }
}
