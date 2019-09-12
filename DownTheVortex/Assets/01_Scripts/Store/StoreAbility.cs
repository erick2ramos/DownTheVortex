using UnityEngine;
using System.Collections;

namespace Store
{
    /// <summary>
    /// An ability representation to be used in the store and
    /// purchased by the player 
    /// </summary>
    [System.Serializable]
    public class StoreAbility
    {
        public int AbilityID;
        public string Name;
        public string Description;
        public int Price;
        public Sprite AbilityHelperImage;
    }
}