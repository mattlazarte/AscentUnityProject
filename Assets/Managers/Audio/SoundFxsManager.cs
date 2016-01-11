using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Ascent.Managers.Pools;

namespace Ascent.Managers.Audio
{
    public enum SoundFx
    {
        HullAlarm,
        LaserBeamClose,
        LaserBeamLoop,
        LaserBeamOpen,
        MissileExplosion,
        MissileShot,
        NavigationHumm,
        PlasmaGunExplosion,
        PlasmaGunShot,
        ShieldAlarm,
        ShieldsDown,
        ShieldsUp,
        ShipExplosion,
        UIBlip01,
        WeaponDepletedBuzz,
        ItemCatch,
        Switcher,
        DoorLocked,
        Door,
    }

    public class SoundFxsManager : MonoBehaviour
    {
        public static SoundFxsManager instance;

        protected Dictionary<SoundFx, AudioClip> soundFxClips;
        protected Dictionary<string, PooledAudioSource> loopedAudioSources;

        protected void Awake()
        {
            instance = this;
            InitSoundFxClips();
            loopedAudioSources = new Dictionary<string, PooledAudioSource>();
        }

        private void InitSoundFxClips()
        {
            soundFxClips = new Dictionary<SoundFx, AudioClip>();

            foreach (var untypedSoundFxValue in Enum.GetValues(typeof(SoundFx)))
            {
                var soundFx = (SoundFx)untypedSoundFxValue;
                var path = "Audio/SoundFxs/" + soundFx.ToString();

                var audioClip = Resources.Load<AudioClip>(path);
                if (audioClip == null)
                    throw new Exception("AudioClip '" + path + "' not found.");

                audioClip.LoadAudioData();
                soundFxClips.Add(soundFx, audioClip);
            }
        }

        public void PlayOneShot(SoundFx soundFx, Vector3 position)
        {
            var clip = soundFxClips[soundFx];
            var audioSource = PoolManager.instance.Spawn<PooledAudioSource>(this.transform);

            audioSource.PlayAs3DAudio();
            audioSource.transform.position = position;
            audioSource.PlayAndGoBackToPool(clip);
        }

        public void PlayOneShot2D(SoundFx soundFx)
        {
            var clip = soundFxClips[soundFx];
            var audioSource = PoolManager.instance.Spawn<PooledAudioSource>(this.transform);

            audioSource.PlayAs2DAudio();
            audioSource.PlayAndGoBackToPool(clip);
        }

        public void LoopPlay(string id, SoundFx soundFx, Transform parent)
        {
            if (loopedAudioSources.ContainsKey(id)) return;

            var clip = soundFxClips[soundFx];
            var audioSource = PoolManager.instance.Spawn<PooledAudioSource>(this.transform);
            loopedAudioSources.Add(id, audioSource);

            audioSource.PlayAs3DAudio();
            audioSource.transform.parent = parent;
            audioSource.transform.localPosition = Vector3.zero;

            audioSource.LoopPlay(clip);
        }

        public void LoopPlay2D(string id, SoundFx soundFx)
        {
            if (loopedAudioSources.ContainsKey(id)) return;

            var clip = soundFxClips[soundFx];
            var audioSource = PoolManager.instance.Spawn<PooledAudioSource>(this.transform);
            loopedAudioSources.Add(id, audioSource);

            audioSource.PlayAs2DAudio();

            audioSource.LoopPlay(clip);
        }

        public void StopLooped(string id)
        {
            if (!loopedAudioSources.ContainsKey(id)) return;

            var audioSource = loopedAudioSources[id];
            audioSource.StopLoopPlayAndGoBackToPool();

            loopedAudioSources.Remove(id);
        }
    }
}