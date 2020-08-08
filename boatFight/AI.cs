using System;
using System.Collections.Generic;
using System.Text;

namespace boatFight
{
    public class AI : Player
    {
        private readonly int _difficultyLevel; //1 is full random
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

                default:
                    break;
            }
            
            newName = nameOptions[(int)Math.Floor(nameOptions.Count * _rand.NextDouble())];

            return newName;
        }

        public override void PlaceShip()
        {
            Point ShipLocation = new Point((int)Math.Floor(GameBoard.BoardSize * _rand.NextDouble()), (int)Math.Floor(GameBoard.BoardSize * _rand.NextDouble()) );
            CreateShip(ShipLocation);
        }

        public override bool Fire(List<Player> players)
        {
            Point shotLocation = new Point(-1, -1);
            Player opponent = players[OtherPlayerIndex()];

            var validInput = false;
            do
            {
                shotLocation = AIShot(players);  
                validInput = opponent.GameBoard.CellExists(shotLocation);
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

        private Point AIShot(List<Player> players)
        {
            Player opponent = players[OtherPlayerIndex()];
            int targetX;
            int targetY;

            if (_difficultyLevel == 1)
            {
                targetX = (int)Math.Floor(_rand.NextDouble() * opponent.GameBoard.BoardSize);
                targetY = (int)Math.Floor(_rand.NextDouble() * opponent.GameBoard.BoardSize);
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
