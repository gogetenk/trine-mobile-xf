using AutoMapper;
using Prism.AppModel;
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
using Trine.Mobile.Model;

namespace Trine.Mobile.Components.ViewModels
{
    /// <summary>
    /// Base class exposing navigation and analytics methods for PRISM ViewModels.
    /// </summary>
    public abstract class ViewModelBase : BindableBase, IInitializeAsync, INavigatedAware, IAutoInitialize, IConfirmNavigationAsync
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

        public virtual async void LogTechnicalError(Exception exc)
        {
            try
            {
                var userRole = Enum.GetName(typeof(UserModel.GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole);
                var logParams = new Dictionary<string, string>();
                logParams.Add("UserId", AppSettings.CurrentUser?.Id);
                logParams.Add("UserName", AppSettings.CurrentUser?.Firstname + " " + AppSettings.CurrentUser?.Lastname);
                logParams.Add("UserType", userRole);
                Logger.Report(exc, logParams);
                await DialogService.DisplayAlertAsync(ErrorMessages.error, ErrorMessages.RandomNetworkErrorMessage, "Ok");
            }
            catch
            {
                Logger.Report(exc, null);
                await DialogService.DisplayAlertAsync(ErrorMessages.error, ErrorMessages.RandomNetworkErrorMessage, "Ok");
            }
        }

        public virtual async Task LogAndShowBusinessError(BusinessException bExc)
        {
            try
            {

                var userRole = Enum.GetName(typeof(UserModel.GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole);
                var logParams = new Dictionary<string, string>();
                logParams.Add("UserId", AppSettings.CurrentUser?.Id);
                logParams.Add("UserName", AppSettings.CurrentUser?.Firstname + " " + AppSettings.CurrentUser?.Lastname);
                logParams.Add("UserType", userRole);
                Logger.Log(bExc.Message, logParams);
                await DialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
            }
            catch
            {
                Logger.Log(bExc.Message, null);
                await DialogService.DisplayAlertAsync(ErrorMessages.error, bExc.Message, "Ok");
            }
        }

        /// <summary>
        /// This method is called before you navigated onto a page. This is a replacement for OnNavigatingTo from Prism 7.2.
        /// </summary>
        /// <param name="parameters">Optional parameters</param>
        /// <returns></returns>
        public virtual async Task InitializeAsync(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// This method is called when you navigated onto a page. This should be the most common overriden method.
        /// </summary>
        /// <param name="parameters">Optional parameters</param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                var userRole = Enum.GetName(typeof(UserModel.GlobalRoleEnum), AppSettings.CurrentUser?.GlobalRole);
                var logParams = new Dictionary<string, string>();
                logParams.Add("UserId", AppSettings.CurrentUser?.Id);
                logParams.Add("UserName", AppSettings.CurrentUser?.Firstname + " " + AppSettings.CurrentUser?.Lastname);
                logParams.Add("UserType", userRole);
                Logger.TrackEvent($"[{GetType().Name}] Navigated To", logParams);
            }
            catch
            {
                Logger.TrackEvent($"[{GetType().Name}] Navigated To", null);
            }
        }

        public virtual async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            return true;
        }
    }
}
