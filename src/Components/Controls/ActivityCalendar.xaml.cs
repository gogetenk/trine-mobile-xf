using System;
using System.Linq;
using Trine.Mobile.Dto;
using Xamarin.Forms;

namespace Trine.Mobile.Components.Controls
{
    public partial class ActivityCalendar : ContentView
    {
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ActivityCalendar)bindable).CreateCalendar();
        }

        public static readonly BindableProperty ActivityProperty = BindableProperty.Create(nameof(Activity), typeof(ActivityDto), typeof(ActivityCalendar), default(ActivityDto), propertyChanged: OnPropertyChanged);
        public ActivityDto Activity
        {
            get => (ActivityDto)GetValue(ActivityProperty);
            set
            {
                SetValue(ActivityProperty, value);

            }
        }

        public ActivityCalendar()
        {
            InitializeComponent();
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

        private void CreateCalendar()
        {
            Container.Children.Clear();

            var width = (2 * Application.Current.MainPage.Width) / 3;
            var columnWidth = width / 7;
            var firstDayOfActivity = Activity.Days.FirstOrDefault();
            
            var columnIndex = GetIndexFromDayOfWeek(firstDayOfActivity.Day.DayOfWeek) - 1;
            var rowIndex = 0;

            foreach (var day in Activity.Days)
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

                Container.Children.Add(flexLayout, columnIndex, rowIndex);
                Container.Children.Add(label, columnIndex, rowIndex);
            }
        }
    }
}
