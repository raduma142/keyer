/* Сервис Диалоговых окон */
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media;
using System.Security.Policy;

namespace Keyer.Services
{
    internal class DialogService
    {
        //Диалог Открыть изображение
        public string OpenImageDialog()
        {
            OpenFileDialog Dialog = new OpenFileDialog()
            {
                Title = "Открыть изображение",
                Filter = "PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|Jpeg (*.jpg, *.jpeg)|*.JPG;*.JPEG|All Picture Files|*.BMP;*.JPG;*.JPEG;*.PNG|All files|*.*"
            };

            if (Dialog.ShowDialog() == DialogResult.Cancel) return null;

            return Dialog.FileName;
        }

        //Диалог Сохранить изображение
        public string SaveImageDialog(string name)
        {
            SaveFileDialog Dialog = new SaveFileDialog()
            {
                Title = "Сохранить изображение",
                FileName = name,
                Filter = "PNG (*.png)|*.png"
            };

            if (Dialog.ShowDialog() == DialogResult.Cancel) return null;
            
            return Dialog.FileName;
        }

        //Диалог Выбрать цвет
        public System.Windows.Media.Brush SelectColorDualog(System.Windows.Media.Brush brush)
        {
            ColorDialog Dialog = new ColorDialog()
            {
                FullOpen = true
            };
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Color DrawingColor = Dialog.Color;
                System.Windows.Media.Color MediaColor = System.Windows.Media.Color.FromArgb(DrawingColor.A, DrawingColor.R, DrawingColor.G, DrawingColor.B);
                return new SolidColorBrush(MediaColor);
            }
            else
            {
                return brush;
            }
        }
    }
}
