using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SignalR.Client._20.Hubs;
using System.Threading.Tasks;
using System.Net;
using SignalR.Client._20;
using UnityEngine.UI;

public class client : MonoBehaviour
{
    string _baseUrl = "http://localhost:60821";

    HubConnection _hubConnection = null;
    IHubProxy _hubProxy;
    Subscription _subscription;
    bool started = false;
    bool isHubbStarted = false;


    void StartSignalR()
    {
        if (_hubConnection == null)
        {
            _hubConnection = new HubConnection(_baseUrl);
            _hubConnection.Error += hubConnection_Error;
            _hubProxy = _hubConnection.CreateProxy("LobbyHub");   
            _subscription = _hubProxy.Subscribe("broadcastMess");
            _subscription.Data += subscription_Data;
            Task.Run(ConnectToHub);
        }
    }

    private void subscription_Data(object[] obj)
    {
        Debug.Log("Name: " + obj[0]);
    }

    private void hubConnection_Error(System.Exception obj)
    {
        Debug.Log("Some error: " + obj.Message);

    }

    private void ConnectToHub()
    {
        _hubConnection.Start();
        isHubbStarted = true;
        Debug.Log("Connection is successfully");
        
    }

    private void DisconnectFromHub()
    {
        if(_hubConnection!=null)
        {
            _hubConnection.Error -= hubConnection_Error;
            _hubConnection.Stop();
        }
        _hubConnection = null;
        _hubProxy = null;
        _subscription = null;
        isHubbStarted = false;
        started = false;
        Debug.Log("Connection is stopped");
    }

    IEnumerator CheckConnection()
    {
        Debug.Log("Checking connections");
        yield return new WaitForSeconds(10);
        if (isHubbStarted == false)
        {
            DisconnectFromHub();
        } else
        {
            
           // some other logic, for example: call method in server
        }
        
    }
    
    public void StartAndConnect()
    {
        started = !started;
        if(started)
        {
            StartSignalR();
            Debug.Log("StartSignalR()");
            StartCoroutine(CheckConnection());
           
        }
        else
        {
            DisconnectFromHub();
        }
    }

    

}
