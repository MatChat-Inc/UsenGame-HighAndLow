using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class TextAssetRWAblity {
#if UNITY_EDITOR
    public static void ExportToFile(object obj, string path) {
        if (!File.Exists(path)) {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            fs.Close();
        }

        var content = JsonSerializablity.Serialize(obj);
        File.WriteAllText(path, content);
        AssetDatabase.Refresh();
    }
#endif

    public static T ImportFromFile<T>(string path) {
#if UNITY_EDITOR
        UnityEngine.Object textObj = AssetDatabase.LoadMainAssetAtPath(path);
#else
        // var filePath = PathUtility.GetResourcesRelativePath(path);
        UnityEngine.Object textObj = Resources.Load(path);
#endif
        
        TextAsset textAsset = TextAsset.Instantiate<TextAsset>(textObj as TextAsset);
        return JsonSerializablity.Deserialize<T>(textAsset.text);
    }
}