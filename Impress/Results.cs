using System;

namespace Impress
{
    public class Results
    {
        /// <summary>
        /// Creates an IResult with the given value
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IResult<V> InValue<V>(V value)
        {
            return new Result<V>(value);
        }

        /// <summary>
        /// Creates an IResult with the given exception
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IResult<V> InError<V>(Exception exception)
        {
            return new Result<V>(exception);
        }

        private Results() { }

    }

    // internal implementation of IResult
    internal struct Result<T> : IResult<T>
    {

        private readonly T value;
        private readonly Exception exception;

        internal Result(T value)
        {
            this.value = value;
            this.exception = null;
        }

        internal Result(Exception exception)
        {

            if (exception == null)
            {
                throw new ArgumentNullException("Exception expected");
            }
            this.exception = exception;
            this.value = default(T);
        }

        public override string ToString()
        {
            return this.exception == null
                ? (this.value == null ? "null" : this.value.ToString())
                : this.exception.ToString();
        }

        public IResult<U> Select<U>(Func<T, U> transform)
        {
            if (this.exception == null)
            {
                return new Result<U>(transform(this.value));
            }
            else
            {
                return new Result<U>(exception);
            }
        }

        public IResult<U> SelectMany<U>(Func<T, IResult<U>> transform)
        {
            if (this.exception == null)
            {
                try
                {
                    return transform(this.value);
                }
                catch (Exception e)
                {
                    return new Result<U>(e);
                }
            }
            else
            {
                return new Result<U>(exception);
            }
        }

        public override bool Equals(object obj)
        {

            if (!(obj is Result<T>))
            {
                return false;
            }

            var other = (Result<T>)obj;

            return this.HasError
                ? this.exception.GetType().Equals(other.exception.GetType())
                : this.value != null
                    ? this.value.Equals(other.value)
                    : other.value == null;
        }

        public override int GetHashCode()
        {
            return exception == null ? (value == null ? 0 : value.GetHashCode()) : exception.GetHashCode();
        }

        public void Consume(Action<T> action)
        {
            if (exception == null)
            {
                action(Payload);
            }
        }
        public IResult<R> Zip<U, R>(IResult<U> other, Func<T, U, R> combinator)
        {
            if (this.HasError)
            {
                return new Result<R>(this.exception);
            }
            else if (other.HasError)
            {
                return new Result<R>(other.Exception);
            }
            else
            {
                try
                {
                    return new Result<R>(combinator(this.Payload, other.Payload));
                }
                catch (Exception e)
                {
                    return new Result<R>(e);
                }
            }
        }

        public T OrThrow()
        {
            if (HasError)
            {
                throw this.exception;
            }
            else
            {
                return Payload;
            }
        }

        public bool HasError { get { return exception != null; } }

        public Exception Exception
        {
            get
            {
                return exception;
            }
        }

        public T Payload
        {
            get
            {
                if (exception != null)
                {
                    throw exception;
                }
                return value;
            }
        }
    }

}
