﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace boatFight
{
    public class Board
    {
        private List<Point> _points = new List<Point>();

        public int BoardSize { get; set; }

        public bool CellExists(int x, int y)
        {
            var pointQuery =
                from pt in _points
                where (pt.X == x) && (pt.Y == y)
                select pt;

            return pointQuery.Any<Point>();
        }

        public bool CellExists(Point location)
        {
            return CellExists(location.X, location.Y);
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

        public Point LocatePoint(Point location)
        {
            return LocatePoint(location.X, location.Y);
        }

        public void ShotMapDisplay(Board board)
        {

            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                                'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                                'V', 'W', 'X', 'Y', 'Z' };

            Console.WriteLine();
            Console.WriteLine("Your shots:");
            Console.WriteLine();

            //generate top line

            Console.Write("  ");
            for (int i = 0; i < board.BoardSize; i++)
            {
                if (i < 9)
                {
                    Console.Write(' ');
                }

                Console.Write(i + 1);
            }
            Console.Write('\n');

            //generate dashy top line like +---------

            Console.Write("  +");
            for (int i = 0; i < board.BoardSize; i++)
            {
                Console.Write("--");
            }
            Console.Write('\n');

            //generate each line of actual content

            for (int i = 0; i < board.BoardSize; i++)
            {
                //space, letter, pipe
                Console.Write(" " + alphabet[i].ToString() + "|");

                for (int j = 0; j < board.BoardSize; j++)
                {
                    //content.  . for not-shot, * for hit, o for miss
                    //then a space
                    Point point = board.LocatePoint(j, i);
                    if (point.HasBoat && point.HasBeenShot)
                    {
                        Console.Write("X ");
                    }
                    else if (point.HasBeenShot)
                    {
                        Console.Write("o ");
                    }
                    else 
                    { 
                    Console.Write(". ");
                    }
                }
                Console.Write('\n');    
            }
        }

         public void ShipMapDisplay(Board board)
        {
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                                'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                                'V', 'W', 'X', 'Y', 'Z' };

            Console.WriteLine();
            Console.WriteLine("Your ships:");
            Console.WriteLine();
            //generate top line

            Console.Write("  ");
            for (int i = 0; i < board.BoardSize; i++)
            {
                if (i < 9)
                {
                    Console.Write(' ');
                }

                Console.Write(i + 1);
            }
            Console.Write('\n');

            //generate dashy top line like +---------

            Console.Write("  +");
            for (int i = 0; i < board.BoardSize; i++)
            {
                Console.Write("--");
            }
            Console.Write('\n');

            //generate each line of actual content

            for (int i = 0; i < board.BoardSize; i++)
            {
                //space, letter, pipe
                Console.Write(" " + alphabet[i].ToString() + "|");

                for (int j = 0; j < board.BoardSize; j++)
                {
                    //content.  . for empty, * for hit boat, o for healthy boat
                    //then a space
                    Point point = board.LocatePoint(j, i);
                    if (point.HasBoat && point.HasBeenShot)
                    {
                        Console.Write("X ");
                    }
                    else if (point.HasBoat)
                    {
                        Console.Write("o ");
                    }
                    else 
                    { 
                    Console.Write(". ");
                    }
                }
                Console.Write('\n');    
            }
        }

        /// <summary>
        /// Checks if a boat can be placed given its constructing parameters
        /// </summary>
        /// <param name="x">X coordinate of rear of boat</param>
        /// <param name="y">Y coordinate of rear of boat</param>
        /// <param name="player">Player placing the ship</param>
        /// <param name="xDirection">-1 for left, 1 for right, 0 for vertical</param>
        /// <param name="yDirection">-1 for up, 1 for down, 0 for horizontal</param>
        /// <param name="shipLength">Length of ship in points</param>
        /// <returns>-1 for invalid point, -2 for occupied, 1 for OK</returns>
        public int CanAddBoat(int x, int y, Player player, int xDirection, int yDirection, int shipLength)
        {
            var testPoints = new List<Point>();
            for (int i = 0; i < shipLength; i++)
            {
                testPoints.Add(new Point(x + xDirection * i, y + yDirection * i));
            }

            foreach (Point p in testPoints)
            {
                if (!CellExists(p.X, p.Y))
                {
                    return -1;
                }
                else
                {
                    Point testPoint = LocatePoint(p);
                    if (testPoint.HasBoat)
                    {
                        return -2;
                    }
                }
            }

            return 1;
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
