using System;
using System.Collections.Generic;
using System.IO;

namespace NumNum
{
    using System.Linq;
    using Translator;

    class Program
    {
        private const string SHOW_TOKENS = "--show-tokens";
        private const string OUT_FILE = "-o";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var showTokens = Array.IndexOf(args, SHOW_TOKENS) != -1;
                    var outFileOptionIndex = Array.IndexOf(args, OUT_FILE);
                    NumTranslator numnum;

                    if (outFileOptionIndex == -1)
                    {
                        numnum = GetTranslator(args.Where(x => x != SHOW_TOKENS), out string fileName);
                        string option = args.Where(x => outputOptions.Contains(x)).LastOrDefault();
                        string output;

                        switch (option)
                        {
                            case "--lua":
                                output = "a.lua";
                                break;
                            default:
                                output = null;
                                break;
                        }

                        if(output != null)
                        {
                            using (var stream = File.Create(output))
                            {
                                numnum.OutputStream = stream;
                                numnum.Compile(fileName);
                            }
                        }
                        else
                        {
                            numnum.Compile(fileName);
                        }
                    }
                    else if (outFileOptionIndex == args.Length - 1)
                    {
                        Console.WriteLine("No set output file name");
                        DisplayUsage();
                        Environment.Exit(1);
                    }
                    else
                    {
                        var outFileIndex = outFileOptionIndex + 1;
                        var argList = args.Where((x, i) => i != outFileOptionIndex && i != outFileIndex && x != SHOW_TOKENS);

                        numnum = GetTranslator(argList, out string fileName);

                        if(!(numnum is NumInterpreter))
                        {
                            using (var stream = File.Create(args.ElementAt(outFileIndex)))
                            {
                                numnum.OutputStream = stream;
                                numnum.Compile(fileName);
                            }
                        }
                        else
                        {
                            numnum.Compile(fileName);
                        }
                    }
                }
                else
                {
                    DisplayUsage();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Environment.Exit(1);
            }
        }

        private static readonly string[] outputOptions = new[]
        {
            "--lua","--run",
        };

        static NumTranslator GetTranslator(IEnumerable<string> args, out string inFileName)
        {
            NumTranslator translator;
            string option = args.Where(x => outputOptions.Contains(x)).LastOrDefault();
            inFileName = args.Where(x => !outputOptions.Contains(x)).First();

            switch (option)
            {
                case "--lua":
                    translator = new NumToLua();
                    break;
                case "--run":
                    translator = new NumInterpreter();
                    break;
                default:
                    translator = new NumInterpreter();
                    break;
            }

            return translator;
        }

        static void DisplayUsage()
        {
            Console.WriteLine("numnum.exe (--run|--lua) [inFileName] (-o [outFileName]) (--show-tokens)");
        }
    }
}
