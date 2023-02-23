using System.Numerics;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.Wrappers
{
    internal static class Math
    {
        public static bool IsPointInRadius(Vector2 point, Vector2 center, float radius)
        {
            return System.Math.Sqrt(((center.X - point.X) * (center.X - point.X)) + ((center.Y - point.Y) * (center.Y - point.Y))) < radius;
        }

        public static float DistanceToPoint(Vector2 point, Vector2 otherPoint)
        {
            float ydist = (otherPoint.Y - point.Y);
            float xdist = (otherPoint.X - point.X);
            float hypotenuse = Convert.ToSingle(System.Math.Sqrt(System.Math.Pow(ydist, 2) + System.Math.Pow(xdist, 2)));
            return hypotenuse;
        }

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
    }
}
