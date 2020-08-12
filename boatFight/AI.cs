using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace boatFight
{
    public class AI : Player
    {
        private readonly int _difficultyLevel; //1 is full random

        public struct Knowledge
        {
            public bool onTheHunt; //true if actively pursuing a ship, until it is sunk
            public bool hitLastShot;
            public Point lastShotLocation;
            public List<Point> lastHitLocations;
            public int checkerboardOffset;
        }

        Knowledge knowledge = new Knowledge()
        {
            onTheHunt = false,
            hitLastShot = false,
            lastHitLocations = new List<Point>()
        };
        
        Random _rand = new Random();



        public AI(int playerNumber, int difficultyLevel) : base(playerNumber)
        {
            _difficultyLevel = difficultyLevel;
            PlayerName = nameMe();
            knowledge.checkerboardOffset = _rand.Next(0, 2);
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

        public override bool Fire(List<Player> players, int shotNumber, int numberOfShots)
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
            knowledge.lastShotLocation = targetPoint;
            var theShotHit = targetPoint.GetShot();

            if (theShotHit)
            {
                ProcessHit(targetPoint, players);
            }
            else
            {
                Console.WriteLine("Drat, it missed.");
            }

            if(opponent.AllShipsSunk())
            {
                Console.WriteLine($"All ships sunk!  {PlayerName} wins!");
            }

            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
            Console.Clear();

            return opponent.AllShipsSunk();
        }

        protected override void InvalidShotLocation()
        {
            Debug.WriteLine("WARNING: Attempted to shoot invalid cell.");
        }

        protected override void PointAlreadyShot()
        {
            Debug.WriteLine("Shot at a location that has already been shot at.");
        }

        protected override void ProcessHit(Point targetPoint, List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];

            knowledge.onTheHunt = true;
            knowledge.hitLastShot = true;
            knowledge.lastHitLocations.Add(targetPoint);

            Console.WriteLine("BOOM!!!  Hit!!!");

            if (!targetPoint.BoatHere.IsAlive())
            {
                Console.WriteLine($"{PlayerName} sunk the {targetPoint.BoatHere.ShipDesignation}!");
                knowledge.onTheHunt = false;
                knowledge.lastHitLocations.RemoveAll(p => p.HasBeenShot);
            }
        }

        private Point AIShot(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];
            int targetX;
            int targetY;

            if (_difficultyLevel == 1)
            {
                //Difficulty level 1: simply fires willy-nilly, at random.
                (targetX, targetY) = ShootRandom(players);
            }
            else if (_difficultyLevel == 2)
            {
                //Difficulty level 2: fires willy-nilly until it hits something, then attempts
                //to follow that ship and sink her.
                if (knowledge.onTheHunt)
                {
                    (targetX, targetY) = ProwlForShips(players);
                }
                else
                {
                    (targetX, targetY) = ShootRandom(players);
                }
            }
            else if (_difficultyLevel == 3)
            {
                //Difficulty level 3: fires randomly at a "checkerboard" pattern until it hits
                //something, then attempts to follow that ship and sink her.
                if (knowledge.onTheHunt)
                {
                    (targetX, targetY) = ProwlForShips(players);
                }
                else
                {
                    (targetX, targetY) = ShootCheckerboard(players);
                }
            }
            else
            {
                targetX = -1;
                targetY = -1;
            }

            Debug.WriteLine($"Shot at {targetX}, {targetY} aka {Point.PointToAlphanumeric(targetX, targetY)}");

            return new Point(targetX, targetY);
        }

        private (int, int) ShootRandom(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];

            return (_rand.Next(0, opponent.GameBoard.BoardSize), _rand.Next(0, opponent.GameBoard.BoardSize));
        }

        private (int, int) ShootCheckerboard(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];
            int targetX;
            int targetY;

            do
            {
                (targetX, targetY) = ShootRandom(players);
            } while ((targetX + targetY) % 2 != knowledge.checkerboardOffset);

            return (targetX, targetY);
        }

        private (int, int) ProwlForShips(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];
            var candidatePoints = new List<Point>();
            int targetX;
            int targetY;
            
            if(knowledge.lastHitLocations.Count == 1)
            {
                candidatePoints.Add(new Point(knowledge.lastHitLocations[0].X - 1, knowledge.lastHitLocations[0].Y));
                candidatePoints.Add(new Point(knowledge.lastHitLocations[0].X + 1, knowledge.lastHitLocations[0].Y));
                candidatePoints.Add(new Point(knowledge.lastHitLocations[0].X, knowledge.lastHitLocations[0].Y - 1));
                candidatePoints.Add(new Point(knowledge.lastHitLocations[0].X, knowledge.lastHitLocations[0].Y + 1));

                candidatePoints.RemoveAll(p => p.X < 0 || p.Y < 0 || p.X >= opponent.GameBoard.BoardSize || p.Y >= opponent.GameBoard.BoardSize);

                for(int i = 0; i < candidatePoints.Count; i++)
                {
                    candidatePoints[i] = opponent.GameBoard.LocatePoint(candidatePoints[i]);
                }

                candidatePoints.RemoveAll(p => p.HasBeenShot);
            }
            else
            {
                int xDirection = knowledge.lastHitLocations[0].X - knowledge.lastHitLocations[1].X;

                if (xDirection == 0)
                {
                    targetX = knowledge.lastHitLocations[0].X;
                    for (int i = 0; i < opponent.GameBoard.BoardSize; i++)
                    {
                        Point testPoint = opponent.GameBoard.LocatePoint(targetX, i);
                        if (!testPoint.HasBeenShot)
                        {
                            candidatePoints.Add(testPoint);
                        }
                    }
                }
                else
                {
                    targetY = knowledge.lastHitLocations[0].Y;
                    for (int i = 0; i < opponent.GameBoard.BoardSize; i++)
                    {
                        Point testPoint = opponent.GameBoard.LocatePoint(i, targetY);
                        if (!testPoint.HasBeenShot)
                        {
                            candidatePoints.Add(testPoint);
                        }
                    }
                }
            }
            (targetX, targetY) = GetNearestTarget(candidatePoints);

            return (targetX, targetY);
        }

        private (int, int) GetNearestTarget(List<Point> candidatePoints)
        {
            var nearestDistance = 999;
            foreach(Point p in candidatePoints)
            {
                var distance = Math.Abs(knowledge.lastHitLocations[0].X - p.X + knowledge.lastHitLocations[0].Y - p.Y);
                nearestDistance = Math.Min(nearestDistance, distance);
            }

            candidatePoints.RemoveAll(p => Math.Abs(knowledge.lastHitLocations[0].X - p.X + knowledge.lastHitLocations[0].Y - p.Y) > nearestDistance);  

            while(candidatePoints.Count > 1) //pick a random point from equals
            {
                candidatePoints.RemoveAt(_rand.Next(0, candidatePoints.Count));
            }

            if (candidatePoints.Count == 1)
            {
                return (candidatePoints[0].X, candidatePoints[0].Y);
            }
            else //if it gets confused, stop hunting and return (0, 0)
            {
                knowledge.onTheHunt = false;
                knowledge.lastHitLocations.RemoveAll(p => p.HasBeenShot);
                return (0, 0);
            }
        }
    }

}
