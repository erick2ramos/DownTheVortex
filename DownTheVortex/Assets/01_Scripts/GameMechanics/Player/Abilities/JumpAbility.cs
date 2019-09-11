using UnityEngine;
using System.Collections;

namespace Gameplay.Ability
{
    public class JumpAbility : CharacterAbility
    {
        enum JumpState
        {
            Grounded,
            Aired,
            Deactivate,
            Count
        }

        public float JumpForce;
        public float Gravity;

        delegate void JumpDelegate();
        JumpDelegate[] _stateMachine;
        JumpState _currentState;

        Vector3 _vectorUpDelta;
        Vector3 _jumpPoint;
        Vector3 _vectorUp;
        Vector3 _currentForce;
        float _maxDistance;
        Transform _body;

        public override void Initialize(Character character)
        {
            base.Initialize(character);
            _stateMachine = new JumpDelegate[(int)JumpState.Count];
            _stateMachine[(int)JumpState.Grounded] = GroundedState;
            _stateMachine[(int)JumpState.Aired] = AiredState;
            _stateMachine[(int)JumpState.Deactivate] = DeactivateState;

            _currentState = JumpState.Grounded;
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
            _currentState = JumpState.Aired;
        }

        private void AiredState()
        {
            _body.position += _currentForce;
            _currentForce += (-_vectorUp * Gravity * Time.deltaTime);

            if (Vector3.Distance(_body.position, _character.Pivot.position) >= _maxDistance)
            {
                _body.position = _jumpPoint;
                _currentState = JumpState.Deactivate;
            }
        }

        private void DeactivateState()
        {
            enabled = false;
            _currentState = JumpState.Grounded;
        }
    }
}