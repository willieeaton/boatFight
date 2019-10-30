using System;
namespace boatFight
{
    public class Player
    {
        private readonly int _playerNumber;

        public int PlayerNumber => _playerNumber;

        public string PlayerName { get; set; }

        public void CreateShip(int x, int y)
        {
            //currently a dummy function
            //will create a new ship
            Console.WriteLine($"Creating ship at {x}, {y}.");
        }

        public Player(int playerNumber)
        {
            _playerNumber = playerNumber;
        }


    }
}
