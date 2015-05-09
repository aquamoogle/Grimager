using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grimager.ImageProcessing
{
    public class ImageProcessor
    {
        protected int Columns { get; set; }

        public ImageProcessor(int columns)
        {
            Columns = columns;
        }

        public string[] ProcessTo(string sourceDirectory, string outputFile)
        {
            var reader = new ImageReader(sourceDirectory);
            return Process(reader, outputFile);
        }

        public string[] Process(ImageReader reader, string outputFile)
        {
            try
            {
                int idx = 1;
                var cfg = Settings.Get();
                var images = new List<Image>();
                foreach (var original in reader.GetImages())
                {
                    var img = original.ResizeImage(cfg.Width, cfg.Height);
                    original.Dispose();
                    using (var graphic = Graphics.FromImage(img))
                    {
                        graphic.FillRectangle(Brushes.White, new Rectangle(0, 0, cfg.LetterBoxSize, cfg.LetterBoxSize));
                        graphic.WriteTextToFit(idx.ToString(), 0, 0, new Size(cfg.LetterBoxSize, cfg.LetterBoxSize));
                    }
                    images.Add(img);
                    idx++;
                }

                var totalImages = images.Count();
                var totalRows = (int)Math.Ceiling(totalImages / (double)Columns);
                var totalWidth = Columns * cfg.Width + ((Columns - 1) * cfg.Spacing);
                var totalHeight = totalRows * cfg.Height + ((totalRows - 1) * cfg.Spacing);
                var output = new Bitmap(totalWidth, totalHeight);
                var row = 0;
                using (var graphic = Graphics.FromImage(output))
                {
                    for (var i = 0; i < totalImages; i++)
                    {
                        if (i > 0 && i % Columns == 0)
                            row++;

                        var column = i % Columns;

                        var x = column * cfg.Width;
                        if (column > 0)
                        {
                            x += cfg.Spacing * column;
                        }
                        var y = row * cfg.Height;

                        if (row > 0)
                        {
                            y += cfg.Spacing * row;
                        }

                        graphic.DrawImage(images[i], new Rectangle(new Point(x, y), new Size(cfg.Width, cfg.Height)));
                        images[i].Dispose();
                    }
                }

                output.Save(outputFile, ImageFormat.Jpeg);

                return new string[0];
            }
            catch (Exception ex)
            {
                return new string[]
                {
                    "Folder does not exist or does not contain any valid images",
                    "Full Error: " + ex.Message
                };
            }
        }
    }
}
