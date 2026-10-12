
using Make10Game.Models;

namespace Make10Game.Services;

public class Game
{
    private readonly List<Puzzle> _puzzles = new()
    {
        new Puzzle(1, new[] { 1, 2, 3, 4 }),
        new Puzzle(2, new[] { 2, 2, 3, 3 }),
        new Puzzle(3, new[] { 1, 2, 5, 8 }),
        new Puzzle(4, new[] { 1, 1, 4, 6 }),
        new Puzzle(5, new[] { 2, 3, 4, 5 }),
        new Puzzle(6, new[] { 1, 3, 3, 9 })
    };

    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int levelIndex = 0;
        int attempts = 0;

        Console.WriteLine("========================================");
        Console.WriteLine("              MAKE 10 GAME");
        Console.WriteLine("========================================");
        Console.WriteLine("Instructions:");
        Console.WriteLine("1. Use all four numbers exactly once.");
        Console.WriteLine("2. Use mathematical operations to reach the target: 10.");
        Console.WriteLine("3. Allowed operators: +, -, *, /, and parentheses ( ).");
        Console.WriteLine("4. Enter Q at any time to quit the game.");
        Console.WriteLine("========================================");
        Console.WriteLine();

        while (levelIndex < _puzzles.Count)
        {
            Puzzle puzzle = _puzzles[levelIndex];
            ShowPuzzle(puzzle, attempts);

            Console.Write("Your expression: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("You have exited the game. Thanks for playing!");
                return;
            }

            attempts++;

            bool valid = ExpressionEvaluator.TryEvaluate(
                input, puzzle.Numbers, out decimal result, out string error);

            if (!valid)
            {
                Console.WriteLine($"Invalid expression: {error}");
                Console.WriteLine("Please try again.");
                Console.WriteLine();
                continue;
            }

            if (result == puzzle.Target)
            {
                Console.WriteLine($"Correct! {input} = {result}");
                Console.WriteLine("Level completed! The next level is now unlocked.");
                Console.WriteLine();

                levelIndex++;
                attempts = 0;
            }
            else
            {
                Console.WriteLine(
                    $"Your result is {result}, but the target is {puzzle.Target}.");

                Console.WriteLine("Not quite! Please try again.");
                Console.WriteLine();
            }
        }

        Console.WriteLine("========================================");
        Console.WriteLine("Congratulations! You completed all levels!");
        Console.WriteLine("Thank you for playing MAKE 10 GAME.");
        Console.WriteLine("========================================");
    }

    private static void ShowPuzzle(Puzzle puzzle, int attempts)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Level: {puzzle.Level}");
        Console.WriteLine($"Numbers: {string.Join(", ", puzzle.Numbers)}");
        Console.WriteLine($"Target: {puzzle.Target}");
        Console.WriteLine($"Attempts: {attempts}");
        Console.WriteLine("----------------------------------------");
    }
}
