using Ascent.Managers.Game;
using Ascent.Managers.Input;
using System;
using UnityEngine;

namespace Ascent.UI
{
    public class UIPauseWindow : UIWindow
    {
        public static UIPauseWindow instance;
    
        public UIButton resumeGameButton;
        public UIButton optionsButton;
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
            if (resumeGameButton == null)
                throw new Exception("resumeGameButton is null.");
    
            if (optionsButton == null)
                throw new Exception("optionsButton is null.");
    
            if (exitButton == null)
                throw new Exception("exitButton is null.");
        }
        protected void InitUI()
        {
            resumeGameButton.onClick.AddListener(() =>
            {
                GameManager.instance.Unpause();
            });

            optionsButton.onClick.AddListener(() =>
            {
                UIOptionsWindow.onClose = () =>
                {
                    UIOptionsWindow.instance.FadeOut(() => FadeIn());
                };
    
                FadeOut(() => UIOptionsWindow.instance.FadeIn());
            });

            exitButton.onClick.AddListener(() =>
            {
                UIConfirmWindow.Setup(
                    text: "You will lose your progress.",
                    onConfirm: () =>
                    {
                        UIConfirmWindow.instance.FadeOut();
                        GameManager.instance.KillGameAndBringBackMainMenu();
                    },
                    onCancel: () =>
                    {
                        UIConfirmWindow.instance.FadeOut(() => FadeIn());
                    }
                );

                FadeOut(() => UIConfirmWindow.instance.FadeIn());
            });
        }
    
        protected override void OnBeforeFadeIn()
        {
            resumeGameButton.MuteSelect();
        }

        protected virtual void Update()
        {
            if (active)
            {
                var input = InputManager.instance.GetPlayerInput();
                if (input.Pause)
                {
                    GameManager.instance.Unpause();
                }
            }
        }
    }
}
