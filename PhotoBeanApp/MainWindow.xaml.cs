using PhotoBeanApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public MainWindow()
        {
            InitializeComponent();
            numberOfCut = 4;
            numberOfPrint = 1;
            imageList = new List<Image>();
            frames = Frames.Instance;
            frames.LoadListType();
            frames.LoadTypeImage("C:\\Users\\Tuan Anh\\Documents\\Amazing Tech\\PhotoBean\\PhotoBeanApp\\PhotoBeanApp\\PhotoBeanApp\\Frames\\type1", "1a");
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            welcomeScreen.StartButtonClick += WelcomeScreen_StartButtonClick;
            contentControl.Content = welcomeScreen;
        }
        private void ResetApp()
        {
            numberOfCut = 4;
            numberOfPrint = 1;
            imageList.Clear();
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
        }
        private void SetUpScreen_ButtonNextClick(object sender, EventArgs e)
        {
            SetUpScreen setUpScreen = (SetUpScreen)sender;
            numberOfCut = setUpScreen.numberOfCut;
            numberOfPrint = setUpScreen.numberOfPrint;
            PaymentScreen paymentScreen = new PaymentScreen(numberOfCut, numberOfPrint);
            paymentScreen.PaymentSuccess += PaymentScreen_PaymentSuccess;
            contentControl.Content = paymentScreen;
        }
        private void PaymentScreen_PaymentSuccess(object sender, EventArgs e)
        {
            PaymentScreen paymentScreen = (PaymentScreen)sender;
            TakePhotoScreen takePhotoScreen = new TakePhotoScreen(numberOfCut, numberOfPrint);
            takePhotoScreen.ContinueButtonClick += TakePhotoScreen_ContinueButtonClick;
            contentControl.Content = takePhotoScreen;
        }

        private void TakePhotoScreen_ContinueButtonClick(object sender, EventArgs e)
        {
            TakePhotoScreen takePhotoScreen = (TakePhotoScreen)sender;
            List<Image> imageList = takePhotoScreen.imageList;
            ChoosePhotoScreen choosePhotoScreen = new ChoosePhotoScreen(numberOfCut,imageList);
            choosePhotoScreen.ButtonContinueClick += ChoosePhotoScreen_ButtonContinueClick;
            contentControl.Content = choosePhotoScreen;
        }

        private void ChoosePhotoScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            ChoosePhotoScreen choosePhotoScreen = (ChoosePhotoScreen)sender;
            imageList = choosePhotoScreen.selectedImages;
            FrameScreen frameScreen = new FrameScreen(imageList);
            frameScreen.ButtonContinueClick += FrameScreen_ButtonContinueClick;
            contentControl.Content = frameScreen;
        }

        private void FrameScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            FrameScreen frameScreen = (FrameScreen)sender;
            System.Drawing.Bitmap image = frameScreen.imgTemp;
            String codeFrameType = frameScreen.codeFrameType;
            BackgroundScreen backgroundAndFilterScreen = new BackgroundScreen(image, codeFrameType);
            backgroundAndFilterScreen.ButtonContinueClick += BackgroundAndFilterScreen_ButtonContinueClick;
            contentControl.Content = backgroundAndFilterScreen;
        }

        private void BackgroundAndFilterScreen_ButtonContinueClick(object? sender, EventArgs e)
        {
            ResetApp(); 
        }
    }
}
