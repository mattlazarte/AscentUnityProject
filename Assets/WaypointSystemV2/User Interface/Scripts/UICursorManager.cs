using UnityEngine;

namespace Ascent.UI
{
    public class UICursorManager : MonoBehaviour
    {
        public static UICursorManager instance;

        public Texture2D normal;
        public Texture2D active;
        public Vector2 hotspot = new Vector2(4f, 4f);

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Cursor.SetCursor(normal, hotspot, CursorMode.Auto);
            HideCursor();
        }

        private void Update()
        {
            if (Cursor.visible)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0) ||
                    UnityEngine.Input.GetMouseButtonDown(1) ||
                    UnityEngine.Input.GetMouseButtonDown(2))
                {
                    Cursor.SetCursor(active, hotspot, CursorMode.Auto);
                }
                else if (UnityEngine.Input.GetMouseButtonUp(0) ||
                         UnityEngine.Input.GetMouseButtonUp(1) ||
                         UnityEngine.Input.GetMouseButtonUp(2))
                {
                    Cursor.SetCursor(normal, hotspot, CursorMode.Auto);
                }
            }
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }
    }
}