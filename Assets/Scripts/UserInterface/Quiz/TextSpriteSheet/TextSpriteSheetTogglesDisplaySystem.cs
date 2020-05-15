using Agate.SpriteSheet;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class TextSpriteSheetTogglesDisplaySystem : DataTogglesDisplaySystem<ITextSpriteSheet>
    {
        [SerializeField]
        private TextSpriteSheetToggleDisplayPool _togglePool = default;
        [SerializeField]
        private SpriteSheetDisplay _display = default;

        [SerializeField]
        private GameObject _placeholderObject = default;
        
        protected override void OnDataDisplayInteraction(ITextSpriteSheet obj)
        {
            base.OnDataDisplayInteraction(obj);
            ToggleImageContent(true);
            _display.SetSpriteSheet(obj.SpriteSheet);
        }

        public override void Reset()
        {
            ToggleImageContent(false);
            base.Reset();
        }

        private void ToggleImageContent(bool toggle)
        {
            _placeholderObject.SetActive(!toggle);
            _display.gameObject.SetActive(toggle);
        }
        protected override DataToggleDisplayPool<ITextSpriteSheet> TogglePool => _togglePool;
    }
}