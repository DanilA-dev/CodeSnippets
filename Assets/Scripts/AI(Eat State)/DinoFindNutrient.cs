using Aviary;
using Cysharp.Threading.Tasks;
using Dinos;
using Dinos.Nutrients;
using System;
using System.Threading;
using UnityEngine;

namespace AgentAI.States
{
    public class DinoFindNutrient : BaseAgentState
    {
        [SerializeReference] private IDinoNutrient _nutrientResource;

        private DinoAdultView _view;
        private AgentLocomotion _locomotion;
        private DinoController _dinoAgent;
        private AviaryFeeder _aviaryFeeder;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private void OnDisable()
        {
            _tokenSource?.Cancel();
        }

        public override void Init(AgentStateController stateController)
        {
            base.Init(stateController);

            _view = (DinoAdultView)stateController.ActiveAgent.View;
            _dinoAgent = (DinoController)stateController.ActiveAgent;
            _locomotion = _dinoAgent.Locomotion;
            _aviaryFeeder = _dinoAgent.MyAviary?.Feeder;
            _nutrientResource.SetStat(stateController);
        }

        public override void EnterState()
        {
            base.EnterState();
            _view.DisableRig();
            _aviaryFeeder = _dinoAgent.MyAviary?.Feeder;
            _nutrientResource.SetAviaryFeeder(_aviaryFeeder);
            ExecuteFeedTask();
        }

        public override void ExitState()
        {
            base.ExitState();
            _locomotion.SetEnable(true);
            _locomotion.SetRotation(true);
            _tokenSource?.Cancel();
        }

        private async UniTaskVoid ExecuteFeedTask()
        {
            if (_nutrientResource.HasNutrient(this.transform))
            {
                await GoToNutrientPos(_nutrientResource.FeedPoint.position, _tokenSource.Token);
                await Feeding(_nutrientResource.FeedPoint.position, _tokenSource.Token);
                _stateController.ChangeState(_stateController.StartState);
            }
        }
  

        private async UniTask Feeding(Vector3 rotateFaceTo, CancellationToken cancelToken)
        {
            _locomotion.SetEnable(false);
            _locomotion.SetRotation(false);
            float rotateTime = 3f;
            Vector3 dir = (rotateFaceTo - _dinoAgent.transform.position).normalized;
            dir.y = 0;

            for (float i = 0; i < rotateTime; i += Time.deltaTime)
            {
                Quaternion lookAt = Quaternion.LookRotation(dir, Vector3.up);
                _dinoAgent.transform.rotation = Quaternion.RotateTowards(_dinoAgent.transform.rotation,
                    lookAt, 400 * Time.deltaTime);
                await UniTask.Yield(cancellationToken: cancelToken);
            }

            _view.EnterFeedAnimation(out float length);
            if(_nutrientResource is IDinoFoodNutrient food)
                _view.SetFoodToMouthBone(food.FoodItem, 1.5f, 3);

            _nutrientResource.IncreaseNutrientValue();
            await UniTask.Delay(TimeSpan.FromSeconds(length + _nutrientResource.FeedTime), cancellationToken: cancelToken);
            _locomotion.SetEnable(true);
            _locomotion.SetRotation(true);
        }

        private async UniTask GoToNutrientPos(Vector3 target, CancellationToken cancelToken)
        {
            _locomotion.MoveToPoint(target);
            await UniTask.WaitWhile(() => _nutrientResource.IsNutrientReachable() && Vector3.Distance(_dinoAgent.transform.position, target)
            > _nutrientResource.FeedDistance, cancellationToken: cancelToken);
        }

        public override string ToString()
        {
            return _nutrientResource.Type.ToString();
        }

       
    }
}

