using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Events
{
    public class NewMessageEvent
    {
        public NewMessageEvent(string message)
        {
            Message = message;
            MessageType = MessageType.Information;
        }

        public NewMessageEvent(string message, int duration)
        {
            Message = message;
            MessageType = MessageType.Information;
            Duration = duration * 1000;
        }

        public NewMessageEvent(string message, MessageType messageType)
        {
            Message = message;
            MessageType = messageType;
            Duration = 3000;
        }

        public NewMessageEvent(string message, MessageType messageType, int duration)
        {
            Message = message;
            MessageType = messageType;
            Duration = duration * 1000;
        }

        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public int Duration { get; set; }
    }

    public enum MessageType
    {
        Success,
        Information,
        Warning,
        Error
    }
}
