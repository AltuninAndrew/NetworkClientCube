using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsBase : MonoBehaviour
{
    [SerializeField]
    List<GameObject> prefabs = new List<GameObject>();

    Dictionary<string, GameObject> prefabsBase = new Dictionary<string, GameObject>();

    void Start()
    {
       
        foreach (GameObject obj in prefabs)
        {
            prefabsBase.Add(obj.name, obj);
        }
    }

    public GameObject GetPrefab(string name)
    {
        if (prefabsBase.ContainsKey(name))
        {
            return prefabsBase[name];
        }else
        {
            return null;
        }
    }
}
