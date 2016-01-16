using UnityEngine;
using System.Collections;

// Call this class to handle audio in the game //////////////////////////////////////////////////////////////////////////////////////
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    private uint bankID;

    private void Awake()
    {
        AkSoundEngine.LoadBank("Ascent_SFX", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID); // Load the WWise sound bank
        Debug.Log("Wwise bankID: " + bankID);
        DontDestroyOnLoad(this.gameObject);
    }

    // Basic methods Play, Stop, Pause, Resume ask for an eventName string
    // To see the eventName list check the static class AudioBank ///////////////////////////////////////////////////////////////////
    public void Play(string eventName, GameObject gameobject)
    {

        AkSoundEngine.PostEvent(AkSoundEngine.GetIDFromString(eventName), gameobject);
    }

    public void Stop(string eventName, GameObject gameobject)
    {

        AkSoundEngine.ExecuteActionOnEvent(AkSoundEngine.GetIDFromString(eventName), AkActionOnEventType.AkActionOnEventType_Stop, gameobject, 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    public void Pause(string eventName, GameObject gameobject)
    {

        AkSoundEngine.ExecuteActionOnEvent(AkSoundEngine.GetIDFromString(eventName), AkActionOnEventType.AkActionOnEventType_Pause, gameobject, 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    public void Resume(string eventName, GameObject gameobject)
    {

        AkSoundEngine.ExecuteActionOnEvent(AkSoundEngine.GetIDFromString(eventName), AkActionOnEventType.AkActionOnEventType_Resume, gameobject, 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    // Special method for UI only ///////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayUI(string switchState, GameObject gameObject)
    {

        AkSoundEngine.SetSwitch(AkSoundEngine.GetIDFromString("UI_SFX"), AkSoundEngine.GetIDFromString(switchState), gameObject);
        AkSoundEngine.PostEvent(AkSoundEngine.GetIDFromString("Play_UI"), gameObject);

    }

    public void SetSwitch(string switchGroup, string switchState, GameObject gameObject)
    {

        AkSoundEngine.SetSwitch(AkSoundEngine.GetIDFromString(switchGroup), AkSoundEngine.GetIDFromString(switchState), gameObject);
    }
}
