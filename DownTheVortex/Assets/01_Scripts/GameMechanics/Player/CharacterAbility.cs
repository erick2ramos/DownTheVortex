using UnityEngine;
using System.Collections;

namespace Gameplay.Ability
{
    public abstract class CharacterAbility : MonoBehaviour
    {
        public bool IsPermitted;
        protected Character _character;

        public virtual void Initialize(Character character)
        {
            _character = character;
        }

        public abstract void ProcessAbility();
    }
}