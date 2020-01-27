using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Common
{
    [Serializable]
    public class AppException : Exception
    {
        public string ErrorMessage { get; set; }
    }
}
