using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FontAwesome.WPF;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for RealtimeScreen.xaml
    /// </summary>
    public partial class RealtimeScreen : UserControl
    {
        public RealtimeScreen()
        {
            InitializeComponent();
            ShowDateIcon(DateTime.Now.Date);
            ShowWeather("Ho Chi Minh City, VN");
        }
        public void ShowWeather(string city)
        {
            try
            {
                string apiKey = "b976b9fdcd1f9ddaa0ae7a3ee362df88";

                string url = $"http://api.openweathermap.org/data/2.5/weather?q={WebUtility.UrlEncode(city)}&appid={apiKey}&units=metric";

                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url);

                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    double temperature = result.main.temp;
                    string iconCode = result.weather[0].icon;

                    string iconUrl = $"http://openweathermap.org/img/wn/{iconCode}.png";

                    byte[] iconData = client.DownloadData(iconUrl);
                    MemoryStream iconStream = new MemoryStream(iconData);
                    BitmapImage iconImage = new BitmapImage();
                    iconImage.BeginInit();
                    iconImage.StreamSource = iconStream;
                    iconImage.EndInit();

                    StackPanel stackPanel = new StackPanel();
                    Image icon = new Image();
                    icon.Source = iconImage;
                    stackPanel.Children.Add(icon);
                    Label temperatureLabel = new Label();
                    temperatureLabel.Content = $"{(int)temperature}°C";
                    stackPanel.Children.Add(temperatureLabel);

                    btnTemperature.Content = stackPanel;
                }
            }
            catch (Exception ex)
            {
                btnTemperature.Content = "Error: " + ex.Message;
            }
        }
        private void ShowDateIcon(DateTime date)
        {
            StackPanel stackPanel = new StackPanel();

            ImageAwesome dayIcon = new ImageAwesome { Icon = FontAwesomeIcon.Calendar };
            Label dayLabel = new Label { Content = date.Day };
            stackPanel.Children.Add(dayIcon);
            stackPanel.Children.Add(dayLabel);

            ImageAwesome monthIcon = new ImageAwesome { Icon = FontAwesomeIcon.CalendarOutline };
            Label monthLabel = new Label { Content = date.Month };
            stackPanel.Children.Add(monthIcon);
            stackPanel.Children.Add(monthLabel);

            ImageAwesome yearIcon = new ImageAwesome { Icon = FontAwesomeIcon.CalendarCheckOutline };
            Label yearLabel = new Label { Content = date.Year };
            stackPanel.Children.Add(yearIcon);
            stackPanel.Children.Add(yearLabel);

            btnTime.Content = date;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
