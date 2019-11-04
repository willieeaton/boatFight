using System;
namespace boatFight
{
    public class Ship
    {
        private readonly int _locationX;
        private readonly int _locationY;

        public Ship(int x, int y, Player player)
        {
            _locationX = x;
            _locationY = y;

            var ShipLocation = player.GameBoard.LocatePoint(x, y);
            ShipLocation.HasBoat = true;
        }
    }
}
