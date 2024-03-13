using System;
using System.Collections.Generic;
using System.Drawing;
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
using QRCoder;

namespace PhotoBeanApp.View
{
    /// <summary>
    /// Interaction logic for PaymentScreen.xaml
    /// </summary>
    public partial class PaymentScreen : UserControl
    {
        public event EventHandler PaymentSuccess;
        public int numberOfCut, numberOfPrint;

        public PaymentScreen(int numberOfCut, int numberOfPrint)
        {
            InitializeComponent();
            this.numberOfCut = numberOfCut;
            this.numberOfPrint = numberOfPrint;
            Loaded += UserControl_Loaded;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPayment();
        }
        public void LoadPayment()
        {
            cutLabel.Content = $"Số lượng tấm ảnh: {numberOfCut}";
            printLabel.Content = $"Số lượng bản in: {numberOfPrint}";
            totalLabel.Content = $"Tổng tiền: {50000 * numberOfPrint}";

            string paymentData = "Tuấn Anh đẹp trai =)))";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(paymentData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImageBitmap = qrCode.GetGraphic(20);

            qrCodeImage.Source = BitmapToImageSource(qrCodeImageBitmap);

            qrCodeImage.MouseLeftButtonDown += (sender, e) =>
            {
                MessageBox.Show("Thanh toán thành công");
                PaymentSuccess?.Invoke(this, EventArgs.Empty);
            };
        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
