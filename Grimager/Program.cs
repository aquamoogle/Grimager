using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grimager.ImageProcessing;

namespace Grimager
{
    class Program
    {
        static void Main(string[] args)
        {
            char rerun = '1';

            do
            {
                int columns = 2;
                var columnInput = String.Empty;
                do
                {
                    Console.WriteLine("How many columns would you like (an integer):");
                    columnInput = Console.ReadLine();
                } while (!int.TryParse(columnInput, out columns));

                var processor = new ImageProcessor(columns);

                Console.WriteLine("Specify the directory path for the source images:");
                var source = Console.ReadLine();
                Console.WriteLine("Specify the full path of the output file you would like:");
                var destination = Console.ReadLine();

                var output = processor.ProcessTo(source, destination);

                foreach (var resp in output)
                {
                    Console.WriteLine(output);
                }

                Console.Write("Press 1 to exit or 2 to run again..");
                rerun = Console.ReadKey().KeyChar;
                Console.Clear();
            } while (rerun == '2');
            //processor.ProcessTo("D:\\Spikes\\Grimager\\Grimager\\Sample\\4302-2E&F", "D:\\Spikes\\Grimager\\Grimager\\Sample\\TestImage.jpeg");
        }
    }
}
