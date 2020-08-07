using System;
using System.Collections.Generic;

namespace boatFight
{
    public class Player
    {
        protected readonly int _playerNumber;
        protected Ship _ships;
        public Board GameBoard;

        public Player(int playerNumber)
        {
            _playerNumber = playerNumber;
            GameBoard = new Board(Program.BoardSize);
        }


        public int PlayerNumber => _playerNumber;

        public string PlayerName { get; set; }

        public virtual void PlaceShip()
        {
            Console.Clear();
            Point ShipLocation = Point.InputCoordinates($"{PlayerName}, please place your boat, {Point.PointToAlphanumeric(0, 0)} to {Point.PointToAlphanumeric(GameBoard.BoardSize - 1, GameBoard.BoardSize - 1)}. ", GameBoard);
            Console.WriteLine($"Placing ship at {Point.PointToAlphanumeric(ShipLocation)}.  Press key to continue.");
            Console.ReadKey();
            Console.Clear();
            CreateShip(ShipLocation);
        }

        public void CreateShip(int x, int y)
        {
            _ships = new Ship(x, y, this);
        }

        public void CreateShip(Point location)
        {
            CreateShip(location.X, location.Y);
        }

        public int OtherPlayerNumber() => (_playerNumber == 1) ? 2 : 1;

        public int OtherPlayerIndex() => (_playerNumber == 1) ? 1 : 0;

        public bool Fire(List<Player> players)
        {
            Point shotLocation = new Point(-1, -1);
            Player opponent = players[OtherPlayerIndex()];

            GameBoard.ShipMapDisplay(GameBoard);
            GameBoard.ShotMapDisplay(opponent.GameBoard);
            

            var validInput = false;
            do
            {
                shotLocation = Point.InputCoordinates($"{PlayerName}, enter coordinates to shoot at. ", opponent.GameBoard);
                validInput = opponent.GameBoard.CellExists(shotLocation);
            } while (validInput == false);

            var targetPoint = opponent.GameBoard.LocatePoint(shotLocation);
            var theShotHit = targetPoint.GetShot();

            if(theShotHit)
            {
                Console.WriteLine("BOOM!!!  Hit!!!");
                GameBoard.ShotMapDisplay(opponent.GameBoard);
            }
            else
            {
                Console.WriteLine("Drat, it missed.");
            }

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
            Console.Clear();

            return theShotHit; //placeholder.  return True if game is over; false otherwise.
        }

    }
}
