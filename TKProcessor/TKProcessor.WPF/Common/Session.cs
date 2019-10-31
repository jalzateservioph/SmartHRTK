using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.Common
{
    public class Session : PropertyChangedBase
    {
        private static Session s_default;
        private User _currentUser;

        public static Session Default => s_default ?? (s_default = new Session());

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
