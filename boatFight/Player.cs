using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace boatFight
{
    public class Player
    {
        protected readonly int _playerNumber;
        protected List<Ship> _ships = new List<Ship>();
        public Board GameBoard;

        public Player(int playerNumber)
        {
            _playerNumber = playerNumber;
            GameBoard = new Board(Program.BoardSize);
        }


        public int PlayerNumber => _playerNumber;

        public string PlayerName { get; set; }

        public virtual void PlaceShip(int shipLength, string shipDesignation)
        {
            Point ShipLocation = new Point(0, 0);
            int xDirection;
            int yDirection; 
            Console.Clear();
            var validInput = false;
            do
            {
                GameBoard.ShipMapDisplay(GameBoard);
                ShipLocation = Point.InputCoordinates($"{PlayerName}, please place your {shipDesignation} (length {shipLength}), {Point.PointToAlphanumeric(0, 0)} to {Point.PointToAlphanumeric(GameBoard.BoardSize - 1, GameBoard.BoardSize - 1)}. ", GameBoard);
                Console.WriteLine($"Placing ship at {Point.PointToAlphanumeric(ShipLocation)}.  Press key to continue.");
                (xDirection, yDirection) = EnterDirection(shipLength, ShipLocation);
                if (Program.IsValidDirection(xDirection, yDirection))
                {
                    validInput = true;
                }
            } while (!validInput);

            Console.WriteLine("Press key to continue.");
            Console.ReadKey();
            Console.Clear();
            CreateShip(ShipLocation, xDirection, yDirection, shipLength, shipDesignation);
        }

        (int, int) EnterDirection(int shipLength, Point startLocation)
        {
            (int, int) attemptedDirection = (0, 0);
            int inputDirection;
            if (shipLength > 1)
            {
                Console.WriteLine("Which direction would you like the ship to face from this point?");
                Console.WriteLine("1) Right");
                Console.WriteLine("2) Down");
                Console.WriteLine("3) Left");
                Console.WriteLine("4) Up");
                Console.Write("> ");
                var validInput = false;
                do
                {
                    inputDirection = int.Parse(Console.ReadLine());
                    if (inputDirection >= 1 && inputDirection <= 4)
                    {
                        validInput = true;
                    }
                } while (!validInput);
                switch(inputDirection)
                {
                    case 1:
                        attemptedDirection = (1, 0);
                        break;

                    case 2:
                        attemptedDirection = (0, 1);
                        break;

                    case 3:
                        attemptedDirection = (-1, 0);
                        break;

                    case 4:
                        attemptedDirection = (0, -1);
                        break;

                    default:
                        return (0, 0);
                }

                int result = GameBoard.CanAddBoat(startLocation.X, startLocation.Y, this, attemptedDirection.Item1, attemptedDirection.Item2, shipLength);
                switch(result)
                {
                    case 1:
                        return attemptedDirection;

                    case -1:
                        InvalidPointReached();
                        return (0, 0);

                    case -2:
                        OverlappingShipReached();
                        return (0, 0);

                    default:
                        Console.WriteLine("Unknown result code.  Attempting to place ship.");
                        return attemptedDirection;
                }
            }
            else
            {
                return (1, 0);
            }
        }

        public void InvalidPointReached()
        {
            Console.WriteLine("This ship would extend off of the board!");
        }

        public void OverlappingShipReached()
        {
            Console.WriteLine("This ship would overlap a ship already placed!");
        }

        public void CreateShip(Point shipLocation, int xDirection, int yDirection, int shipLength, string shipDesignation)
        {
            _ships.Add(new Ship(shipLocation.X, shipLocation.Y, this, xDirection, yDirection, shipLength, shipDesignation));
        }
        public int OtherPlayerNumber() => (_playerNumber == 1) ? 2 : 1;

        public int OtherPlayerIndex() => (_playerNumber == 1) ? 1 : 0;

        public virtual bool Fire(List<Player> players)
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
