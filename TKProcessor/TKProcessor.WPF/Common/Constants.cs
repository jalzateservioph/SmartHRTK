using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Common
{
    public static class Constants
    {
        public static string OpenFilter { get => "Excel files (*.xlsx)|*.xlsx"; }
        public static string SaveFilter { get => "Excel files (*.xlsx)|*.xlsx"; }

        public static string DateFormatForFilter { get => "MM/dd/yyyy MM-dd-yyyy h:mm tt hh:mm - dddd"; }
    }
}
