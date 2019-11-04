using System;
using System.Collections.Generic;
namespace boatFight
{
    public class Board
    {
        private List<Point> _points = new List<Point>();

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
        }
    }
}
