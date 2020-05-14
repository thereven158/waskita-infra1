using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.SpriteSheet
{
    [CreateAssetMenu(fileName = "DataSprites", menuName = "WaskitaInfra1/DataSprites", order = 0)]
    public class ScriptableDataSprites : ScriptableObject
    {
        [SerializeField]
        private Sprite[] _dataSprites = default;

        public Sprite[] DataSprites => _dataSprites;


    }

}
