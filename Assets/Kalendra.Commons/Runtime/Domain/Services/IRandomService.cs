using System.Collections.Generic;

namespace Kalendra.Commons.Runtime.Domain.Services
{
    public interface IRandomService
    {
        int Seed { set; }

        int Next(int min, int exclusiveMax);

        float Next();
        float Next(float min, float max);
        
        bool TossUp();
        bool TossUp(float chanceToWin);
        bool TossUpToBeat(float chanceToLose);
        bool TossUpPercentage(float percentageChangeToWin);

        int RollDie();
        int RollDieOfFaces(int facesAmount);

        T GetRandom<T>(IEnumerable<T> enumerable);
        T GetRandom<T>(ICollection<T> collection);
        T GetRandom<T>(IList<T> list);
    }
}