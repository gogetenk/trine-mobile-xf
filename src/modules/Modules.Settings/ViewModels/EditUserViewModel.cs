using AutoMapper;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;

namespace Modules.Settings.ViewModels
{
    public class EditUserViewModel : ViewModelBase
    {
        public UserDto User { get => _user; set { _user = value; RaisePropertyChanged(); } }
        private UserDto _user;

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); } }
        private bool _isLoading;

        private readonly IUserService _userService;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand TakePictureCommand { get; set; }

        public EditUserViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IUserService userService) : base(navigationService, mapper, logger, dialogService)
        {
            SaveCommand = new DelegateCommand(async () => await OnSave(), () => !IsLoading);
            TakePictureCommand = new DelegateCommand(async () => await OnTakePicture());
            DeleteCommand = new DelegateCommand(async () => await OnDelete(), () => !IsLoading);

            _userService = userService;
        }

        private async Task OnTakePicture()
        {
            MediaFile photo;

            if (!CrossMedia.Current.IsPickPhotoSupported)
                throw new BusinessException("Votre téléphone ne supporte pas l'upload de photos, ou vous avez refusé que nous accédions à votre gallerie.");

            photo = await CrossMedia.Current.PickPhotoAsync();
            if (photo is null)
                return;

            User.ProfilePicUrl = photo.Path; // TODO: Upload les photos quelque part 
        }

        private async Task OnSave()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                var user = Mapper.Map<ActivityDto>(await _userService.UpdateUser(Mapper.Map<UserModel>(User)));
                if (user is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour de vos données.");

                AppSettings.CurrentUser = Mapper.Map<UserModel>(user);
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnDelete()
        {
            try
            {
                if (IsLoading)
                    return;

                var isConfirmed = await DialogService.DisplayAlertAsync("Êtes-vous sûr ?", "Cette supression est définitive.", "Supprimer", "Annuler");
                if (!isConfirmed)
                    return;

                IsLoading = true;
                await _userService.DeleteUser(User.Id);
            }
            catch (BusinessException bExc)
            {
                await LogAndShowBusinessError(bExc);
            }
            catch (Exception exc)
            {
                LogTechnicalError(exc);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
