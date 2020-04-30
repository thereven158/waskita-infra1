using A3.Unity;
using Agate.WaskitaInfra1.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ToggleSpriteDataDisplay : InteractiveDisplayBehavior<Sprite>
    {
        [SerializeField]
        private Image _activeToggleImage;
        [SerializeField]
        private Image _inactiveToggleImage;
        [SerializeField]
        private Toggle _toggle = default;

        public Toggle Toggle => _toggle; 

        protected override void ConfigureDisplay(Sprite data)
        {
            _toggle.isOn = false;
            _activeToggleImage.sprite = data;
            _inactiveToggleImage.sprite = data;
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool value)
        {
            if (value) Interact();
        }
    }
}