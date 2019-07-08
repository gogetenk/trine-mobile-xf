using System;

namespace Trine.Mobile.Dto
{
    public  class DashboardViewMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
    }
}