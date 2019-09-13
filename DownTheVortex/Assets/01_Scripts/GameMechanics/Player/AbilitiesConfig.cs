using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Gameplay.Ability
{
    [CreateAssetMenu(fileName ="CharacterAbilitiesConfiguration", menuName ="Character/Abilities configuration")]
    public class AbilitiesConfig : ScriptableObject
    {
        public List<AbilityConfig> Abilities;

        public AbilityConfig GetByID(int id)
        {
            return Abilities.Find(x => x.AbilityID == id);
        }
    }
}