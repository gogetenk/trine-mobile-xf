using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityCalendarView : ContentView
    {
        #region Bindable properties

        public static readonly BindableProperty CurrentActivityProperty = BindableProperty.Create(nameof(CurrentActivity), typeof(ActivityDto), typeof(ActivityCalendarView), default(ActivityDto), propertyChanged: OnPropertyChanged);
        public ActivityDto CurrentActivity
        {
            get => (ActivityDto)GetValue(CurrentActivityProperty);
            set
            {
                SetValue(CurrentActivityProperty, value);
                PopulateCalendar(value);
            }
        }

        public static readonly BindableProperty DaySettingsCommandProperty = BindableProperty.Create(nameof(DaySettingsCommand), typeof(ICommand), typeof(ActivityCalendarView), default(ICommand));
        public ICommand DaySettingsCommand
        {
            get => (ICommand)GetValue(DaySettingsCommandProperty);
            set => SetValue(DaySettingsCommandProperty, value);
        }

        /// <summary>
        /// Used to know if we have to update the CRA
        /// </summary>
        //public static readonly BindableProperty DayClickedCommandProperty = BindableProperty.Create(nameof(DayClickedCommand), typeof(ICommand), typeof(ActivityCalendarView), default(ICommand));
        //public ICommand DayClickedCommand
        //{
        //    get => (ICommand)GetValue(DayClickedCommandProperty);
        //    set => SetValue(DayClickedCommandProperty, value);
        //}

        public static readonly BindableProperty ColumnWidthProperty = BindableProperty.Create(nameof(ColumnWidth), typeof(int), typeof(ActivityCalendarView), default(int));
        public int ColumnWidth
        {
            get => (int)GetValue(ColumnWidthProperty);
            set => SetValue(ColumnWidthProperty, value);
        }

        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(ActivityCalendarView), default(int));
        public int RowHeight
        {
            get => (int)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }

        public static readonly BindableProperty WorkedDaysProperty = BindableProperty.Create(nameof(WorkedDays), typeof(float), typeof(ActivityCalendarView), default(float));
        public float WorkedDays
        {
            get => (float)GetValue(WorkedDaysProperty);
            set => SetValue(WorkedDaysProperty, value);
        }

        public static readonly BindableProperty IsInputEnabledProperty = BindableProperty.Create(nameof(IsInputEnabled), typeof(bool), typeof(ActivityCalendarView), default(bool), propertyChanged: OnInputEnabledPropertyChanged);
        public bool IsInputEnabled
        {
            get => (bool)GetValue(IsInputEnabledProperty);
            set => SetValue(IsInputEnabledProperty, value);
        }

        public static readonly BindableProperty DaysDictionaryProperty = BindableProperty.Create(nameof(DaysDictionary), typeof(List<GridDayDto>), typeof(ActivityCalendarView), default(List<GridDayDto>));
        public List<GridDayDto> DaysDictionary
        {
            get => (List<GridDayDto>)GetValue(DaysDictionaryProperty);
            set => SetValue(DaysDictionaryProperty, value);
        }

        public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(ActivityCalendarView), default(Color));
        public Color CellBackgroundColor
        {
            get => (Color)GetValue(CellBackgroundColorProperty);
            set => SetValue(CellBackgroundColorProperty, value);
        }

        public static readonly BindableProperty AbsenceCellBackgroundColorProperty = BindableProperty.Create(nameof(AbsenceCellBackgroundColor), typeof(Color), typeof(ActivityCalendarView), default(Color));
        public Color AbsenceCellBackgroundColor
        {
            get => (Color)GetValue(AbsenceCellBackgroundColorProperty);
            set => SetValue(AbsenceCellBackgroundColorProperty, value);
        }

        public static readonly BindableProperty CellForegroundColorProperty = BindableProperty.Create(nameof(CellForegroundColor), typeof(Color), typeof(ActivityCalendarView), default(Color));
        public Color CellForegroundColor
        {
            get => (Color)GetValue(CellForegroundColorProperty);
            set => SetValue(CellForegroundColorProperty, value);
        }

        public static readonly BindableProperty CellFontSizeProperty = BindableProperty.Create(nameof(CellForegroundColor), typeof(int), typeof(ActivitySphereView), default(int));
        public int CellFontSize
        {
            get => (int)GetValue(CellFontSizeProperty);
            set => SetValue(CellFontSizeProperty, value);
        }

        #endregion

        public EventHandler OnDrawBitmap;
        private ActivityDto _activity;

        public ActivityCalendarView()
        {
            InitializeComponent();

            DaysDictionary = new List<GridDayDto>();

            //foreach (var column in grid_calendar.ColumnDefinitions)
            //    column.SetBinding(ColumnDefinition.WidthProperty, new Binding(nameof(ColumnWidth), source: this));

            //foreach (var column in grid_calendar.RowDefinitions.Skip(1))
            //    column.SetBinding(RowDefinition.HeightProperty, new Binding(nameof(RowHeight), source: this));
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ActivityCalendarView)bindable)._activity = (ActivityDto)newValue;
            ((ActivityCalendarView)bindable).PopulateCalendar(((ActivityCalendarView)bindable)._activity);
        }

        private bool _isInputEnabled;
        private static void OnInputEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ActivityCalendarView)bindable)._isInputEnabled = (bool)newValue;
            ((ActivityCalendarView)bindable).PopulateCalendar(((ActivityCalendarView)bindable)._activity);
        }


        private void PopulateCalendar(ActivityDto activity)
        {
            if (activity == null)
                return;

            // Clean
            grid_calendar.Children.Clear();

            // On commence par la colonne représentant le premier jour du mois
            int columnIndex = GetIndexFromDayOfWeek(activity.Days.FirstOrDefault().Day.DayOfWeek) - 1; // -1 pour compenser le +1 du foreach
            int rowIndex = 1;

            foreach (var day in activity.Days)
            {
                if (columnIndex < 6)
                    columnIndex++;
                else
                {
                    columnIndex = 0;
                    rowIndex++;
                }

                DayPartEnum dayPart = DayPartEnum.None;

                // Si le CRA n'a pas encore été rempli, alors on pré rempli les jours ouvrés
                // TODO : Bug possible si l'activity est vide volontairement mais pas encore signé => ça va toujours remplir tous les jours ouvrés
                if (activity.Days.All(x => x.WorkedPart != DayPartEnum.None) && activity.Consultant.SignatureDate == null && activity.Consultant.SignatureDate == default(DateTime))
                    dayPart = (day.IsOpen) ? DayPartEnum.Full : DayPartEnum.None;
                else
                    dayPart = day.WorkedPart; // Sinon, on assigne les valeurs déjà renseignées

                // Mise à jour de l'activity (pour que le viewmodel soit au courant)
                CurrentActivity.Days.Where(x => x.Day == day.Day).FirstOrDefault().WorkedPart = dayPart;

                // Ajout des spheres sur l'interface
                var sphere = new ActivitySphereView()
                {
                    GridDay = day,
                    CellBackgroundColor = (day.Absence == null) ? CellBackgroundColor : AbsenceCellBackgroundColor,
                    CellForegroundColor = CellForegroundColor,
                    CellFontSize = CellFontSize,
                    ActivityDate = day.Day,
                    DayPart = dayPart,
                    TappedCommand = new Command(() => SphereClicked()),
                    LongPressCommand = new Command((gridDay) => SphereLongPressed((GridDayDto)gridDay)),
                    IsInputEnabled = _isInputEnabled,
                };

                grid_calendar.Children.Add(
                    sphere,
                    columnIndex,
                    rowIndex
                );
            }

            //for (int i = 0; i < grid_calendar.Children.Count() - 1; i++)
            //{
            //    if (i % 2 != 0)
            //    {
            //        grid_calendar.Children.RemoveAt(i);
            //    }
            //}

            //var t = grid_calendar.Children.Where(x => x.Height > 50).ToList();
            //grid_calendar.Children.RemoveAt(0);
            //grid_calendar.Children.RemoveAt(grid_calendar.Children.Count() - 1);

            SphereClicked();

            if (IsInputEnabled)
                return;

            // We are calling the custom renderer if we are in readonly, so we can generate a bitmap from the calendar
            //OnDrawBitmap.Invoke(this, new EventArgs());
        }

        // Called by native renderer to generate an image of the calendar. So it can be an image for read only purpose.
        public void SetImage(byte[] bytes)
        {
            if (IsInputEnabled)
                return;

            if (bytes is null)
                return;

            image.IsVisible = true;
            image.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            layout.IsVisible = false;
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

        private void SphereClicked()
        {
            var spheres = grid_calendar
                                .Children
                                .Where(x => x is ActivitySphereView)
                                .Select(x => (ActivitySphereView)x);

            CurrentActivity.Days = spheres.Select(x => x.GridDay).ToList();

            float fulldays = spheres
               .Count(x => (x.DayPart == DayPartEnum.Full));

            float halfdays = spheres
                .Count(x => (x.DayPart == DayPartEnum.Afternoon) || x.DayPart == DayPartEnum.Morning);

            WorkedDays = fulldays + (halfdays / 2f);
            CurrentActivity.DaysNb = WorkedDays;
            // Trigger an event
            //DayClickedCommand?.Execute(null);
        }

        private void SphereLongPressed(GridDayDto gridDay)
        {
            DaySettingsCommand.Execute(gridDay);
        }
    }
}