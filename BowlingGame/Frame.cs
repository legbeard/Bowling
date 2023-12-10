﻿namespace BowlingGame
{
    internal class Frame
    {
        /// <summary>
        /// Whether or not the frame is resolved, meaning no more rolls can be performed
        /// </summary>
        /// <returns>
        /// <c>false</c> if less than the maximum number of pins have been hit and less than the maximum number of rolls have been used or if this is the last frame and bonus rolls are available.
        /// Otherwise <c>true</c> 
        /// </returns>
        public bool IsResolved
        {
            get
            {
                if (_pinsHit < MAX_PINS && _rolls < MAX_ROLLS)
                {
                    return false;
                }

                if (IsLastAndHasBonus)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Whether or not the frame has available bonus rolls
        /// </summary>
        public bool HasBonus => _bonusRolls > 0;

        private const int MAX_PINS = 10;
        private const int MAX_ROLLS = 2;

        private readonly Frame? _previous = null;

        private bool IsLastAndHasBonus => _isLast && HasBonus;
        private bool PreviousHasBonus => _previous?.HasBonus ?? false;

        private bool _isLast = false;
        private int _pinsHit = 0;
        private int _bonusPoints = 0;
        private int _rolls = 0;
        private int _bonusRolls = 0;


        /// <summary>
        /// Creates a new Frame
        /// </summary>
        /// <param name="previous">The previous frame or null</param>
        /// <param name="isLast">Whether or not this is the last frame</param>
        public Frame(Frame? previous = null, bool isLast = false)
        {
            _previous = previous;
            _isLast = isLast;
        }

        /// <summary>
        /// Indicate that <paramref name="pinsHit"/> have been hit in this frame.
        /// </summary>
        /// <param name="pinsHit">The number of pins that have been hit</param>
        /// <exception cref="ArgumentException">Thrown when either the frame has already been resolved or if the number of hit pins is invalid in the context of the frame</exception>
        public void HitPins(int pinsHit)
        {
            ValidateRoll(pinsHit);

            // If this is the last frame, we have bonus rolls and any number of pins is hit, the first condition will always be true
            // To resolve bonus rolls for the last frame, we explicitly skip this sanity check in the special case of IsLastAndHasBonus
            if (pinsHit + _pinsHit > MAX_PINS && !IsLastAndHasBonus)
            {
                throw new ArgumentException(
                    $"Attempted to hit {pinsHit} pins, but {_pinsHit} pins were already hit in this frame " +
                    $"and hitting {_pinsHit + pinsHit} pins in a regular frame is impossible");
            }

            if (IsLastAndHasBonus)
            {
                ResolveBonus(pinsHit);
                return;
            }

            _rolls++;
            _pinsHit += pinsHit;
            _previous?.ResolveBonus(pinsHit);

            if (_pinsHit == 10)
            {
                _bonusRolls = MAX_ROLLS + 1 - _rolls;
            }
        }


        /// <summary>
        /// Gets the cumulative score for this and all previous frames
        /// </summary>
        /// <returns>
        /// The cumulative points for this and all previous frames or <c>null</c> if this or any previous frames is not resolved or has unresolved bonus rolls.</returns>
        public int? GetCumulativeScore() =>
            _previous != null
                    ? _previous.GetCumulativeScore() + GetScore()
                    : GetScore();

        /// <summary>
        /// Gets the score for this frame.
        /// </summary>
        /// <returns>
        /// The score for this frame, if it is resolved and does not have unresolved bonus rolls, otherwise <c>null</c>
        /// </returns>
        public int? GetScore()
        {
            if (!IsResolved || HasBonus)
            {
                return null;
            }

            return _pinsHit + _bonusPoints;
        }


        /// <summary>
        /// Resolves a bonus roll.
        /// </summary>
        /// <param name="pinsHit">The number of pins hit for the bonus roll</param>
        public void ResolveBonus(int pinsHit)
        {
            if (PreviousHasBonus)
            {
                _previous?.ResolveBonus(pinsHit);
            }

            if (_bonusRolls < 1)
            {
                return;
            }

            _bonusPoints += pinsHit;
            _bonusRolls--;
        }

        /// <summary>
        /// Validates a single roll
        /// </summary>
        /// <param name="pinsHit">The amount of pins that were hit </param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateRoll(int pinsHit)
        {
            if (IsResolved)
            {
                throw new ArgumentException($"Attempted to hit pins in a frame that was already resolved");
            }

            if (pinsHit < 0)
            {
                throw new ArgumentException($"Attempted to hit {pinsHit} pins, but hitting a negative number of pins is impossible");
            }

            if (pinsHit > MAX_PINS)
            {
                throw new ArgumentException($"Attempted to hit {pinsHit} pins, but hitting more than 10 pins is impossible");
            }
        }
    }
}
