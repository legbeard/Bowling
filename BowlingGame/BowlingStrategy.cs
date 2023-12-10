namespace BowlingGame
{
    public class BowlingStrategy : IScoringStrategy
    {
        private readonly List<Frame> _frames = new();
        private const int maxFrames = 10;

        public IEnumerable<int?> GetIntermediateScores()
        {
            return _frames.Select(x => x.GetCumulativeScore());
        }

        public int? GetCurrentScore() => _frames.LastOrDefault()?.GetCumulativeScore();

        public void Score(int score)
        {
            if(_frames.Count == maxFrames && IsCurrentFrameResolved)
            {
                throw new ArgumentException("Attempted to score a game which has ended");
            }

            if(!_frames.Any() || IsCurrentFrameResolved)
            {
                _frames.Add(new Frame(CurrentFrame, isLast: _frames.Count == maxFrames - 1));
            }

            CurrentFrame?.HitPins(score);
        }

        private Frame? CurrentFrame => _frames.LastOrDefault();
        private bool IsCurrentFrameResolved => CurrentFrame?.IsResolved ?? false;
    }
}
