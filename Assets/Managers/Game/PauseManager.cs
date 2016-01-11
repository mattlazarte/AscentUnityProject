using System.Collections.Generic;
using UnityEngine;

namespace Ascent.Managers.Game
{
    public delegate void OnPauseDelegate();
    public delegate void OnUnpauseDelegate(float pauseDeltaTime);

    public class PauseManager : MonoBehaviour
    {
        public static PauseManager instance;
        public static event OnPauseDelegate OnPause;
        public static event OnUnpauseDelegate OnUnpause;

        public bool paused { get; protected set; }
        protected float pauseTime;
        protected List<ParticleSystem> pausedParticleSystems;

        protected void Awake()
        {
            paused = false;
            pausedParticleSystems = new List<ParticleSystem>();
            instance = this;
        }

        public void Pause()
        {
            pauseTime = Time.time;
            paused = true;

            if (OnPause != null)
                OnPause();

            PauseAllParticles();
        }
        public void Unpause()
        {
            paused = false;

            if (OnUnpause != null)
                OnUnpause(Time.time - pauseTime);

            UnpausePausedParticles();
        }
        public void GameKilled()
        {
            paused = false;
            pausedParticleSystems.Clear();
        }

        private void PauseAllParticles()
        {
            var allParticleSystemsInScene = FindObjectsOfType<ParticleSystem>();
            foreach (var ps in allParticleSystemsInScene)
            {
                if (ps.isPlaying)
                {
                    ps.Pause(true);
                    pausedParticleSystems.Add(ps);
                }
            }
        }
        private void UnpausePausedParticles()
        {
            foreach (var ps in pausedParticleSystems)
            {
                ps.Play();
            }
            pausedParticleSystems.Clear();
        }
    }
}