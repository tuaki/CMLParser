using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLParser
{
    public abstract class Parameter<T>
    {
        protected T _value;
        public T Value
        {
            get => _value;
            internal set { _value = value; }
        }
        public static implicit operator T(Parameter<T> parameter) => parameter.Value;

        public string Identifier { get; }
        public char? ShortIdentifier { get; }
        public string Description { get; internal set; }
        public Action<T> OnParseCallback { get; internal set; }

        // Index on which was this parameter given by user. Might be useful for analytics of user behavior.
        public int GivenIndex { get; internal set; }
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
        public bool Required { get; private set; } = true;

        internal Default() { }
    }

    public sealed class Optional<T> : Parameter<T>
    {
        public new T Value
        {
            get
            {
                if (!IsSet)
                    throw new Exception("Value not set.");
                return _value;
            }
            internal set
            {
                _value = value;
                IsSet = true;
            }
        }
        public bool IsSet { get; private set; } = false;

        internal Optional() { }
    }

    public sealed class ParameterFactory<T>
    {
        // When the identifier is set, the parameter can not be a plain argument anymore.
        public ParameterFactory<T> Identifier(string longIdentifier, char? shortIdentifier = null)
        {
            return this;
        }

        public ParameterFactory<T> Description(string description)
        {
            return this;
        }

        public ParameterFactory<T> DefaultValue(T defaultValue)
        {
            return this;
        }

        // So the index of a plain argument can be derermined by some value and not by relative order in the code
        public ParameterFactory<T> Index(T defaultValue)
        {
            return this;
        }

        public ParameterFactory<T> OnParseCallback(Action<T> callback)
        {
            return this;
        }

        public Optional<T> CreateOptional()
        {
            return new Optional<T>();
        }

        public Default<T> CreateDefault()
        {
            return new Default<T>();
        }
    }
}
