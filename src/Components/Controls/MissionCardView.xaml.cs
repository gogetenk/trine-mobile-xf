using System.Windows.Input;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MissionCardView : ContentView
    {
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(MissionCardView), null);
        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }

        public static readonly BindableProperty MissionProperty = BindableProperty.Create(nameof(Mission), typeof(MissionDto), typeof(MissionCardView), null, propertyChanged: OnMissionChanged);
        public MissionDto Mission
        {
            get => (MissionDto)GetValue(MissionProperty);
            set => SetValue(MissionProperty, value);
        }

        public MissionCardView()
        {
            InitializeComponent();
        }

        private static void OnMissionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((MissionCardView)bindable).FillFrame(newValue as MissionDto);
        }

        private void FillFrame(MissionDto missionDto)
        {
            if (missionDto is null)
                return;

            const string profilePicturePlaceholder = "https://i0.wp.com/w.media/wp-content/uploads/2018/01/placeholderhead.jpg";

            lb_title.Text = missionDto.ProjectName;
            lb_startDate.Text = missionDto.StartDate.ToString("dd/MM/yyyy");
            lb_endDate.Text = missionDto.EndDate.ToString("dd/MM/yyyy");
            bv_status.Color = missionDto.PinColor;
            img_user2.Source = (missionDto.Customer?.ProfilePicUrl == null) ? profilePicturePlaceholder : missionDto.Customer.ProfilePicUrl;
            img_user3.Source = (missionDto.Consultant?.ProfilePicUrl == null) ? profilePicturePlaceholder : missionDto.Consultant.ProfilePicUrl;
            img_user1.Source = (missionDto.Commercial?.ProfilePicUrl == null) ? profilePicturePlaceholder : missionDto.Commercial.ProfilePicUrl;
            img_user1.IsVisible = missionDto.IsTripartite;
            lb_consultantName.Text = missionDto.Consultant.IsDummy ? missionDto.Consultant?.Firstname + " " + missionDto.Consultant?.Lastname : missionDto.Consultant?.Mail;
        }
    }
}