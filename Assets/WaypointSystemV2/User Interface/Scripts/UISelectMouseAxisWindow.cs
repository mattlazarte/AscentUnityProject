using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectMouseAxisWindow : UIWindow
    {
        public static UISelectMouseAxisWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<MouseAxis> onSelect;

        public Text label;
        public UIButton buttonVertical;
        public UIButton buttonHorizontal;
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

            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");

            if (buttonVertical == null)
                throw new Exception("buttonVertical is null.");

            if (buttonHorizontal == null)
                throw new Exception("buttonHorizontal is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());
            buttonVertical.onClick.AddListener(() => onSelect(MouseAxis.Vertical));
            buttonHorizontal.onClick.AddListener(() => onSelect(MouseAxis.Horizontal));
        }
        protected override void OnBeforeFadeIn()
        {
            label.text = controlName;
            buttonCancel.MuteSelect();
        }
    }

}
