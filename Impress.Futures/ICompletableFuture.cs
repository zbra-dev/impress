using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Futures
{
    public interface ICompletableFuture<out T> : IFuture<T>
    {
        bool IsDone();
        /// <summary>
        /// Transforms this future of T into another of R
        /// </summary>
        /// <typeparam name="R">The type of the new future</typeparam>
        /// <param name="transform">The transformation function</param>
        /// <returns></returns>
        ICompletableFuture<R> Select<R>(Func<T, R> transform);
        ICompletableFuture<R> Zip<R, U>(ICompletableFuture<U> other, Func<T, U, R> combinator);
        /// <summary>
        /// Returns the completable value or throws an exception.
        /// Shorter path to GerResult().OrThrowable()
        /// </summary>
        /// <returns></returns>
        T Wait();
        /// <summary>
        /// Returns the completable value or throws an exception.
        /// Shorter path to GerResult(timeout).OrThrowable()
        /// </summary>
        /// <returns></returns>
        T Wait(TimeSpan timeout);

    }
}
