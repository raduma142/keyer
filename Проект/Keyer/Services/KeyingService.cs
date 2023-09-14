/* Сервис обработки изображения */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Keyer.Services
{
    internal class KeyingService
    {
        //Свойства
        private Bitmap Image;

        private Stack<Bitmap> Bitmaps;

        private System.Drawing.Color KeyingColor;

        //Конструктор
        public KeyingService()
        {
            Bitmaps = new Stack<Bitmap>();
        }
        //Загрузить изображение для дальнейщей обработки
        public System.Windows.Media.Brush LoadImage(BitmapImage BitmapImage)
        {
            Image = ConvertService.BitmapImage2Bitmap(BitmapImage);
            Bitmaps.Clear();
            return FindKeyingColor();
        }
        //Получить цвет пикселя
        public System.Windows.Media.Brush GetPixel(int x, int y)
        {
            System.Drawing.Color Color = Image.GetPixel(x, y);
            return new SolidColorBrush(ConvertService.DrawingColor2MediaColor(Color));
        }
        //Определить KeyingColor
        private System.Windows.Media.Brush FindKeyingColor()
        {
            System.Drawing.Color Color = Image.GetPixel(0, 0);
            byte R = Color.R;
            byte G = Color.G;
            byte B = Color.B;

            for (int x = 0; x < Image.Width; x += 10)
            {
                for (int y = 0; y < Image.Height; y += 10)
                {
                    Color = Image.GetPixel(x, y);
                    R = (byte) ((R + Color.R) / 2);
                    G = (byte) ((G + Color.G) / 2);
                    B = (byte) ((B + Color.B) / 2);
                }
            }

            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        }
        //Сравнение цветов
        private int CompareColors(byte R, byte G, byte B, byte R2, byte G2, byte B2)
        {
            return Math.Abs(R - R2) + Math.Abs(G - G2) + Math.Abs(B - B2);
        }
        //Обработать изображение
        public BitmapImage ProcessImage(System.Windows.Media.Brush KeyingBrush, int KeyingSensitivity)
        {
            Bitmaps.Push(new Bitmap(Image));

            SolidColorBrush SolidKeyingBrush = (SolidColorBrush)KeyingBrush;
            KeyingColor = ConvertService.MediaColor2DrawingColor(SolidKeyingBrush.Color);
            //byte KeyR = KeyingColor.R;
            //byte KeyG = KeyingColor.G;
            //byte KeyB = KeyingColor.B;

            System.Drawing.Color AlfaColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);

            //int w = Image.Width;
            //int h = Image.Height;
            //for (int x = 0; x < w; x++)
            //{
            //    for (int y = 0; y < h; y++)
            //    {
            //        System.Drawing.Color C = Image.GetPixel(x, y);
            //        if (CompareColors(KeyR, KeyG, KeyB, C.R, C.G, C.B) < 30)
            //        {
            //            Image.SetPixel(x, y, AlfaColor);
            //        }
            //    }
            //}

            LockProcessUnlockBits(Image, KeyingSensitivity);

            Image.MakeTransparent(AlfaColor);

            return ConvertService.Bitmap2BitmapImage(Image);
        }
        //Обработчик блока данных изображения
        //Идёт обращение к памяти напрямую для более быстрой обработки
        private void LockProcessUnlockBits(Bitmap bitmap, int sensitivity)
        {
            byte KeyR = KeyingColor.R;
            byte KeyG = KeyingColor.G;
            byte KeyB = KeyingColor.B;

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int counter = 2; counter < rgbValues.Length; counter += 4)
            {
                if (CompareColors(rgbValues[counter], rgbValues[counter - 1], rgbValues[counter - 2], KeyR, KeyG, KeyB) < sensitivity)
                {
                    rgbValues[counter] = 255;
                    rgbValues[counter - 1] = 255;
                    rgbValues[counter - 2] = 255;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bitmap.UnlockBits(bmpData);
        }
        //Отменить обработку
        public BitmapImage UndoProcessImage(BitmapImage BitmapImage)
        {
            if (Bitmaps.Count > 0)
            {
                Image = Bitmaps.Pop();
                return ConvertService.Bitmap2BitmapImage(Image);
            }
            else
            {
                return BitmapImage;
            }
            
        }
        //Сохранить изображение в файл
        public void SaveImage(string path)
        {
            Image.Save(path);
        }
    }
}
