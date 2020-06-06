using Agate.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet
{
    public class SpriteSheetToggleDisplay : DataToggleDisplayBehavior<ISpriteSheet>
    {
        [SerializeField]
        private SpriteSheetDisplay _display = default;

        protected override void ConfigureDisplay(ISpriteSheet data)
        {
            base.ConfigureDisplay(data);
            _display.SetSpriteSheet(data);
        }
    }
}