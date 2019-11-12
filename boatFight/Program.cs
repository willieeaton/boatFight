using System;
using System.Collections.Generic;

namespace boatFight
{
    class Program
    {
        //magic number elimination station
        const int NumberOfPlayers = 2;
        public const int BoardSize = 5;

        static List<Player> players = new List<Player>();

        public static void CreatePlayers()
        {
            for (int i = 0; i<NumberOfPlayers; i++)
            {
                players.Add(new Player(i + 1, BoardSize));
            }

            for (int i = 0; i<NumberOfPlayers; i++)
            {
                Console.Write($"Player {players[i].PlayerNumber}, please enter your name. ");
                string playerName = Console.ReadLine();
                players[i].PlayerName = playerName;
            }
        }

        public static void PlaceShips()
        {
            foreach (Player player in players)
            {
                Console.Clear();
                Point ShipLocation = Point.InputCoordinates($"{player.PlayerName}, please place your boat, {Point.PointToAlphanumeric(0, 0)} to {Point.PointToAlphanumeric(player.GameBoard.BoardSize - 1, player.GameBoard.BoardSize - 1)}. ", player.GameBoard);
                Console.WriteLine($"Placing ship at {Point.PointToAlphanumeric(ShipLocation)}.  Press key to continue.");
                Console.ReadKey();
                Console.Clear();
                player.CreateShip(ShipLocation);
            }
        }

        private static void Main(string[] args)
        {
            CreatePlayers();
            PlaceShips();

            bool GameOver = false;
            do
            {
                foreach (Player player in players)
                {
                    GameOver = player.Fire(players);
                    if (GameOver)
                        break;
                }
            } while (!GameOver);
        }
    }
}
