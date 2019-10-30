using System;
using System.Collections.Generic;

namespace boatFight
{
    class Program
    {
        private static void Main(string[] args)
        { 
            //magic number elimination station
            const int NumberOfPlayers = 2;
            List<Player> players = new List<Player>();
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                players.Add(new Player(i + 1));
            }

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Console.Write($"Player {players[i].PlayerNumber}, please enter your name. ");
                string playerName = Console.ReadLine();
                players[i].PlayerName = playerName;
            }

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Console.Clear();
                Console.Write($"{players[i].PlayerName}, please enter your X coordinate, from 1 to DUMMY. ");
                int coordinateX = int.Parse(Console.ReadLine());
                Console.Write($"{players[i].PlayerName}, please enter your Y coordinate, from 1 to DUMMY. ");
                int coordinateY = int.Parse(Console.ReadLine());

                players[i].CreateShip(coordinateX, coordinateY);
            }
        }
    }
}
