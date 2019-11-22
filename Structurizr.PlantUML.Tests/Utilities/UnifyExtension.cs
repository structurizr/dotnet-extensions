using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Structurizr.IO.PlantUML.Tests.Utilities
{
    public static class UnifyExtension
    {
        public static string UnifyNewLine(this string multiLineText)
        {
            var nPos = multiLineText.IndexOf('\n');
            var rPos = multiLineText.IndexOf('\r');
            if (rPos==-1 || nPos>=0 && nPos < rPos)
                return multiLineText
                    .Replace("\n\r", "<<NEWLINE>>")
                    .Replace("\n", "<<NEWLINE>>")
                    .Replace("<<NEWLINE>>", Environment.NewLine);

            return multiLineText
                .Replace("\r\n", "<<NEWLINE>>")
                .Replace("\r", "<<NEWLINE>>")
                .Replace("<<NEWLINE>>", Environment.NewLine);
        }

        /// <summary>
        /// C4LibraryPlantUmlWriter create some names with hash codes (__[0-9a-f]+) which are dynamic created
        /// and has to be harmonized
        /// </summary>
        /// <param name="textWithMultipleHashValues"></param>
        /// <returns></returns>
        public static string UnifyHashValues(this string textWithMultipleHashValues)
        {
            var hashRegex = new Regex("__([0-9a-f]+)");
            var hashToPlaceholder = new Dictionary<string, string>();
            var placeholderIndex = 0;

            var unified = hashRegex.Replace(textWithMultipleHashValues, m =>
            {
                var orig = m.Groups[1].ToString();
                if (!hashToPlaceholder.TryGetValue(orig, out var placeholder))
                {
                    placeholder = $"HASH{placeholderIndex++}";
                    hashToPlaceholder[orig] = placeholder;
                }

                return $"__{placeholder}";
            });

            return unified;
        }
    }
}