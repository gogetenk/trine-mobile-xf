using System;
using System.Windows.Input;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitySphereView : ContentView
    {
        #region Bindings

        private readonly BindableProperty DayProperty = BindableProperty.Create(nameof(Day), typeof(string), typeof(ActivitySphereView), default(string));
        public string Day
        {
            get => (string)GetValue(DayProperty);
            set => SetValue(DayProperty, value);
        }

        private readonly BindableProperty DayPartProperty = BindableProperty.Create(nameof(DayPart), typeof(DayPartEnum), typeof(ActivitySphereView), default(DayPartEnum));
        public DayPartEnum DayPart
        {
            get => (DayPartEnum)GetValue(DayPartProperty);
            set
            {
                SetValue(DayPartProperty, value);
                ChangeDayPart();
            }
        }

        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ActivitySphereView), null);
        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }

        public static readonly BindableProperty LongPressCommandProperty = BindableProperty.Create(nameof(LongPressCommand), typeof(ICommand), typeof(ActivitySphereView), null);
        public ICommand LongPressCommand
        {
            get => (ICommand)GetValue(LongPressCommandProperty);
            set => SetValue(LongPressCommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ActivitySphereView), null);
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(ActivitySphereView), default(Color));
        public Color CellBackgroundColor
        {
            get => (Color)GetValue(CellBackgroundColorProperty);
            set => SetValue(CellBackgroundColorProperty, value);
        }

        public static readonly BindableProperty CellForegroundColorProperty = BindableProperty.Create(nameof(CellForegroundColor), typeof(Color), typeof(ActivitySphereView), default(Color));
        public Color CellForegroundColor
        {
            get => (Color)GetValue(CellForegroundColorProperty);
            set => SetValue(CellForegroundColorProperty, value);
        }

        public static readonly BindableProperty CellFontSizeProperty = BindableProperty.Create(nameof(CellFontSize), typeof(int), typeof(ActivitySphereView), default(int));
        public int CellFontSize
        {
            get => (int)GetValue(CellFontSizeProperty);
            set => SetValue(CellFontSizeProperty, value);
        }

        #endregion

        private DateTime _activityDate;
        public DateTime ActivityDate
        {
            get => _activityDate;
            set
            {
                _activityDate = value;
                Day = value.Day.ToString();
            }
        }

        public GridDayDto GridDay { get; set; }
        public event EventHandler<EventArgs> Tapped;
        public event EventHandler<EventArgs> LongPressed;
        public bool IsInputEnabled { get; set; }
        private int _tapCount;

        public ActivitySphereView()
        {
            InitializeComponent();

            lb_day.SetBinding(Label.TextProperty, new Binding(nameof(Day), source: this));
            lb_day.SetBinding(Label.TextColorProperty, new Binding(nameof(CellForegroundColor), source: this));
            lb_day.SetBinding(Label.FontSizeProperty, new Binding(nameof(CellFontSize), source: this));
            frame.SetBinding(Label.BackgroundColorProperty, new Binding(nameof(CellBackgroundColor), source: this));

            //LongPressed += ActivitySphereView_LongPressed;
            //RightClicked += ActivitySphereView_LongPressed;

        }

        private void ActivitySphereView_LongPressed(object sender, EventArgs e)
        {
            var param = GridDay;
            if (LongPressCommand?.CanExecute(param) ?? true)
            {
                //LongPressed?.Invoke(this, e);
                LongPressCommand?.Execute(param);
            }
        }

        private void MultiGestureView_LongPressed_1(object sender, EventArgs e)
        {
            var param = GridDay;
            if (LongPressCommand?.CanExecute(param) ?? true)
            {
                //LongPressed?.Invoke(this, e);
                LongPressCommand?.Execute(param);
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!IsInputEnabled)
                return;

            // Dans le cas où le jour est présélectionné car il est ouvré, on change le daypart a full automatiquement
            if (_tapCount == 0 && DayPart == DayPartEnum.Full)
                _tapCount = 1;

            _tapCount++;
            if (_tapCount > 3)
                _tapCount = 0;

            await frame.ScaleTo(1.4, 100, Easing.BounceIn);
            frame.ScaleTo(1.0, 200, Easing.BounceOut);

            switch (_tapCount)
            {
                case 0: // empty
                    DayPart = DayPartEnum.None;
                    break;
                case 1: // full
                    DayPart = DayPartEnum.Full;
                    break;
                case 2: // matin
                    DayPart = DayPartEnum.Morning;
                    break;
                case 3: // soir
                    DayPart = DayPartEnum.Afternoon;
                    break;
                default:
                    break;
            }

            ChangeDayPart();
            GridDay.WorkedPart = DayPart;

            object resolvedParameter;

            if (CommandParameter != null)
                resolvedParameter = CommandParameter;
            else
                resolvedParameter = e;

            if (TappedCommand?.CanExecute(resolvedParameter) ?? true)
            {
                Tapped?.Invoke(this, e);
                TappedCommand?.Execute(resolvedParameter);
            }
        }

        private void ChangeDayPart()
        {
            switch (DayPart)
            {
                case DayPartEnum.None: // empty
                    boxview.TranslateTo(0, 70, 100, Easing.CubicInOut);
                    break;
                case DayPartEnum.Full: // full
                    boxview.ScaleTo(5, 100, Easing.CubicInOut);
                    CellForegroundColor = Color.White;
                    break;
                case DayPartEnum.Morning: // matin
                    boxview.ScaleTo(1, 100, Easing.CubicInOut);
                    boxview.TranslateTo(0, -15, 100, Easing.CubicInOut);
                    CellForegroundColor = Color.White;
                    break;
                case DayPartEnum.Afternoon: // soir
                    boxview.ScaleTo(1, 100, Easing.CubicInOut);
                    boxview.TranslateTo(0, 25, 100, Easing.CubicInOut);
                    CellForegroundColor = Color.White;
                    break;
                default:
                    break;
            }

            //DayClicked.Execute(DayPart);
        }
    }
}