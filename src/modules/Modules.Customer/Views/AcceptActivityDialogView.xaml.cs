using SignaturePad.Forms;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Customer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptActivityDialogView : Grid
    {
        public AcceptActivityDialogView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                byte[] signatureBytes;
                // Save the signature before updating
                Stream image = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
                if (image is null)
                    return;

                // Write the signature to the orderheader.
                using (BinaryReader binaryReader = new BinaryReader(image))
                {
                    binaryReader.BaseStream.Position = 0;
                    signatureBytes = binaryReader.ReadBytes((int)image.Length);
                }
                MessagingCenter.Send<AcceptActivityDialogView, byte[]>(this, "SignatureBytes", signatureBytes);
            }
            catch
            {
                return;
            }
        }
    }
}