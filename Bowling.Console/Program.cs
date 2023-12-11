using Bowling;
using ConsoleTables;

// This is primarily an afterthought - the primary work has gone into the underlying logic and testing and testing

do
{
    var bowlingStrategy = new BowlingGame();
    while (!bowlingStrategy.IsResolved)
    {
        PrintScores(bowlingStrategy);
        Console.WriteLine("Enter pins hit:");

        int pins;

        while (!int.TryParse(Console.ReadLine(), out pins))
        {
            Console.WriteLine("Please enter a number...");
        }

        try
        {
            bowlingStrategy.Score(pins);
        } 
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Your input was invalid:\n{ex.Message}\n");
            Console.WriteLine("Hit any key to continue");
            Console.ReadKey();
        }
    }

    PrintScores(bowlingStrategy);

    Console.WriteLine("The game has ended, press Y to play again or any other key to exit.");
} while (Console.ReadKey().Key == ConsoleKey.Y);

void PrintScores(BowlingGame bowlingStrategy)
{
    Console.Clear();

    // There's a lot of fiddling with enumerables and converting to arrays here to make ConsoleTables play nicely.
    var frameHeaders = Enumerable.Range(1, 10).Select(x => x.ToString()).ToArray();
    frameHeaders = new string[] { "Frame" }.Concat(frameHeaders).ToArray();

    var table = new ConsoleTable(frameHeaders).Configure(x => x.EnableCount = false);

    var frameScores = bowlingStrategy
        .GetCumulativeScores()
        .Select(x => x != null ? x.ToString() : string.Empty)
        .ToArray();

    frameScores = new string[] { "Score" }.Concat(frameScores).ToArray();

    var rollsPerFrame = bowlingStrategy
        .GetRollsPerFrame()
        .Select(x => GetRollsString(x))
        .ToArray();

    rollsPerFrame = new string[] { "Rolls" }.Concat(rollsPerFrame).ToArray();

    table.AddRow(frameScores);
    table.AddRow(rollsPerFrame);
    table.Write();
}

string GetRollsString(List<int> rolls)
{
    var rollStrings = new List<string>();

    for (int i = 0; i < rolls.Count; i++)
    {
        // Handle spares as '{number} | /'
        if (i >= 1 && // Don't compare first number
            rolls[i - 1] != 10 && // In last frame, it is possible to have a strike followed by 0, which is not a spare
            rolls[i - 1] + rolls[i] == 10) // Does this roll make this and the previous a spare?
        {
            rollStrings.Add("/");
            continue;
        }

        // Handle strikes as 'X'
        if (rolls[i] == 10)
        {
            rollStrings.Add("X");
            continue;
        }

        rollStrings.Add(rolls[i].ToString());
    }

    return string.Join(" | ", rollStrings);
}