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
            Point ShipLocation;
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

        protected virtual (int, int) EnterDirection(int shipLength, Point startLocation)
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
                    if(int.TryParse(Console.ReadLine(), out inputDirection))
                    {
                        if (inputDirection >= 1 && inputDirection <= 4)
                        {
                            validInput = true;
                        }
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

        protected virtual void InvalidPointReached()
        {
            Console.WriteLine("This ship would extend off of the board!");
        }

        protected virtual void OverlappingShipReached()
        {
            Console.WriteLine("This ship would overlap a ship already placed!");
        }

        public void CreateShip(Point shipLocation, int xDirection, int yDirection, int shipLength, string shipDesignation)
        {
            _ships.Add(new Ship(shipLocation.X, shipLocation.Y, this, xDirection, yDirection, shipLength, shipDesignation));
        }
        public int OtherPlayerNumber() => (_playerNumber == 1) ? 2 : 1;

        public int OtherPlayerIndex() => (_playerNumber == 1) ? 1 : 0;

        public virtual bool Fire(List<Player> players, int shotNumber, int numberOfShots)
        {
            Point shotLocation = new Point(-1, -1);
            Player opponent = players[OtherPlayerIndex()];

            GameBoard.ShipMapDisplay(GameBoard);
            GameBoard.ShotMapDisplay(opponent.GameBoard);

            var validInput = false;
            do
            {
                var promptString = numberOfShots == 1 ? "enter coordinates to shoot at." : $"enter coordinates for shot {shotNumber} out of {numberOfShots}.";
                shotLocation = Point.InputCoordinates($"{PlayerName}, {promptString} ", opponent.GameBoard);
                shotLocation = opponent.GameBoard.LocatePoint(shotLocation);
                if (!opponent.GameBoard.CellExists(shotLocation))
                {
                    InvalidShotLocation();
                }
                else if(shotLocation.HasBeenShot)
                {
                    PointAlreadyShot();
                }
                else
                {
                    validInput = true;
                }
            } while (validInput == false);

            var targetPoint = opponent.GameBoard.LocatePoint(shotLocation);
            var theShotHit = targetPoint.GetShot();

            if(theShotHit)
            {
                ProcessHit(targetPoint, players);
            }
            else
            {
                Console.WriteLine("Missed...");
            }

            if (opponent.AllShipsSunk())
            {
                Console.WriteLine($"All ships sunk!  {PlayerName} wins!");
            }

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
            Console.Clear();

            return opponent.AllShipsSunk(); 
        }

        protected virtual void InvalidShotLocation ()
        {
            Console.WriteLine("This location would be offboard.");
        }

        protected virtual void PointAlreadyShot ()
        {
            Console.WriteLine("This location has already been shot at!");
        }

        protected virtual void ProcessHit(Point targetPoint, List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];

            Console.WriteLine("BOOM!!!  Hit!!!");

            if(!targetPoint.BoatHere.IsAlive())
            {
                Console.WriteLine($"You sunk the {targetPoint.BoatHere.ShipDesignation}!");
            }
        }

        public int RemainingShips ()
        {
            int shipsLeft = 0;
            foreach(Ship s in _ships)
            {
                if (s.IsAlive())
                {
                    shipsLeft++;
                }
            }

            return shipsLeft;
        }

        public bool AllShipsSunk ()
        {
            if (RemainingShips() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
