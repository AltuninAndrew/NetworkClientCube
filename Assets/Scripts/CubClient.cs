using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SignalR.Client._20.Hubs;
using System.Threading.Tasks;
using System.Net;
using SignalR.Client._20;
using UnityEngine.UI;

public class CubClient : MonoBehaviour
{


    NetworkCnt _networkCnt;

    Vector3 _pos;

    void Start()
    {
        _pos = transform.position;
        try
        {
            _networkCnt = GameObject.Find("GameController").GetComponent<NetworkCnt>();
        }
        catch
        {
            Debug.Log("NetCnt not found");
        }

    }

    void Update()
    {
        if (_networkCnt != null && _pos != transform.position && _networkCnt.IsHubbStarted)
        {
            _pos = transform.position;
            _networkCnt.SendPositionOnServer(_pos.x, _pos.y, _pos.z);
        }
    }


   

    

}
