using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Impress.Futures
{
    public abstract class CompletableFuture
    {
        public static ICompletableFuture<P> CompletedWithResult<P>(IResult<P> result)
        {
            return new CompletedFuture<P>(result);
        }

        public static ICompletableFuture<P> CompletedWithValue<P>(P payload)
        {
            return new CompletedFuture<P>(Results.InValue<P>(payload));
        }

        public static ICompletableFuture<P> CompleteExceptionally<P>(Exception e)
        {
            return new CompletedFuture<P>(Results.InError<P>(e));
        }

        public static ICompletableFuture<P> RunAsync<P>(Func<IResult<P>> runnable)
        {
            return TaskFuture<P>.RunAsync<P>(() => {
                try
                {
                    return runnable();
                }
                catch (Exception e)
                {
                    return Results.InError<P>(e);
                }
            });
        }

        public static ICompletableFuture<P> RunAsync<P>(Func<P> runnable)
        {
            return TaskFuture<P>.RunAsync<P>(() => {
                try
                {
                    return Results.InValue<P>(runnable());
                }
                catch (Exception e)
                {
                    return Results.InError<P>(e);
                }
            });
        }

        public static ICompletableFuture<IEnumerable<P>> SelectAsync<T, P>(IEnumerable<T> source, Func<T, P> mapper)
        {
            return Reduce(source.Select(item => RunAsync(() => {
                return mapper(item);
            })));
        }

        private static ICompletableFuture<IEnumerable<P>> Reduce<P>(IEnumerable<ICompletableFuture<P>> source)
        {

            ICompletableFuture<List<P>> root = CompletableFuture.CompletedWithValue(new List<P>());

            return source.Aggregate(root, (a, b) => a.Zip(b, (x, y) => { x.Add(y); return x; }));
        }

        internal CompletableFuture() { }
    }
}
