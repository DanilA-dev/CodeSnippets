using UnityEngine;
using Upclimbing.Genes.Data;

namespace Upclimbing.Genes
{
    public class TotalDamageGene : BaseGene
    {
        private AggressiveGeneData _data;
        private int _totalDamage;
        private int _defaultPlayerDamage;
        private Player _player;

        public TotalDamageGene(int currentLevel, GameState gameState, AggressiveGeneData aggressiveGeneData) : base(
            currentLevel, gameState)
        {
            _data = aggressiveGeneData;
        }

        public override void Visit(Player player)
        {
            _player = player;
            _defaultPlayerDamage = player.PlayerDataSettings.BallDamage.Value;
            _totalDamage = _player.PlayerDataSettings.BallDamage.Value * (1 + (int) GetModificationByLevel() / 100);
            _player.PlayerDataSettings.BallDamage.Value += _totalDamage;
        }

        public override object GetModificationByLevel()
        {
            return _data.GetSettings(_currentLevel).AdditionalDamageAmountProcentage;
        }

        public override object BallCollideAction()
        {
            var critProcentage = 1;
            var rand = Random.Range(0, 100);
            var isCrit = critProcentage >= rand;
            if (isCrit)
            {
                var crit = _totalDamage * (1 + 300 / 100);
                _player.TotalDamage.Value = crit;
            }

            _player.TotalDamage.Value = _totalDamage;
            return isCrit;
        }

        public override void Dispose()
        {
            _player.PlayerDataSettings.BallDamage.Value = _defaultPlayerDamage;
        }
    }
}