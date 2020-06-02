using Agate.WaskitaInfra1.Level;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet
{
    public class TextSpriteSheetToggleDisplayPool : DataToggleDisplayPool<ITextSpriteSheet>
    {
        [SerializeField]
        private TextSpriteSheetToggleDisplay _objectToPool = default;

        protected override DataToggleDisplayBehavior<ITextSpriteSheet> ObjectToPool => _objectToPool;
    }
}