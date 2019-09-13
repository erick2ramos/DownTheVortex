using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BaseSystems.DataPersistance;
using Gameplay.Ability;

namespace Store
{
    public class StoreAbilityVisual : MonoBehaviour
    {
        [SerializeField]
        Text _nameLabel, /*_descriptionLabel,*/ _priceLabel;

        [SerializeField]
        GameObject _equipedTag;

        [SerializeField]
        Image _abilityImage;

        [SerializeField]
        Button _purchaseButton;

        StoreSceneController _handler;
        AbilityConfig _config;
        bool _isEquiped;

        public void Initialize(StoreSceneController handler, AbilityConfig config, bool IsEquiped)
        {
            _handler = handler;
            _config = config;

            StoreSceneController.OnAbilityPurchased += Refresh;

            // Setup UI elements
            _nameLabel.text = config.Name;
            //_descriptionLabel.text = config.Description;
            _priceLabel.text = config.Price.ToString("n0");
            _abilityImage.sprite = config.AbilityHelperImage;
            Refresh(IsEquiped ? config : null);
        }

        public void Refresh(AbilityConfig equipedAbility)
        {
            _isEquiped = equipedAbility != null && (_isEquiped = equipedAbility.AbilityID == _config.AbilityID);
            // if the equiped ability is this, show equiped ui elements
            // else set item to be purchaseable
            _equipedTag.SetActive(_isEquiped);
            _purchaseButton.gameObject.SetActive(!_isEquiped);
            _purchaseButton.interactable = DataPersistanceManager.PlayerData.CurrentCurrency >= _config.Price;
        }

        public void Purchase()
        {
            // Notify handler that this ability is requested to be purchased
            _handler.Purchase(this, _config);
        }
    }
}