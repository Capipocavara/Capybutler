using System;
using System.Collections.Generic;
using System.IO;
using Capibutler.Events.Base;
using Capibutler.Values.Base;
using UnityEditor;
using UnityEngine;

namespace Capibutler.Editor.Utils
{
    public static class PathUtils
    {
        private const string PackageFolder = "Capibutler";
        private const string PackageName = "de.capipocavara.capibutler";
        private const string SettingsSavePath = "SettingsSavePath";
        public static string ApplicationProjectPath => Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
        public static string DefaultSavePath => Path.Combine(Application.dataPath, PackageFolder).Replace("\\", "/");
        public static string ProjectPathToFullPath(string relativePath) => Path.GetFullPath(Path.Combine(ApplicationProjectPath, relativePath.Trim('\\', '/', ' ')));
        public static string PackagePath(string relativePath) => Path.Combine($"Packages/{PackageName}", relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
        public static string AssetPathToNamespace(string relativePath) => Path.GetRelativePath(Application.dataPath, relativePath.Trim('\\', '/', ' ')).Replace("\\", "/").Replace("/", ".");
        public static string PersistentDataPathToFullPath(string relativePath) => Path.GetFullPath(Path.Combine(Application.persistentDataPath, relativePath.Trim('\\', '/', ' ')));
        public static string AssetPathToProjectPath(string relativePath) => Path.GetRelativePath(ApplicationProjectPath, Path.Combine("Assets", relativePath.Trim('\\', '/', ' ')));
        
        
    
        // Editor Preferences keys
    
       

        // Custom script icons
        public static readonly Dictionary<Type, string> TypeToIconMap = new() {
            [typeof(BaseEvent)] = "BaseEvent",
            [typeof(BaseEventListener)] = "BaseEventListener",
            [typeof(GenericValue)] = "GenericValue"
        };

      

        


        public static string VoodooAssetPath(string relativePath)
        {
            var path = Path.Combine(Path.GetFullPath(EditorPrefs.GetString(GetEditorKey(SettingsSavePath), DefaultSavePath)), relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        
            return path;
        }

      

    }
}