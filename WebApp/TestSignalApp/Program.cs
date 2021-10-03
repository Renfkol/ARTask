using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Text;

namespace TestSignalApp
{


    //Для теста хаба у SignalR, запустить несколько приложений на дебаг, 
    //сообщения должны приходить и в Unity

    class Program
    {
        static HubConnection hubConnection;

        static async Task Main()
        {
            hubConnection = new HubConnectionBuilder()
                 .WithUrl("http://192.168.0.92:1038/animation")
                 .Build();

            hubConnection.On<string>("Send", message => Console.WriteLine($"Message from server: {message}"));

            await hubConnection.StartAsync();

            bool isExit = false;

            while (!isExit)
            {
                var message = Console.ReadLine();

                if(message =="setip")
                {
                    var ip = Console.ReadLine();
                    await hubConnection.SendAsync("SetIP", ip);
                    Console.WriteLine("ip установлен");
                }
                else if (message != "exit")
                    await hubConnection.SendAsync("SendMessage", message);
                else
                    isExit = true;
            }

            Console.ReadLine();
        }

    }
}
