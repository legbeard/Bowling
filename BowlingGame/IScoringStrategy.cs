using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame
{
    public interface IScoringStrategy
    {
        public IEnumerable<int?> GetIntermediateScores();
        public int? GetCurrentScore();
        public void Score(int score);
    }
}
