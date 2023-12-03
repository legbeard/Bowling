namespace BowlingGame.Test
{
    public class BowlingStrategyTests
    {
        private readonly BowlingStrategy _sut;

        public BowlingStrategyTests()
        {
            _sut = new BowlingStrategy();
        }

        public static IEnumerable<object[]> GetExampleScores()
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
        [MemberData(nameof(GetExampleScores))]
        public void Should_CorrectlyScoreExample_When_GivenExampleInput(
            IEnumerable<int> knockedPins, 
            IEnumerable<int?> expectedFrameScores)
        {
            foreach(var roll in knockedPins)
            {
                _sut.Score(roll);
            }

            var scores = _sut.GetIntermediateScores();
            scores.Should().BeEquivalentTo(expectedFrameScores);
        }
    }
}