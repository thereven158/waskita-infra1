using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.Particle
{
    public class CloudSystemControl : MonoBehaviour
    {
        [SerializeField]
        public ParticleSystem CloudParticle;

        private ParticleSystem.EmissionModule _psEmission;

        private void Start()
        {
            _psEmission = CloudParticle.emission;

        }

        public void SetCloudIntensity(float intensity)
        {
            _psEmission.rateOverTime = intensity;
        }
    }

}
