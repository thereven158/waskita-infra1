using System;

namespace UserInterface.Display
{
    [Serializable]
    public class YesNoPopUpViewData
    {
        public string TitleText;
        public string MessageText;
        public string YesButtonText;
        public string NoButtonText;
        public Action YesAction;
        public Action NoAction;
        public Action CloseAction;
    }
}