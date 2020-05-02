using UnityEngine;

namespace Agate.WaskitaInfra1.Object
{
    [CreateAssetMenu(fileName = "SoilCondition", menuName = "WaskitaInfra1/SoilCondition", order = 0)]
    public class SoilCondition : ScriptableObject
    {
        [SerializeField]
        private Sprite _image = null;
        public Sprite Image => _image;
    }
}