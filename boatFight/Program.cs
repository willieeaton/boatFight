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
                int x = InputCoordinate($"{player.PlayerName}, please enter your X coordinate, from 1 to DUMMY. ");
                int y = InputCoordinate($"{player.PlayerName}, please enter your Y coordinate, from 1 to DUMMY. ");
                Console.WriteLine($"Placing ship at {OutputCoordinate(x)}, {OutputCoordinate(y)}.  Press key to continue.");
                Console.ReadKey();
                Console.Clear();
                player.CreateShip(x, y);
            }
        }

        public static int InputCoordinate(string prompt)
        {
            Console.Write(prompt + " ");
            return int.Parse(Console.ReadLine()) - 1;
        }

        public static int OutputCoordinate(int coordinate)
        {
            return coordinate + 1;
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
