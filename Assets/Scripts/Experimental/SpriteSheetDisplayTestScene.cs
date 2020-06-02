using Agate.SpriteSheet;
using Agate.WaskitaInfra1.UserInterface.SpriteSheet;
using UnityEngine;

namespace Experimental
{
    public class SpriteSheetDisplayTestScene : MonoBehaviour
    {
        [SerializeField]
        private SpriteSheetDisplay _spritesDisplay = default;
        [SerializeField]
        private ScriptableSpriteSheet _spriteSheet = default;

        private void Start()
        {
            _spritesDisplay.SetSpriteSheet(_spriteSheet);
        }
    }

}
