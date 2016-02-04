using Ascent.Managers.Game;
using System;
using UnityEngine;

namespace Ascent.UI
{
    public class UIMainMenuWindow : UIWindow
    {
        public static UIMainMenuWindow instance;

        public UIButton startGameButton;
        public UIButton optionsButton;
        public UIButton creditsButton;
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
            if (startGameButton == null)
                throw new Exception("startGameButton is null.");

            if (optionsButton == null)
                throw new Exception("optionsButton is null.");

            if (creditsButton == null)
                throw new Exception("creditsButton is null.");

            if (exitButton == null)
                throw new Exception("exitButton is null.");
        }
        protected void InitUI()
        {
            startGameButton.onClick.AddListener(() =>
            {
                AudioManager.instance.Play(AudioBank.SFX_UI_START, this.gameObject);
                GameManager.instance.StartGame();
            });

            optionsButton.onClick.AddListener(() =>
            {
                UIOptionsWindow.onClose = () =>
                {
                    UIOptionsWindow.instance.FadeOut(() => FadeIn());
                };

                FadeOut(() => UIOptionsWindow.instance.FadeIn());
            });

            creditsButton.onClick.AddListener(() =>
            {
                FadeOut(() => UICreditsWindow.instance.FadeIn());
            });

            exitButton.onClick.AddListener(() => {

                    Application.Quit();
                
            });
        }

        protected override void OnBeforeFadeIn()
        {
            startGameButton.MuteSelect();
        }
    }
}