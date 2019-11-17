using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Components.Controls;
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
                CreateCalendar();
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

        public static readonly BindableProperty DownloadCommandProperty = BindableProperty.Create(nameof(DownloadCommand), typeof(ICommand), typeof(ActivityCardView), default(ICommand));
        public ICommand DownloadCommand
        {
            get => (ICommand)GetValue(DownloadCommandProperty);
            set => SetValue(DownloadCommandProperty, value);
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
            bt_download.IsVisible = true;
            lb_expand.RotateTo(-180, 250, Easing.CubicInOut);
            _isExpanded = true;
            CreateCalendar();
            bt_download.FadeTo(1, 250, Easing.CubicInOut);
        }


        private async Task Close(Frame frame)
        {
            lb_expand.RotateTo(0, 250, Easing.CubicInOut);
            _isExpanded = false;
            calendar_placeholder.Children.Clear();
            bt_download.Opacity = 0;
            bt_download.IsVisible = false;
        }

        private void CreateCalendar()
        {
            var cal = new ActivityCalendarView()
            {
                CurrentActivity = Activity,
                IsEnabled = false,
                InputTransparent = true,
                IsInputEnabled = false,
                Margin = new Thickness(0, 25, 0, 0),
                CellFontSize = 12,
                CellBackgroundColor = Color.FromHex("#CCCCCC"),
                CellForegroundColor = Color.White,
                AbsenceCellBackgroundColor = Color.FromHex("#FF5A39")
            };
            calendar_placeholder.Children.Add(cal);
        }

        private void bt_refuse_Clicked(object sender, EventArgs e)
        {
            if (RefuseCommand.CanExecute(Activity.Id))
                RefuseCommand.Execute(Activity.Id);
        }

        private void bt_accept_Clicked(object sender, EventArgs e)
        {
            if (AcceptCommand.CanExecute(Activity.Id))
                AcceptCommand.Execute(Activity.Id);
        }

        private void bt_download_Clicked(object sender, EventArgs e)
        {
            if (DownloadCommand.CanExecute(Activity.Id))
                DownloadCommand.Execute(Activity.Id);
        }
    }
}