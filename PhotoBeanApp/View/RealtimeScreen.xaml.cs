using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static QRCoder.PayloadGenerator;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for RealtimeScreen.xaml
    /// </summary>
    public partial class RealtimeScreen : UserControl
    {
        private DispatcherTimer timer;
        public RealtimeScreen()
        {
            InitializeComponent();
            Loaded += RealTimeClock_Loaded;
            ShowWeather("Ho Chi Minh City, VN");
        }

        private void RealTimeClock_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            btnTime.Content = DateTime.Now.ToString("HH:mm:ss");
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


                    btnTemperature.Content = $"{temperature}°C";
                }
            }
            catch (Exception ex)
            {
                btnTemperature.Content = "Error: " + ex.Message;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
