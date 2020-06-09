using A3.UserInterface;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Display
{
    public class PopUpDisplay : DisplayBehavior
    {
        [SerializeField]
        private TMP_Text _text = default;

        [SerializeField]
        private Button _closeButton = default;

        private Action _onClose;

        public override void Init()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.AddListener(OnCloseButton);
        }

        public void Open(string text, Action onClose)
        {
            _text.text = text;
            _onClose = onClose;
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            Action onClose = _onClose;
            _onClose = null;
            onClose?.Invoke();
        }

        public override bool IsOpen => gameObject.activeSelf;

        private void OnCloseButton()
        {
            Close();
        }
    }
}