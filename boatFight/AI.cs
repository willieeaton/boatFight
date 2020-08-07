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
    }
}
