using System;
using System.Collections.Generic;

namespace boatFight
{
    public class Ship
    {
        private readonly List<Point> _shipPoints = new List<Point>();

        private readonly int _shipLength = 1;
        private int _shipHealth;

        public bool IsAlive() => _shipHealth > 0;

        public Ship(int x, int y, Player player)
        {
            var ShipLocation = player.GameBoard.LocatePoint(x, y);
            ShipLocation.HasBoat = true;

            _shipHealth = _shipLength;

            for (int i = 0; i < _shipLength; i++)
            {
                _shipPoints.Add(new Point(x, y));
            }
        }
    }
}
