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

        public NewMessageEvent(string message, MessageType messageType)
        {
            Message = message;
            MessageType = messageType;
        }

        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Success,
        Information,
        Warning,
        Error
    }
}
