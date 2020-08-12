using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace boatFight
{
    class Program
    {
        //magic number elimination station
        const int NumberOfPlayers = 2;
        const int NumberOfAIDifficultyLevels = 3;
        //define statics
        public static int BoardSize;
        public static int NumberOfShips;
        public static int NumberOfHumanPlayers;
        public static bool SalvoMode;
        private static List<int> _shipSizes = new List<int>();
        private static List<string> _shipDesignations = new List<String>();

        static List<Player> players = new List<Player>();
        private static void Main(string[] args)
        {
            bool donePlaying = false;

            WelcomeScreen();

            do
            {
                ClearMemory();
                GameTypeSelect();
                CreatePlayers();
                PlaceShips();
                GameLoop();
                donePlaying = !PlayAgainPrompt();
            } while (!donePlaying);
        }
        public static void GameLoop()
        {
            bool GameOver = false;
            do
            {
                foreach (Player player in players)
                {
                    var numberOfShots = SalvoMode ? player.RemainingShips() : 1;
                    for (int i = 1; i <= numberOfShots; i++)
                    {
                        if (!GameOver)
                        {
                            GameOver = player.Fire(players, i, numberOfShots);
                        }
                    }
                }
            } while (!GameOver);

        }

        public static void GameTypeSelect()
        {
            Console.WriteLine("Please select one of the following game modes.");
            Console.WriteLine("1. Quick (1 ship, 5x5 board)");
            Console.WriteLine("2. Traditional (5 ships, 10x10 board)");
            Console.WriteLine("3. Salvo (Traditional, with one shot each turn per unsunk ship)");

            bool validInput = false;
            int menuChoice = 0;
            do
            {
                Console.Write("> ");
                validInput = int.TryParse(Console.ReadLine(), out menuChoice);
                if(menuChoice < 1 || menuChoice > 3)
                {
                    validInput = false;
                }
            } while (!validInput);

            switch(menuChoice)
            {
                case 1:
                    BoardSize = 5;
                    NumberOfShips = 1;
                    SalvoMode = false;
                    _shipSizes.AddRange(new List<int>() { 1 });
                    _shipDesignations.AddRange(new List<string> { "Ship" });
                    break;

                case 2:
                    BoardSize = 10;
                    NumberOfShips = 5;
                    SalvoMode = false;
                    _shipSizes.AddRange(new List<int>() {
                        5, 4, 3, 3, 2 });
                    _shipDesignations.AddRange(new List<string> { 
                        "Carrier", "Battleship", "Cruiser", "Submarine", "Destroyer" });
                    break;

                case 3:
                    BoardSize = 10;
                    NumberOfShips = 5;
                    SalvoMode = true;
                    _shipSizes.AddRange(new List<int>() {
                        5, 4, 3, 3, 2 });
                    _shipDesignations.AddRange(new List<string> {
                        "Carrier", "Battleship", "Cruiser", "Submarine", "Destroyer" });
                    break;

                default:

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
                players.Add(new AI(i + 1, SelectAIDifficulty(i + 1)));
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
                for (int i = 0; i < NumberOfShips; i++)
                {
                    player.PlaceShip(_shipSizes[i], _shipDesignations[i]);
                }
            }
        }

        public static bool IsValidDirection(int xDirection, int yDirection) => (xDirection * yDirection == 0 && Math.Abs(xDirection + yDirection) == 1);

        public static int SelectAIDifficulty (int playerNumber)
        {
            Console.WriteLine();
            Console.WriteLine("Difficulty options:");
            Console.WriteLine("1) Beginner");
            Console.WriteLine("2) Intermediate");
            Console.WriteLine("3) Professional");
            Console.WriteLine();
            Console.Write($"Select a difficulty for player {playerNumber}. ");

            int returnDifficulty;

            bool validInput = false;
            do
            {
                if (int.TryParse(Console.ReadLine(), out returnDifficulty))
                {
                    if (returnDifficulty >= 1 && returnDifficulty <= NumberOfAIDifficultyLevels)
                    {
                        validInput = true;
                    }
                }
            } while (!validInput);

            return returnDifficulty;
        }

        public static bool PlayAgainPrompt ()
        {
            Console.WriteLine("Play another game?  (Y/N)");
            string playAgain = Console.ReadLine().ToUpper();
            if (playAgain[0] == 'Y')
            {
                return true;
            }
            return false;
        }

        public static void ClearMemory ()
        {
            players.RemoveAll(p => p.ToString() != "");
            _shipDesignations.RemoveAll(p => p != "");
            _shipSizes.RemoveAll(p => p != 0);
        }

        public static void WelcomeScreen ()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Boat Fight!");
            Console.WriteLine("An open-source game based on the board game Battleship.");
            Console.WriteLine("By P. Willie Eaton.");
            Console.WriteLine("Version v1, released 2020, August 12, 18:38 EDT.");
            Console.WriteLine();
        }
    }
}
