using System.Collections.Generic;
using Agate.SpriteSheet;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.SpriteSheet
{
    [CreateAssetMenu(fileName = "SpriteSheet", menuName = "WaskitaInfra1/SpriteSheet", order = 0)]
    public class ScriptableSpriteSheet : ScriptableObject, ISpriteSheet
    {
        [SerializeField]
        private Sprite[] _sprites = default;

        [SerializeField]
        private int _framerate;

        [SerializeField]
        private bool _loop;

        public IEnumerable<Sprite> Sprites => _sprites;
        public int Framerate => _framerate;
        public bool Loop => _loop;
    }
}