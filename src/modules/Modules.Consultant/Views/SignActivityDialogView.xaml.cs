
using SignaturePad.Forms;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Modules.Consultant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignActivityDialogView : Grid
    {
        public SignActivityDialogView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            byte[] signatureBytes;
            // Save the signature before updating
            Stream image = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            // Write the signature to the orderheader.
            using (BinaryReader binaryReader = new BinaryReader(image))
            {
                binaryReader.BaseStream.Position = 0;
                signatureBytes = binaryReader.ReadBytes((int)image.Length);
            }
            MessagingCenter.Send<SignActivityDialogView, byte[]>(this, "SignatureBytes", signatureBytes);
        }
    }
}