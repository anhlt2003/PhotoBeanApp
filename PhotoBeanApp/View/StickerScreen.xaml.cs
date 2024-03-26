﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public StickerScreen(Bitmap photo)
        {
            InitializeComponent();
            string currentDirectory = Directory.GetCurrentDirectory();
            string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
            string stickerDirectory = Path.Combine(projectDirectory, $"Helper\\Stickers");
            LoadImagesFromFolder(stickerDirectory);
            Bin.Visibility = Visibility.Hidden;
            Photo.Loaded += Photo_Loaded;
            Photo.Source = ConvertToBitmapSource(photo);

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
        private void Photo_Loaded(object sender, RoutedEventArgs e)
        {
            double imageWidth = Photo.ActualWidth;
            double imageHeight = Photo.ActualHeight;

            canvasSticker.Width = imageWidth;
            canvasSticker.Height = imageHeight;
        }

        private void LoadImagesFromFolder(string folderName)
        {
            string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"Folder '{folderName}' not found.");
                return;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg");

            foreach (string imagePath in imageFiles)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
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

            System.Windows.Controls.Image clickedImage = sender as System.Windows.Controls.Image;

            Sticker sticker = new Sticker();

            sticker.SetImageSource(clickedImage.Source as BitmapImage);

            StickerInfo stickerInfo = new StickerInfo(clickedImage.Source as BitmapImage, new System.Windows.Point(clickedImage.ActualWidth / 2, clickedImage.ActualHeight / 2));
            _stickerList.Add(stickerInfo);

            Canvas.SetLeft(sticker, 0);
            Canvas.SetTop(sticker, 0);

            sticker.StickerInfo = stickerInfo;

            sticker.MouseLeftButtonDown += Sticker_MouseLeftButtonDown;
            sticker.MouseLeftButtonUp += Sticker_MouseLeftButtonUp;
            sticker.MouseMove += Sticker_MouseMove;

            canvasSticker.Children.Add(sticker);

        }
        private void Sticker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Bin.Visibility = Visibility.Hidden;
        }
        private void Sticker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Sticker clickedSticker)
            {
                Bin.Visibility = Visibility.Visible;

                if (canvasSticker.Children.Contains(clickedSticker))
                {
                    
                    canvasSticker.Children.Remove(clickedSticker);
                    canvasSticker.Children.Add(clickedSticker);
                    _stickerList.Remove(clickedSticker.StickerInfo);
                    _stickerList.Add(clickedSticker.StickerInfo);
                }
            }
        }

        private void Sticker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Bin.Visibility = Visibility.Visible;
            }
            else
            {
                Bin.Visibility = Visibility.Hidden;

                System.Windows.Point dropPosition = e.GetPosition(canvasSticker);

                System.Windows.Point binPosition = Bin.TranslatePoint(new System.Windows.Point(0, 0), canvasSticker);

                double binX = binPosition.X;
                double binY = binPosition.Y;
                double binWidth = Bin.ActualWidth;
                double binHeight = Bin.ActualHeight;

                if (e.OriginalSource is System.Windows.Controls.Image draggedImage)
                {

                    FrameworkElement parentElement = draggedImage;
                    while (parentElement.Parent != null && !(parentElement.Parent is Sticker))
                    {
                        parentElement = parentElement.Parent as FrameworkElement;
                    }

                    if (parentElement.Parent is Sticker draggedSticker)
                    {
                        if (canvasSticker.Children.Contains(draggedSticker))
                        {
                            //check if drop position is inside image bin
                            if (dropPosition.X >= binX && dropPosition.X <= binX + binWidth &&
                                dropPosition.Y >= binY && dropPosition.Y <= binY + binHeight)
                            {
                                canvasSticker.Children.Remove(draggedSticker);
                                _stickerList.Remove(draggedSticker.StickerInfo);
                            }
                        }
                    }
                }
            }
        }


        private void canvas_DragOver(object sender, DragEventArgs e)
        {

            System.Windows.Point dropPosition = e.GetPosition(canvasSticker);

            double canvasWidth = canvasSticker.Width;
            double canvasHeight = canvasSticker.Height;

            System.Windows.Point binPosition = Bin.TranslatePoint(new System.Windows.Point(0, 0), canvasSticker);

            double binX = binPosition.X;
            double binY = binPosition.Y;
            double binWidth = Bin.ActualWidth;
            double binHeight = Bin.ActualHeight;

            if (e.OriginalSource is System.Windows.Controls.Image draggedImage)
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

                        draggedSticker.StickerInfo.Position = new System.Windows.Point(dropPosition.X - x, dropPosition.Y - y);

                        Canvas.SetLeft(draggedSticker, dropPosition.X - x);
                        Canvas.SetTop(draggedSticker, dropPosition.Y - y);

                        double curX = dropPosition.X - x;
                        double curY = dropPosition.Y - y;

                        if (dropPosition.X < x)
                        {
                            Canvas.SetLeft(draggedSticker, 0);
                            curX = 0;
                        }
                        if (dropPosition.Y < y)
                        {
                            Canvas.SetTop(draggedSticker, 0);
                            curY = 0;
                        }
                        if (dropPosition.X > canvasWidth - x)
                        {
                            Canvas.SetLeft(draggedSticker, canvasWidth - 2 * x);
                            curX = canvasWidth - 2 * x;
                        }
                        if (dropPosition.Y > canvasHeight - y)
                        {
                            Canvas.SetTop(draggedSticker, canvasHeight - 2 * y);
                            curY = canvasHeight - 2 * y;
                        }
                        draggedSticker.StickerInfo.Position = new System.Windows.Point(curX, curY);

                        //check if drop position is inside image bin
                        if (dropPosition.X >= binX && dropPosition.X <= binX + binWidth &&
                            dropPosition.Y >= binY && dropPosition.Y <= binY + binHeight)
                        {
                            draggedSticker.stickerImage.Opacity = 0.6;
                        }
                        else
                        {
                            draggedSticker.stickerImage.Opacity = 1.0;
                        }
                    }
                }
            }

        }

        private void Sticker_StickerRemoved(object sender, StickerEventArgs e)
        {
            _stickerList.Remove(e.RemovedStickerInfo);
        }

    }
}