namespace Bars.Gkh.Utils
{
    using System.Drawing;
    using System.IO;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    public static class ImageResizeExtension
    {
        public static void SaveTemporary(Bitmap bmp, MemoryStream ms, int quality, string format)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var codec = GetImageCodecInfo(format);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            if (codec != null)
                bmp.Save(ms, codec, encoderParams);
            else
                bmp.Save(ms, GetImageFormat(format));
        }

        public static Bitmap ScaleImage(Bitmap image, double scale)
        {
            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            var result = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        public static ImageCodecInfo GetImageCodecInfo(string format)
        {
            switch (format)
            {
                case "bmp": return ImageCodecInfo.GetImageEncoders()[0];
                case "jpg":
                case "jpeg": return ImageCodecInfo.GetImageEncoders()[1];
                case "gif": return ImageCodecInfo.GetImageEncoders()[2];
                case "tiff": return ImageCodecInfo.GetImageEncoders()[3];
                case "png": return ImageCodecInfo.GetImageEncoders()[4];
                default: return null;
            }
        }

        public static ImageFormat GetImageFormat(string format)
        {
            switch (format)
            {
                case ".jpg": return ImageFormat.Jpeg;
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".png": return ImageFormat.Png;
                case ".tiff": return ImageFormat.Tiff;
                default: return ImageFormat.Png;
            }
        }
    }
}