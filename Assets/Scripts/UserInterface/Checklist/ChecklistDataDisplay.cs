using A3.Unity;
using A3.Quiz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class ChecklistDataDisplay : InteractiveDisplayBehavior<IQuiz>
    {
        [SerializeField]
        private Button _button = default;
        [SerializeField]
        private TMP_Text _nameText = default;

        internal Button Button => _button;

        private int _counter = 0;
        
        private void Awake()
        {
            _button.onClick.AddListener(Interaction);
        }

        protected override void ConfigureDisplay(IQuiz data)
        {
            _nameText.text = data.Question + "";
        }

        private void Interaction()
        {
            Interact();
        }
    }
}