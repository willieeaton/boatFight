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
                Console.Write($"{player.PlayerName}, please enter your X coordinate, from 1 to DUMMY. ");
                int coordinateX = int.Parse(Console.ReadLine());
                Console.Write($"{player.PlayerName}, please enter your Y coordinate, from 1 to DUMMY. ");
                int coordinateY = int.Parse(Console.ReadLine());
                Console.WriteLine($"Placing ship at {coordinateX}, {coordinateY}.  Press key to continue.");
                Console.ReadKey();
                Console.Clear();
                player.CreateShip(coordinateX, coordinateY);
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
                    GameOver = player.Fire();
                    if (GameOver)
                        break;
                }
            } while (!GameOver);
        }
    }
}
