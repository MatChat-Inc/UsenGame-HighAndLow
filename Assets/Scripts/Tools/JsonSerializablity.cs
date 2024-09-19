using UnityEngine;

public class JsonSerializablity {
    public static string Serialize(object obj) {
        string jsonString = JsonUtility.ToJson(obj);
        return jsonString;
    }

    public static T Deserialize<T>(string content) {
        var t = JsonUtility.FromJson<T>(content);
        return t;
    }
}