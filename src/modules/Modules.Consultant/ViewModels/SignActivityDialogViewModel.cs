﻿using AutoMapper;
using Modules.Consultant.Views;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.Navigation;
using Trine.Mobile.Components.ViewModels;
using Xamarin.Forms;

namespace Modules.Consultant.ViewModels
{
    public class SignActivityDialogViewModel : ViewModelBase, IDialogAware
    {
        //public ICommand SignCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public event Action<IDialogParameters> RequestClose;

        public SignActivityDialogViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService) : base(navigationService, mapper, logger, dialogService)
        {
            //SignCommand = new DelegateCommand(() => OnSignActivity());
            CancelCommand = new DelegateCommand(() => RequestClose.Invoke(null));

            MessagingCenter.Subscribe<SignActivityDialogView, byte[]>(subscriber: this, message: "SignatureBytes", callback: (sender, arg) =>
           {
               try
               {
                   var dialogParams = new DialogParameters();
                   dialogParams.Add(NavigationParameterKeys._IsActivitySigned, true);
                   dialogParams.Add(NavigationParameterKeys._Bytes, arg);
                   RequestClose.Invoke(dialogParams);
               }
               catch (Exception exc)
               {
                   logger.Report(exc, new Dictionary<string, string> {
                    { "UserId", AppSettings.CurrentUser.Id },
                    { "UserName", $"{AppSettings.CurrentUser?.Firstname} {AppSettings.CurrentUser?.Lastname}"} }); ;
               }
           });
        }

        //private void OnSignActivity()
        //{

        //}

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
