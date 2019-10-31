using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.Events
{
    public class LoginEvent
    {
        public User CurrentUser { get; set; }
    }
}
