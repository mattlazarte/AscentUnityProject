using Ascent.Managers.Audio;
using System;
using UnityEngine;

namespace Ascent.UI
{
    public class UIManager : MonoBehaviour
    {
        private void Awake()
        {
            var allWindows = GetComponentsInChildren<UIWindow>(true);

            foreach (var window in allWindows)
            {
                window.AwakeFromManager();
                window.DisableAndHide();
            }
        }
    }
}