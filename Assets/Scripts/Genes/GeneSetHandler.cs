using System.Collections.Generic;
using Genes.PersistentData;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Upclimbing.Genes.Data;
using Upclimbing.Ui.Signals;
using Zenject;

namespace Upclimbing.Genes.Services
{
    public class GeneSetHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly] private List<BaseGeneData> _activeGenes = new List<BaseGeneData>();

        private Player _player;
        private IMessageBroker _messageBroker;
        private IGenePersistentDataService _genePersistentData;
        private GeneContainer _geneContainer;
        
        [Inject]
        private void Construct(Player player, IGenePersistentDataService persistentDataService, GeneContainer geneContainer)
        {
            _player = player;
            _genePersistentData = persistentDataService;
            _geneContainer = geneContainer;
            _messageBroker = MessageBroker.Default;
            
            _messageBroker.Receive<GeneUISignals.Equip>()
                .Subscribe(_ => AddAndEquipGene(_.ViewItem.Data)).AddTo(gameObject);
            
            _messageBroker.Receive<GeneUISignals.Remove>()
                .Subscribe(_ => RemoveGene(_.ViewItem.Data)).AddTo(gameObject);
        }

        private void Awake()
        {
            LoadGenes();
        }

        private void LoadGenes()
        {
            var loadedGenes = _geneContainer.GeneDatas;
            {
                foreach (var geneData in loadedGenes)
                {
                    if (geneData.State == GeneState.Equiped)
                        AddAndEquipGene(geneData);
                }
            }
        }

        private void AddAndEquipGene(BaseGeneData geneData)
        {
            if (geneData.State != GeneState.Bought)
                return;

            if (_activeGenes.Contains(geneData))
                return;
            
            EquipGene(geneData);
            _activeGenes.Add(geneData);
            _genePersistentData.Save(geneData);
            _messageBroker.Publish(new GeneSignals.EquipGene(geneData));
        }

        private void RemoveGene(BaseGeneData geneData)
        {
            if (geneData.State != GeneState.Equiped)
                return;

            if (!_activeGenes.Contains(geneData))
                return;
            
            UnEquipGene(geneData);
            _activeGenes.Remove(geneData);
            _genePersistentData.Save(geneData);
            _messageBroker.Publish(new GeneSignals.RemoveGene(geneData));
        }

        private void EquipGene(BaseGeneData geneData)
        {
            geneData.ChangeState(GeneState.Equiped);
           _player.AcceptVisit(geneData.Gene);
        }
        
        private static void UnEquipGene(BaseGeneData geneData)
        {
            geneData.ChangeState(GeneState.Bought);
            geneData.Gene.Dispose();
        }
    }
}