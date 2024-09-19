using UnityEditor;

public class EditorTools
{
    [MenuItem("Tools/Clear User Data")]
    public static void ClearCache()
    {
        PreferencesStorage.DeleteAll();
    }
}