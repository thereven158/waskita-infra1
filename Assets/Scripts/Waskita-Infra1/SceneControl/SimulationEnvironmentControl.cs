using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class SimulationEnvironmentControl : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> _rainParticle = default;
        [SerializeField]
        public float _rainIntensity = default;
        private float _currentIntensity;

        private List<ParticleSystem.EmissionModule> _emissionRain;



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
            _emissionRain = new List<ParticleSystem.EmissionModule>(_rainParticle.Select(particle => particle.emission));
            _zeroFloodNum = _floodTransform.position.y;
            _currentIntensity = _emissionRain[0].rateOverTime.constant;
            UpdateRainIntensity(_currentIntensity);
            _rainIntensity = _currentIntensity;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateFlood();
            if (_rainIntensity != _currentIntensity)
                UpdateRainIntensity(_rainIntensity);
        }
        private void UpdateFlood()
        {
            if (ActualFloodHeight == TargetFloodHeight) return;
            float floodModifier = actualToTargetDifference / Mathf.Abs(TargetFloodHeight - ActualFloodHeight);
            _floodTransform.position += Vector3.up * floodModifier * Time.deltaTime;
            if (Mathf.Abs(actualToTargetDifference) >= 0.1) return;

            _floodTransform.position += Vector3.up * actualToTargetDifference;
        }

        private void UpdateRainIntensity(float value)
        {
            _emissionRain.ForEach(emssion => emssion.rateOverTime = value);
            _currentIntensity = value;
        }

    }
}