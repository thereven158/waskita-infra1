using Agate.SpriteSheet;
using Agate.WaskitaInfra1.UserInterface.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    public class ScriptableTextSpriteSheet : ScriptableObject, ITextSpriteSheet
    {
        [SerializeField]
        [TextArea]
        private string _text = default;

        [SerializeField]
        private ScriptableSpriteSheet _spriteSheet = default;

        public string Text => _text;
        public ISpriteSheet SpriteSheet => _spriteSheet;
    }
}