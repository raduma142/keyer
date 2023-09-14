using Keyer.Infrastructure.Commands;
using Keyer.Services;
using Keyer.ViewModels.Base;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Keyer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Properties
        //Заголовок окна
        private string _Title = "Keyer";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        //Путь к исходному изображению
        private string _ImagePath = null;
        public string ImagePath
        {
            get => _ImagePath;
            set
            {
                if (value != null) ImageName = Path.GetFileName(value);
                Set(ref _ImagePath, value);
            }
        }
        //Название изображения
        private string _ImageName = null;
        public string ImageName
        {
            get => _ImageName;
            set
            {
                if (value != null) Title = value + " - Keyer";
                Set(ref _ImageName, value);
            }
        }
        //Изображение
        private BitmapImage _Image = null;
        public BitmapImage Image
        {
            get => _Image;
            set => Set(ref _Image, value);
        }
        //Путь сохранения изображения
        private string _SavePath = null;
        public string SavePath
        {
            get => _SavePath;
            set => Set(ref _SavePath, value);
        }
        //Цвет кеинга
        private Brush _KeyingBrush = Brushes.White;
        public Brush KeyingBrush
        {
            get => _KeyingBrush;
            set => Set(ref _KeyingBrush, value);
        }
        //Чувствительность кеинга
        private int _KeyingSensitivity = 30;
        public int KeyingSensitivity
        {
            get => _KeyingSensitivity;
            set => Set(ref _KeyingSensitivity, value);
        }
        #endregion

        #region Commands
        //Команда открытия картинки
        public ICommand OpenImageCommand { get; }
        private void OnOpenImageCommandExecuted(object p)
        {
            ImagePath = DialogService.OpenImageDialog();

            if (ImagePath == null) return;

            Image = new BitmapImage(new Uri(ImagePath));

            KeyingBrush = KeyingService.LoadImage(Image);
        }
        //Команда выбора цвета
        public ICommand SelectColorCommand { get; }
        private void OnSelectColorCommandExecuted(object p)
        {
            KeyingBrush = DialogService.SelectColorDualog(KeyingBrush);
        }
        //Команда нажатия на картинку
        public ICommand ClickImageCommand { get; }
        public void OnClickImageCommandExecuted(object p)
        {
            //Координаты курсора в окне
            Point Point = Mouse.GetPosition(null);
            double x = Point.X;
            double y = Point.Y;
            //Размер области окна без панели кнопок
            double win_w = App.Current.Windows[0].ActualWidth;
            double win_h = App.Current.Windows[0].ActualHeight - 30;
            //Размер исходного изображения
            double img_w = Image.Width;
            double img_h = Image.Height;
            //Расчёт размеров адаптированного изображения для двух случаев
            double act_w = win_h / img_h * img_w;
            double act_h = win_w / img_w * img_h;
            //Расчёт точного размера изображения в окне
            if (act_w > win_w)
            {
                act_w = act_h / img_h * img_w;
            }
            else
            {
                act_h = act_w / img_w * img_h;
            }
            //Положение изображения с учётом его центрирования
            double img_x = (win_w - act_w) / 2;
            double img_y = (win_h - act_h) / 2;
            //Вычисление координат курсора относительно изображения
            x -= img_x;
            y -= img_y;
            //Пересчёт координат курсора в точку на исходном изображении
            x = x / act_w * img_w;
            y = y / act_h * img_h;
            //Исключение выхода за пределы
            if (x < 0) x = 0;
            if (x > img_w) x = img_w - 1;
            if (y < 0) y = 0;
            if (y > img_h) y = img_h - 1;
            //Получение цвета пикселя
            KeyingBrush = KeyingService.GetPixel((int) x, (int) y);
        }
        //Команда обработки изображений
        public ICommand ProcessImageCommand { get; }
        private bool CanProcessImageCommandExecute(object p)
        {
            return (ImagePath != null);
        }
        private void OnProcessImageCommandExecuted(object p)
        {
            Image = KeyingService.ProcessImage(KeyingBrush, KeyingSensitivity);
        }
        //Команда отмены обработки
        public ICommand UndoProcessImageCommand { get; }
        private bool CanUndoProcessImageCommandExecute(object p)
        {
            return (ImagePath != null);
        }
        private void OnUndoProcessImageCommandExecuted(object p)
        {
            Image = KeyingService.UndoProcessImage(Image);
        }
        //Команда сохранения изображения
        public ICommand SaveImageCommand { get; }
        private bool CanSaveImageCommandExecute(object p)
        {
            return (ImagePath != null);
        }
        private void OnSaveImageCommandExecuted(object p)
        {
            if (Image == null)
            {
                MessageBox.Show("Изображение отсутствует", "Ошибка сохранения!");
                return;
            }
            SavePath = DialogService.SaveImageDialog(ImageName);

            if (SavePath == null) return;

            KeyingService.SaveImage(SavePath);
        }
        #endregion

        #region Services
        private DialogService DialogService;
        private KeyingService KeyingService;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            //Присваивание команд
            OpenImageCommand        = new ActionCommand(OnOpenImageCommandExecuted);
            SelectColorCommand      = new ActionCommand(OnSelectColorCommandExecuted);
            ProcessImageCommand     = new ActionCommand(OnProcessImageCommandExecuted, CanProcessImageCommandExecute);
            SaveImageCommand        = new ActionCommand(OnSaveImageCommandExecuted, CanSaveImageCommandExecute);
            UndoProcessImageCommand = new ActionCommand(OnUndoProcessImageCommandExecuted, CanUndoProcessImageCommandExecute);
            ClickImageCommand       = new ActionCommand(OnClickImageCommandExecuted);
            //Инициализация сервисов
            DialogService = new DialogService();
            KeyingService = new KeyingService();
        }
        #endregion
    }
}
