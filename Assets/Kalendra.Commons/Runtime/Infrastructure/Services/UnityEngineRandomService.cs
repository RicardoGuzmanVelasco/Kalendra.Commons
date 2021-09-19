using Kalendra.Commons.Runtime.Domain;
using Kalendra.Commons.Runtime.Domain.Services;
using UnityEngine;

namespace Kalendra.Commons.Runtime.Infraestructure.Services
{
    /// <summary>
    /// Performance: 1000 requests : 0.1s, 100000 requests: 98s.
    /// </summary>
    public sealed class UnityEngineRandomService : TemplateRandomService
    {
        int timesRequested;
        int currentSeed;
        
        #region Accesors
        public override int Seed
        {
            set
            {
                currentSeed = value;
                Random.InitState(value);
                
            }
        }
        #endregion

        #region Constructors
        public UnityEngineRandomService(int seed) => Seed = seed;
        #endregion

        #region Random providing
        public override int Next(int inclusiveMin, int exclusiveMax)
        {
            ResetSeed();
            return Random.Range(inclusiveMin, exclusiveMax);
        }
        
        public override float Next()
        {
            ResetSeed();
            return Random.value;
        }

        public override float Next(float inclusiveMin, float inclusiveMax)
        {
            ResetSeed();
            return Random.Range(inclusiveMin, inclusiveMax);
        }
        #endregion
        
        void ResetSeed()
        {
            Random.InitState(currentSeed);
            for(var i = 0; i < timesRequested; i++)
                Random.Range(0, 1f);
            timesRequested++;
        }

    }
}