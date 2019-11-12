﻿using System;
using System.Collections.Generic;

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
            _ships = new Ship(x, y, this);
        }

        public void CreateShip(Point location)
        {
            CreateShip(location.X, location.Y);
        }

        public int OtherPlayerNumber() => (_playerNumber == 1) ? 2 : 1;

        public int OtherPlayerIndex() => (_playerNumber == 1) ? 1 : 0;

        public bool Fire(List<Player> players)
        {
            Point shotLocation = new Point(-1, -1);
            GameBoard.ShotMapDisplay();
            Player opponent = players[OtherPlayerIndex()];

            Console.WriteLine($"Your player number is {_playerNumber}.");
            Console.WriteLine($"You opponent's index should be {OtherPlayerIndex()}.");
            Console.WriteLine($"Your oppnent's name should be {opponent.PlayerName}.");
            var validInput = false;
            do
            {
                shotLocation = Point.InputCoordinates($"{PlayerName}, enter coordinates to shoot at. ", opponent.GameBoard);
                validInput = opponent.GameBoard.CellExists(shotLocation);
            } while (validInput == false);

            var targetPoint = opponent.GameBoard.LocatePoint(shotLocation);
            var theShotHit = targetPoint.GetShot();

            if(theShotHit)
            {
                Console.WriteLine("BOOM!!!  Hit!!!");
                GameBoard.ShotMapDisplay();
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

        public Player(int playerNumber, int boardSize)
        {
            _playerNumber = playerNumber;
            GameBoard = new Board(boardSize);
        }


    }
}
