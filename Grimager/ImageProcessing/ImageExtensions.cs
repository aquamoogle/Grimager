using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grimager.ImageProcessing
{
    public static class ImageExtensions
    {
        public static Image ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void WriteTextToFit(this Graphics graphic, string text, int x, int y, Size size)
        {
            var output = new Bitmap(size.Width, size.Height);
            using (var trialFont = new Font("Arial", size.Width < size.Height ? size.Width : size.Height, FontStyle.Regular, GraphicsUnit.Pixel))
            {
                using (var font = GetFont(graphic, size, trialFont, text))
                {
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    graphic.DrawString(text, font, Brushes.Red, new RectangleF(0, 0, size.Width, size.Height), format);
                }
            }
        }

        private static Font GetFont(Graphics graphic, Size size, Font font, string text)
        {
            var realSize = graphic.MeasureString(text, font);
            var heightRatio = size.Height/realSize.Height;
            var widthRatio = size.Width/realSize.Width;
            var ratio = (heightRatio < widthRatio) ? heightRatio : widthRatio;
            var fontSize = font.Size*ratio;
            var style = font.Style;
            var family = font.FontFamily;
            var fontUnit = font.Unit;
            var newFont = new Font(family, fontSize, style, fontUnit);
            return newFont;
        }
    }
}
