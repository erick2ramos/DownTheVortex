using UnityEngine;
using System.Collections;

namespace Gameplay.Ability
{
    /// <summary>
    /// Allows the character to jump towards the center of the vortex
    /// </summary>
    public class JumpAbility : CharacterAbility
    {
        enum AbilityInternalState
        {
            Starting,
            Processing,
            Deactivate,
            Count
        }

        public float JumpForce;
        public float Gravity;

        delegate void StateMachineDelegate();
        StateMachineDelegate[] _stateMachine;
        AbilityInternalState _currentState;

        Vector3 _vectorUpDelta;
        Vector3 _jumpPoint;
        Vector3 _vectorUp;
        Vector3 _currentForce;
        float _maxDistance;
        Transform _body;

        public override void Initialize(Character character)
        {
            base.Initialize(character);
            // Setup of the simple static state machine
            _stateMachine = new StateMachineDelegate[(int)AbilityInternalState.Count];
            _stateMachine[(int)AbilityInternalState.Starting] = GroundedState;
            _stateMachine[(int)AbilityInternalState.Processing] = AiredState;
            _stateMachine[(int)AbilityInternalState.Deactivate] = DeactivateState;

            _currentState = AbilityInternalState.Starting;
            _body = character.Model;
        }

        public override void ProcessAbility()
        {
            _stateMachine[(int)_currentState]();
        }

        private void GroundedState()
        {
            _jumpPoint = _body.position;
            _vectorUpDelta = _character.Pivot.position - _body.position;
            _maxDistance = _vectorUpDelta.magnitude;
            _vectorUp = _vectorUpDelta.normalized;
            _currentForce = _vectorUp * JumpForce;
            _currentState = AbilityInternalState.Processing;
        }

        private void AiredState()
        {
            _body.position += _currentForce * Time.deltaTime;
            _currentForce += (-_vectorUp * Gravity);

            if (Vector3.Distance(_body.position, _character.Pivot.position) >= _maxDistance)
            {
                _body.position = _jumpPoint;
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