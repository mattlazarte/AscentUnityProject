using Ascent.Managers.Input;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectMouseButtonWindow : UIWindow
    {
        public static UISelectMouseButtonWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<MouseButton> onSelect;
    
        public Text label;
        public UIButton buttonLeft;
        public UIButton buttonMiddle;
        public UIButton buttonRight;
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
    
            if (buttonLeft == null)
                throw new Exception("buttonLeft is null.");
    
            if (buttonMiddle == null)
                throw new Exception("buttonMiddle is null.");
    
            if (buttonRight == null)
                throw new Exception("buttonRight is null.");
    
            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());
            buttonLeft.onClick.AddListener(() => onSelect(MouseButton.Left));
            buttonMiddle.onClick.AddListener(() => onSelect(MouseButton.Middle));
            buttonRight.onClick.AddListener(() => onSelect(MouseButton.Right));
        }
        protected override void OnBeforeFadeIn()
        {
            label.text = controlName;
            buttonCancel.MuteSelect();
        }
    }


}
