using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SignalR.Client._20.Hubs;
using System.Threading.Tasks;
using System.Net;
using SignalR.Client._20;
using UnityEngine.UI;
//using Newtonsoft.Json;

public class NetworkCnt : MonoBehaviour
{
    string _baseUrl = "http://localhost:60821";

    HubConnection _hubConnection = null;
    IHubProxy _hubProxy;
    Subscription _subscriptionPos;
    Subscription _subscriptionRegClient;
    Subscription _subscriptionInitClientInScene;
    bool _started = false;
    public bool IsHubbStarted { get; private set; } = false;

    [SerializeField]
    InputField _inputField;

    PlayersBehaviour playersBehaviour;
    GameObject _gameObjClient;
    bool _isReg = false;

    void StartSignalR()
    {
        if (_hubConnection == null)
        {
            _hubConnection = new HubConnection(_baseUrl);
            _hubConnection.Error += hubConnection_Error;
            _hubProxy = _hubConnection.CreateProxy("LobbyHub");
            _subscriptionPos = _hubProxy.Subscribe("broadcastPosition");
            _subscriptionPos.Data += subscriptionPos_Data;
            _subscriptionRegClient = _hubProxy.Subscribe("broadcastRegClient");
            _subscriptionRegClient.Data += subscriptionReg_Data;
            _subscriptionInitClientInScene = _hubProxy.Subscribe("initClientInScene");
            _subscriptionInitClientInScene.Data += subscriptionInit_Data;
            Task.Run(ConnectToHub);
            
        }
    }

    private void subscriptionPos_Data(object[] obj)
    {

        //string jsData = JsonConvert.SerializeObject(obj);
        //Debug.Log(obj);

        //Debug.Log(jsData);
        
    }

    //WARNING! It's a hard code!
    private void subscriptionReg_Data(object[] obj)
    {

        InitNewObjInScene(obj);

    }

    private void subscriptionInit_Data(object[] obj)
    {
        Debug.Log(obj[0]);
    }


    private void hubConnection_Error(System.Exception obj)
    {
        Debug.Log("Some error: " + obj.Message);

    }

    private void ConnectToHub()
    {
        _hubConnection.Start();
        IsHubbStarted = true;
        Debug.Log("Connection is successfully");
    }

    private void DisconnectFromHub()
    {
        if (_hubConnection != null)
        {
            _hubConnection.Error -= hubConnection_Error;
            _hubConnection.Stop();
        }
        _hubConnection = null;
        _hubProxy = null;
        _subscriptionPos = null;
        _subscriptionRegClient = null;
        IsHubbStarted = false;
        _started = false;
        _isReg = false;
        Debug.Log("Connection is stopped");
    }

    IEnumerator CheckConnection()
    {
        Debug.Log("Checking connections");
        yield return new WaitForSeconds(10);
        if (IsHubbStarted == false)
        {
            DisconnectFromHub();
        }
        else
        {
            // some other logic, for example: call method in server
        }

    }

    public void StartAndConnect()
    {
        _started = !_started;
        
        if (_started)
        {
            StartSignalR();
            Debug.Log("StartSignalR()");
            StartCoroutine(CheckConnection());
            _gameObjClient = GetComponent<StartGameEvents>().InitNewObj();
            playersBehaviour = gameObject.GetComponent<PlayersBehaviour>();
           
        }
        else
        {
            DisconnectFromHub();
        }

    }

    private void RegisterClientOnServer()
    {
        if(_inputField.text!="")
        {
            if(_gameObjClient!=null)
            {
                Vector3 pos = _gameObjClient.transform.position;
               Color clr = _gameObjClient.GetComponent<Renderer>().sharedMaterial.color;

                _hubProxy.Invoke("RegisterInLobby", _inputField.text, _hubConnection.ConnectionId,
                    pos.x, pos.y, pos.z,clr.r,clr.g,clr.b,clr.a);
                Debug.Log("Game object (client) successfully registered on the server");
            }
            else
            {
                Debug.Log("Game object(client) not registered on the server");
            }           
        }
    }

    public void SendPositionOnServer(float x,float y,float z)
    {
        _hubProxy.Invoke("SetNewPosition", _hubConnection.ConnectionId, x, y, z);
    }

    void Update()
    {
        if(IsHubbStarted == true && !_isReg)
        {
            RegisterClientOnServer();
            _isReg = true;
        }

    }

    void InitNewObjInScene(object[] objs)
    {
        string nameClient = objs[0].ToString();
        Vector3 posForNewPlayer = new Vector3();
        Color colorForNewPlayer = new Color();
        float.TryParse(objs[1].ToString(), out posForNewPlayer.x);
        float.TryParse(objs[2].ToString(), out posForNewPlayer.y);
        float.TryParse(objs[3].ToString(), out posForNewPlayer.z);
        float.TryParse(objs[4].ToString(), out colorForNewPlayer.r);
        float.TryParse(objs[5].ToString(), out colorForNewPlayer.g);
        float.TryParse(objs[6].ToString(), out colorForNewPlayer.b);
        float.TryParse(objs[7].ToString(), out colorForNewPlayer.a);

        playersBehaviour.InitNewPlayer("Cube", posForNewPlayer);
        //newPlayer.GetComponent<Renderer>().sharedMaterial.color = colorForNewPlayer; //encapsulate in class!
    }

}
