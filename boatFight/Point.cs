using System;
namespace boatFight
{
    public class Point
    {
        public int X;
        public int Y;
        public bool HasBoat { get; set; } = false;
        public Ship BoatHere { get; set; }
        public bool HasBeenShot { get; set; } = false;

        public static Point AlphanumericToPoint(string coordinates)
        {
            var yPreParse = coordinates.ToUpper()[0];
            var xPreParse = coordinates.Substring(1);
            int x = -1;
            if (int.TryParse(xPreParse, out x))
            {
                x--;
            }
            int y = -1;
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                                'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                                'V', 'W', 'X', 'Y', 'Z' }; 

            for(int i = 0; i < alphabet.Length; i++)
            {
                if(yPreParse == alphabet[i])
                {
                    y = i;
                }
            }

            if (x > -1 && y > -1)
            {
                return new Point(x, y);
            }
            else
            {
                return new Point(-1, -1);
            }
        }

        public static string PointToAlphanumeric(Point point)
        {
            string x = (point.X + 1).ToString(); 
            string y = "";
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                                'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                                'V', 'W', 'X', 'Y', 'Z' };
            for (int i = 0; i < alphabet.Length; i++)
            {
                if(point.Y == i)
                {
                    y = alphabet[i].ToString();
                }
            }

            string coordinates = y + x;
            return coordinates;
        }

        public static Point InputCoordinates(string prompt)
        {
            var inputIsValid = false;
            var shipLocation = new Point(-1, -1);
            while (!inputIsValid)
            {
                Console.Write(prompt + " ");
                shipLocation = AlphanumericToPoint(Console.ReadLine());
                inputIsValid |= shipLocation.X > -1 && shipLocation.Y > -1;
            }

            return shipLocation;
        }

        public static Point InputCoordinates(string prompt, Board board)
        {
            var inputIsValid = false;
            var shipLocation = new Point(-1, -1);
            while (!inputIsValid)
            {
                shipLocation = InputCoordinates(prompt);
                inputIsValid |= shipLocation.X < board.BoardSize && shipLocation.Y < board.BoardSize;
            }

            return shipLocation;
        }


        public static string PointToAlphanumeric(int x, int y)
        {
            return PointToAlphanumeric(new Point(x, y));
        }

        public bool GetShot()
        {
            HasBeenShot = true;
            return HasBoat;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
    }
}
