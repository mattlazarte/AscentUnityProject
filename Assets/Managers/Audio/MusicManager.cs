using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Ascent.Managers.Pools;
using DG.Tweening;

namespace Ascent.Managers.Audio
{
    public enum Music
    {
        InGameMusic,
        InGameMusicIntro,
        MenuMusic,
        DeathMusic
    }

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        protected AudioSource audioSource;
        protected Dictionary<Music, AudioClip> musicClips;
        protected float? scheduledPlayTime;
        protected Music? scheduledMusic;

        public float volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }

        protected void Awake()
        {
            instance = this;

            audioSource = GetComponent<AudioSource>();
            audioSource.spatialBlend = 0;

            InitMusicClips();
        }

        private void InitMusicClips()
        {
            musicClips = new Dictionary<Music, AudioClip>();

            foreach (var untypedMusicValue in Enum.GetValues(typeof(Music)))
            {
                var music = (Music)untypedMusicValue;
                var path = "Audio/Music/" + music.ToString();

                var audioClip = Resources.Load<AudioClip>(path);
                if (audioClip == null)
                    throw new Exception("AudioClip '" + path + "' not found.");

                audioClip.LoadAudioData();
                musicClips.Add(music, audioClip);
            }
        }

        private void Update()
        {
            if (scheduledPlayTime.HasValue && scheduledMusic.HasValue && Time.time >= scheduledPlayTime.Value)
            {
                PlayLooped(scheduledMusic.Value);

                scheduledPlayTime = null;
                scheduledMusic = null;
            }
        }

        public void PlayOnce(Music music)
        {
            scheduledPlayTime = null;
            scheduledMusic = null;

            audioSource.clip = musicClips[music];
            audioSource.loop = false;
            audioSource.Play();
        }

        public void PlayLooped(Music music)
        {
            scheduledPlayTime = null;
            scheduledMusic = null;

            audioSource.clip = musicClips[music];
            audioSource.loop = true;
            audioSource.Play();
        }

        public void PlayIntroThenLooped(Music intro, Music looped)
        {
            PlayLooped(intro);
            audioSource.loop = false;
            scheduledPlayTime = Time.time + musicClips[intro].length;
            scheduledMusic = looped;
        }

        public void FadeOut(float fadeDuration, Action onComplete = null)
        {
            var currVolume = audioSource.volume;
            audioSource.DOFade(0, fadeDuration).OnComplete(() =>
            {
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.volume = currVolume;

                if (onComplete != null) onComplete();
            });
        }

        public void FadeInAndPlayLooped(Music music, float fadeDuration, Action onComplete = null)
        {
            var currVolume = audioSource.volume;
            audioSource.volume = 0;
            PlayLooped(music);
            audioSource.DOFade(currVolume, fadeDuration).OnComplete(() =>
            {
                if (onComplete != null) onComplete();
            });
        }

        public void FadeToMusic(Music music, float fadeDuration)
        {
            if (!audioSource.isPlaying)
            {
                FadeInAndPlayLooped(music, fadeDuration);
            }
            else
            {
                var halfFadeDuration = fadeDuration / 2;
                FadeOut(halfFadeDuration, () => FadeInAndPlayLooped(music, halfFadeDuration));
            }
        }
    }
}