using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.SpriteSheet;

namespace Experimental
{
    public class SpriteSheetDisplayTestScene : MonoBehaviour
    {
        [SerializeField]
        private SpriteSheetDisplay _spritesDisplay = default;
        [SerializeField]
        private ScriptableDataSprites _dataSprites = default;

        private void Start()
        {
            //_levelDisplay.OpenDisplay(_level.Object, Debug.Log, null);
            _spritesDisplay.LoadSpriteSheet(_dataSprites.DataSprites, true, 1f);
        }
    }
    
}
