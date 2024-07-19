using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ParticleSystem))]
    public class GSParticle : MonoBehaviour
    {
        [SerializeField] GrassSpawner _grassSpawner;

        private ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
            _grassSpawner.OnStartLoading += PlayParticle;
            _grassSpawner.OnStopLoading += StopParticle;
        }

        private void StopParticle()
        {
            _particle.Stop();
        }

        private void PlayParticle()
        {
            _particle.Play();
        }
    }
}