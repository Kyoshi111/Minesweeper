
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            var objs = FindObjectsOfType(typeof(T)) as T[];
                
            if (objs != null && objs.Length > 0)
                _instance = objs[0];
                
            if (objs != null && objs.Length > 1)
                Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                
            if (_instance != null) return _instance;
                
            var obj = new GameObject
            {
                name = typeof(T).Name,
                hideFlags = HideFlags.HideAndDontSave
            };
                    
            _instance = obj.AddComponent<T>();

            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
