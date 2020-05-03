using System;
using A3.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Display
{
    public class ConfirmationPopUpDisplay : DisplayBehavior
    {
        [SerializeField]
        private TMP_Text _text = default;

        [SerializeField]
        private Button _confirmButton = default;
        
        [SerializeField]
        private Button _closeButton = default;

        private Action _onClose;
        private Action _onConfirm;
        
        public override void Init()
        {
            gameObject.SetActive(false);
            _confirmButton.onClick.AddListener(Confirm);
            _closeButton.onClick.AddListener(Close);
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }
        public void Open(string text, Action onConfirm, Action onClose)
        {
            _text.text = text;
            _onClose = onClose;
            _onConfirm = onConfirm;
            Open();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _onClose?.Invoke();
        }

        public override bool IsOpen => gameObject.activeSelf;
        
        private  void OnCloseButton()
        {
            Close();
        }
        private void Confirm()
        {
            _onConfirm?.Invoke();
            Close();
        }
    }
}