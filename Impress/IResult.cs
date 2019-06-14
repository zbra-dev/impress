using System;

namespace Impress
{
    /// <summary>
    /// A monad that either contains a value or an exception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResult<out T>
    {
        /// <summary>
        /// Return the exception associated with the result or null if the result is not in an error state
        /// </summary>
        Exception Exception { get; }
        /// <summary>
        /// Signalizes if the result is in an error state
        /// </summary>
        bool HasError { get; }
        /// <summary>
        /// Retrieves the result payload, or throws an exception if the result has an error
        /// </summary>
        T Payload { get; }
        /// <summary>
        /// Transforms the result in another result. If this result is in an error state no transformation is called.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        IResult<U> Select<U>(Func<T, U> transform);
        /// <summary>
        /// Combines this result with another result. If either result is in an error state, return an error result. Otherwise combine the values into a new value.
        /// If the combination it self originates an error a Result in an error state is returned
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="u"></param>
        /// <param name="combinator"></param>
        /// <returns></returns>
        IResult<R> Zip<U, R>(IResult<U> u, Func<T, U, R> combinator);
        IResult<U> SelectMany<U>(Func<T, IResult<U>> transform);
        /// <summary>
        /// Applies an action to the value in the result if no error occured
        /// </summary>
        /// <param name="action"></param>
        void Consume(Action<T> action);
        /// <summary>
        /// Returns the value in the result, or throws the exception in the result
        /// </summary>
        /// <returns></returns>
        T OrThrow();
    }

    /// <summary>
    /// Interaction extensions with maybe
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Extract the value in the result as a maybe. If the result is in an erro state, retrive a Nothing maybe.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Maybe<U> MaybeGet<U>(this IResult<U> result)
        {
            return result.Exception == null ? result.Payload.ToMaybe() : Maybe<U>.Nothing;
        }

        /// <summary>
        /// Extract the value in the result as a maybe. If the result is in an erro state, retrive a Nothing maybe.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Maybe<U> MaybeGet<U>(this IResult<Maybe<U>> result)
        {
            return result.Exception == null ? result.Payload : Maybe<U>.Nothing;
        }

        /// <summary>
        /// Operates over a maybe producing an IResult 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static IResult<Maybe<V>> Select<T, V>(this Maybe<T> m, Func<T, IResult<V>> transform)
        {
            if (!m.HasValue)
            {
                return Results.InValue(Maybe<V>.Nothing);
            }

            return transform(m.Value).Select(value => value.ToMaybe());
        }

        /// <summary>
        /// Operates over a maybe producing an IResult 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static IResult<Maybe<V>> Select<T, V>(this Maybe<T> m, Func<T, IResult<Maybe<V>>> transform)
        {
            if (!m.HasValue)
            {
                return Results.InValue(Maybe<V>.Nothing);
            }

            return transform(m.Value);
        }
    }
}
