using System;

namespace CMLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();


        }
    }

    class Options
    {
        Default<string> fileName = new ParameterFactory<string>()
            .Identifier("file-name", 'f')
            .Description("The name of the output file")
            .CreateDefault();
    }
}
