using UnityEngine;

public static class PreferencesStorage
{
    public static bool ReadBoolean(string key)
    {
        return ReadBoolean(key, false);
    }

    public static bool ReadBoolean(string key, bool defaultValue)
    {

        if (string.IsNullOrEmpty(key))
        {
            return defaultValue;
        }
        int v = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        return v == 1;
    }
    
    public static void SaveBoolean(string key, bool value)
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }


    public static int ReadInt(string key)
    {
        return ReadInt(key, 0);
    }

    public static int ReadInt(string key, int defaultValue)
    {

        if (string.IsNullOrEmpty(key))
        {
            return defaultValue;
        }
        int v = PlayerPrefs.GetInt(key, defaultValue);
        return v;
    }

    public static void SaveInt(string key, int value)
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }

    public static float ReadFloat(string key)
    {
        return ReadFloat(key, 0.0f);
    }

    public static float ReadFloat(string key, float defaultValue)
    {

        if (string.IsNullOrEmpty(key))
        {
            return defaultValue;
        }
        float v = PlayerPrefs.GetFloat(key, defaultValue);
        return v;
    }

    public static void SaveFloat(string key, float value)
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }
    }

    public static string ReadString(string key)
    {
        return ReadString(key, "");
    }

    public static string ReadString(string key, string defaultValue)
    {

        if (string.IsNullOrEmpty(key))
        {
            return defaultValue;
        }
        string v = PlayerPrefs.GetString(key, defaultValue);
        return v;
    }

    public static void SaveString(string key, string value)
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
    }

    public static void DeleteKey(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
#if UNITY_WEBGL
            WebGLUtility.FlushPersist();
#endif
        }
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}