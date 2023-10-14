namespace SnakeGame
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            SnakeGame snakeGame = new();
            await snakeGame.Run();

            Console.ReadKey(true);
            
        }

    }
}