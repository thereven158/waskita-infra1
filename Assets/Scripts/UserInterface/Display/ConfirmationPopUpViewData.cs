using System;

namespace Agate.WaskitaInfra1.UserInterface.Display
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