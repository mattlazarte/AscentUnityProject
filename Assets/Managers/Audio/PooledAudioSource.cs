using UnityEngine;
using System.Collections;
using Ascent.Managers.Game;
using Ascent.Managers.Pools;
using System;
using System.Collections.Generic;

namespace Ascent.Managers.Audio
{
    [IncludeInPoolManager]
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour, IPoolOnSpawnListener
    {
        public static List<PooledAudioSource> allPooledAudioSources = new List<PooledAudioSource>();
        private static float globalVolume = .5f;
        public static void SetGlobalVolume(float volume)
        {
            globalVolume = volume;

            foreach (var pooledAudioSources in allPooledAudioSources)
                pooledAudioSources.volume = volume;
        }
        public static float GetGlobalVolume()
        {
            return globalVolume;
        }

        protected AudioSource audioSource;
        protected float timeToGoBackToPool;
        protected bool unpause;

        private void Awake()
        {
            allPooledAudioSources.Add(this);

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = globalVolume;
        }

        private void Start()
        {
            PauseManager.OnPause += OnPause;
            PauseManager.OnUnpause += OnUnpause;
        }
        protected virtual void OnDestroy()
        {
            PauseManager.OnPause -= OnPause;
            PauseManager.OnUnpause -= OnUnpause;
        }

        public virtual void OnSpawn()
        {
            timeToGoBackToPool = float.MaxValue;
        }

        private void Update()
        {
            if (!PauseManager.instance.paused && Time.time >= timeToGoBackToPool)
            {
                PoolManager.instance.Despawn<PooledAudioSource>(this);
            }
        }

        public void OnPause()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                unpause = true;
            }
            else
            {
                unpause = false;
            }
        }

        private void OnUnpause(float pauseDeltaTime)
        {
            if (unpause)
            {
                timeToGoBackToPool += pauseDeltaTime;
                audioSource.UnPause();
            }
        }

        public float volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }

        public void PlayAs2DAudio()
        {
            audioSource.spatialBlend = 0f;
        }

        public void PlayAs3DAudio()
        {
            audioSource.spatialBlend = 1f;
        }

        public void PlayAndGoBackToPool(AudioClip clip)
        {
            timeToGoBackToPool = Time.time + clip.length + 1f;
            audioSource.loop = false;
            audioSource.PlayOneShot(clip);
        }

        public void LoopPlay(AudioClip clip)
        {
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopLoopPlayAndGoBackToPool()
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
            PoolManager.instance.Despawn<PooledAudioSource>(this);
        }
    }
}