using BLOG.Application.Common.Abstractions;
using BLOG.Application.Features.Comment.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Services
{
    public class CommunicationServiceClient : Hub, ICommunicationServiceClient
    {
        private readonly IHubContext<CommunicationServiceClient> _hubContext;

        public CommunicationServiceClient(IHubContext<CommunicationServiceClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Broadcast(CommentChangedEvent @event)
        {
            await _hubContext.Clients.All.SendAsync(@event.Name , @event.PostId);
        }
    }
}
