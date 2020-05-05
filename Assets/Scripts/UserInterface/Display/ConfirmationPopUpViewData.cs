using System;

namespace UserInterface.Display
{
    [Serializable]
    public struct ConfirmationPopUpViewData
    {
        public string TitleText;
        public string MessageText;
        public string ConfirmButtonText;
        public string CloseButtonText;
        public Action ConfirmAction;
        public Action CloseAction;
    }
}