using System.Collections.Generic;
using AutoMapper;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Navigation;

namespace Trine.Mobile.Components.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        public INavigationService NavigationService { get; }
        public IMapper Mapper { get; }
        public ILogger Logger { get; }

        public ViewModelBase(INavigationService navigationService, IMapper mapper, ILogger logger)
        {
            NavigationService = navigationService;
            Mapper = mapper;
            Logger = logger;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("UserId", null);
            Logger.TrackEvent("[" + base.GetType().Name + "] Navigated To", dictionary);
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }
    }
}
