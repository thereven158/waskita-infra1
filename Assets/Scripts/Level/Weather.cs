using UnityEngine;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(fileName = "Weather", menuName = "WaskitaInfra1/Weather", order = 0)]
    public class Weather : ScriptableObject
    {
        [SerializeField]
        private Sprite _image = null;

        public Sprite Image => _image;
    }
}