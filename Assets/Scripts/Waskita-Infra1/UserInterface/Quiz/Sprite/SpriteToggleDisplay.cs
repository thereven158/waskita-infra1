using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class SpriteToggleDisplay : DataToggleDisplayBehavior<Sprite>
    {
        [SerializeField]
        private Image _image = default;

        protected override void ConfigureDisplay(Sprite data)
        {
            base.ConfigureDisplay(data);
            _image.sprite = data;
        }
    }
}