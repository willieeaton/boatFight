using System;
using System.Collections.Generic;
using System.Transactions;

namespace boatFight
{
    public class Ship
    {
        private readonly List<Point> _shipPoints = new List<Point>();

        private readonly int _shipLength;
        private int _shipHealth;
        private readonly int _xDirection;
        private readonly int _yDirection;
        public readonly string ShipDesignation;

        public bool IsAlive() => _shipHealth > 0;

        public Ship(int x, int y, Player player)
        {
            var ShipLocation = player.GameBoard.LocatePoint(x, y);
            ShipLocation.HasBoat = true;

            _shipLength = 1;
            _shipHealth = _shipLength;
            _xDirection = 1;
            _yDirection = 0;

            for (int i = 0; i < _shipLength; i++)
            {
                _shipPoints.Add(new Point(x, y));
            }
        }

        public Ship(int x, int y, Player player, int xDirection, int yDirection, int shipLength, string shipDesignation)
        {
            _shipLength = shipLength;
            _shipHealth = _shipLength;
            _xDirection = xDirection;
            _yDirection = yDirection;
            ShipDesignation = shipDesignation;

            for (int i = 0; i < _shipLength; i++)
            {
                Point newPoint = player.GameBoard.LocatePoint(x + i * xDirection, y + i * yDirection);
                newPoint.HasBoat = true;
                _shipPoints.Add(newPoint);
            }
        }

        public int ShipHealth()
        {


            return 0;
        }
    }
}
