using PhotoBeanApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using TestImage.Frame;

namespace PhotoBeanApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfCut;
        private int numberOfPrint;
        List<Image> imageList;
        Frames frames;
        private DispatcherTimer countdownTimer;
        private int remainingTimeInSeconds = 30;
        private int currentScreenIndex;
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            numberOfCut = 4;
            numberOfPrint = 1;
            currentScreenIndex = 0;
            imageList = new List<Image>();
            frames = Frames.Instance;
            frames.LoadListType();
            frames.LoadTypeImage("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\1a", "1a");
            frames.LoadTypeImage("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\4a", "4a");
            frames.LoadTypeImage("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\6a", "6a");
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.StartButtonClick += WelcomeScreen_StartButtonClick;
            contentControl.Content = welcomeScreen;
        }
        private void InitializeTimer()
        {
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            remainingTimeInSeconds--;

            if (remainingTimeInSeconds <= 0)
            {
                StopTimer();
            }
            totalTime.Content = remainingTimeInSeconds.ToString();
        }

        private void StopTimer()
        {
            countdownTimer.Stop();
            switch (currentScreenIndex)
            {
                case 0:
                    break;
                case 1:
                    ResetApp(); 
                    break;
                case 2:
                    ResetApp();
                    break;
                case 3:
                    if (contentControl.Content is TakePhotoScreen takePhotoScreen)
                    {
                        TakePhotoScreen_ContinueButtonClick(takePhotoScreen, EventArgs.Empty);
                    }
                    break;
                case 4:
                    if (contentControl.Content is ChoosePhotoScreen choosePhotoScreen)
                    {
                        ChoosePhotoScreen_ButtonContinueClick(choosePhotoScreen, EventArgs.Empty);
                    }
                    break;
                case 5:
                    if (contentControl.Content is FrameScreen frameScreen)
                    {
                        FrameScreen_ButtonContinueClick(frameScreen, EventArgs.Empty);
                    }
                    break;
                case 6:
                    if (contentControl.Content is BackgroundScreen backgroundScreen)
                    {
                        BackgroundScreen_ButtonContinueClick(backgroundScreen, EventArgs.Empty);
                    }
                    break;
            }
        }

        private void StartTimer()
        {
            remainingTimeInSeconds = 30;
            totalTime.Content = remainingTimeInSeconds.ToString();
            countdownTimer.Start();
            currentScreenIndex++;
        }
        private void ResetApp()
        {
            numberOfCut = 4;
            numberOfPrint = 1;
            imageList.Clear();
            currentScreenIndex = 0;
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.StartButtonClick += WelcomeScreen_StartButtonClick;
            contentControl.Content = welcomeScreen;
        }
        private void WelcomeScreen_StartButtonClick(object sender, EventArgs e)
        {
            SetUpScreen setUpScreen = new SetUpScreen();
            setUpScreen.ButtonNextClick += SetUpScreen_ButtonNextClick;
            contentControl.Content = setUpScreen;
            setUpScreen.LoadFrames(setUpScreen.numberOfCut);
            StartTimer();
        }
        private void SetUpScreen_ButtonNextClick(object sender, EventArgs e)
        {
            SetUpScreen setUpScreen = (SetUpScreen)sender;
            numberOfCut = setUpScreen.numberOfCut;
            numberOfPrint = setUpScreen.numberOfPrint;
            PaymentScreen paymentScreen = new PaymentScreen(numberOfCut, numberOfPrint);
            paymentScreen.PaymentSuccess += PaymentScreen_PaymentSuccess;
            contentControl.Content = paymentScreen;
            StartTimer();
        }
        private void PaymentScreen_PaymentSuccess(object sender, EventArgs e)
        {
            PaymentScreen paymentScreen = (PaymentScreen)sender;
            TakePhotoScreen takePhotoScreen = new TakePhotoScreen(numberOfCut, numberOfPrint);
            takePhotoScreen.ContinueButtonClick += TakePhotoScreen_ContinueButtonClick;
            contentControl.Content = takePhotoScreen;
            StartTimer();
        }

        private void TakePhotoScreen_ContinueButtonClick(object sender, EventArgs e)
        {
            TakePhotoScreen takePhotoScreen = (TakePhotoScreen)sender;
            List<Image> imageList = takePhotoScreen.imageList;
            ChoosePhotoScreen choosePhotoScreen = new ChoosePhotoScreen(numberOfCut,imageList);
            choosePhotoScreen.ButtonContinueClick += ChoosePhotoScreen_ButtonContinueClick;
            contentControl.Content = choosePhotoScreen;
            StartTimer();
        }

        private void ChoosePhotoScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            ChoosePhotoScreen choosePhotoScreen = (ChoosePhotoScreen)sender;
            imageList = choosePhotoScreen.selectedImages;
            FrameScreen frameScreen = new FrameScreen(imageList);
            frameScreen.ButtonContinueClick += FrameScreen_ButtonContinueClick;
            contentControl.Content = frameScreen;
            StartTimer();
        }

        private void FrameScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            FrameScreen frameScreen = (FrameScreen)sender;
            System.Drawing.Bitmap image = frameScreen.imgTemp;
            string codeFrameType = frameScreen.codeFrameType;
            BackgroundScreen backgroundScreen = new BackgroundScreen(image, codeFrameType);
            backgroundScreen.ButtonContinueClick += BackgroundScreen_ButtonContinueClick;
            contentControl.Content = backgroundScreen;
            StartTimer();
        }

        private void BackgroundScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            BackgroundScreen backgroundScreen = (BackgroundScreen)sender;
            backgroundScreen.imgTemp.Save("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Images\\img.png");
            ResetApp();
        }
    }
}
