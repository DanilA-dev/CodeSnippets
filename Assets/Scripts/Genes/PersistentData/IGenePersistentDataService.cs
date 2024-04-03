using System.Collections.Generic;
using Upclimbing.Genes.Data;

namespace Genes.PersistentData
{
    public interface IGenePersistentDataService
    {
        public void Save(BaseGeneData data);
        public void Load(List<BaseGeneData> datas);
    }
}