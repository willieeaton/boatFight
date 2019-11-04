using System;
namespace boatFight
{
    public class Point
    {
        private int _x;
        private int _y;
        public bool HasBoat = false;
        public bool HasBeenShot = false;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
