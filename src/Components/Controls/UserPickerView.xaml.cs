﻿using System;
using System.Windows.Input;
using Trine.Mobile.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trine.Mobile.Components.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPickerView : ContentView
    {
        #region Bindable Properties

        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(UserPickerView), null);
        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }

        public static readonly BindableProperty RemovedCommandProperty = BindableProperty.Create(nameof(RemovedCommand), typeof(ICommand), typeof(UserPickerView), null);
        public ICommand RemovedCommand
        {
            get => (ICommand)GetValue(RemovedCommandProperty);
            set => SetValue(RemovedCommandProperty, value);
        }

        public static readonly BindableProperty PickedUserProperty = BindableProperty.Create(nameof(PickedUser), typeof(UserDto), typeof(UserPickerView), null, propertyChanged: OnPickedUser);
        public UserDto PickedUser
        {
            get => (UserDto)GetValue(PickedUserProperty);
            set { SetValue(PickedUserProperty, value); }
        }

        #endregion

        public UserPickerView()
        {
            InitializeComponent();
        }

        static void OnPickedUser(BindableObject bindable, object oldValue, object newValue)
        {
            ((UserPickerView)bindable).FillFrame(newValue as UserDto);
        }

        // Tapped on the empty frame
        private void EmptyFrameTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (TappedCommand?.CanExecute(null) ?? true)
            {
                TappedCommand?.Execute(null);
            }
        }

        // Tapped on the remove button
        private void Remove_Tapped(object sender, EventArgs e)
        {
            if (RemovedCommand?.CanExecute(null) ?? true)
            {
                EmptyFrame();
                RemovedCommand?.Execute(null);
            }
        }

        private void FillFrame(UserDto user)
        {
            if (user is null)
                return;

            bt_remove.IsVisible = true;
            frame_userPickerFilled.IsVisible = true;
            lb_username.Text = user.DisplayName;
            img_user.Source = user.ProfilePicUrl;
        }

        private void EmptyFrame()
        {
            PickedUser = null;
            bt_remove.IsVisible = false;
            frame_userPickerFilled.IsVisible = false;
            lb_username.Text = string.Empty;
            img_user.Source = string.Empty;
        }

    }
}