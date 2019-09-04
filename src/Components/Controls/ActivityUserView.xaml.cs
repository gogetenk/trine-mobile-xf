using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityUserView : ContentView
    {
        public static readonly BindableProperty UserProperty = BindableProperty.Create(nameof(User), typeof(UserActivityDto), typeof(ActivityUserView), null, propertyChanged: OnUserChanged);
        public UserActivityDto User
        {
            get => (UserActivityDto)GetValue(ActivityProperty);
            set => SetValue(ActivityProperty, value);
        }

        public static readonly BindableProperty ActivityProperty = BindableProperty.Create(nameof(Activity), typeof(ActivityDto), typeof(ActivityUserView), null, propertyChanged: OnMissionChanged);
        public ActivityDto Activity
        {
            get => (ActivityDto)GetValue(ActivityProperty);
            set => SetValue(ActivityProperty, value);
        }

        private static void OnMissionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ActivityUserView)bindable).Fill(newValue as ActivityDto);
        }

        private static void OnUserChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ActivityUserView)bindable).FillUser(newValue as UserActivityDto);
        }



        public ActivityUserView()
        {
            InitializeComponent();
        }

        private void Fill(ActivityDto activityDto)
        {

        }

        private void FillUser(UserActivityDto user)
        {

        }
    }
}