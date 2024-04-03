using System.Linq;
using Genes.PersistentData;
using UniRx;
using UnityEngine;
using Upclimbing.Core;
using Upclimbing.Genes.Data;
using Upclimbing.Genes.Services;
using Upclimbing.Ui;
using Upclimbing.Ui.Signals;
using Zenject;

namespace Scriptables.Genes
{
    public class GeneBuyController : MonoBehaviour
    {
        [SerializeField] private int _initDeposit;
        
        private Currency _geneCurrency;
        private CurrenciesData _currenciesData;
        private IMessageBroker _messageBroker;
        private IGenePersistentDataService _genePersistentData;

        [Inject]
        private void Construct(IGenePersistentDataService persistentDataService, CurrenciesData currenciesData)
        {
            _genePersistentData = persistentDataService;
            _currenciesData = currenciesData;

            _geneCurrency = _currenciesData.Currencies.FirstOrDefault(c => c.Type == CurrencyType.Genes);
            
            Initalize();
        }
        
        private void Initalize()
        {
            _messageBroker = MessageBroker.Default;
            _messageBroker.Receive<GeneUISignals.Buy>()
                .Subscribe(_ => TryBuyGene(_.ViewItem));
            
            _geneCurrency.Deposit(_initDeposit);
        }

        private void TryBuyGene(GeneViewItem viewItem)
        {
            var price = viewItem.Data.GetNextUpgrade().UpgradeCost;
            if (_geneCurrency.Value >= price)
            {
                _geneCurrency.Withdraw(price);
                viewItem.Data.ChangeState(GeneState.Bought);
                viewItem.Data.UpdateLevel();
                _genePersistentData.Save(viewItem.Data);
                _messageBroker.Publish(new GeneSignals.BuyGene(viewItem.Data));
            }
        }
    }
}