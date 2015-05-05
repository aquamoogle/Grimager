using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grimager.ImageProcessing
{
    public class ImageReader
    {
        protected string[] Files { get; set; }

        public ImageReader(string path)
        {
            var cfg = Settings.Get();
            Files = Directory.GetFiles(path).Where(x => cfg.ValidFormats.Any(y => x.EndsWith(y, StringComparison.InvariantCultureIgnoreCase))).ToArray();
        }

        public IEnumerable<Image> GetImages()
        {
            foreach (var f in Files.OrderBy(x => x))
            {
                yield return Image.FromFile(f);
            }
        }
    }
}
