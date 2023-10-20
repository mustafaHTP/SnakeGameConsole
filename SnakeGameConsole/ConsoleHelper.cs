using SnakeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SnakeGameConsole
{
    internal static class ConsoleHelper
    {
        internal static void DrawBorder(int borderX, int borderY, int borderWidth, int borderHeight, char borderChar)
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

        internal static void DrawSnakeBody(LinkedList<SnakeBodyPart> snakeBodyParts, char snakeBodyChar, Direction direction, bool isBoostMode)
        {
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

        internal static void DrawFood(Food food, char foodChar)
        {
            Console.SetCursorPosition(food.X, food.Y);
            WriteWithColor(foodChar, ConsoleColor.Yellow);
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

        internal static void PrintGameOver(int borderX, int borderY, int borderWidth, int borderHeight, char borderChar, int score)
        {
            Console.Clear();
            ConsoleHelper.DrawBorder(borderX, borderY, borderWidth, borderHeight, borderChar);

            int middleOfBorderX = (borderX + borderWidth - 1) / 2;
            int middleOfBorderY = (borderY + borderHeight) / 2;

            string gameOverText = "... GAME OVER ...";

            Console.SetCursorPosition(
                middleOfBorderX - gameOverText.Length / 2,
                middleOfBorderY
                );
            ConsoleHelper.WriteWithColor(gameOverText, ConsoleColor.Red);

            string userScore = "Score: " + score;
            Console.SetCursorPosition(
                middleOfBorderX - userScore.Length / 2,
                middleOfBorderY + 2
                );
            ConsoleHelper.WriteWithColor(userScore, ConsoleColor.Green);

            string tryAgainText = "Try Again ? : Y/N";
            Console.SetCursorPosition(
                middleOfBorderX - tryAgainText.Length / 2,
                middleOfBorderY + 4
                );
            ConsoleHelper.WriteWithColor(tryAgainText, ConsoleColor.Yellow);
        }

        internal static void PrintStartGame(int borderX, int borderY, int borderWidth, int borderHeight, char borderChar)
        {
            Console.Clear();
            ConsoleHelper.DrawBorder(borderX, borderY, borderWidth, borderHeight, borderChar);

            int middleOfBorderX = (borderX + borderWidth) / 2 + 3;
            int middleOfBorderY = (borderY + borderHeight) / 2;

            string startGameText = "...Press Any Key to START...";

            Console.SetCursorPosition(
                middleOfBorderX - startGameText.Length / 2,
                middleOfBorderY
                );
            ConsoleHelper.WriteWithColor(startGameText, ConsoleColor.Green);
        }

    }
}
