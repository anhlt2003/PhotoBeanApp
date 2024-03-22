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

namespace WPFStickerDemo
{
    /// <summary>
    /// Interaction logic for Sticker.xaml
    /// </summary>
    public partial class Sticker : UserControl
    {
        public event EventHandler<StickerEventArgs> StickerRemoved;

        public StickerInfo StickerInfo { get; set; }
        public Sticker()
        {
            InitializeComponent();
        }
        


        private void removeSticker_Click(object sender, RoutedEventArgs e)
        {
            var data = (Canvas)Parent;
            data.Children.Remove(this);

            StickerRemoved?.Invoke(this, new StickerEventArgs(StickerInfo));
        }

        public void SetImageSource(BitmapImage source)
        {
            stickerImage.Source = source;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(sticker, new DataObject(DataFormats.Serializable, sticker), DragDropEffects.Move);
            }
        }
    }
}
