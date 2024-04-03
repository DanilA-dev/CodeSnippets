namespace Upclimbing.Genes
{
    public abstract class BaseGene : IGeneVisitor
    {
        protected int _currentLevel;
        protected GameState _gameState;
        
        public BaseGene(int currentLevel, GameState gameState)
        {
            _currentLevel = currentLevel;
            _gameState = gameState;
        }

        public virtual void Visit(Player player) {}

        public virtual void Dispose(){}
        
        public virtual object StartTurnAction() => 0;
        public virtual object EndTurnAction() => 0;
        public virtual object ShootAction() => 0;
        public virtual object BallCollideAction() => 0;
        public abstract object GetModificationByLevel();
    }
}