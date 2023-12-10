namespace BowlingGame.Test;

public class FrameTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(2, 2, 4)]
    [InlineData(4, 5, 9)]
    public void Should_HaveScore_When_HittingLessThan10PinsIn2Rolls(int firstRoll, int secondRoll, int expectedScore)
    {
        var frame = new Frame();
        frame.HitPins(firstRoll);
        frame.HitPins(secondRoll);

        frame.GetScore().Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 9)]
    [InlineData(9, 1)]
    public void Should_HaveBonusAndUnresolvedScore_When_HittingSpares(int firstRoll, int secondRoll)
    {
        var frame = new Frame();

        frame.HitPins(firstRoll);
        frame.HitPins(secondRoll);

        frame.HasBonus.Should().BeTrue();
        frame.GetScore().Should().Be(null);
    }

    [Fact]
    public void Should_HaveBonusAndUnresolvedScore_When_HittingStrike()
    {
        var frame = new Frame();

        frame.HitPins(10);

        frame.HasBonus.Should().BeTrue();
        frame.GetScore().Should().Be(null);
    }

    [Fact]
    public void Should_AddBonusPointsToPreviousWithBonus()
    {
        var previous = new Frame();
        previous.HitPins(10);
            
        var frame = new Frame(previous);
        frame.HitPins(1);
        frame.HitPins(1);

        previous.GetScore().Should().Be(12);
        frame.GetScore().Should().Be(2);
        frame.GetCumulativeScore().Should().Be(14);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(int.MaxValue)]
    public void Should_ThrowArgumentException_When_HittingAnInvalidNumberOfPins(int pinsHit)
    {
        var frame = new Frame();

        frame.Invoking(x => x.HitPins(pinsHit))
            .Should()
            .Throw<ArgumentException>($"it should not be possible to hit {pinsHit} pins");
    }

    [Theory]
    [InlineData(0, 11)]
    [InlineData(8, 3)]
    [InlineData(3, 8)]
    [InlineData(11, 0)]

    public void Should_ThrowArgumentException_When_HittingMoreThan10PinsIn2Rolls(int firstRoll, int secondRoll)
    {
        var frame = new Frame();

        frame.Invoking(x =>
            {
                x.HitPins(firstRoll);
                x.HitPins(secondRoll);
            })
            .Should()
            .Throw<ArgumentException>($"it should not be possible to hit {firstRoll} and {secondRoll} consecutively in the same frame");
    }
}