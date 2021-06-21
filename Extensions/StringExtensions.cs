using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtension {
    public static string ToCamelCase(this string str) {
        return string.IsNullOrEmpty(str) || str.Length < 2
        ? str
        : str[0] + str.Substring(1).ToLowerInvariant();
    }
}