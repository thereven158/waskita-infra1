using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class SpriteTogglesDisplaySystem : DataTogglesDisplaySystem<Sprite>
    {
        [SerializeField]
        private SpriteToggleDisplayPool _togglePool = default;

        protected override DataToggleDisplayPool<Sprite> TogglePool => _togglePool;
    }
}