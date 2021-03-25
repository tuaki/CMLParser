using System;
using System.Collections.Generic;
using System.IO;

namespace CMLParser.Example
{
    class Program
    {
        static void Main(string[] args) {
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

            if (!result.Status) {
                // Handle invalid input
                Console.Write(result.ErrorMessage);
                Console.Write(parser.GetHelperText());
                return;
            }
        }
    }

    class Options
    {
        /// <value>
        /// This parameter has a default value. It seems optional to the user because he or she can overwrite it, but its in fact required from the programmer's point of view.
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

        public Default<string[]> CommandAndArguments = new ParameterFactory<string[]>()
            .Description("The command and its arguments to measure")
            .CreateDefault();
    }
}
