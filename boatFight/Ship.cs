using System;
namespace boatFight
{
    public class Ship
    {
        private readonly int _locationX;
        private readonly int _locationY;

        public Ship(int x, int y)
        {
            _locationX = x;
            _locationY = y;
        }
    }
}
