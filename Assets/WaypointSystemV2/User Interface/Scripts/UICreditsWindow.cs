using System;
using UnityEngine;

namespace Ascent.UI
{
    public class UICreditsWindow : UIWindow
    {
        public static UICreditsWindow instance;
    
        public UIButton backButton;
    
        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();
    
            instance = this;
        }
        protected void CheckBindings()
        {
            if (backButton == null)
                throw new Exception("exitButton is null.");
        }
        protected void InitUI()
        {
            backButton.onClick.AddListener(() =>
            {
                FadeOut(() => UIMainMenuWindow.instance.FadeIn());
            });
        }
    
        protected override void OnBeforeFadeIn()
        {
            backButton.MuteSelect();
        }
    }
}
