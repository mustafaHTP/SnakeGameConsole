using SnakeGameConsole;

namespace SnakeGame
{
    internal partial class SnakeGame
    {
        //BORDER CONSTANTS
        private const int BorderX = 5;
        private const int BorderY = 7;
        private const int BorderWidth = 60;
        private const int BorderHeight = 20;

        //CONSOLE CONSTANTS
        private const int ConsoleBufferWidth = 200;
        private const int ConsoleBufferHeight = 200;

        //CHAR CONSTANTS
        /*
         * More details about special characters
         * https://theasciicode.com.ar/extended-ascii-code/bottom-half-block-ascii-code-220.html
         * **/
        private const char BorderChar = '█';
        private const char FoodChar = '@';
        private const char SnakeBodyChar = '■';
        private const char ObstacleChar = '█';

        //GAME DELAY CONSTANTS
        /*
         * Values that between 50 and 100 are fine 
         * for smooth gameplay
         * **/
        private const int GameDefaultDelay = 75;
        private const int GameBoostDelay = GameDefaultDelay / 3;

        private LinkedList<SnakeBodyPart> _snakeBodyParts;
        /*
         * When snake moves, last element of snake body part is removed 
         * and new part of snake body part is added to first element of linked list.
         * When snake eat food, new part of snake body must add to tail of snake.
         * Since we remove the last part of snake body while moving the snake, 
         * we must keep the last part holded.
         * **/
        private SnakeBodyPart _lastRemovedTail;
        private List<Obstacle> _obstacles;
        private Food _food;
        private Direction _currentDirection;
        private SnakeAI _snakeAI;

        private int _userScore;
        private int _gameDelayAmount;

        //STATES
        private bool _isBoostMode;
        private bool _isMute;
        private bool _isPaused;
        private bool _hasObstacleSpawned;
        private bool _isSnakeAIMode;

        private CancellationTokenSource cancellationTokenSource;

        private void SetupGame()
        {
            Console.Clear();
            Console.SetBufferSize(ConsoleBufferWidth, ConsoleBufferHeight);

            //SNAKE DEFAULT VALUES
            //Get random position for snake at start
            Random snakeBodyPartSpawner = new();
            int snakeBodyPartStartPointX =
                snakeBodyPartSpawner.Next(BorderX + 1, BorderX + BorderWidth - 1 / 2);
            int snakeBodyPartStartPointY =
                snakeBodyPartSpawner.Next(BorderY + 1, BorderY + BorderHeight - 1 / 2);

            _snakeBodyParts = new();
            _snakeBodyParts.AddFirst(
                new SnakeBodyPart
                {
                    X = snakeBodyPartStartPointX,
                    Y = snakeBodyPartStartPointY
                });


            //Obstacles
            //DO NOT CHANGE ORDER _obstacles and SpawnFood
            //Because SpawnFood need to access _obstacles
            _obstacles = new();

            //Spawn food at startup
            SpawnFood();

            //SCORE DEFAULT VALUE
            _userScore = 0;

            //DIRECTION DEFAULT VALUE
            _currentDirection = Direction.Right;

            //Cancellation Token Source
            /*
             * When you cancel the token using cancellationTokenSource.Cancel(), 
             * it sets the token to a canceled state, and this state is irreversible.
             * That's why we need to recreate the new one after the game ends**/
            cancellationTokenSource = new();

            //GAME DEFAULT DELAY
            _gameDelayAmount = GameDefaultDelay;

            //SNAKE AI 
            _snakeAI = new();
            _snakeAI.SetupSnakeAI(BorderX, BorderY, BorderWidth, BorderHeight);

            _isBoostMode = false;
            _isPaused = false;
            _hasObstacleSpawned = true;
        }

        private void MoveSnake(Direction snakeDirection)
        {
            ////Fetch cursor to last part of snake
            //SnakeBodyPart lastSnakeBodyPart = _snakeBodyParts.Last();
            //Console.SetCursorPosition(lastSnakeBodyPart.X, lastSnakeBodyPart.Y);
            ////Overwrite last part of snake body
            //Console.Write(' ');

            //Move head according to direction
            SnakeBodyPart firstSnakeBodyPart = _snakeBodyParts.First();
            int newSnakeHeadX = firstSnakeBodyPart.X;
            int newSnakeHeadY = firstSnakeBodyPart.Y;

            switch (snakeDirection)
            {
                case Direction.Up:
                    newSnakeHeadY -= 1;
                    break;
                case Direction.Down:
                    newSnakeHeadY += 1;
                    break;
                case Direction.Left:
                    newSnakeHeadX -= 1;
                    break;
                case Direction.Right:
                    newSnakeHeadX += 1;
                    break;
            }

            //Add new snake head based on direction
            _snakeBodyParts.AddFirst(new SnakeBodyPart { X = newSnakeHeadX, Y = newSnakeHeadY });

            //Pop last part of snake body
            _lastRemovedTail = _snakeBodyParts.Last();
            _snakeBodyParts.RemoveLast();
        }

        private void SpawnObstacle()
        {
            Random obstacleSpawner = new();
            int obstacleX = default;
            int obstacleY = default;
            do
            {
                obstacleX = obstacleSpawner.Next(BorderX + 1, BorderX + BorderWidth - 1);
                obstacleY = obstacleSpawner.Next(BorderY + 1, BorderY + BorderHeight - 1);
            } while (_snakeBodyParts.Any(spb => spb.X == obstacleX && spb.Y == obstacleY)
                || _obstacles.Any(obs => obs.X == obstacleX && obs.Y == obstacleY));

            _obstacles.Add(new Obstacle(obstacleX, obstacleY));
        }

        private void SpawnFood()
        {
            Random foodSpawner = new();
            do
            {
                _food.X = foodSpawner.Next(BorderX + 1, BorderX + BorderWidth - 1);
                _food.Y = foodSpawner.Next(BorderY + 1, BorderY + BorderHeight - 1);
            } while (_snakeBodyParts.Any(spb => spb.X == _food.X && spb.Y == _food.Y)
                || _obstacles.Any(obs => obs.X == _food.X && obs.Y == _food.Y));
            //Food that will be generated must not be on snake's body
        }

        private bool HasCollision()
        {
            SnakeBodyPart head = _snakeBodyParts.First();

            bool hasSelfCollision = _snakeBodyParts.Skip(1).Any(spb =>
                spb.X == head.X &&
                spb.Y == head.Y);

            bool hasBorderCollision =
                head.X == BorderX ||
                head.X == BorderX + BorderWidth - 1 ||
                head.Y == BorderY ||
                head.Y == BorderY + BorderHeight - 1;

            bool hasObstacleCollision = false;
            bool hasSnakeAICollision = false;

            if (!_isSnakeAIMode)
            {
                hasObstacleCollision = _obstacles.Any(obs =>
                    obs.X == head.X && obs.Y == head.Y);
            }
            else
            {
                bool hasSnakeCollideWithAI = _snakeAI._snakeBodyParts.
                    Any(spb => spb.X == head.X && spb.Y == head.Y);

                SnakeBodyPart headAI = _snakeAI._snakeBodyParts.First();
                bool hasAICollideWithSnake = _snakeBodyParts.Any(spb => spb.X == headAI.X && spb.Y == headAI.Y);

                hasSnakeAICollision = hasSnakeCollideWithAI || hasAICollideWithSnake ? true : false;
            }

            return hasBorderCollision || hasSelfCollision || hasObstacleCollision || hasSnakeAICollision;
        }

        private void HaveFoodEaten()
        {
            SnakeBodyPart head = _snakeBodyParts.First();
            if (head.X == _food.X && head.Y == _food.Y)
            {
                _food.FoodEaten = true;
                ++_userScore;
                _snakeBodyParts.AddLast(_lastRemovedTail);
                if (!_isMute)
                {
                    AudioHelper.PlayEatEffect();
                }
                return;
            }

            SnakeBodyPart headAI = _snakeAI._snakeBodyParts.First();
            if (headAI.X == _food.X && headAI.Y == _food.Y)
            {
                _food.FoodEaten = true;
                _snakeAI._snakeBodyParts.AddLast(_snakeAI._lastRemovedTail);
                if (!_isMute)
                {
                    AudioHelper.PlayEatEffectAI();
                }
            }
        }

        private async Task GetInputAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                ConsoleKey snakeDirectionInput = default;
                if (Console.KeyAvailable)
                {
                    snakeDirectionInput = Console.ReadKey(true).Key;
                }

                switch (snakeDirectionInput)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:

                        if (_currentDirection != Direction.Right)
                        {
                            _currentDirection = Direction.Left;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:

                        if (_currentDirection != Direction.Left)
                        {
                            _currentDirection = Direction.Right;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:

                        if (_currentDirection != Direction.Down)
                        {
                            _currentDirection = Direction.Up;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:

                        if (_currentDirection != Direction.Up)
                        {
                            _currentDirection = Direction.Down;
                        }
                        break;
                    case ConsoleKey.Q:
                        cancellationTokenSource.Cancel();
                        break;
                    case ConsoleKey.Spacebar:
                        if (!_isBoostMode)
                        {
                            _gameDelayAmount = GameBoostDelay;
                            _isBoostMode = true;
                        }
                        else
                        {
                            _gameDelayAmount = GameDefaultDelay;
                            _isBoostMode = false;
                        }
                        break;
                    case ConsoleKey.P:
                        _isPaused = !_isPaused;
                        break;
                    case ConsoleKey.M:
                        _isMute = !_isMute;
                        break;

                }

                await Task.Delay(_gameDelayAmount);
            }
        }

        private async Task GameLoopAsync()
        {
            ConsoleHelper.DrawSnakeHeader(BorderX);
            ConsoleHelper.DrawBorder(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                ConsoleHelper.DrawSnakeBody(_snakeBodyParts,
                    _lastRemovedTail,
                    SnakeBodyChar,
                    _currentDirection,
                    _isBoostMode);

                if (_isSnakeAIMode)
                {
                    ConsoleHelper.DrawSnakeBodyAI(
                        _snakeAI._snakeBodyParts,
                        _snakeAI._lastRemovedTail,
                        SnakeAI.snakeBodyChar,
                        _snakeAI._currentDirection,
                        _isBoostMode);
                }

                #region PAUSE_CONTROL

                if (_isPaused)
                {
                    ConsoleHelper.PrintPauseGame(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);

                    //Keep take input until user press P key
                    ConsoleKey pauseInput;
                    do
                    {
                        pauseInput = Console.ReadKey(true).Key;
                    } while (pauseInput != ConsoleKey.P);

                    _isPaused = false;

                    Console.Clear();
                    ConsoleHelper.DrawSnakeHeader(BorderX);
                    ConsoleHelper.DrawBorder(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);
                }

                #endregion

                #region FOOD_CONTROL

                if (_food.FoodEaten)
                {
                    SpawnFood();
                    _food.FoodEaten = false;
                }
                ConsoleHelper.DrawFood(_food, FoodChar);

                #endregion

                #region OBSTACLE_CONTROL

                if (!_isSnakeAIMode)
                {
                    if (!_hasObstacleSpawned && _userScore % 5 == 0)
                    {
                        SpawnObstacle();
                        _hasObstacleSpawned = true;
                    }

                    if (_hasObstacleSpawned && _userScore % 5 != 0)
                    {
                        _hasObstacleSpawned = false;
                    }

                    ConsoleHelper.DrawObstacles(_obstacles, ObstacleChar);
                }

                #endregion

                await Task.Delay(_gameDelayAmount);

                MoveSnake(_currentDirection);
                if (_isSnakeAIMode)
                {
                    _snakeAI.MoveSnake(_food);
                }

                if (HasCollision())
                {
                    cancellationTokenSource.Cancel();
                }

                HaveFoodEaten();
            }
        }

        public async Task Run()
        {
            bool tryAgainRequested = false;
            do
            {
                SetupGame();
                ConsoleHelper.PrintStartGame(BorderX, BorderY, BorderWidth, BorderHeight, BorderChar);

                //Get Game Mode
                ConsoleKey gameModInput = Console.ReadKey(true).Key;
                while (gameModInput != ConsoleKey.Q && gameModInput != ConsoleKey.E)
                {
                    gameModInput = Console.ReadKey(true).Key;
                }

                //Set Game mode according to input
                _isSnakeAIMode = gameModInput == ConsoleKey.E ? true : false;
                if (!_isMute)
                {
                    AudioHelper.PlayStartGameEffect();
                }
                Console.Clear();

                var getInputTask = GetInputAsync();
                var gameLoopTask = GameLoopAsync();

                await Task.WhenAll(gameLoopTask, getInputTask);

                if (!_isMute)
                {
                    AudioHelper.PlayGameOverEffect();
                }
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
