using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using System.IO;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for TakePhotoScreen.xaml
    /// </summary>
    public partial class TakePhotoScreen : UserControl
    {
        private DispatcherTimer countdownTimer;
        private int remainingTimeInSeconds = 3;
        private int imageIndex = 1;
        private bool isTimerRunning = false;
        private int numberOfCut, numberOfPrint;
        public List<System.Windows.Controls.Image> imageList = new List<System.Windows.Controls.Image>();
        public event EventHandler ContinueButtonClick;
        private VideoCaptureDevice _videoSource;

        public TakePhotoScreen(int numberOfCut, int numberOfPrint)
        {
            InitializeComponent();
            InitializeTimer();
            this.numberOfCut = numberOfCut;
            this.numberOfPrint = numberOfPrint;
            StartButton.Visibility = Visibility.Collapsed;
            StartCamera();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (sender, e) =>
            {
                timer.Stop(); 
                StartButton.Visibility = Visibility.Visible;
            };
            timer.Start();
        }

        private void InitializeTimer()
        {
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            ContinueButton.Visibility = Visibility.Collapsed;
            ReplayButton.Visibility = Visibility.Collapsed;
            countdownLabel.Visibility = Visibility.Collapsed;
        }

        private void StartTimer()
        {
            StartCamera();
            remainingTimeInSeconds = 3;
            countdownLabel.Visibility = Visibility.Visible;
            countdownLabel.Content = remainingTimeInSeconds.ToString();
            countdownTimer.Start();
            isTimerRunning = true;
        }
        private void StopTimer()
        {
            countdownTimer.Stop();
            isTimerRunning = false;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            remainingTimeInSeconds--;

            if (remainingTimeInSeconds <= 0)
            {
                StopTimer();
                imageControl.Source = Capture();
                if((numberOfCut == 1 && imageIndex == numberOfCut + 1) || imageIndex == numberOfCut + 2)
                {
                    ContinueButton.Content = "Hoàn thành";
                }
                ContinueButton.Visibility = Visibility.Visible;
                ReplayButton.Visibility= Visibility.Visible;
                countdownLabel.Visibility = Visibility.Collapsed;
                return;
            }
            countdownLabel.Content = remainingTimeInSeconds.ToString();
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isTimerRunning)
            {
                if(numberOfCut == 1)
                {
                    countPhotoLable.Content = $"{imageIndex}/{numberOfCut + 1}";
                }
                else
                {
                    countPhotoLable.Content = $"{imageIndex}/{numberOfCut + 2}";
                }
                StartButton.Visibility = Visibility.Collapsed;
                StartTimer();
            }
        }


        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isTimerRunning)
            {
                ContinueButton.Visibility = Visibility.Collapsed;
                ReplayButton.Visibility = Visibility.Collapsed;
                StartTimer();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = imageControl.Source; 
                Button btn = (Button)sender;
                if (btn.Name.Equals("ContinueButton"))
                {
                    imageList.Add(image);
                    imageIndex++;
                    if(numberOfCut == 1)
                    {
                        countPhotoLable.Content = $"{imageIndex}/{numberOfCut + 1}";
                        if (imageIndex > numberOfCut + 1)
                        {
                            ContinueButtonClick?.Invoke(this, EventArgs.Empty);

                        }
                    }
                    else
                    {
                        countPhotoLable.Content = $"{imageIndex}/{numberOfCut + 2}";
                    if (imageIndex > numberOfCut + 2)
                    {
                        ContinueButtonClick?.Invoke(this, EventArgs.Empty);
                    }
                    }

                }
            }
        }

        private void StartCamera()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }
            FilterInfo videoDevice = videoDevices[0];
            _videoSource = new VideoCaptureDevice(videoDevice.MonikerString);
            _videoSource.NewFrame += Video_NewFrame;
            _videoSource.Start();
        }



        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
            {
                Dispatcher.Invoke(() =>
                {
                    imageControl.Source = BitmapToImageSource(bitmap);
                });
            }
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
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

        private BitmapSource Capture()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= Video_NewFrame;
            }

            BitmapSource bitmapSource = imageControl.Source as BitmapSource;
            return bitmapSource;
        }
    }
}
