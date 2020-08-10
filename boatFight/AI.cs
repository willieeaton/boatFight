using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace boatFight
{
    public class AI : Player
    {
        private readonly int _difficultyLevel; //1 is full random

        private struct Knowledge
        {
            bool onTheHunt;
            bool hitLastShot;
            Point lastShotLocation;
            Point lastHitLocation;
            int checkerboardOffset;
        }
        
        Random _rand = new Random();



        public AI(int playerNumber, int difficultyLevel) : base(playerNumber)
        {
            _difficultyLevel = difficultyLevel;
            PlayerName = nameMe();
        }

        private string nameMe()
        {
            var newName = "";

            List<String> nameOptions = new List<string>();

            switch(_difficultyLevel)
            {
                case 1:
                    nameOptions.AddRange(new List<string>()
                    {
                        "Rando McGuesser", "Wildcard", "Mystery Man"
                    });
                    break;

                case 2:
                    nameOptions.AddRange(new List<string>()
                    {
                        "Lt. Crackshot", "Tracker", "Strafeman"
                    });
                    break;

                case 3:
                    nameOptions.AddRange(new List<string>()
                    {
                        "Admiral Awesome", "Commodore Soixante-Quatre", "Ole Ironsights"
                    });
                    break;

                default:
                    break;
            }
            
            newName = nameOptions[(int)Math.Floor(nameOptions.Count * _rand.NextDouble())];

            return newName;
        }
        public override void PlaceShip(int shipLength, string shipDesignation)
        {
            Point ShipLocation;
            int xDirection;
            int yDirection;
            Console.Clear();
            var validInput = false;
            do
            {
                ShipLocation = new Point(_rand.Next(0, GameBoard.BoardSize), _rand.Next(0, GameBoard.BoardSize));
                (xDirection, yDirection) = EnterDirection(shipLength, ShipLocation);
                if (Program.IsValidDirection(xDirection, yDirection))
                {
                    validInput = true;
                }
            } while (!validInput);
            CreateShip(ShipLocation, xDirection, yDirection, shipLength, shipDesignation);
        }

        protected override (int, int) EnterDirection(int shipLength, Point startLocation)
        {
            (int, int) attemptedDirection = (0, 0);
            int inputDirection;
            if (shipLength > 1)
            {
                inputDirection = _rand.Next(1, 4);
                switch (inputDirection)
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
                switch (result)
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

        protected override void InvalidPointReached()
        {
            Debug.WriteLine("Hey, I tried to place a ship offstage, sorry");
        }

        protected override void OverlappingShipReached()
        {
            Debug.WriteLine("Dang, my ships overlapped.");
        }

        public override bool Fire(List<Player> players)
        {
            Point shotLocation = new Point(-1, -1);
            Player opponent = players[OtherPlayerIndex()];

            var validInput = false;
            do
            {
                shotLocation = AIShot(players);
                shotLocation = opponent.GameBoard.LocatePoint(shotLocation);
                if (!opponent.GameBoard.CellExists(shotLocation))
                {
                    InvalidShotLocation();
                }
                else if (shotLocation.HasBeenShot)
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

            if (theShotHit)
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

        protected override void InvalidShotLocation()
        {
            Debug.WriteLine("WARNING: Attempted to shoot invalid cell.");
        }

        protected override void PointAlreadyShot()
        {
            Debug.WriteLine("Shot at a location that has already been shot at.");
        }

        private Point AIShot(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];
            int targetX;
            int targetY;

            if (_difficultyLevel == 1)
            {
                //Difficulty level 1: simply fires willy-nilly, at random.
                targetX = _rand.Next(0, opponent.GameBoard.BoardSize);
                targetY = _rand.Next(0, opponent.GameBoard.BoardSize);
                Debug.WriteLine($"Shot at {targetX}, {targetY} aka {Point.PointToAlphanumeric(targetX, targetY)}");
            }
            else if (_difficultyLevel == 2)
            {
                //Difficulty level 2: fires willy-nilly until it hits something, then attempts
                //to follow that ship and sink her.
                targetX = _rand.Next(0, opponent.GameBoard.BoardSize);
                targetY = _rand.Next(0, opponent.GameBoard.BoardSize);
                Debug.WriteLine($"Shot at {targetX}, {targetY} aka {Point.PointToAlphanumeric(targetX, targetY)}");
            }
            else if (_difficultyLevel == 3)
            {
                //Difficulty level 3: fires randomly at a "checkerboard" pattern until it hits
                //something, then attempts to follow that ship and sink her.
                targetX = _rand.Next(0, opponent.GameBoard.BoardSize);
                targetY = _rand.Next(0, opponent.GameBoard.BoardSize);
                Debug.WriteLine($"Shot at {targetX}, {targetY} aka {Point.PointToAlphanumeric(targetX, targetY)}");
            }
            else
            {
                targetX = -1;
                targetY = -1;
            }

            return new Point(targetX, targetY);
        }
    }

}
