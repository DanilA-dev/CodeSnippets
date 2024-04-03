using Upclimbing.Genes.Data;

namespace Upclimbing.Genes.Services
{
    public class GeneSignals
    {
        public class BuyGene
        {
            public BaseGeneData Data { get; private set; }
            public BuyGene(BaseGeneData data)
            {
                Data = data;
            }
        }
        
        public class EquipGene
        {
            public BaseGeneData Data { get; private set; }
            public EquipGene(BaseGeneData data)
            {
                Data = data;
            }
        }
        
        public class RemoveGene
        {
            public BaseGeneData Data { get; private set; }
            public RemoveGene(BaseGeneData data)
            {
                Data = data;
            }
        }
       
    }
}