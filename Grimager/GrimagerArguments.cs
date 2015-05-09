using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Grimager
{
    public class GrimagerArguments
    {
        protected Dictionary<string, Tuple<string, Action<string>>> Arguments { get; set; }
        protected Dictionary<string, Action> SpecialArguments { get; set; }

        public List<string> Files { get; set; }
        public int? Columns { get; set; }
        public string Output { get; set; }
        public bool WasSpecialArgument { get; set; }

        protected void PrintHelp()
        {
            foreach (var arg in Arguments)
            {
                Console.WriteLine("{0}  {1}", arg.Key, arg.Value.Item1);
            }
        }

        public GrimagerArguments(IEnumerable<string> arguments)
        {
            Files = new List<string>();

            SpecialArguments = new Dictionary<string, Action>
            {
                { "-help", PrintHelp },
                { "-?", PrintHelp }
            };

            Arguments = new Dictionary<string, Tuple<string, Action<string>>>
            {
                { "-files", new Tuple<string, Action<string>>("Provides the full file paths for the files to be processed separated by spaces, use % to account for spaces in the path", (x) => Files.Add(x))},
                { "-columns", new Tuple<string, Action<string>>("How many columns in the output grid image", (x) => Columns = Int32.Parse(x))},
                { "-output", new Tuple<string, Action<string>>("The full file path for the output file, use % to account for spaces in the path", (x) => Output = x)}
            };

            var currentArgument = String.Empty;
            foreach (var arg in arguments)
            {
                if (arg.StartsWith("-"))
                {
                    if (SpecialArguments.ContainsKey(arg))
                    {
                        SpecialArguments[arg]();
                        WasSpecialArgument = true;
                        break;
                    }
                    currentArgument = arg;
                    continue;
                }

                Arguments[currentArgument].Item2(arg);
            }
        }
    }
}