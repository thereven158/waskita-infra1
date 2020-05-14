using Agate.SpriteSheet;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.SpriteSheet;

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
            //_levelDisplay.OpenDisplay(_level.Object, Debug.Log, null);
            _spritesDisplay.SetSpriteSheet(_spriteSheet);
        }
    }
    
}
