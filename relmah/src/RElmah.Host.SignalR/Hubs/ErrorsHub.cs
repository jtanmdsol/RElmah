﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RElmah.Host.SignalR.Hubs
{
    [HubName("relmah-errors")]
    public class ErrorsHub : Hub
    {
        private readonly IConnector      _connector;
        private readonly IUserIdProvider _userIdProvider;

        public ErrorsHub(IConnector connector, IUserIdProvider userIdProvider)
        {
            _connector = connector;
            _userIdProvider = userIdProvider;
        }

        public override Task OnConnected()
        {
            var apps = Enumerable.Empty<string>();
            _connector.Connect(_userIdProvider.GetUserId(Context.Request), a => apps = apps.Concat(new [] { a }));

            Clients.Caller
                .applications(apps);

            return base.OnConnected();
        }

        public void Monitor(IEnumerable<string> subscribe, IEnumerable<string> unsubscribe)
        {
            foreach (var app in subscribe)
                Groups.Add(Context.ConnectionId, app);

            foreach (var app in unsubscribe)
                Groups.Remove(Context.ConnectionId, app);
        }
    }
}
