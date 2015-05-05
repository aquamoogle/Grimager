using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Grimager.ImageProcessing
{
    public class Settings
    {
        protected Settings()
        {
            var cfg = ConfigurationManager.AppSettings;
            Width = int.Parse(cfg["Image.Width"]);
            Height = int.Parse(cfg["Image.Height"]);
            Spacing = int.Parse(cfg["Image.Spacing"]);
            LetterBoxSize = int.Parse(cfg["Image.Numbering.Size"]);
            ValidFormats = cfg["Image.ValidFormats"].Split(',').Select(x => x.ToLower()).ToArray();
        }

        private static Settings _instance;

        public static Settings Get()
        {
            if (_instance == null)
            {
                _instance = new Settings();
            }

            return _instance;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Border { get; set; }
        public int Spacing { get; set; }
        public int LetterBoxSize { get; set; }
        public string[] ValidFormats { get; set; }
    }
}
