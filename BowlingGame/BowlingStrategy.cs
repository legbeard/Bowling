using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame
{
    public class BowlingStrategy : IScoringStrategy
    {
        private readonly List<Frame> _frames = new();
        private const int maxFrames = 10;

        public IEnumerable<int?> GetIntermediateScores()
        {
            return _frames.Select(x => x.GetScore());
        }

        public int? GetCurrentScore() => _frames.LastOrDefault()?.GetScore();

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
        private bool IsCurrentFrameResolved => CurrentFrame?.IsResolved() ?? false;
    }

    internal class Frame
    {
        private readonly Frame? _previous = null;

        private int Score => _pinsHit + _bonusPoints;
        private bool _isLast = false;
        private int _pinsHit = 0;
        private int _bonusPoints = 0;
        private int _rolls = 0;
        private int _bonusRolls = 0;

        public Frame(Frame? previous = null, bool isLast = false)
        {
            _previous = previous;
            _isLast = isLast;
        }

        public bool IsResolved()
        {
            if (IsLastAndHasBonus)
            {
                return false;
            }

            if (_pinsHit < 10 && _rolls < 2)
            {
                return false;
            }

            return true;
        }

        private bool IsLastAndHasBonus => _isLast && _bonusRolls > 0;

        public void HitPins(int pinsHit)
        {
            if (IsLastAndHasBonus)
            {
                ResolveBonus(pinsHit);
                return;
            }

            if (pinsHit + _pinsHit > 10)
            {
                throw new ArgumentException($"Cannot hit {pinsHit} pins in a regular frame");
            }

            if (IsResolved())
            {
                throw new ArgumentException($"Attempted to hit pins in a frame that was already resolved");
            }

            _rolls++;
            _pinsHit += pinsHit;
            _previous?.ResolveBonus(pinsHit);

            if (_pinsHit == 10)
            {
                _bonusRolls = 3 - _rolls;
            }
        }

        public void ResolveBonus(int pinsHit)
        {
            _previous?.ResolveBonus(pinsHit);

            if(_bonusRolls < 1)
            {
                return;
            }

            _bonusPoints += pinsHit;
            _bonusRolls--;
        }

        public int? GetScore()
        {
            if (!IsResolved())
            {
                return null;
            }

            var previousScore = _previous?.GetScore() ?? 0;
            return previousScore + Score;
        }
    }
}
