using UniRx;
using Upclimbing.Genes.Data;

namespace Upclimbing.Genes
{
    public class TotalHeatlhGene : BaseGene
    {
        private StrongGeneData _data;
        private Player _player;
        private bool _isPlayerDamaged;
        private int _defaultPlayerHealth;

        public TotalHeatlhGene(int currentLevel, GameState gameState, StrongGeneData strongGeneData) : base(currentLevel, gameState)
        {
            _data = strongGeneData;
            _gameState.CurrentCycle.Subscribe((i => UpdateCycle()));
        }
           
        public override void Visit(Player player)
        {
            base.Visit(player);
            
            _player = player;
            _defaultPlayerHealth = _player.PlayerDataSettings.InitialHealth.Value;
            _player.OnTakeDamage += OnPlayerDamaged;
            _player.PlayerDataSettings.InitialHealth.Value += (int) GetModificationByLevel();
        }

        public override object GetModificationByLevel()
        {
            return _data.GeneSettings(_currentLevel).AdditionalHealth;
        }
        
        public override object EndTurnAction()
        {
            int healAmount = _data.HealAmountSpecialValue;
            int endHeal = _isPlayerDamaged ? 0 : healAmount;
            _player.HealPlayer(endHeal);
            return endHeal;
        }
        
        private void UpdateCycle()
        {
            _isPlayerDamaged = false;
        }
        
        private void OnPlayerDamaged(TakeDamageEvent damageEvent)
        {
            _isPlayerDamaged = true;
        }

        public override void Dispose()
        {
            _player.PlayerDataSettings.InitialHealth.Value = _defaultPlayerHealth;
            _player.OnTakeDamage -= OnPlayerDamaged;
        }
    }
}