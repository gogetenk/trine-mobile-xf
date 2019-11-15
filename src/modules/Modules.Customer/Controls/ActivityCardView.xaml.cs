using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Customer.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityCardView : ContentView
    {
        private bool _isExpanded;

        public static readonly BindableProperty ActivityProperty = BindableProperty.Create(nameof(Activity), typeof(ActivityDto), typeof(ActivityCardView), default(ActivityDto));
        public ActivityDto Activity
        {
            get => (ActivityDto)GetValue(ActivityProperty);
            set
            {
                SetValue(ActivityProperty, value);
                calendar.CurrentActivity = value;
            }
        }

        public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(ActivityCardView), default(ICommand));
        public ICommand AcceptCommand
        {
            get => (ICommand)GetValue(AcceptCommandProperty);
            set => SetValue(AcceptCommandProperty, value);
        }

        public static readonly BindableProperty RefuseCommandProperty = BindableProperty.Create(nameof(RefuseCommand), typeof(ICommand), typeof(ActivityCardView), default(ICommand));
        public ICommand RefuseCommand
        {
            get => (ICommand)GetValue(RefuseCommandProperty);
            set => SetValue(RefuseCommandProperty, value);
        }


        public ActivityCardView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var frame = sender as Frame;
            if (frame is null)
                return;

            if (_isExpanded)
                Close(frame);
            else
                Open(frame);
        }

        private async Task Open(Frame frame)
        {
            lb_expand.RotateTo(-180, 250, Easing.CubicInOut);
            calendar.IsVisible = true;
            _isExpanded = true;
        }

        private async Task Close(Frame frame)
        {
            lb_expand.RotateTo(0, 250, Easing.CubicInOut);
            calendar.IsVisible = false;
            _isExpanded = false;
        }

        private void bt_refuse_Clicked(object sender, EventArgs e)
        {
            RefuseCommand.Execute(Activity.Id);
        }

        private void bt_accept_Clicked(object sender, EventArgs e)
        {
            AcceptCommand.Execute(Activity.Id);
        }
    }
}