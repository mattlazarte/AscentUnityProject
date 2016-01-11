using Ascent.Managers.Input;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIConfirmWindow : UIWindow
    {
        public static UIConfirmWindow instance;
        public static Action onCancel;
        public static Action onConfirm;
        public static string text;
        public static void Setup(string text, Action onConfirm, Action onCancel)
        {
            UIConfirmWindow.text = text;
            UIConfirmWindow.onConfirm = onConfirm;
            UIConfirmWindow.onCancel = onCancel;
        }
    
        public Text label;
        public UIButton buttonOk;
        public UIButton buttonCancel;
    
        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();
    
            instance = this;
        }
        protected void CheckBindings()
        {
            if (label == null)
                throw new Exception("label is null.");
    
            if (buttonOk == null)
                throw new Exception("buttonOk is null.");
    
            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");
        }
        protected void InitUI()
        {
            buttonOk.onClick.AddListener(() =>
            {
                if (onConfirm == null)
                    throw new Exception("onConfirm callback is null.");
    
                onConfirm();
            });
    
            buttonCancel.onClick.AddListener(() =>
            {
                if (onCancel == null)
                    throw new Exception("onCancel callback is null.");
    
                onCancel();
            });
        }
        protected override void OnAfterFadeOut()
        {
            onConfirm = null;
            onCancel = null;
            text = string.Empty;
        }
        protected override void OnBeforeFadeIn()
        {
            label.text = text;
            buttonOk.MuteSelect();
        }
    }
}
