using System;
using System.Threading.Tasks;

namespace Impress.Futures
{
    internal static class TaskExtensions
    {
        // monad unit
        public static Task<R> Then<T, R>(this Task<T> first, Func<T, R> transform)
        {
            return Then(first, t => Task.FromResult(transform(t)));
        }

        // monad bind
        public static Task<R> Then<T, R>(this Task<T> first, Func<T, Task<R>> next)
        {

            var tcs = new TaskCompletionSource<R>();
            first.ContinueWith(delegate
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var t = next(first.Result);
                        if (t == null)
                        {
                            tcs.TrySetCanceled();
                        }
                        else {
                            t.ContinueWith(delegate
                            {
                                if (t.IsFaulted)
                                {
                                    tcs.TrySetException(t.Exception.InnerExceptions);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(t.Result);
                                }
                            }, TaskContinuationOptions.ExecuteSynchronously);
                        }
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }
    }
}
