using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Model
{
    public class DashboardMissionModel
    {
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationIcon { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsTripartite { get; set; }
        public string CommercialName { get; set; }
        public string ConsultantName { get; set; }
        public string CustomerName { get; set; }
    }
}
