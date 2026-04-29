using UnityEditor;
using UnityEngine;

namespace Capibutler.Utils
{
    #if UNITY_EDITOR
    public static class SettingsUtils
    {
        private const string EditorKeyPrefix = "Capibutler";
        
        private const string ShowGenericReferenceValue = "ShowGenericReferenceValue";
        private const string ShowGenericEventDebugger = "ShowGenericEventDebugger";
        private const string DebuggerFilter = "LastDebuggerFilter";
        private const string DebuggerFilterType = "LastDebuggerFilterType";
        
        private static string SettingsKey(string name) => $"{EditorKeyPrefix}.{Application.productName}.{name}";
        
        public static bool GetShowGenericReferenceValue =>  EditorPrefs.GetBool(SettingsKey(ShowGenericReferenceValue), false);
        public static bool GetShowGenericEventDebugger =>  EditorPrefs.GetBool(SettingsKey(ShowGenericEventDebugger), false);
        public static void SetShowGenericReferenceValue(bool value) =>  EditorPrefs.SetBool(SettingsKey(ShowGenericReferenceValue), value);
        public static void SetShowGenericEventDebugger(bool value) =>  EditorPrefs.SetBool(SettingsKey(ShowGenericEventDebugger), value);
    }
    #endif
}