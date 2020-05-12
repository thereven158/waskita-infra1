using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.Particle
{
    public class RainSystemControl : MonoBehaviour
    {
        [SerializeField]
        public ParticleSystem RainParticle;

        private ParticleSystem.MainModule _psMain;
        private ParticleSystem.EmissionModule _psEmission;

        private void Start()
        {
            _psMain = RainParticle.main;
            _psEmission = RainParticle.emission;

        }

        public void StartRain()
        {
            RainParticle.Play();
        }

        public void SetRainDuration(float duration, bool isLooping)
        {
            _psMain.duration = duration;
            _psMain.loop = isLooping;
        }

        public void SetRainIntensity(float intensity)
        {
            _psEmission.rateOverTime = intensity;
        }
    }

}
