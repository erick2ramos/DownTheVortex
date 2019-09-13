using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;
using BaseSystems.DataPersistance;
using BaseSystems.Managers;
using UnityEngine.UI;
using Gameplay.Ability;

namespace Store
{
    public class StoreSceneModel : SceneModel
    {
        public SceneModel parentSceneModel;
    }

    public class StoreSceneController : SceneController<StoreSceneModel>
    {
        public static event System.Action<AbilityConfig> OnAbilityPurchased;

        [SerializeField]
        public AbilitiesConfig _allAbilities;
        [SerializeField]
        StoreAbilityVisual _storeItemPrefab;
        [SerializeField]
        Transform _itemsHolder;
        [SerializeField]
        Text _currencyLabel;

        public override IEnumerator Initialization()
        {
            int activeAbility = DataPersistanceManager.PlayerData.ActiveAbility;
            // Initialize the store
            // show the buyable abilities
            foreach (var storeAbility in _allAbilities.Abilities)
            {
                StoreAbilityVisual visual = Instantiate(_storeItemPrefab, _itemsHolder, false);
                visual.Initialize(this, storeAbility, storeAbility.AbilityID == activeAbility);
            }
            _currencyLabel.text = DataPersistanceManager.PlayerData.CurrentCurrency.ToString("n0");
            yield return null;
        }

        public void Purchase(StoreAbilityVisual storeItem, AbilityConfig config)
        {
            if (DataPersistanceManager.PlayerData.CurrentCurrency >= config.Price)
            {
                // Store in persistance the ability equiped after discounting the 
                // ability price from the player currency
                DataPersistanceManager.PlayerData.CurrentCurrency -= config.Price;
                DataPersistanceManager.PlayerData.ActiveAbility = config.AbilityID;
                ManagerHandler.Get<DataPersistanceManager>().Save();
                OnAbilityPurchased?.Invoke(config);
                _currencyLabel.text = DataPersistanceManager.PlayerData.CurrentCurrency.ToString("n0");
            }
        }

        public void Back()
        {
            ManagerHandler.Get<SceneTransitionManager>().LoadScene(SceneIndex.GameScene, Model.parentSceneModel);
        }
    }
}