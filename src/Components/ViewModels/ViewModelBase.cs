﻿using AutoMapper;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Bll.Impl.Settings;

namespace Trine.Mobile.Components.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IInitializeAsync, INavigatedAware
    {
        public INavigationService NavigationService { get; }
        public IMapper Mapper { get; }
        public ILogger Logger { get; }
        public IPageDialogService DialogService { get; }

        public ViewModelBase(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            Mapper = mapper;
            Logger = logger;
            DialogService = dialogService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void LogTechnicalError(Exception exc)
        {
            Logger.Report(exc, null);
        }

        public virtual async Task LogAndShowBusinessError(BusinessException bExc)
        {
            Logger.Log(bExc.Message);
            await DialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
        }

        public virtual async Task InitializeAsync(INavigationParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("UserId", AppSettings.CurrentUser?.Id);
            Logger.TrackEvent("[" + base.GetType().Name + "] Navigated To", dictionary);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
