using System;

namespace Agate.WaskitaInfra1.UserInterface.Display
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