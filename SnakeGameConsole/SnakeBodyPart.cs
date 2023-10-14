namespace SnakeGame
{
    internal struct SnakeBodyPart
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SnakeBodyPart(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
