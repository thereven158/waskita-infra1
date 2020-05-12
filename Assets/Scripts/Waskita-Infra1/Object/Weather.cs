using UnityEngine;

namespace Agate.WaskitaInfra1.Object
{
    [CreateAssetMenu(fileName = "Weather", menuName = "WaskitaInfra1/Weather", order = 0)]
    public class Weather : ScriptableObject
    {
        [SerializeField]
        private Sprite _image = default;

        [SerializeField]
        private string _season = default;

        [SerializeField]
        private string _rainFall = default;

        public Sprite Image => _image;

        public string Season => _season;

        public string RainFall => _rainFall;
    }
}