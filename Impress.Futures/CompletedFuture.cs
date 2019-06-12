using System;
using System.Threading.Tasks;

namespace Impress.Futures
{
    internal struct CompletedFuture<T> : ICompletableFuture<T>, AsynTaskBasedFuture<T>
    {
        private readonly IResult<T> result;

        internal CompletedFuture(IResult<T> result)
        {
            this.result = result;
        }

        public IResult<T> GetResult()
        {
            return result;
        }

        public IResult<T> GetResult(TimeSpan waitPeriod)
        {
            return result;
        }

        public Task<IResult<T>> GetTask()
        {
            return Task.FromResult(result);
        }

        public bool IsDone()
        {
            return true;
        }

        public ICompletableFuture<R> Select<R>(Func<T, R> transform)
        {
            return new CompletedFuture<R>(this.result.Select(transform));
        }

        public T Wait()
        {
            return result.OrThrow();
        }

        public T Wait(TimeSpan timeout)
        {
            return result.OrThrow();
        }

        public ICompletableFuture<R> Zip<R, U>(ICompletableFuture<U> other, Func<T, U, R> combinator)
        {
            if (this.result.HasError)
            {
                return new CompletedFuture<R>( Results.InError<R>(result.Exception));
            }

            if (other.IsDone())
            {
                var otherResult = other.GetResult();

                if (otherResult.HasError)
                {
                    return new CompletedFuture<R>(Results.InError<R>(otherResult.Exception));
                }
                else
                {
                    try
                    {
                        return new CompletedFuture<R>(Results.InValue<R>(combinator(result.Payload, otherResult.Payload)));
                    }
                    catch (Exception e)
                    {
                        return new CompletedFuture<R>(Results.InError<R>(e));
                    }

                }
            }
            else
            {
                return other.Zip(this, (u, t) => combinator(t, u));
            }
        }
    }
}
