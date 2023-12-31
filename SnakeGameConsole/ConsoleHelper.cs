﻿using SnakeGame;

namespace SnakeGameConsole
{
    internal static class ConsoleHelper
    {
        internal static void DrawBorder(int borderX, int borderY, 
            int borderWidth, int borderHeight, char borderChar)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            //Top 
            Console.SetCursorPosition(borderX, borderY);
            for (int i = 0; i < borderWidth; i++)
            {
                Console.Write(borderChar);
            }

            //Left
            for (int i = 0; i < borderHeight - 2; i++)
            {
                Console.SetCursorPosition(borderX, borderY + i + 1);
                Console.Write(borderChar);
            }

            //Right
            for (int i = 0; i < borderHeight - 2; i++)
            {
                Console.SetCursorPosition(borderX + borderWidth - 1, borderY + i + 1);
                Console.Write(borderChar);
            }


            //Bottom
            Console.SetCursorPosition(borderX, borderY + borderHeight - 1);
            for (int i = 0; i < borderWidth; i++)
            {
                Console.Write(borderChar);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void DrawSnakeBody(LinkedList<SnakeBodyPart> snakeBodyParts, SnakeBodyPart lastRemovedTail, 
            char snakeBodyChar, Direction direction, bool isBoostMode)
        {
            //Fetch cursor to last part of snake
            Console.SetCursorPosition(lastRemovedTail.X, lastRemovedTail.Y);
            //Overwrite last part of snake body
            Console.Write(' ');

            SnakeBodyPart head = snakeBodyParts.First();
            Console.SetCursorPosition(head.X, head.Y);
            Console.ForegroundColor = isBoostMode switch
            {
                true => ConsoleColor.Magenta,
                _ => ConsoleColor.Green
            };

            switch (direction)
            {
                case Direction.Up:
                    Console.Write('^');
                    break;
                case Direction.Down:
                    Console.Write('v');
                    break;
                case Direction.Left:
                    Console.Write('<');
                    break;
                case Direction.Right:
                    Console.Write('>');
                    break;
                default:
                    break;
            }


            foreach (var item in snakeBodyParts.Skip(1))
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(snakeBodyChar);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void DrawSnakeBodyAI(LinkedList<SnakeBodyPart> snakeBodyParts, SnakeBodyPart lastRemovedTail,
            char snakeBodyChar, Direction direction, bool isBoostMode)
        {
            //Fetch cursor to last part of snake
            Console.SetCursorPosition(lastRemovedTail.X, lastRemovedTail.Y);
            //Overwrite last part of snake body
            Console.Write(' ');

            SnakeBodyPart head = snakeBodyParts.First();
            Console.SetCursorPosition(head.X, head.Y);
            Console.ForegroundColor = isBoostMode switch
            {
                true => ConsoleColor.Magenta,
                _ => ConsoleColor.Blue
            };

            Console.Write('O');

            foreach (var item in snakeBodyParts.Skip(1))
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(snakeBodyChar);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void DrawFood(Food food, char foodChar)
        {
            Console.SetCursorPosition(food.X, food.Y);
            WriteWithColor(foodChar, ConsoleColor.Yellow);
        }

        internal static void DrawObstacles(List<Obstacle> obstacles, char obstacleChar)
        {
            foreach (var obstacle in obstacles)
            {
                Console.SetCursorPosition(obstacle.X, obstacle.Y);
                WriteWithColor(obstacleChar, ConsoleColor.DarkRed);
            }
        }

        internal static void WriteWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        internal static void WriteWithColor(char character, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.ResetColor();
        }

        /*
         * The snake figure : https://www.asciiart.eu/animals/reptiles/snakes
         * The snake title : https://ascii.co.uk/art/snake 
         * **/
        internal static void DrawSnakeHeader(int borderX)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 0);

            WriteSpace(borderX);
            Console.Write("           ____              \n");
            WriteSpace(borderX);
            Console.Write("          / . .\\             \n");
            WriteSpace(borderX);
            Console.Write("          \\  ---<              | |\n");
            WriteSpace(borderX);
            Console.Write("           \\  / ___ _ __   __ _| | _____ \n");
            WriteSpace(borderX);
            Console.Write("           / / / __| '_ \\ / _` | |/ / _ \\\n");
            WriteSpace(borderX);
            Console.Write("   _______/ /  \\__ \\ | | | (_| |   <  __/\n");
            WriteSpace(borderX);
            Console.Write("-=:________/  |___/_|  |_|\\__,_|_|\\_\\___|\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /*
         * To align Snake Space Header with border,
         * it adds padding left**/
        internal static void WriteSpace(int borderX)
        {
            for (int i = 0; i < borderX; i++)
            {
                Console.Write(" ");
            }
        }

        internal static void PrintGameOver(int borderX, int borderY, 
            int borderWidth, int borderHeight, char borderChar, int score)
        {
            Console.Clear();
            DrawBorder(borderX, borderY, borderWidth, borderHeight, borderChar);

            int middleOfBorderX = (borderX + borderWidth - 1) / 2;
            int middleOfBorderY = (borderY + borderHeight) / 2;

            string gameOverText = "... GAME OVER ...";

            Console.SetCursorPosition(
                middleOfBorderX - gameOverText.Length / 2,
                middleOfBorderY
                );
            WriteWithColor(gameOverText, ConsoleColor.Red);

            string userScore = "Score: " + score;
            Console.SetCursorPosition(
                middleOfBorderX - userScore.Length / 2,
                middleOfBorderY + 2
                );
            WriteWithColor(userScore, ConsoleColor.Green);

            string tryAgainText = "Try Again ? : Y/N";
            Console.SetCursorPosition(
                middleOfBorderX - tryAgainText.Length / 2,
                middleOfBorderY + 4
                );
            WriteWithColor(tryAgainText, ConsoleColor.Yellow);
        }

        internal static void PrintStartGame(int borderX, int borderY, 
            int borderWidth, int borderHeight, char borderChar)
        {
            Console.Clear();
            DrawBorder(borderX, borderY, borderWidth, borderHeight, borderChar);

            int middleOfBorderX = (borderX + borderWidth) / 2 + 3;
            int middleOfBorderY = (borderY + borderHeight) / 2;

            string startGameText = "...Press Q to PLAY NORMAL MODE...";

            Console.SetCursorPosition(
                middleOfBorderX - startGameText.Length / 2,
                middleOfBorderY
                );
            WriteWithColor(startGameText, ConsoleColor.Green);

            string startGameText2 = "...Press E to PLAY AI MODE...";

            Console.SetCursorPosition(
                middleOfBorderX - startGameText2.Length / 2,
                middleOfBorderY + 1
                );
            WriteWithColor(startGameText2, ConsoleColor.Blue);
        }

        internal static void PrintPauseGame(int borderX, int borderY, 
            int borderWidth, int borderHeight, char borderChar)
        {
            Console.Clear();
            DrawBorder(borderX, borderY, borderWidth, borderHeight, borderChar);

            int middleOfBorderX = (borderX + borderWidth) / 2 + 3;
            int middleOfBorderY = (borderY + borderHeight) / 2;

            string pauseGameText = "...PAUSED...";

            Console.SetCursorPosition(
                middleOfBorderX - pauseGameText.Length / 2,
                middleOfBorderY
                );

            WriteWithColor(pauseGameText, ConsoleColor.Blue);

            string pauseGameText2 = "Press P to Continue";

            Console.SetCursorPosition(
                middleOfBorderX - pauseGameText2.Length / 2,
                middleOfBorderY + 1
                );

            WriteWithColor(pauseGameText2, ConsoleColor.Blue);
        }
    }
}
