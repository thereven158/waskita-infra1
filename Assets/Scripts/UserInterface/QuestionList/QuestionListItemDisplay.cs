using A3.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class QuestionListItemDisplay : InteractiveDisplayBehavior<QuestionListItemViewData>
    {
        [SerializeField]
        private Button _button = default;

        [SerializeField]
        private Image _buttonImage = default;

        [SerializeField]
        private Sprite _trueSprite;

        [SerializeField]
        private Sprite _falseSprite;

        [SerializeField]
        private TMP_Text _nameText = default;

        [SerializeField]
        private Toggle _toggle = default;

        internal Button Button => _button;

        private void Awake()
        {
            _button.onClick.AddListener(Interaction);
        }

        protected override void ConfigureDisplay(QuestionListItemViewData data)
        {
            _buttonImage.sprite = data.State ? _trueSprite : _falseSprite;
            _nameText.text = data.Item.DisplayName;
            _toggle.isOn = data.State;
        }

        private void Interaction()
        {
            Interact();
        }
    }
}