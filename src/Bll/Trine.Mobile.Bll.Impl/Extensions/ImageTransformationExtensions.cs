using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;

namespace Trine.Mobile.Bll.Impl.Extensions
{
    public static class ImageTransformationExtensions
    {
        public static Image<TPixel> Resize<TPixel>(this Image<TPixel> image, int maxResolution) where TPixel : struct, IPixel<TPixel>
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Min,
                Size = new Size(maxResolution)
            }));

            return image;
        }

        public static byte[] GetBytes<TPixel>(this Image<TPixel> image, IImageFormat format) where TPixel : struct, IPixel<TPixel>
        {
            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, format);
                result = memoryStream.ToArray();
            }

            return result;
        }

    }
}
