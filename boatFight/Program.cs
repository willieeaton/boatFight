using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace boatFight
{
    class Program
    {
        //magic number elimination station
        const int NumberOfPlayers = 2;

        //define statics
        public static int BoardSize;
        public static int NumberOfShips;
        public static int NumberOfHumanPlayers;

        static List<Player> players = new List<Player>();
        private static void Main(string[] args)
        {
            GameTypeSelect();
            CreatePlayers();
            PlaceShips();
            GameLoop();
        }
        public static void GameLoop()
        {
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

        public static void GameTypeSelect()
        {
            Console.WriteLine("Welcome to Boat Fight!");
            Console.WriteLine();
            Console.WriteLine("Please select one of the following game modes.");
            Console.WriteLine("1. Quick (1 ship, 5x5 board)");
            Console.WriteLine("2. Traditional (5 ships, 10x10 board)");

            bool validInput = false;
            int menuChoice = 0;
            do
            {
                Console.Write("> ");
                validInput = int.TryParse(Console.ReadLine(), out menuChoice);
                if(menuChoice < 1 || menuChoice > 2)
                {
                    validInput = false;
                }
            } while (!validInput);

            switch(menuChoice)
            {
                case 1:
                    BoardSize = 5;
                    NumberOfShips = 1;
                    break;

                case 2:
                default:
                    BoardSize = 10;
                    NumberOfShips = 5;
                    break;
            }

            Console.WriteLine();
            Console.WriteLine($"Please enter how many AI players you want, from 0 to {NumberOfPlayers}.");

            menuChoice = 0;
            do
            {
                Console.Write("> ");
                validInput = int.TryParse(Console.ReadLine(), out menuChoice);
                if (menuChoice < 0 || menuChoice > NumberOfPlayers)
                {
                    validInput = false;
                }
            } while (!validInput);

            NumberOfHumanPlayers = NumberOfPlayers - menuChoice;
        }

        public static void CreatePlayers()
        {
            for (int i = 0; i < NumberOfHumanPlayers; i++)
            {
                players.Add(new Player(i + 1));
            }

            for (int i = NumberOfHumanPlayers; i < NumberOfPlayers; i++)
            {
                players.Add(new AI(i + 1, 1));
            }

            for (int i = 0; i < NumberOfHumanPlayers; i++)
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
                player.PlaceShip();
            }
        }


    }
}
