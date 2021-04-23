using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TestSolution.Util
{
    public static class Helper
    {
        private static readonly Regex UuidPattern =
            new Regex(" [0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");

        public static string GetNameWithoutUUID(this string name)
        {
            var matcher = UuidPattern.Match(name);
            if (!matcher.Success) return name;

            var group = matcher.Groups[0];
            return name.Replace(group.Value, "");
        }

        public static bool CompareContent(this FileInfo first, FileInfo second)
        {
            var result = true;

            using var firstReader = new StreamReader(first.FullName);
            using var secondReader = new StreamReader(second.FullName);

            int char1;
            int char2;

            while ((char1 = firstReader.Read()) != -1 && (char2 = secondReader.Read()) != -1)
            {
                if (char1 == char2) continue;

                result = false;
                break;
            }

            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source) action(element);
        }
    }
}