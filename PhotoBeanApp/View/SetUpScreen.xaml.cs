using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for SetUpScreen.xaml
    /// </summary>
    public partial class SetUpScreen : UserControl
    {
        public event EventHandler ButtonNextClick;
        public int numberOfCut;
        public int numberOfPrint;
        public SetUpScreen()
        {
            InitializeComponent();
            this.numberOfPrint = 1;
            this.numberOfCut = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name.ToString())
            {
                case "btn1cut":
                    numberOfCut = 1;
                    break;
                case "btn2cut":
                    numberOfCut = 2;
                    break;
                case "btn4cut":
                    numberOfCut = 4;
                    break;
                case "btn6cut":
                    numberOfCut = 6;
                    break;
                case "btn8cut":
                    numberOfCut = 8;
                    break;
            }
            FrameWrapPanel.Children.Clear();
            LoadFrames(numberOfCut);
        }

        public void LoadFrames(int numberOfCut)
        {
            double itemWidth = FrameWrapPanel.Width/4;
            for (int i = 1; i <= 4; i++)
            {
                Image frame = new Image();
                frame.Source = new BitmapImage(new Uri($"/Frames/frame_{numberOfCut}_{i}.png", UriKind.Relative));
                frame.Width = itemWidth;
                frame.Stretch = Stretch.Uniform;
                frame.HorizontalAlignment = HorizontalAlignment.Center;
                frame.VerticalAlignment = VerticalAlignment.Center;
                FrameWrapPanel.Children.Add(frame);
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            ButtonNextClick?.Invoke(this, EventArgs.Empty);
        }

        private void increaseButton_Click(object sender, RoutedEventArgs e)
        {
            numberOfPrint = Int32.Parse(printLabel.Content.ToString());
            printLabel.Content = $"{++numberOfPrint}";
        }

        private void decreaseButton_Click(object sender, RoutedEventArgs e)
        {
            numberOfPrint = Int32.Parse(printLabel.Content.ToString());
            printLabel.Content = $"{--numberOfPrint}";
        }
    }
}
