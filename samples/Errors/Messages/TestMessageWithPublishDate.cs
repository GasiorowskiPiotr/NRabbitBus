using System;
using NRabbitBus.Framework.Shared;

namespace Messages
{
    public class TestMessageWithPublishDate : IMessage
    {
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class TestRpcMessageWithPublishDate : IMessage
    {
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public int SequenceNo { get; set; }
    }
}