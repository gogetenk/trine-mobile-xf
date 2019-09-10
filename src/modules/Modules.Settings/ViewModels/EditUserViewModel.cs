using AutoMapper;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Components.ViewModels;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Essentials;

namespace Modules.Settings.ViewModels
{
    public class EditUserViewModel : ViewModelBase
    {

        #region Bindings 

        public UserDto User { get => _user; set { _user = value; RaisePropertyChanged(); } }
        private UserDto _user;

        public bool IsLoading { get => _isLoading; set { _isLoading = value; RaisePropertyChanged(); SaveCommand.RaiseCanExecuteChanged(); DeleteCommand.RaiseCanExecuteChanged(); TakePictureCommand.RaiseCanExecuteChanged(); } }
        private bool _isLoading;

        private Stream _profilePicture;
        private readonly IUserService _userService;

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand TakePictureCommand { get; set; }

        #endregion

        public EditUserViewModel(INavigationService navigationService, IMapper mapper, ILogger logger, IPageDialogService dialogService, IUserService userService) : base(navigationService, mapper, logger, dialogService)
        {
            SaveCommand = new DelegateCommand(async () => await OnSave(), () => !IsLoading);
            TakePictureCommand = new DelegateCommand(async () => await OnTakePicture());
            DeleteCommand = new DelegateCommand(async () => await OnDelete(), () => !IsLoading);

            _userService = userService;
        }

        private async Task OnTakePicture()
        {
            try
            {
                MediaFile photo;

                if (!CrossMedia.Current.IsPickPhotoSupported)
                    throw new BusinessException("Votre téléphone ne supporte pas l'upload de photos, ou vous avez refusé que nous accédions à votre gallerie.");

                photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    CompressionQuality = 70,
                    MaxWidthHeight = 512,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });

                if (photo is null)
                    return;

                User.ProfilePicUrl = photo.Path;
                var user = User;
                User = user;
                _profilePicture = photo.GetStream();
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

        private async Task OnSave()
        {
            try
            {
                if (IsLoading)
                    return;

                IsLoading = true;

                if (_profilePicture != null)
                {
                    var profilePicUri = await Task.Run(async () => await _userService.UploadProfilePicture(_profilePicture));
                    User.ProfilePicUrl = profilePicUri;
                }

                var user = Mapper.Map<UserDto>(await _userService.UpdateUser(Mapper.Map<UserModel>(User)));
                if (user is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour de vos données.");

                User = user;
                AppSettings.CurrentUser = Mapper.Map<UserModel>(user);
                await UpdateCacheAsync();
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

        private static async Task UpdateCacheAsync()
        {
            try
            {
                SecureStorage.Remove(CacheKeys._CurrentUser);
                await SecureStorage.SetAsync(CacheKeys._CurrentUser, JsonConvert.SerializeObject(AppSettings.CurrentUser));
            }
            catch
            {
                return;
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
