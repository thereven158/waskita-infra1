using Agate.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet
{
    public class SpriteSheetToggleDisplayPool : DataToggleDisplayPool<ISpriteSheet>
    {
        [SerializeField]
        private SpriteSheetToggleDisplay _objectToPool = default;

        protected override DataToggleDisplayBehavior<ISpriteSheet> ObjectToPool => _objectToPool;
    }
}