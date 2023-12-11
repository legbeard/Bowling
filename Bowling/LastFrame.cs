namespace Bowling
{
    /// <summary>
    /// A specialization of the <see cref="Frame"/> class, which allows bonus rolls to be rolled before the frame is resolved.
    /// </summary>
    public class LastFrame : Frame
    {
        /// <summary>
        /// Creates a new LastFrame instance
        /// </summary>
        /// <param name="previous">The previous frame</param>
        public LastFrame(Frame? previous = null) : base(previous)
        {

        }

        /// <summary>
        /// Whether or not the frame is resolved, meaning no more rolls can be performed
        /// </summary>
        /// <returns>
        /// <c>true</c> if the maximum number of pins have been hit or the maximum number of rolls have been used and no bonus rolls are available, otherwise <c>false</c>
        /// </returns>
        public override bool IsResolved => base.IsResolved && !HasBonus;

        /// <inheritdoc/>
        public override void HitPins(int pinsHit)
        {
            if (HasBonus)
            {
                ValidateRoll(pinsHit);
                _rolls.Add(pinsHit);
                ResolveBonus(pinsHit);
                return;
            }

            base.HitPins(pinsHit);
        }
    }
}
