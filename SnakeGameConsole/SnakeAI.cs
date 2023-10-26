using SnakeGame;

namespace SnakeGameConsole
{
    internal class SnakeAI
    {
        internal const char snakeBodyChar = '*';

        internal LinkedList<SnakeBodyPart> _snakeBodyParts;
        internal SnakeBodyPart _lastRemovedTail;

        internal Direction _currentDirection;

        public void SetupSnakeAI(int borderX, int borderY, int borderWidth, int borderHeight)
        {
            _snakeBodyParts = new();

            //Get random position for snake at start
            Random snakeBodyPartSpawner = new();
            int snakeBodyPartStartPointX =
                snakeBodyPartSpawner.Next(borderX + 1, (borderX + borderWidth - 1) / 2);
            int snakeBodyPartStartPointY =
                snakeBodyPartSpawner.Next(borderY + 1, (borderY + borderHeight - 1) / 2);

            _snakeBodyParts = new();
            _snakeBodyParts.AddFirst(
                new SnakeBodyPart
                {
                    X = snakeBodyPartStartPointX,
                    Y = snakeBodyPartStartPointY
                });
            _lastRemovedTail = new();
            _currentDirection = Direction.Right;
        }

        public void MoveSnake(Food food)
        {
            if (food.FoodEaten) return;

            SetDirection(food);

            SnakeBodyPart head = _snakeBodyParts.First();
            int newSnakeHeadX = head.X;
            int newSnakeHeadY = head.Y;

            switch (_currentDirection)
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

            SnakeBodyPart newHead = new(newSnakeHeadX, newSnakeHeadY);
            _snakeBodyParts.AddFirst(newHead);

            _lastRemovedTail = _snakeBodyParts.Last();
            _snakeBodyParts.RemoveLast();
        }

        public void SetDirection(Food food)
        {
            SnakeBodyPart head = _snakeBodyParts.First();

            int distanceToFoodX = head.X - food.X;
            int distanceToFoodY = head.Y - food.Y;

            if (distanceToFoodX != 0)
            {
                _currentDirection = distanceToFoodX switch
                {
                    < 0 => Direction.Right,
                    > 0 => Direction.Left,
                    _ => _currentDirection
                };
            }

            if (distanceToFoodY != 0)
            {
                _currentDirection = distanceToFoodY switch
                {
                    < 0 => Direction.Down,
                    > 0 => Direction.Up,
                    _ => _currentDirection
                };
            }
        }
    }
}
