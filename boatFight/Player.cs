using System;
namespace boatFight
{
    public class Player
    {
        private readonly int _playerNumber;
        private Ship _ships;
        public Board GameBoard;

        public int PlayerNumber => _playerNumber;

        public string PlayerName { get; set; }

        public void CreateShip(int x, int y)
        {
            _ships = new Ship(x, y);
        }

        public int OtherPlayerNumber() => (_playerNumber == 1) ? 2 : 1;

        public bool Fire()
        {
            GameBoard.ShotMapDisplay();

            Console.Write($"{PlayerName}, enter X coordinate to shoot at. ");
            int shotX = int.Parse(Console.ReadLine());
            Console.Write($"{PlayerName}, enter Y coordinate to shoot at. ");
            int shotY = int.Parse(Console.ReadLine());

            //actually shoot the shot
            //if hit, redisplay board
            //press any key to continue
            //clearscreen

            return true; //if game is over
        }

        public Player(int playerNumber, int boardSize)
        {
            _playerNumber = playerNumber;
            GameBoard = new Board(boardSize);
        }


    }
}
