using Hexed.Core;
using Hexed.SDK;
using System.Drawing;
using System.Numerics;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.Wrappers
{
    internal static class Math
    {

        public static string ByteSizeToString(long size)
        {
            string[] strArrays = new string[] { "B", "KB", "MB", "GB", "TB" };
            int num = 0;
            while (size > 1024)
            {
                size /= 1024;
                num++;
            }
            return string.Format("{0} {1}", size.ToString(), strArrays[num]);
        }

        public static IntPtr Add(this IntPtr first, IntPtr second)
        {
            if (IntPtr.Size == 4) return new IntPtr(first.ToInt32() + second.ToInt32());
            return new IntPtr(first.ToInt64() + second.ToInt64());
        }

        public static IntPtr Subtract(this IntPtr first, IntPtr second)
        {
            if (IntPtr.Size == 4) return new IntPtr(first.ToInt32() - second.ToInt32());
            return new IntPtr(first.ToInt64() - second.ToInt64());
        }

        public static int MaxValue(this Dictionary<string, IntPtr> dict)
        {
            var max = 0;
            foreach (var val in dict.Values)
            {
                var current = val.ToInt32();
                if (current > max) max = current;
            }
            return max;
        }

        public static bool Check(this PlayerFlag one, PlayerFlag other)
        {
            return (one & other) > 0;
        }

        public static void Sort(this Dictionary<string, IntPtr> dict)
        {
            var copy = new Dictionary<string, IntPtr>(dict.Count);

            foreach (var item in dict)
                copy.Add(item.Key, item.Value);

            dict.Clear();

            IOrderedEnumerable<KeyValuePair<string, IntPtr>> sorted;

            if (IntPtr.Size == 4)
            {
                sorted = from pair in copy
                         orderby pair.Value.ToInt32() ascending
                         select pair;
            }
            else
            {
                sorted = from pair in copy
                         orderby pair.Value.ToInt64() ascending
                         select pair;
            }

            foreach (var item in sorted)
                dict.Add(item.Key, item.Value);
        }

        public static Vector2 WorldToScreen(Vector3 target, Vector2 Screen)
        {
            Vector2 _worldToScreenPos;
            Vector3 to;
            float[] viewmatrix = EngineClient.ViewMatrix;

            to.X = viewmatrix[0] * target.X + viewmatrix[1] * target.Y + viewmatrix[2] * target.Z + viewmatrix[3];
            to.Y = viewmatrix[4] * target.X + viewmatrix[5] * target.Y + viewmatrix[6] * target.Z + viewmatrix[7];

            float w = viewmatrix[12] * target.X + viewmatrix[13] * target.Y + viewmatrix[14] * target.Z + viewmatrix[15];

            if (w < 0.01f) return new Vector2(0, 0);

            to.X *= (1.0f / w);
            to.Y *= (1.0f / w);

            float x = Screen.X / 2;
            float y = Screen.Y / 2;

            x += 0.5f * to.X * Screen.X + 0.5f;
            y -= 0.5f * to.Y * Screen.Y + 0.5f;

            to.X = x;
            to.Y = y;

            _worldToScreenPos.X = to.X;
            _worldToScreenPos.Y = to.Y;
            return _worldToScreenPos;
        }

        public static Color MapHealthToColor(float value)
        {
            // Ensure the value is within the valid range (0 to 100)
            value = System.Math.Max(0, System.Math.Min(100, value));

            // Calculate the Red and Green components based on the value
            int red = (int)(255 * (1 - value / 100));
            int green = (int)(255 * (value / 100));

            // Create a new Color object with the calculated RGB values
            Color color = Color.FromArgb(red, green, 0);

            return color;
        }
    }
}
