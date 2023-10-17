using SnakeGameConsole;

namespace SnakeGame
{
    internal class SnakeGame
    {
        //BORDER CONSTANTS
        private const int BorderX = 5;
        private const int BorderY = 7;
        private const int BorderWidth = 60;
        private const int BorderHeight = 20;

        //CHAR CONSTANTS
        /*
         * More details about special characters
         * https://theasciicode.com.ar/extended-ascii-code/bottom-half-block-ascii-code-220.html
         * **/
        private const char BorderChar = '▄'; 
        private const char FoodChar = '@';
        private const char SnakeBodyChar = '■';

        private LinkedList<SnakeBodyPart> _snakeBodyParts;
        /*
         * When snake moves, last element of snake body part is removed 
         * and new part of snake body part is added to first element of linked list.
         * When snake eat food, new part of snake body must add to tail of snake.
         * Since we remove the last part of snake body while moving the snake, 
         * we must keep the last part holded.
         * **/
        private SnakeBodyPart _lastRemovedTail;

        private Food _food;

        private Direction _currentDirection;

        private int _userScore;

        /*
         * Values that between 50 and 100 are fine 
         * for smooth gameplay
         * **/
        private const int _gameDelayAmount = 75;

        private CancellationTokenSource cancellationTokenSource;

        private void SetupGame()
        {
            Console.Clear();
            Console.BufferWidth = 200;
            Console.BufferHeight = 200;

            //Snake Initial Values

            //Get random position for snake at start
            Random snakeBodyPartSpawner = new();
            int snakeBodyPartStartPointX = snakeBodyPartSpawner.Next(BorderX + 1, BorderX + BorderWidth - 1);
            int snakeBodyPartStartPointY = snakeBodyPartSpawner.Next(BorderY + 1, BorderY + BorderHeight - 1);

            _snakeBodyParts = new();
            _snakeBodyParts.AddFirst(
                new SnakeBodyPart
                {
                    X = snakeBodyPartStartPointX,
                    Y = snakeBodyPartStartPointY
                });

            //Spawn food at startup
            SpawnFood();

            //Score Initial Values
            _userScore = 0;

            //Direction Initial Values
            _currentDirection = Direction.Right;

            //Cancellation Token Source
            /*
             * When you cancel the token using cancellationTokenSource.Cancel(), 
             * it sets the token to a canceled state, and this state is irreversible.
             * That's why we need to recreate the new one after the game ends**/
            cancellationTokenSource = new();
        }
    
        private void MoveSnake(Direction snakeDirection)
        {
            //Fetch cursor to last part of snake
            SnakeBodyPart lastSnakeBodyPart = _snakeBodyParts.Last();
            Console.SetCursorPosition(lastSnakeBodyPart.X, lastSnakeBodyPart.Y);
            //Overwrite last part of snake body
            Console.Write(' ');

            //Move head according to direction
            SnakeBodyPart firstSnakeBodyPart = _snakeBodyParts.First();

            switch (snakeDirection)
            {
                case Direction.Up:
                    Console.SetCursorPosition(firstSnakeBodyPart.X, firstSnakeBodyPart.Y - 1);
                    _snakeBodyParts.AddFirst(new SnakeBodyPart { X = firstSnakeBodyPart.X, Y = firstSnakeBodyPart.Y - 1 });
                    break;
                case Direction.Down:
                    Console.SetCursorPosition(firstSnakeBodyPart.X, firstSnakeBodyPart.Y + 1);
                    _snakeBodyParts.AddFirst(new SnakeBodyPart { X = firstSnakeBodyPart.X, Y = firstSnakeBodyPart.Y + 1 });
                    break;
                case Direction.Left:
                    Console.SetCursorPosition(firstSnakeBodyPart.X - 1, firstSnakeBodyPart.Y);
                    _snakeBodyParts.AddFirst(new SnakeBodyPart { X = firstSnakeBodyPart.X - 1, Y = firstSnakeBodyPart.Y });
                    break;
                case Direction.Right:
                    Console.SetCursorPosition(firstSnakeBodyPart.X + 1, firstSnakeBodyPart.Y);
                    _snakeBodyParts.AddFirst(new SnakeBodyPart { X = firstSnakeBodyPart.X + 1, Y = firstSnakeBodyPart.Y });
                    break;
            }

            //Pop last part of snake body
            _lastRemovedTail = _snakeBodyParts.Last();
            _snakeBodyParts.RemoveLast();
        }

        private void SpawnFood()
        {
            Random foodSpawner = new();
            do
            {
                _food.X = foodSpawner.Next(BorderX + 1, BorderX + BorderWidth - 1);
                _food.Y = foodSpawner.Next(BorderY + 1, BorderY + BorderHeight - 1);
            } while (_snakeBodyParts.Any(spb => spb.X == _food.X && spb.Y == _food.Y));
            //Spawn food until food coords collides with any of the snake body parts
        }

        private bool HasCollision()
        {
            SnakeBodyPart head = _snakeBodyParts.First();

            bool hasSelfCollision = _snakeBodyParts.Skip(1).Any(spb =>
                spb.X == head.X &&
                spb.Y == head.Y);

            bool hasBorderCollision = _snakeBodyParts.Take(1).Any(spb =>
                spb.X == BorderX ||
                spb.X == BorderX + BorderWidth - 1 ||
                spb.Y == BorderY ||
                spb.Y == BorderY + BorderHeight - 1);

            return hasBorderCollision || hasSelfCollision;
        }

        private void HaveFoodEaten()
        {
            if (_snakeBodyParts.First().X == _food.X && _snakeBodyParts.First().Y == _food.Y)
            {
                _food.FoodEaten = true;
                ++_userScore;
                _snakeBodyParts.AddLast(_lastRemovedTail);
            }
        }

        private void PrintDebugInfo()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("Has Collision: " + HasCollision());
            Console.Write(" Score:" + _userScore);
        }

        private async Task GetInputAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                ConsoleKey snakeDirectionInput = Console.ReadKey(true).Key;

                switch (snakeDirectionInput)
                {
                    case ConsoleKey.LeftArrow:

                        if (_currentDirection != Direction.Right)
                        {
                            _currentDirection = Direction.Left;
                        }
                        break;
                    case ConsoleKey.RightArrow:

                        if (_currentDirection != Direction.Left)
                        {
                            _currentDirection = Direction.Right;
                        }
                        break;
                    case ConsoleKey.UpArrow:

                        if (_currentDirection != Direction.Down)
                        {
                            _currentDirection = Direction.Up;
                        }
                        break;
                    case ConsoleKey.DownArrow:

                        if (_currentDirection != Direction.Up)
                        {
                            _currentDirection = Direction.Down;
                        }
                        break;
                    case ConsoleKey.Q:
                        cancellationTokenSource.Cancel();
                        break;
                }

                await Task.Delay(_gameDelayAmount);
            }
        }

        private async Task GameLoopAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                //PrintDebugInfo();
                ConsoleHelper.DrawSnakeHeader(BorderX);
                ConsoleHelper.DrawBorder(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);
                ConsoleHelper.DrawSnakeBody(_snakeBodyParts, SnakeBodyChar, _currentDirection);

                if (_food.FoodEaten)
                {
                    SpawnFood();
                    _food.FoodEaten = false;
                }
                ConsoleHelper.DrawFood(_food, FoodChar);

                await Task.Delay(_gameDelayAmount);

                MoveSnake(_currentDirection);
                if (HasCollision())
                {
                    cancellationTokenSource.Cancel();
                }

                HaveFoodEaten();
                Console.Clear();
            }

        }

        public async Task Run()
        {
            bool tryAgainRequested = false;
            do
            {
                SetupGame();
                ConsoleHelper.PrintStartGame(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);

                ConsoleKey startInput = Console.ReadKey(true).Key;

                var getInputTask = GetInputAsync();
                var gameLoopTask = GameLoopAsync();

                await Task.WhenAll(gameLoopTask, getInputTask);

                ConsoleHelper.PrintGameOver(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar, _userScore);

                ConsoleKey tryAgainInput = Console.ReadKey(true).Key;
                while (tryAgainInput != ConsoleKey.Y && tryAgainInput != ConsoleKey.N)
                {
                    tryAgainInput = Console.ReadKey(true).Key;
                }

                tryAgainRequested = tryAgainInput switch
                {
                    ConsoleKey.Y => true,
                    ConsoleKey.N => false,
                    _ => false
                };

            } while (tryAgainRequested);

        }

    }
}
