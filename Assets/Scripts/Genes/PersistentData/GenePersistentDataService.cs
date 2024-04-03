using System.Collections.Generic;
using UnityEngine;
using Upclimbing.Genes.Data;

namespace Genes.PersistentData
{
    public class GenePersistentDataService : IGenePersistentDataService
    {
        private readonly string _geneKey = "_gene";


        public GenePersistentDataService(GeneContainer geneContainer)
        {
            Load(geneContainer.GeneDatas);
        }
        
        public void Save(BaseGeneData data)
        {
            PlayerPrefs.SetInt(_geneKey + data.Name, (int)data.State);
            PlayerPrefs.SetInt(_geneKey + data.Name, data.Level);
        }

        public void Load(List<BaseGeneData> datas)
        {
            foreach (var data in datas)
            {
                data.State = (GeneState)PlayerPrefs.GetInt(_geneKey + data.Name, (int)GeneState.UnLocked);
                data.SetLevel(PlayerPrefs.GetInt(_geneKey + data.Name, 0));  
            }
        }

    }
}