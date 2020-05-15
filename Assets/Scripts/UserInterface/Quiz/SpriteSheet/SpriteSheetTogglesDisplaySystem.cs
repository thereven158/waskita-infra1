using Agate.SpriteSheet;
using Agate.WaskitaInfra1.UserInterface.Quiz.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Quiz
{
    public class SpriteSheetTogglesDisplaySystem : DataTogglesDisplaySystem<ISpriteSheet>
    {
        [SerializeField]
        private SpriteSheetToggleDisplayPool _togglePool = default;
        
        protected override DataToggleDisplayPool<ISpriteSheet> TogglePool => _togglePool;
    }
}