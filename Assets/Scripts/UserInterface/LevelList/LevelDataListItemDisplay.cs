using A3.Unity;
using Agate.WaskitaInfra1.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.LevelList
{
    public class LevelDataListItemDisplay: InteractiveDisplayBehavior<LevelData>
    {
        [SerializeField]
        private Button _button = default;
        [SerializeField]
        private TMP_Text _nameText = default;

        internal Button Button => _button;

        private void Awake()
        {
            _button.onClick.AddListener(Interaction);
        }

        protected override void ConfigureDisplay(LevelData data)
        {
            _nameText.text = data.Name;
        }

        private void Interaction()
        {
            Interact();
        }
    }
}