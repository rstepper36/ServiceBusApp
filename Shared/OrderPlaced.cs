using NServiceBus;
using System;

namespace Shared
{
    public class OrderPlaced : IEvent
    {
        public Guid OrderId { get; set; }
    }
}
