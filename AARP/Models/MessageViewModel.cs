using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models
{
    public class MessageViewModel
    {
        public MessageType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }

    public enum MessageType
    {
        Success,
        Error,
        Warning,
        Info
    }
}