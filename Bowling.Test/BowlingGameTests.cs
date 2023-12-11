namespace Bowling.Test;

public class BowlingGameTests
{
    private readonly BowlingGame _sut;

    public BowlingGameTests()
    {
        _sut = new BowlingGame();
    }

    public static IEnumerable<object[]> BowlingStrategyTestCases()
    {
        yield return new object[] // Example Game
        {
            new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 },
            new int?[] { 5, 14, 29, 49, 60, 61, 77, 97, 117, 133 }
        };

        yield return new object[] // Gutter Game
        {
            Enumerable.Range(0, 20).Select(_ => 0).ToArray(), // Hit 0 pins 20 times
            Enumerable.Range(0, 10).Select<int, int?>(_ => 0).ToArray() // All frames should resolve to 0
        };

        yield return new object[] // Perfect Game
        {
            Enumerable.Range(0, 12).Select(_ => 10).ToArray(), // Hit 10 pins 12 times
            Enumerable.Range(1, 10).Select<int, int?>(x => x*30).ToArray() // All frames should resolve to (frame number)*30
        };
    }

    [Theory]
    [MemberData(nameof(BowlingStrategyTestCases))]
    public void Should_CorrectlyScore_When_ScoringExampleGame(
        IEnumerable<int> knockedPins,
        IEnumerable<int?> expectedFrameScores)
    {
        foreach(var roll in knockedPins)
        {
            _sut.Score(roll);
        }

        var scores = _sut.GetCumulativeScores();
        scores.Should().BeEquivalentTo(expectedFrameScores);
    }

    [Fact]
    public void Should_ThrowInvalidOperationException_When_ScoringAFinishedGame()
    {
        // Score a gutter game
        foreach (var roll in Enumerable.Range(0, 20).Select(_ => 0))
        {
            _sut.Score(roll);
        }

        // Then attempt to roll again
        _sut.Invoking(x => x.Score(10))
            .Should()
            .Throw<InvalidOperationException>();
    }
}