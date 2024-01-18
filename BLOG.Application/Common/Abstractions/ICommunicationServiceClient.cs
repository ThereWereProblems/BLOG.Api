using BLOG.Application.Features.Comment.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Application.Common.Abstractions
{
    public interface ICommunicationServiceClient
    {
        Task Broadcast(CommentChangedEvent @event);
    }
}
