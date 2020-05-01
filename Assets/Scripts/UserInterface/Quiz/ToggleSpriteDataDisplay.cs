using A3.Unity;
using Agate.WaskitaInfra1.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ToggleSpriteDataDisplay : InteractiveDisplayBehavior<Sprite>
    {
        [SerializeField]
        private Image _activeToggleImage = default;
        [SerializeField]
        private Image _inactiveToggleImage = default;
        [SerializeField]
        private Toggle _toggle = default;

        public Toggle Toggle => _toggle; 

        private void Awake()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        
        protected override void ConfigureDisplay(Sprite data)
        {
            _toggle.isOn = false;
            _activeToggleImage.sprite = data;
            _inactiveToggleImage.sprite = data;
        }

        private void OnToggleValueChanged(bool value)
        {
            if (value) Interact();
        }
    }
}