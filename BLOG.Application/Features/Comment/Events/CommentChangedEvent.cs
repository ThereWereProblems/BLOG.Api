using BLOG.Application.Common.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Features.Comment.Events
{
    public class CommentChangedEvent : INotification
    {
        public string Name => "ReloadComments";
        public int PostId { get; set; }
    }

    public class CommentChangedEventListener : INotificationHandler<CommentChangedEvent>
    {
        private readonly ICommunicationServiceClient _communicationServiceClient;

        public CommentChangedEventListener(ICommunicationServiceClient communicationServiceClient)
        {
            _communicationServiceClient = communicationServiceClient;
        }
        
        public async Task Handle(CommentChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _communicationServiceClient.Broadcast(notification);
            }
            catch (Exception)
            {

            }
        }
    }
}
