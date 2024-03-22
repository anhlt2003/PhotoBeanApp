using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStickerDemo
{
    public class StickerEventArgs : EventArgs
    {
        public StickerInfo RemovedStickerInfo { get; }

        public StickerEventArgs(StickerInfo removedStickerInfo)
        {
            RemovedStickerInfo = removedStickerInfo;
        }
    }
}
