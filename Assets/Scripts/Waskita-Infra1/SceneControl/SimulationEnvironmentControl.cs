using UnityEngine;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class SimulationEnvironmentControl : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _rainParticle = default;
        [SerializeField]
        private int _rainIntensity = default;

        private ParticleSystem.EmissionModule _emissionRain;



        [Header("Flood / Bog")]
        [SerializeField]
        private Transform _floodTransform = default;
        [SerializeField]
        public float _floodHeight = default;


        private float _zeroFloodNum;
        private float ActualFloodHeight => _floodTransform.position.y;
        private float TargetFloodHeight => _zeroFloodNum + _floodHeight;

        private float actualToTargetDifference => TargetFloodHeight - ActualFloodHeight;

        private void Awake()
        {
            _emissionRain = _rainParticle.emission;
            _zeroFloodNum = _floodTransform.position.y;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateFlood();
            UpdateRain();
        }
        private void UpdateFlood()
        {
            if (ActualFloodHeight == TargetFloodHeight) return;
            float floodModifier = actualToTargetDifference / Mathf.Abs(TargetFloodHeight - ActualFloodHeight);
            _floodTransform.position += Vector3.up * floodModifier * Time.deltaTime;
            if (Mathf.Abs(actualToTargetDifference) >= 0.1) return;

            _floodTransform.position += Vector3.up * actualToTargetDifference;
        }

        private void UpdateRain()
        {
            _emissionRain.rateOverTime = _rainIntensity;
        }

    }
}