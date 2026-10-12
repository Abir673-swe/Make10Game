namespace Make10Game.Models;

public class Puzzle
{
    public int Level { get; }
    public int[] Numbers { get; }
    public decimal Target { get; }

    public Puzzle(int level, int[] numbers, decimal target = 10)
    {
        Level = level;
        Numbers = numbers;
        Target = target;
    }
}
