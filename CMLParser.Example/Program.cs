using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CMLParser.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser<Options>(new Options());

#if DEBUG
            // Let's perform consistency check first
            var check = parser.Check();
            if (!check.Status) {
                Console.WriteLine("Invalid options object!");
                Console.Write(check.ErrorMessage);
                return;
            }
#endif

            // Assume we have some arguments we need to parse
            string commandLineArguments = " ... ";
            var result = parser.Parse(commandLineArguments);

            // Handle invalid input
            if (!result.Status) {
                Console.Write(result.ErrorMessage);
                Console.Write(parser.GetHelperText());
                return;
            }

            // Now we can use parsed options
            var options = result.Options;

            // Show help
            if (options.ShowHelp)
            {
                Console.WriteLine("time [options] command [arguments...]");
                Console.WriteLine(parser.GetHelperText());
                Environment.Exit(0);
            }

            // Show version
            if (options.ShowVersion)
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Console.WriteLine("Version: " + version);
                Environment.Exit(0);
            }

            // The application itsef ...

        }
    }

    class Options
    {
        /// <value>
        /// This parameter has a default value. It seems optional to the user because he or she can overwrite it, but it's in fact required from the programmer's point of view.
        /// </value>
        public Default<string> Format = new ParameterFactory<string>()
            .Identifier("format", 'f')
            .Description(
                "Specify output format, possibly overriding the format specified in the environment variable TIME.")
            .DefaultValue(Environment.GetEnvironmentVariable("TIME"))
            .CreateDefault();

        public Default<bool> Portability = new ParameterFactory<bool>()
            .Identifier("portability", 'p')
            .Description("Use the portable output format.")
            .DefaultValue(false)
            .CreateDefault();

        /// <value>
        /// This parameter is optional - if the user provides it, the behavior of the program will change.
        /// </value>
        public Optional<string> OutputFile = new ParameterFactory<string>()
            .Identifier("output", 'o')
            .Description("Do not send the results to stderr, but overwrite the specified file.")
            .CreateOptional();

        public Default<bool> Append = new ParameterFactory<bool>()
            .Identifier("append", 'a')
            .Description("(Used together with -o.) Do not overwrite but append.")
            .DefaultValue(false)
            .CreateDefault();

        public Default<bool> Verbose = new ParameterFactory<bool>()
            .Identifier("append", 'a')
            .Description("Give very verbose output about all the program knows about.")
            .DefaultValue(false)
            .CreateDefault();

        /// <value>
        /// This is a plain argument because it doesn't have identifier. It is also an array so it will match all remaining arguments.
        /// </value>
        public Default<string[]> CommandAndArguments = new ParameterFactory<string[]>()
            .Description("The command and its arguments to measure")
            .CreateDefault();

        public Default<bool> ShowHelp = new ParameterFactory<bool>()
            .Identifier("help")
            .Description("Print a usage message on standard output and exit successfully.")
            .DefaultValue(false)
            .CreateDefault();

        public Default<bool> ShowVersion = new ParameterFactory<bool>()
            .Identifier("version", 'V')
            .Description("Print version information on standard output, then exit successfully.")
            .DefaultValue(false)
            .CreateDefault();
    }
}
