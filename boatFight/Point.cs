using System;
namespace boatFight
{
    public class Point
    {
        public int X;
        public int Y;
        public bool HasBoat { get; set; } = false;
        public bool HasBeenShot { get; set; } = false;

        public static Point AlphanumericToPoint(string coordinates)
        {
            var yPreParse = coordinates.ToUpper()[0];
            var xPreParse = coordinates.Substring(1);
            var x = int.Parse(xPreParse) - 1;
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

            return new Point(x, y);
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
