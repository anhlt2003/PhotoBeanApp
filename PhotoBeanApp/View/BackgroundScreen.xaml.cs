using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestImage.Frame;
using TestImage.Render;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for BackgroundScreen.xaml
    /// </summary>
    public partial class BackgroundScreen : UserControl
    {
        public event EventHandler ButtonContinueClick;
        private Bitmap photo;
        private string codeFrameType;
        public Bitmap imgTemp;
        public Frames frameList;
        public BackgroundScreen(Bitmap photo, string codeFrameType, Frames frameList)
        {
            InitializeComponent();
            this.photo = photo;
            this.codeFrameType = codeFrameType;
            this.frameList = frameList; 
            LoadBackgrounds();
        }

        private void LoadBackgrounds()
        {
            string backgroundsDirectory = $"C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames" +
                $"\\{codeFrameType}";

            string[] backgroundFiles = Directory.GetFiles(backgroundsDirectory, $"*.png" +
                $"");
        
            foreach (string file in backgroundFiles)
            {
                System.Windows.Controls.Image background = new System.Windows.Controls.Image();
                background.Source = new BitmapImage(new Uri(file));
                double itemWidth = background.Width / 2;
                background.Width = itemWidth;
                background.Height = 60;
                background.Stretch = Stretch.Uniform;
                background.MouseLeftButtonDown += Background_MouseLeftButtonDown;
                Backgrounds.Children.Add(background);
            }
            imgTemp = RenderManager.FrameImage(frameList.GetType(codeFrameType), photo, "default.png");
            Print.Source = ConvertToBitmapSource(imgTemp);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonContinueClick?.Invoke(this, EventArgs.Empty);
        }

        private void Background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedBackground = sender as System.Windows.Controls.Image;
            string fileName = System.IO.Path.GetFileName(clickedBackground.Source.ToString());
            imgTemp = RenderManager.FrameImage(frameList.GetType(codeFrameType), photo, fileName);
            Print.Source = ConvertToBitmapSource(imgTemp);
        }

        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory,ImageFormat.Bmp);
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
