using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFStickerDemo
{
    public class StickerInfo
    {
        public BitmapImage StickerSource { get; set; }
        public Point Position { get; set; }
        public double stickerX { get; set; }
        public double stickerY { get; set; }

        public StickerInfo(BitmapImage stickerSource, Point position, double stickerX, double stickerY)
        {
            StickerSource = stickerSource;
            Position = position;
            this.stickerX = stickerX;
            this.stickerY = stickerY;
        }
    }
}
