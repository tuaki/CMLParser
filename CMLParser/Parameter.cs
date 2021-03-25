using System;

namespace CMLParser
{
    public abstract class Parameter<T>
    {
        protected T _value;
        public T Value
        {
            get => _value;
            internal set => _value = value;
        }
        public static implicit operator T(Parameter<T> parameter) => parameter.Value;

        public string LongIdentifier { get; internal set; }
        public char? ShortIdentifier { get; internal set; }
        public string Description { get; internal set; }
        public Action<T> OnParseCallback { get; internal set; }

        /*
        // Index on which was this parameter given by user. Might be useful for analytics of user behavior.
        public int GivenIndex { get; internal set; }
        */
    }

    public sealed class Default<T> : Parameter<T>
    {
        private T _defaultValue;
        public T DefaultValue
        {
            get => _defaultValue;
            internal set
            {
                _defaultValue = value;
                Required = false;
            }
        }
        public bool Required { get; internal set; } = true;

        internal Default() { }
    }

    public sealed class Optional<T> : Parameter<T>
    {
        public new T Value
        {
            get
            {
                if (!IsSet)
                    throw new OptionValueNotSetException();
                return _value;
            }
            internal set
            {
                _value = value;
                IsSet = true;
            }
        }
        public bool IsSet { get; internal set; } = false;

        internal Optional() { }
    }

    public class OptionValueNotSetException : Exception
    {

    }

    public sealed class ParameterFactory<T>
    {
        /// <summary>
        /// Set identifiers of the parameter.
        /// If the identifier is not set the parameter will be automatically recognized as a plain argument.
        /// </summary>
        /// <param name="longIdentifier">The parameter argmuent can be set as: --longIdentifier=value</param>
        /// <param name="shortIdentifier">The parameter argument can be set as: -shortIdentifier value</param>
        /// <returns>This factory.</returns>
        public ParameterFactory<T> Identifier(string longIdentifier, char? shortIdentifier = null)
        {
            return this;
        }

        /// <summary>
        /// Set description of the parameter. It will appear in the helper text (see <see cref="Parser{T}.GetHelperText()"/>)
        /// </summary>
        /// <param name="description">Description text for the parameter.</param>
        /// <returns>This factory.</returns>
        public ParameterFactory<T> Description(string description)
        {
            return this;
        }

        /// <summary>
        /// Set default value of the parameter.
        /// If it is not set the parameter will be required.
        /// </summary>
        /// <param name="defaultValue">Default value for the parameter.</param>
        /// <returns>This factory.</returns>
        public ParameterFactory<T> DefaultValue(T defaultValue)
        {
            return this;
        }

        /// <summary>
        /// Set callback which will be called on a successful parse (all parameters, not just this one, have to be parsed successfully).
        /// The parsed value of this parameter will be used as an argument for the callback.
        /// </summary>
        /// <param name="callback">Function which will be called on successful parse.</param>
        /// <returns>This factory.</returns>
        public ParameterFactory<T> OnParseCallback(Action<T> callback)
        {
            return this;
        }

        // So the index of a plain argument can be determined by some value and not by relative order in the code
        /// <summary>
        /// If this parameter is a plain argument (see <see cref="Identifier(string, char?)"/>) this will be its index.
        /// All plain arguments will be parsed in ascending order by their indices.
        /// Default order in case of the equality of the indices is not defined.
        /// </summary>
        /// <param name="index">Index for the parameter if it was a plain argument.</param>
        /// <returns>This factory.</returns>
        public ParameterFactory<T> Index(T index)
        {
            return this;
        }

        /// <summary>
        /// Create new <see cref="Default{T}"/> object from this factory.
        /// </summary>
        /// <returns>New <see cref="Default{T}"/> option object.</returns>
        public Default<T> CreateDefault()
        {
            return new Default<T>();
        }

        /// <summary>
        /// Create new <see cref="Optional{T}"/> object from this factory.
        /// </summary>
        /// <returns>New <see cref="Optional{T}"/> option object.</returns>
        public Optional<T> CreateOptional()
        {
            return new Optional<T>();
        }
    }
}
