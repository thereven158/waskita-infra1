using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class SpriteToggleDisplayPool : DataToggleDisplayPool<Sprite>
    {
        [SerializeField]
        private SpriteToggleDisplay _objectToPool = default;

        protected override DataToggleDisplayBehavior<Sprite> ObjectToPool => _objectToPool;
    }
}