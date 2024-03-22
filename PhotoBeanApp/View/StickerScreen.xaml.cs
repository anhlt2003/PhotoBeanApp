using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFStickerDemo;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for StickerScreen.xaml
    /// </summary>
    public partial class StickerScreen : UserControl
    {
        private List<StickerInfo> _stickerList = new List<StickerInfo>();
        public StickerScreen()
        {
            InitializeComponent();
            LoadImagesFromFolder($"D:\\FPT\\2024_Spring_OJT\\WPFStickerDemo\\WPFStickerDemo\\Images");
            canvasSticker.Width = this.Width / 2;
            canvasSticker.Height = this.Height;

        }

        private void LoadImagesFromFolder(string folderName)
        {
            string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"Folder '{folderName}' not found.");
                return;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg"); // You can modify the file extension according to your image types

            foreach (string imagePath in imageFiles)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                Image image = new Image();
                image.Source = bitmapImage;
                image.Width = image.Width / 2;
                image.Height = 60;
                image.Stretch = System.Windows.Media.Stretch.Uniform;
                image.Margin = new Thickness(5);
                image.MouseLeftButtonDown += Image_MouseLeftButtonDown;

                wrapPanel.Children.Add(image);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;

            // Create a new instance of the sticker user control
            Sticker sticker = new Sticker();

            // Set the image source of the sticker to the clicked image source
            sticker.SetImageSource(clickedImage.Source as BitmapImage);

            sticker.StickerRemoved += Sticker_StickerRemoved;

            StickerInfo stickerInfo = new StickerInfo(clickedImage.Source as BitmapImage, new Point(clickedImage.ActualWidth / 2, clickedImage.ActualHeight / 2));
            _stickerList.Add(stickerInfo);

            // Optionally, set initial position of the sticker
            Canvas.SetLeft(sticker, 0);
            Canvas.SetTop(sticker, 0);

            sticker.StickerInfo = stickerInfo;

            // Add the sticker to the canvas
            canvasSticker.Children.Add(sticker);


        }



        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvasSticker);

            double canvasWidth = canvasSticker.Width;
            double canvasHeight = canvasSticker.Height;

            if (e.OriginalSource is Image draggedImage)
            {
                double x = draggedImage.ActualWidth / 2;
                double y = draggedImage.ActualHeight / 2;

                FrameworkElement parentElement = draggedImage;
                while (parentElement.Parent != null && !(parentElement.Parent is Sticker))
                {
                    parentElement = parentElement.Parent as FrameworkElement;
                }

                if (parentElement.Parent is Sticker draggedSticker)
                {
                    if (canvasSticker.Children.Contains(draggedSticker))
                    {
                        // Update sticker info position
                        draggedSticker.StickerInfo.Position = new Point(dropPosition.X - x, dropPosition.Y - y);

                        // Update sticker position on canvas
                        Canvas.SetLeft(draggedSticker, dropPosition.X - x);
                        Canvas.SetTop(draggedSticker, dropPosition.Y - y);

                        double curX = dropPosition.X - x;
                        double curY = dropPosition.Y - y;

                        // Adjust position if the sticker is going out of bounds
                        if (dropPosition.X < x)
                        {
                            Canvas.SetLeft(draggedSticker, 0);
                            curX = x;
                        }
                        if (dropPosition.Y < y)
                        {
                            Canvas.SetTop(draggedSticker, 0);
                            curY = y;
                        }
                        if (dropPosition.X > canvasWidth - x)
                        {
                            Canvas.SetLeft(draggedSticker, canvasWidth - 2 * x);
                            curX = canvasWidth - x;
                        }
                        if (dropPosition.Y > canvasHeight - y)
                        {
                            Canvas.SetTop(draggedSticker, canvasHeight - 2 * y);
                            curY = canvasHeight - y;
                        }
                        draggedSticker.StickerInfo.Position = new Point(curX, curY);
                    }
                }
            }
        }

        private void Sticker_StickerRemoved(object sender, StickerEventArgs e)
        {
            // Remove the corresponding StickerInfo from _stickerList
            _stickerList.Remove(e.RemovedStickerInfo);
        }
    }
}
