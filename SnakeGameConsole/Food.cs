namespace SnakeGame
{
    internal struct Food
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool FoodEaten { get; set; }

        public Food(int x, int y, bool foodEaten)
        {
            X = x;
            Y = y;
            FoodEaten = foodEaten;
        }
    }
}
