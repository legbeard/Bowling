namespace Bowling
{
    public class BowlingGame
    {
        private readonly List<Frame> _frames = new();
        private const int MAX_FRAMES = 10;
        private int _currentFrameIndex = 0;

        public BowlingGame()
        {
            Frame? previousFrame = null;

            for (int i = 0; i < MAX_FRAMES - 1; i++)
            {
                var nextFrame = new Frame(previousFrame);
                _frames.Add(nextFrame);
                previousFrame = nextFrame;
            }

            _frames.Add(new LastFrame(previousFrame));
        }

        public List<List<int>> GetRollsPerFrame() => _frames.Select(x => x.GetRolls()).ToList();

        public IEnumerable<int?> GetCumulativeScores()
        {
            return _frames.Select(x => x.GetCumulativeScore());
        }

        public int? GetCurrentScore() => _frames.LastOrDefault(x => x.GetScore() != null)?.GetScore();

        public void Score(int score)
        {
            if(IsResolved)
            {
                throw new InvalidOperationException("Attempted to score a game which has ended");
            }

            if (CurrentFrame.IsResolved)
            {
               _currentFrameIndex++;
            }

            CurrentFrame?.HitPins(score);
        }

        public bool IsResolved => _frames.All(x => x.IsResolved);

        private Frame CurrentFrame => _frames[_currentFrameIndex];
    }
}
