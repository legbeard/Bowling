namespace BowlingGame
{
    internal class LastFrame : Frame
    {
        public LastFrame(Frame? previous = null) : base(previous)
        {

        }

        public override bool IsResolved => base.IsResolved && !HasBonus;

        public override void HitPins(int pinsHit)
        {
            if (HasBonus)
            {
                ValidateRoll(pinsHit);
                ResolveBonus(pinsHit);
                return;
            }

            base.HitPins(pinsHit);
        }
    }
}
