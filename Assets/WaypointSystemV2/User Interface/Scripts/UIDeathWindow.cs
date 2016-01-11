using Ascent.Managers.Game;
using Ascent.Managers.Input;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIDeathWindow : UIWindow
    {
        public static UIDeathWindow instance;

        public Text statsLabel;
        public UIButton exitButton;
    
        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();
    
            instance = this;
        }
        protected void CheckBindings()
        {
            if (exitButton == null)
                throw new Exception("exitButton is null.");

            if (statsLabel == null)
                throw new Exception("statsLabel is null.");
        }
        protected void InitUI()
        {
            exitButton.onClick.AddListener(() =>
            {
                GameManager.instance.KillGameAndBringBackMainMenu();
                FadeOut();
            });
        }
    
        protected override void OnBeforeFadeIn()
        {
            exitButton.MuteSelect();
        }
    }
}
