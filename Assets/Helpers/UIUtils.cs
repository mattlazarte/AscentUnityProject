using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ascent
{
    public static class UIUtils
    {
        public static IEnumerator BlinkUICoroutine(Graphic graphic, int times = 3, float speed = .03f)
        {
            for (var i = 0; i < times; i++)
            {
                graphic.enabled = false;
                yield return new WaitForSeconds(speed);
                graphic.enabled = true;
                yield return new WaitForSeconds(speed);
            }
        }

        public static void BlinkUI(Graphic graphic)
        {
            graphic.StartCoroutine(BlinkUICoroutine(graphic));
        }
    }
}
