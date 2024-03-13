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
        private System.Drawing.Bitmap photo;
        private string codeFrameType;
        private Bitmap imgTemp;
        public BackgroundScreen(System.Drawing.Bitmap photo, String codeFrameType)
        {
            InitializeComponent();
            this.photo = photo;
            this.codeFrameType = codeFrameType;
            LoadBackgrounds();
        }

        private void LoadBackgrounds()
        {
            string backgroundsDirectory = "C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\type1";

            string[] backgroundFiles = Directory.GetFiles(backgroundsDirectory, $"background*.png" +
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
            Print.Source = ConvertToBitmapSource(RenderManager.FrameImage(Frames.Instance.GetType(codeFrameType), photo, "background_2_1.png"));
            imgTemp = RenderManager.FrameImage(Frames.Instance.GetType(codeFrameType), photo, "background_2_1.png");
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            imgTemp.Save("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Images\\img.png");
            ButtonContinueClick?.Invoke(this, EventArgs.Empty);
        }

        private void Background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedBackground = sender as System.Windows.Controls.Image;
            string fileName = System.IO.Path.GetFileName(clickedBackground.Source.ToString());
            Print.Source = ConvertToBitmapSource(RenderManager.FrameImage(Frames.Instance.GetType(codeFrameType), photo, fileName));
            imgTemp = RenderManager.FrameImage(Frames.Instance.GetType(codeFrameType), photo, fileName);
        }

        private BitmapSource ConvertToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
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
