using System.Collections.Generic;
using System.Linq;

namespace Kalendra.Commons.Runtime.Domain.Services
{
    /// <summary>
    /// Base template method to autocomplete sugar syntax.
    /// </summary>
    public abstract class TemplateRandomService : IRandomService
    {
        public abstract int Seed { set; }
        
        #region Constructors
        protected TemplateRandomService() { }
        protected TemplateRandomService(int seed) => Seed = seed;
        #endregion

        #region Random providing (hooks)
        public abstract int Next(int min, int exclusiveMax);
        public abstract float Next();
        public abstract float Next(float min, float max);
        #endregion

        #region Coin sugar syntax
        public bool TossUp() => TossUp(.5f);
        public bool TossUp(float chanceToWin) => Next() <= chanceToWin;
        public bool TossUpToBeat(float chanceToLose) => TossUp(1 - chanceToLose);
        public bool TossUpPercentage(float percentageChangeToWin) => TossUp(percentageChangeToWin / 100);
        #endregion

        #region Dice sugar syntax
        public int RollDie() => RollDieOfFaces(6);
        public int RollDieOfFaces(int facesAmount) => Next(1, facesAmount + 1);
        #endregion

        #region GetRandom sugar syntax
        public T GetRandom<T>(ICollection<T> collection)
        {
            var randomMemberIndex = Next(0, collection.Count);
            return collection.ElementAt(randomMemberIndex);
        }
        
        public T GetRandom<T>(IEnumerable<T> enumerable)
        {
            return GetRandom(enumerable.ToList());
        }

        public T GetRandom<T>(IList<T> list)
        {
            var randomMemberIndex = Next(0, list.Count);
            return list[randomMemberIndex];
        }
        #endregion
    }
}