using AgentAI;
using Aviary;
using AgentAI.Stats;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Dinos.Nutrients
{
    [Serializable]
    public class DinoFoodNutrient : IDinoFoodNutrient
    {
        [SerializeField, Range(3, 10)] private float _eatTime;
        [InfoBox("Dino uses this radius only off aviary")]
        [SerializeField, Range(5, 30)] private float _foodCheckRadius;
        [SerializeField, Min(0)] private float _eatPointStopDistance;
        [Space]
        [SerializeField, ReadOnly] private DinoFoodItem _foodNearBy;

        private FoodStat _foodStat;
        private AviaryFeeder _feeder;

        #region Properties

        public float FeedTime => _eatTime;
        public float FeedDistance => _eatPointStopDistance;
        public Transform FeedPoint => _foodNearBy?.transform;
        public DinoFoodItem FoodItem => _foodNearBy;
        public NutrientType Type => NutrientType.Food;

        #endregion


        public void SetStat(AgentStateController stateController)
        {
            _foodStat = _foodStat ?? stateController.GetComponent<AgentStatsManager>().GetStat<FoodStat>();
        }


        public bool IsNutrientReachable()
        {
            return null != _foodNearBy && _foodNearBy.FoodState != DinoFoodState.InEatProcess;
        }

        public bool TryFindNutrientsAround(Transform center)
        {
            if(null != _feeder && _feeder.GetBowlFood(out var bowlFood))
            {
                _foodNearBy = bowlFood;
                return true;
            }

            Collider[] colls = Physics.OverlapSphere(center.position, _foodCheckRadius);

            if (colls.Length <= 0 || !colls.Any(c => c.TryGetComponent(out DinoFoodItem foodItem)))
                return false;
            else
            {
                var collFood = colls.Where(c => c.TryGetComponent(out DinoFoodItem food) && food.FoodType == _foodStat.FoodType).ToList();
                for (int i = 0; i < collFood.Count; i++)
                {
                    if(collFood[i].TryGetComponent(out DinoFoodItem foodItem) &&
                        foodItem.FoodState == DinoFoodState.Free)
                    {
                        _foodNearBy = foodItem;
                        return true;
                    }
                }
            }
            return false;
        }

        public void IncreaseNutrientValue()
        {
            _foodStat.Value += _foodNearBy.FoodValue;
            _foodStat.Reset();
        }

        public void SetAviaryFeeder(AviaryFeeder feeder)
        {
            _feeder = feeder;
        }

        public bool HasNutrient(Transform transform)
        {
            return TryFindNutrientsAround(transform);
        }
    }

}
