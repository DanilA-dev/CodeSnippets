using AgentAI;
using Aviary;
using AgentAI.Stats;
using System;
using UnityEngine;

namespace Dinos.Nutrients
{
    [Serializable]
    public class DinoWaterNutrient : IDinoWaterNutrient
    {
        [SerializeField, Range(3, 10)] private float _drinkTime;
        [SerializeField, Min(0)] private float _drinkPointStopDistance;
        [SerializeField] private float _waterStatValue;

        private WaterStat _waterStat;
        private AviaryFeeder _feeder;

        public float FeedTime => _drinkTime;
        public float FeedDistance => _drinkPointStopDistance;
        public Transform FeedPoint => _feeder.WaterPoint;
        public NutrientType Type => NutrientType.Water;


        public void IncreaseNutrientValue()
        {
            if (null == _feeder)
                return;

            _feeder.TryWaterDino(_drinkTime);
            _waterStat.Value += _waterStatValue;
            _waterStat.Reset();
        }

        public bool IsNutrientReachable()
        {
           return !_feeder.IsWaterBowlBusy;
        }

        public void SetStat(AgentStateController stateController)
        {
            _waterStat = _waterStat ?? (WaterStat)stateController.GetComponent<AgentStatsManager>().GetStat<WaterStat>();
        }

        public void SetAviaryFeeder(AviaryFeeder feeder)
        {
            _feeder = feeder;
        }

        public bool HasNutrient(Transform transform)
        {
            return null != _feeder && _feeder.HasWater();
        }
    }

}
