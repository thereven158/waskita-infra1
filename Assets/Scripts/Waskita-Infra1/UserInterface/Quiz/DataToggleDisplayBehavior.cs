using A3.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class DataToggleDisplayBehavior<TData> : InteractiveDisplayBehavior<TData>
    {
        [SerializeField]
        private GameObject toggleEffects = default;
        [SerializeField]
        private Toggle _toggle = default;

        public Toggle Toggle => _toggle;

        private void Awake()
        {
            Toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected override void ConfigureDisplay(TData data)
        {
            Toggle.isOn = false;

        }

        private void OnToggleValueChanged(bool value)
        {
            toggleEffects.SetActive(value);
            if (value) Interact();
        }

    }
}