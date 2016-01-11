using Ascent.Managers.Input;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectKeyboardKeyWindow : UIWindow
    {
        public static UISelectKeyboardKeyWindow instance;
        public static Action onCancel;
        public static Action<MappableKeys> onSelect;
    
        public Text labelSelectedKey;
        public UIButton buttonOn;
        public UIButton buttonCancel;

        protected List<MappableKeys> allMappableKeys;
        protected MappableKeys? selectedKey;
    
        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();

            allMappableKeys = new List<MappableKeys>();
            foreach (var value in Enum.GetValues(typeof(MappableKeys)))
                allMappableKeys.Add((MappableKeys)value);

            instance = this;
        }
        protected void CheckBindings()
        {
            if (labelSelectedKey == null)
                throw new Exception("labelSelectedKey is null.");

            if (buttonOn == null)
                throw new Exception("buttonOn is null.");

            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());
            buttonOn.onClick.AddListener(() => onSelect(selectedKey.Value));
        }
        protected override void OnBeforeFadeIn()
        {
            selectedKey = null;
            labelSelectedKey.text = string.Empty;
        }

        protected void Update()
        {
            foreach (var mappableKey in allMappableKeys)
            {
                if (UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode)mappableKey))
                {
                    selectedKey = mappableKey;
                    break;
                }
            }

            if (selectedKey.HasValue)
            {
                buttonOn.Enable();
                labelSelectedKey.text = InputManagerHelpers.GetMappableKeysDescription(selectedKey.Value);
            }
            else
            {
                buttonOn.Disable();
                labelSelectedKey.text = string.Empty;
            }
        }
    }
}
