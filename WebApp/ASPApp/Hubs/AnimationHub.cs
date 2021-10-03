using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPApp.Hubs
{
    public class AnimationHub:Hub
    {
        //Методы приема сообщений у хаба
        //В данном случае только 2 с рассылкой и опросом всех, 
        //Но если прицепить Identity, то можно будет рассылать по юзерам или группам направленно
        public Task SendMessage(string messege)
        {
            Debug.WriteLine(Context.ConnectionId);
            if (Context.Items.ContainsKey("ip"))
                messege = messege + " IP отправителя:" + Context.Items["ip"];
            return Clients.Others.SendAsync("Animation", messege);
        }

        public Task SetIP(string ip)
        {
            Context.Items.TryAdd("ip", ip);
            Debug.WriteLine("Reserved IP:" + ip);
            return Task.CompletedTask;
        }
    }
}
