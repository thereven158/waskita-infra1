using A3.UserInterface;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Display
{
    public class YesNoPopUpDisplay : DisplayBehavior
    {
        [SerializeField]
        private YesNoPopUpViewData defaultData = default;

        [SerializeField]
        private TMP_Text _titleText = default;

        [SerializeField]
        private TMP_Text _messageText = default;

        [SerializeField]
        private TMP_Text _yesText = default;

        [SerializeField]
        private Button _yesButton = default;

        [SerializeField]
        private TMP_Text _noText = default;

        [SerializeField]
        private Button _noButton = default;

        private Action _onNo;
        private Action _onClose;
        private Action _onYes;

        public override void Init()
        {
            gameObject.SetActive(false);
            _yesButton.onClick.AddListener(Yes);
            _noButton.onClick.AddListener(No);
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public void Open(string text, Action onYes, Action onNo, Action onClose = null)
        {
            Open(new YesNoPopUpViewData()
            {
                MessageText = text,
                CloseAction = onClose,
                YesAction = onYes,
                NoAction = onNo
            });
            Open();
        }

        public void Open(YesNoPopUpViewData data)
        {
            _titleText.text = string.IsNullOrEmpty(data.TitleText)
                ? defaultData.TitleText
                : data.TitleText;
            _messageText.text = string.IsNullOrEmpty(data.MessageText)
                ? defaultData.MessageText
                : data.MessageText;
            _noText.text = string.IsNullOrEmpty(data.NoButtonText)
                ? defaultData.NoButtonText
                : data.NoButtonText;
            _yesText.text = string.IsNullOrEmpty(data.YesButtonText)
                ? defaultData.YesButtonText
                : data.YesButtonText;
            _onClose = data.CloseAction;
            _onYes = data.YesAction;
            _onNo = data.NoAction;
            Open();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _onClose?.Invoke();
            _onClose = null;
            _onYes = null;
            _onNo = null;
        }

        public override bool IsOpen => gameObject.activeSelf;

        private void Yes()
        {
            _onYes?.Invoke();
            Close();
        }

        private void No()
        {
            _onNo?.Invoke();
            Close();
        }
    }
}