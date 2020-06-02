using Agate.WaskitaInfra1.Level;
using TMPro;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet
{
    public class TextSpriteSheetToggleDisplay : DataToggleDisplayBehavior<ITextSpriteSheet>
    {
        [SerializeField]
        private TMP_Text _text = default;

        protected override void ConfigureDisplay(ITextSpriteSheet data)
        {
            base.ConfigureDisplay(data);
            _text.text = data.Text;
        }
    }
}