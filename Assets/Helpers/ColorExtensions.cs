using UnityEngine;
using System.Collections;

namespace Ascent
{
    public static class ColorExtensions
    {
        public static Color Copy(this Color c)
        {
            return new Color(c.r, c.g, c.b, c.a);
        }

        public static Color Copy(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static Color CopyWithHalfAlpha(this Color c)
        {
            return new Color(c.r, c.g, c.b, c.a / 2);
        }
    }
}