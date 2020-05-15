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
        private ConfirmationPopUpViewData defaultData = default;

        [SerializeField]
        private TMP_Text _titleText = default;

        [SerializeField]
        private TMP_Text _messageText = default;

        [SerializeField]
        private TMP_Text _confirmText = default;

        [SerializeField]
        private Button _confirmButton = default;

        [SerializeField]
        private TMP_Text _closeText = default;

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
            Open(new ConfirmationPopUpViewData()
            {
                MessageText = text,
                CloseAction = onClose,
                ConfirmAction = onConfirm
            });
            Open();
        }

        public void Open(ConfirmationPopUpViewData data)
        {
            _titleText.text =  string.IsNullOrEmpty(data.TitleText)
                ? defaultData.TitleText
                : data.TitleText;
            _messageText.text =  string.IsNullOrEmpty(data.MessageText)
                ? defaultData.MessageText
                : data.MessageText;
            _closeText.text =  string.IsNullOrEmpty(data.CloseButtonText)
                ? defaultData.CloseButtonText
                : data.CloseButtonText;
            _confirmText.text = string.IsNullOrEmpty(data.ConfirmButtonText)
                ? defaultData.ConfirmButtonText
                : data.ConfirmButtonText;
            _onClose = data.CloseAction;
            _onConfirm = data.ConfirmAction;
            Open();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _onClose?.Invoke();
        }

        public override bool IsOpen => gameObject.activeSelf;

        private void Confirm()
        {
            _onConfirm?.Invoke();
            Close();
        }
    }
}