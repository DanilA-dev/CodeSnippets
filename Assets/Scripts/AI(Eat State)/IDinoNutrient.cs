using AgentAI;
using Aviary;
using UnityEngine;

namespace Dinos.Nutrients
{
    public enum NutrientType
    {
        Food,
        Water
    }

    public interface IDinoNutrient
    {
        public float FeedTime { get; }
        public float FeedDistance { get; }
        public Transform FeedPoint { get; }
        public NutrientType Type { get; }

        public bool IsNutrientReachable();
        public bool HasNutrient(Transform transform);
        public void SetStat(AgentStateController stateController);
        public void SetAviaryFeeder(AviaryFeeder feeder);
        public void IncreaseNutrientValue();
    }

    public interface IDinoFoodNutrient : IDinoNutrient
    {
        public DinoFoodItem FoodItem { get; }
    }

    public interface IDinoWaterNutrient : IDinoNutrient { }


}
