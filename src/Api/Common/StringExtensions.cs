﻿using System.Text;

namespace Api.Common;

public static class StringExtensions
{
    private static int GetHexVal(char hex) => hex - (hex < 58 ? 48 : hex < 97 ? 55 : 87);

    public static byte[] ToBytesFromHex(this string hex)
    {
        // add a 0 before the last character
        if (hex.Length % 2 == 1)
            hex = hex.Insert(hex.Length - 1, "0");

        byte[] arr = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

        return arr;
    }

    public static string ToSnakeCase(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (text.Length < 2)
            return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));

        for (var i = 1; i < text.Length; ++i)
        {
            var c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}