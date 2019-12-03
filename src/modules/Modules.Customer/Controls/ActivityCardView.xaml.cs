using System;
using System.Linq;
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

        public static readonly BindableProperty ShowDownloadButtonProperty = BindableProperty.Create(nameof(ShowDownloadButton), typeof(bool), typeof(ActivityCardView), default(bool), propertyChanged: OnPropertyChanged);
        public bool ShowDownloadButton
        {
            get => (bool)GetValue(ShowDownloadButtonProperty);
            set => SetValue(ShowDownloadButtonProperty, value);
        }


        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var value = (bool)newValue;
            if (value)
            {
                (((ActivityCardView)bindable).bt_download).IsVisible = true;
                (((ActivityCardView)bindable).bt_accept).IsVisible = false;
                (((ActivityCardView)bindable).bt_refuse).IsVisible = false;
            }
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
            //bt_download.IsVisible = true;
            lb_expand.RotateTo(-180, 250, Easing.CubicInOut);
            _isExpanded = true;
            CreateCalendar();
            //bt_download.FadeTo(1, 250, Easing.CubicInOut);
        }


        private async Task Close(Frame frame)
        {
            lb_expand.RotateTo(0, 250, Easing.CubicInOut);
            _isExpanded = false;
            calendar_placeholder.Children.Clear();
            //bt_download.Opacity = 0;
            //bt_download.IsVisible = false;
        }

        private void InitActionButtons()
        {
            //bt_accept.IsVisible = Activity.Status == ActivityStatusEnum.ConsultantSigned;
            //bt_refuse.IsVisible = Activity.Status == ActivityStatusEnum.ConsultantSigned;
            bt_download.IsVisible = Activity.Status == ActivityStatusEnum.CustomerSigned;
        }

        private int GetIndexFromDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }

        public DateTime GetStartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        private void CreateCalendar()
        {
            var width = (2 * this.Width) / 3;
            var columnWidth = width  / 7;

            var firstDayOfActivity = this.Activity.Days.FirstOrDefault();
  
            var grid = new Grid();
           

            var columnIndex = GetIndexFromDayOfWeek(firstDayOfActivity.Day.DayOfWeek) - 1;
            var rowIndex = 0;

            foreach (var day in this.Activity.Days)
            {

                if (columnIndex < 6)
                {
                    columnIndex++;
                }
                else
                {
                    columnIndex = 0;
                    rowIndex++;
                }

                var flexLayout = new FlexLayout()
                {
                    JustifyContent = FlexJustify.Center,
                    AlignItems = FlexAlignItems.Center
                };

                var circle = new BoxView()
                {
                    BackgroundColor = Color.FromHex("#6969E5"),
                    WidthRequest = columnWidth,
                    HeightRequest = columnWidth,
                    CornerRadius = columnWidth / 2
                };

                var isFullDay = day.WorkedPart == DayPartEnum.Full;

                var label = new Label()
                {
                    FontAttributes = isFullDay ? FontAttributes.Bold : FontAttributes.None,
                    TextColor = isFullDay ? Color.White : Color.FromHex("#3E3E3E"),
                    Text = day.Day.ToString("dd"),
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                if (isFullDay)
                {
                    flexLayout.Children.Add(circle);
                }
                else
                {
                    label.FontSize = 12;
                }

                grid.Children.Add(flexLayout, columnIndex, rowIndex);
                grid.Children.Add(label, columnIndex, rowIndex);
            }


            calendar_placeholder.Children.Add(grid);

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
