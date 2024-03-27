using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for GoodbyeScreen.xaml
    /// </summary>
    public partial class GoodbyeScreen : UserControl
    {
        private Bitmap _photo;
        public GoodbyeScreen(Bitmap photo)
        {
            InitializeComponent();
            _photo = photo;
            Print.Source = ConvertToBitmapSource(photo);
        }
        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
