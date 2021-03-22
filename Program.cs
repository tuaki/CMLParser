using System;
using System.IO;
using CMLParser;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            // TODO - parse options

            // Load input
            string input;
            using (var streamReader = new StreamReader(options.inputFile))
                input = streamReader.ReadToEnd();

            // Compute something from the input
            string output = " ... ";
            using (var streamWriter = new StreamWriter(options.outputFile))
                streamWriter.Write(output);


            // Write log if it's required
            if (options.logFile.IsSet)
            {
                string log = " ... ";
                using (var streamWriter = new StreamWriter(options.logFile))
                    streamWriter.Write(log);
            }
        }
    }

    class Options
    {
        /// <value>
        /// This paramete is requied, because it's <see cref="Default{T}"/> and it's default value is not set.
        /// </value>
        public Default<string> inputFile = new ParameterFactory<string>()
            .Identifier("input-file", 'i')
            .Description("(Required) The name of the input file.")
            .CreateDefault();

        const string defaultOutputFile = "output.csv";
        /// <value>
        /// This parameter has default value. It seems optional to the user because he or she can overwrite it, but its in fact required from the programmer's point of view.
        /// </value>
        public Default<string> outputFile = new ParameterFactory<string>()
            .Identifier("output-file", 'o')
            .Description("(Optional) The name of the output file. Deault value: " + defaultOutputFile)
            .DefaultValue(defaultOutputFile)
            .CreateDefault();

        /// <value>
        /// This parameter is optional - if the user provides it, the behavior of the program will change.
        /// </value>
        public Optional<string> logFile = new ParameterFactory<string>()
            .Identifier("log-file", 'l')
            .Description("(Optional) The name of the log file. If it's provided, progam will write some additional information here.")
            .CreateOptional();
    }
}
