using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sogetrel.Sinapse.Framework.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Organization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateOrganizationView : ContentPage
    {
        public CreateOrganizationView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MediaFile photo;

            if (!CrossMedia.Current.IsPickPhotoSupported)
                throw new BusinessException("Votre téléphone ne supporte pas l'upload de photos, ou vous avez refusé que nous accédions à votre gallerie.");

            photo = await CrossMedia.Current.PickPhotoAsync();
            if (photo is null)
                return;

            img_pick.Source = photo.Path;
        }
    }
}