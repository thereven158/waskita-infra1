using TMPro;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class StringToggleDisplay : DataToggleDisplayBehavior<string>
    {
        [SerializeField]
        private TMP_Text _text = default;

        protected override void ConfigureDisplay(string data)
        {
            base.ConfigureDisplay(data);
            _text.text = data;
        }
    }
}