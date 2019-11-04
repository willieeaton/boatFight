using System;
namespace boatFight
{
    public class Point
    {
        public int X;
        public int Y;
        public bool HasBoat = false;
        public bool HasBeenShot = false;

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
    }
}
