using Upclimbing.Genes.Data;

namespace Upclimbing.Genes
{
    public class BallCountGene : BaseGene
    {
        private MultiGeneData _data;
        private Player _player;
        private int _endBallCount;
        private int _defaultBallCount;


        public BallCountGene(int currentLevel, GameState gameState, MultiGeneData data) : base(currentLevel, gameState)
        {
            _data = data;
            _defaultBallCount = 3;
        }

        public override void Visit(Player player)
        {
            _player = player;
            _endBallCount = (int) GetModificationByLevel();
            _player.PlayerDataSettings.BallsCount.Value += _endBallCount;
        }

        public override object GetModificationByLevel()
        {
            var settings = _data.GetSettingsByLevel(_currentLevel);
            return settings?.AdditionalBallAmount ?? 0;
        }

        public override void Dispose()
        {
            base.Dispose();
            _player.PlayerDataSettings.BallsCount.Value = _defaultBallCount;
        }
    }
}