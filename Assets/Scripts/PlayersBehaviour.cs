using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBehaviour : MonoBehaviour
{
    List<GameObject> _otherPlayers;
    PrefabsBase objectBase;

    string globalTypeOfObj;
    Vector3 position;

    bool needInst = false;
    void Start()
    {
        _otherPlayers = new List<GameObject>();
        objectBase = GetComponent<PrefabsBase>();
    }

    public void InitNewPlayer(string typeOfObj,Vector3 pos)
    {
       globalTypeOfObj = typeOfObj;
       position = pos;
       needInst = true;
    }

    void Update()
    {
        if(needInst)
        {
            Instantiate(objectBase.GetPrefab(globalTypeOfObj), position, Quaternion.identity);
            needInst = false;
        }
    }



}
