using Agate.WaskitaInfra1.Level;
using TMPro;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class ImgTextToggleDisplay : DataToggleDisplayBehavior<ScriptableImgText>
    {
        [SerializeField]
        private TMP_Text _text = default;
        
        protected override void ConfigureDisplay(ScriptableImgText data)
        {
            base.ConfigureDisplay(data);
            _text.text = data.Text;
        }
    }
}