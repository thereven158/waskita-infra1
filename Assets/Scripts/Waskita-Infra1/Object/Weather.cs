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

        [SerializeField]
        private float _floodHeight = default;

        [SerializeField]
        private float _rainIntensity = default;

        [SerializeField]
        private Material _skybox = default;

        public Sprite Image => _image;

        public string Season => _season;

        public string RainFall => _rainFall;

        public float FloodHeight => _floodHeight;

        public float RainIntensity => _rainIntensity;

        public Material Skybox => _skybox;
    }
}