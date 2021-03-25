namespace CMLParser
{
    public sealed class Parser<T>
    {
        private T inputOptions;

        public Parser(T options)
        {
            inputOptions = options;
        }

        /// <summary>
        /// Generates helper text from all parameters. It consists of:
        /// [-shortIdentifier], [--longIdentifier] [description]
        /// </summary>
        /// <returns>Helper text with identifiers and descriptions of all parameters.</returns>
        public string GetHelperText()
        {
            return "You should have read the documentation.";
        }

        /// <summary>
        /// Parse given command line input to match expected pattern.
        /// </summary>
        /// <param name="commandLineInput">Command line input to be parsed.</param>
        /// <returns><see cref="ParseResult{T}"/></returns>
        public ParseResult<T> Parse(string commandLineInput)
        {
            return new ParseResult<T>() { Status = false, ErrorMessage = "" };
        }

        /// <summary>
        /// Checks if given options object is valid (e.g. if it can be unambiguously parsed to). The validity depends only on the object itself, not on any user input.
        /// </summary>
        /// <returns><see cref="CheckResult{T}"/></returns>
        public CheckResult<T> Check()
        {
            return new CheckResult<T>() { Status = false, ErrorMessage = "" };
        }
    }

    public sealed class ParseResult<T>
    {
        public bool Status { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public T Options { get; internal set; }

        internal ParseResult() { }
    }

    public sealed class CheckResult<T>
    {
        public bool Status { get; internal set; }
        public string ErrorMessage { get; internal set; }

        internal CheckResult() { }
    }
}
