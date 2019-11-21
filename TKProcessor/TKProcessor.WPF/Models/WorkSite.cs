using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TKProcessor.WPF.Models
{
    public class WorkSite : BaseModel
    {
        public string SiteId { get; set; }
        public string Name { get; set; }

        public string IntegrationAuthUsername { get; set; }
        public string IntegrationAuthPassword { get; set; }

        public ObservableCollection<EmployeeWorkSite> WorkSiteEmployees { get; set; }
    }
}
