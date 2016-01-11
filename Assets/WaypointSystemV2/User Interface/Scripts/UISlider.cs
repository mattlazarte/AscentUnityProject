using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISlider : Slider
    {
        public event Action<float> onValueChangedNotWithMouse;
        public event Action onPointerDown;
        public event Action onPointerUp;

        protected bool draggingWithMouse = false;

        protected override void Awake()
        {
            base.Awake();

            this.onValueChanged.AddListener((value) =>
            {
                if (!draggingWithMouse && onValueChangedNotWithMouse != null)
                {
                    onValueChangedNotWithMouse(value);
                }
            });
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            draggingWithMouse = true;

            if (onPointerDown != null)
                onPointerDown();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            draggingWithMouse = false;

            if (onPointerUp != null)
                onPointerUp();
        }
    }
}