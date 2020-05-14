using A3.CodePattern;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.LevelList
{
    public class LevelDataDisplayPool : ObjectPool<LevelDataListItemDisplay>
    {
        [SerializeField]
        private LevelDataListItemDisplay _objectToPool;

        protected override LevelDataListItemDisplay ObjectToPool => _objectToPool;
    }
}