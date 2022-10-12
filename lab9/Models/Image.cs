using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace lab9.Models
{
    public class Image : FileItem
    {
        public Image(FileItem item)
        {
            Name = item.Name;
            Path = item.Path;
            ImageBitmap = new Bitmap(Path);
        }
        public Bitmap ImageBitmap { get; set; }
    }
}
