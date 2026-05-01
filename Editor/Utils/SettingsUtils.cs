using System;
using System.Collections.Generic;
using Capibutler.Events.Base;
using Capibutler.Values.Base;
using UnityEditor;
using UnityEngine;

namespace Capibutler.Editor.Utils
{
    public static class SettingsUtils
    {
        private const string EditorKeyPrefix = "Capibutler";

        private static string SettingsKey(string name) => $"{EditorKeyPrefix}.{Application.productName}.{name}";

        public static bool GetBool(string key, bool defaultValue = false) => EditorPrefs.GetBool(SettingsKey(key), defaultValue);
        public static void SetBool(string key, bool value) => EditorPrefs.SetBool(SettingsKey(key), value);
        public static float GetFloat(string key, float defaultValue = 0f) => EditorPrefs.GetFloat(SettingsKey(key), defaultValue);
        public static void SetFloat(string key, float value) => EditorPrefs.SetFloat(SettingsKey(key), value);
        public static int GetInt(string key, int defaultValue = 0) => EditorPrefs.GetInt(SettingsKey(key), defaultValue);
        public static void SetInt(string key, int value) => EditorPrefs.SetInt(SettingsKey(key), value);
        public static string GetString(string key, string defaultValue = "") => EditorPrefs.GetString(SettingsKey(key), defaultValue);
        public static void SetString(string key, string value) => EditorPrefs.SetString(SettingsKey(key), value);
        public static bool HasKey(string key) => EditorPrefs.HasKey(SettingsKey(key));

        public static readonly Dictionary<Type, string> TypeToIconMap = new() {
            [typeof(BaseEvent)] = "BaseEvent",
            [typeof(BaseEventListener)] = "BaseEventListener",
            [typeof(GenericValue)] = "GenericValue"
        };
    }
}