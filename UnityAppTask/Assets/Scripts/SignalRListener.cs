using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Collections;

public class SignalRListener : MonoBehaviour
{        
    private static HubConnection connection;
    void Start()
    {
        
    }

    public void StartListening()
    {
        Debug.Log("Hello World!");

        //Инициализация клиента SignalR

        connection = new HubConnectionBuilder()
            .WithUrl(Manager.instance.urlHub)
            .Build();

        connection.Closed += async (error) =>
        {
            await Task.Delay(Random.Range(0, 5) * 1000);
            await connection.StartAsync();
        };

        Connect();
    }

    private async void Connect()
    {
        //Подключение к хабу animation на сервере

        connection.On<string>("Animation", message =>
        {
            Debug.Log($"Message from server: {message}");
            if (message == Manager.instance.deviceIP)
            {
                Debug.Log("IP confirmation");

                //В данном месте корутину просто запустить не получалось - ломает соединение
                Manager.instance.animate = true;
            }
            else Debug.Log("IP no access");
        });
        try
        {
            await connection.StartAsync();
            Debug.Log("Connection started");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        await connection.SendAsync("SetIP", Manager.instance.deviceIP);
    }


}