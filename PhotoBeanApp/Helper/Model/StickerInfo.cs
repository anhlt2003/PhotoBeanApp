using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFStickerDemo
{
    public class StickerInfo
    {
        public BitmapImage StickerSource { get; set; }
        public Point Position { get; set; }

        public StickerInfo(BitmapImage stickerSource, Point position)
        {
            StickerSource = stickerSource;
            Position = position;
        }
    }
}
