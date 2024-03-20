using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TestImage.Frame;
using TestImage.Render;
namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for FrameScreen.xaml
    /// </summary>
    public partial class FrameScreen : UserControl
    {
        public System.Windows.Controls.Image image;
        public event EventHandler ButtonContinueClick;
        public List<System.Windows.Controls.Image> selectedImages;
        public int index;
        List<System.Drawing.Image> images;
        public Bitmap imgTemp;
        public string codeFrameType;
        public Frames frameList;
        public int numberOfCut;
        public FrameScreen(List<System.Windows.Controls.Image> selectedImages, Frames frameList, int numberOfCut)
        {
            InitializeComponent();
            image = new System.Windows.Controls.Image();
            index = 0;
            this.frameList = frameList;
            this.selectedImages = selectedImages;
            this.numberOfCut = numberOfCut;
            codeFrameType = $"{numberOfCut}a";
            images = ConvertToDrawingImages(selectedImages);
            LoadFrames();
        }

        private void LoadFrames()
        {
            string framesDirectory = $"C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\{numberOfCut}cut";

            List<string> defaultFramesList = GetDefaultFramesList(framesDirectory);

            foreach (string frameFile in defaultFramesList)
            {
                System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                frame.Source = new BitmapImage(new Uri(frameFile));
                double itemWidth = frames.Width / 2;
                frame.Width = itemWidth;
                frame.Height = 60;
                frame.Stretch = Stretch.Uniform;
                frame.MouseLeftButtonDown += Frame_MouseLeftButtonDown;
                frames.Children.Add(frame);
            }
            imgTemp = RenderManager.CombineImage(frameList.GetType($"{numberOfCut}a"), images);
            Photo.Source = ConvertToBitmapSource(RenderManager.CombineImage(frameList.GetType($"{numberOfCut}a"), images));
        }

        private List<string> GetDefaultFramesList(string directory)
        {
            List<string> defaultframeLists = new List<string>();

            foreach (string file in Directory.GetFiles(directory, "*.png", SearchOption.AllDirectories))
            {
                defaultframeLists.Add(file);
            }

            return defaultframeLists;
        }
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonContinueClick?.Invoke(this, EventArgs.Empty);
        }

        private void Frame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedFrame = sender as System.Windows.Controls.Image;

            index = frames.Children.IndexOf(clickedFrame);
            if(index == 0)
            {
                codeFrameType = $"{numberOfCut}a";
            }else if(index == 1)
            {
                codeFrameType = $"{numberOfCut}b";
            }else if(index == 2)
            {
                codeFrameType = $"{numberOfCut}c";
            }else if (index==3)
            {
                codeFrameType = $"{numberOfCut}d";
            }
            imgTemp = RenderManager.CombineImage(frameList.GetType(codeFrameType), images);
            Photo.Source = ConvertToBitmapSource(RenderManager.CombineImage(frameList.GetType(codeFrameType), images));
        }
        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
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
        public List<System.Drawing.Image> ConvertToDrawingImages(List<System.Windows.Controls.Image> wpfImages)
        {
            List<System.Drawing.Image> drawingImages = new List<System.Drawing.Image>();

            foreach (System.Windows.Controls.Image wpfImage in wpfImages)
            {
                BitmapSource bitmapSource = (BitmapSource)wpfImage.Source;
                Bitmap bitmap = BitmapFromSource(bitmapSource);
                System.Drawing.Image drawingImage = System.Drawing.Image.FromHbitmap(bitmap.GetHbitmap());

                drawingImages.Add(drawingImage);
            }

            return drawingImages;
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
    }
}

