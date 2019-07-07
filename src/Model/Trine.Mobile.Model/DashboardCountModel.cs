namespace Trine.Mobile.Model
{
    /// <summary>
    /// Contains the informations to show on the home dashboard UI
    /// </summary>
    public class DashboardCountModel
    {
        public int MissionCount { get; set; }
        public int ClientCount { get; set; }
        public int EventCount { get; set; }
        public UserModel CurrentUser { get; set; }
    }
}
