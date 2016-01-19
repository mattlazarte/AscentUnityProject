using Ascent.Managers.Audio;
using Ascent.Managers.Input;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIOptionsWindow : UIWindow
    {
        public static Action onClose;
        public static UIOptionsWindow instance;

        public UIToggle toggleEnableJoystickVibration;
        public UIToggle toggleInvertAxis;
        public UISlider sliderMusicVolume;
        public UISlider sliderSoundFxVolume;
        public UISlider sliderJoystickSensibility;
        public UISlider sliderMouseSensibility;
        public UIButton buttonControls;
        public UIButton buttonBack;

        protected bool muteSoundFxValueChangedSoundFeedback = false;

        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();
            InitAudioSettings();

            instance = this;
        }
        protected void CheckBindings()
        {
            if (toggleEnableJoystickVibration == null)
                throw new Exception("toggleEnableJoystickVibration is null.");

            if (toggleInvertAxis == null)
                throw new Exception("toggleInvertAxis is null.");

            if (sliderMusicVolume == null)
                throw new Exception("sliderMusicVolume is null.");

            if (sliderSoundFxVolume == null)
                throw new Exception("sliderSoundFxVolume is null.");

            if (sliderJoystickSensibility == null)
                throw new Exception("sliderJoystickSensibility is null.");

            if (sliderMouseSensibility == null)
                throw new Exception("sliderMouseSensibility is null.");

            if (buttonControls == null)
                throw new Exception("buttonControls is null.");

            if (buttonBack == null)
                throw new Exception("buttonBack is null.");
        }
        protected void InitUI()
        {
            buttonBack.onClick.AddListener(() =>
            {
                SaveAudioSettings();
                UpdateAndSaveInputManager();
                onClose();
            });

            buttonControls.onClick.AddListener(() =>
            {
                FadeOut(() => UIControlsWindow.instance.FadeIn());
            });

            sliderMusicVolume.onValueChanged.AddListener((newValue) =>
            {
                //MusicManager.instance.volume = newValue;
            });

            sliderSoundFxVolume.onValueChanged.AddListener((newValue) =>
            {
                //PooledAudioSource.SetGlobalVolume(newValue);
            });

            sliderSoundFxVolume.onValueChangedNotWithMouse += (newValue) =>
            {
                if (!muteSoundFxValueChangedSoundFeedback)
                {
                    SoundFxsManager.instance.PlayOneShot2D(SoundFx.PlasmaGunShot);
                }
            };

            var sliderSoundFxLoopId = Guid.NewGuid().ToString();
            sliderSoundFxVolume.onPointerDown += () =>
            {
                SoundFxsManager.instance.LoopPlay2D(sliderSoundFxLoopId, SoundFx.PlasmaGunShot);
                //AudioManager.instance.Play(AudioBank.SFX_UI_BEEP, this.gameObject);
            };
            sliderSoundFxVolume.onPointerUp += () =>
            {
                SoundFxsManager.instance.StopLooped(sliderSoundFxLoopId);
                //AudioManager.instance.Play(AudioBank.SFX_UI_BEEP, this.gameObject);
            };
        }
        protected override void OnBeforeFadeIn()
        {
            InitUIFromInputManager();
            InitUIFromAudioManagers();
            buttonBack.MuteSelect();
        }
        protected void InitAudioSettings()
        {
            var audioSettings = LoadAudioSettings();
            //MusicManager.instance.volume = audioSettings.musicVolume;
            PooledAudioSource.SetGlobalVolume(audioSettings.soundFxVolume);
        }
        protected void InitUIFromAudioManagers()
        {
            //sliderMusicVolume.value = MusicManager.instance.volume;

            muteSoundFxValueChangedSoundFeedback = true;
            sliderSoundFxVolume.value = PooledAudioSource.GetGlobalVolume();
            muteSoundFxValueChangedSoundFeedback = false;
        }
        protected void InitUIFromInputManager()
        {
            toggleEnableJoystickVibration.isOn = InputManager.instance.settings.enableJoystickVibration;
            toggleInvertAxis.isOn = InputManager.instance.settings.invertVerticalAxis;
            sliderJoystickSensibility.value = InputManager.instance.settings.joystickSensibility;
            sliderMouseSensibility.value = InputManager.instance.settings.mouseSensibility;
        }
        protected void UpdateAndSaveInputManager()
        {
            InputManager.instance.settings.enableJoystickVibration = toggleEnableJoystickVibration.isOn;
            InputManager.instance.settings.invertVerticalAxis = toggleInvertAxis.isOn;
            InputManager.instance.settings.joystickSensibility = sliderJoystickSensibility.value;
            InputManager.instance.settings.mouseSensibility = sliderMouseSensibility.value;
            InputManager.instance.Save();
        }

        private string GetPersistenceFilePath()
        {
            return Path.Combine(Application.persistentDataPath, "Audio.json");
        }
        private void SaveAudioSettings()
        {
            try
            {
                var obj = new AudioSettingsPersistence(sliderMusicVolume.value, sliderSoundFxVolume.value);
                var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented);
                var path = GetPersistenceFilePath();
                File.WriteAllText(path, serialized);
                Debug.LogFormat("Audio settings saved at: {0}", path);
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("InputManager saving error: {0}", ex.Message);
            }
        }
        private AudioSettingsPersistence LoadAudioSettings()
        {
            try
            {
                var path = GetPersistenceFilePath();
                var contents = File.ReadAllText(path);
                var obj = JsonConvert.DeserializeObject<AudioSettingsPersistence>(contents);
                Debug.LogFormat("Audio settings loaded from: {0}", path);
                return obj;
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("InputManager loading error: {0}", ex.Message);
                Debug.LogWarning("Audio using default settings.");
                return new AudioSettingsPersistence(.5f, .5f);
            }
        }
    }

    public class AudioSettingsPersistence
    {
        public AudioSettingsPersistence() { }
        public AudioSettingsPersistence(float musicVolume, float soundFxVolume)
        {
            this.musicVolume = musicVolume;
            this.soundFxVolume = soundFxVolume;
        }

        public float musicVolume;
        public float soundFxVolume;
    }
}