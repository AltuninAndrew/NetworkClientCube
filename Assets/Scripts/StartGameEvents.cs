using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameEvents : MonoBehaviour
{

    GameObject _obj = null;
    GameObject _prefab;
    PrefabsBase prefabsBase;

    int _numObjOnScene = 0;

    void Start()
    {
        prefabsBase = GetComponent<PrefabsBase>();
       
    }

    public GameObject InitNewObj()
    {
        _prefab = prefabsBase.GetPrefab("CubePlayer");
        if (_numObjOnScene==0)
        {
            float randX = Random.Range(-4, 4.78f);
            float randZ = Random.Range(-8.7f, -0.37f);

           
            _obj = Instantiate(_prefab, new Vector3(randX, 1, randZ), Quaternion.identity);

            _obj.GetComponent<Renderer>().sharedMaterial.color = Random.ColorHSV();
            _numObjOnScene++;
        }
        return _obj;
    }

}
