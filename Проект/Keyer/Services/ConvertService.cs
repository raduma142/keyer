//Сервис конвертирования типов
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

namespace Keyer.Services
{
    internal static class ConvertService
    {
        //Ковертировать BitmapImage в Bitmap
        public static Bitmap BitmapImage2Bitmap(BitmapImage BitmapImage)
        {
            Bitmap Bitmap;
            using (MemoryStream MemoryStream = new MemoryStream())
            {
                BitmapEncoder BitmapEncoder = new BmpBitmapEncoder();
                BitmapEncoder.Frames.Add(BitmapFrame.Create(BitmapImage));
                BitmapEncoder.Save(MemoryStream);
                Bitmap = new Bitmap(MemoryStream);
            }
            return Bitmap;
        }
        //Конвертировать Bitmap в BitmapImage
        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            BitmapImage BitmapImage = new BitmapImage();
            using (MemoryStream MemoryStream = new MemoryStream())
            {
                Bitmap Bitmap = new Bitmap(bitmap);
                Bitmap.Save(MemoryStream, ImageFormat.Png);
                MemoryStream.Position = 0;
                BitmapImage.BeginInit();
                BitmapImage.StreamSource = MemoryStream;
                BitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                BitmapImage.EndInit();
            }
            return BitmapImage;
        }
        //Конвертировать Drawing.Color в Media.Color
        public static System.Windows.Media.Color DrawingColor2MediaColor(System.Drawing.Color DrawingColor)
        {
            return System.Windows.Media.Color.FromArgb(DrawingColor.A, DrawingColor.R, DrawingColor.G, DrawingColor.B);
        }
        //Конвертировать Media.Color в Drawing.Color
        public static System.Drawing.Color MediaColor2DrawingColor(System.Windows.Media.Color MediaColor)
        {
            return System.Drawing.Color.FromArgb(MediaColor.A, MediaColor.R, MediaColor.G, MediaColor.B);
        }
    }
}
