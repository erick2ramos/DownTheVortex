using UnityEngine;
using System.Collections;

namespace Gameplay.Ability
{
    public class SwapSideAbility : CharacterAbility
    {
        enum AbilityInternalState
        {
            Starting,
            Processing,
            Deactivate,
            Count
        }

        public float ChangeSpeed;

        delegate void StateMachineDelegate();
        StateMachineDelegate[] _stateMachine;
        AbilityInternalState _currentState;

        Vector3 _vectorUpDelta;
        Vector3 _targetPosition;
        Vector3 _vectorUp;
        Vector3 _currentForce;
        float _maxDistance;
        Transform _body;

        public override void Initialize(Character character)
        {
            base.Initialize(character);
            _stateMachine = new StateMachineDelegate[(int)AbilityInternalState.Count];
            _stateMachine[(int)AbilityInternalState.Starting] = StartingState;
            _stateMachine[(int)AbilityInternalState.Processing] = ProcessingState;
            _stateMachine[(int)AbilityInternalState.Deactivate] = DeactivateState;

            _currentState = AbilityInternalState.Starting;
            _body = character.Model;
        }

        public override void ProcessAbility()
        {
            _stateMachine[(int)_currentState]();
        }

        private void StartingState()
        {
            _vectorUpDelta = _character.Pivot.position - _body.position;
            _targetPosition = _body.position + (2 * _vectorUpDelta);
            _maxDistance = _vectorUpDelta.magnitude;
            _vectorUp = _vectorUpDelta.normalized;
            _currentForce = _vectorUp * ChangeSpeed;
            _currentState = AbilityInternalState.Processing;
        }

        private void ProcessingState()
        {
            _body.position += _currentForce * Time.deltaTime;
            //_currentForce += ()

            if (Vector3.Distance(_body.position, _character.Pivot.position) >= _maxDistance)
            {
                _character.CurrentAngle += 180;
                _character.Pivot.localRotation = Quaternion.Euler(0, 0, _character.CurrentAngle);
                _body.position = _targetPosition;
                _currentState = AbilityInternalState.Deactivate;
            }
        }

        private void DeactivateState()
        {
            enabled = false;
            _currentState = AbilityInternalState.Starting;
        }
    }
}