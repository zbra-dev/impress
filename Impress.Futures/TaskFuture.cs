using System;
using System.Threading.Tasks;

namespace Impress.Futures
{
    internal struct TaskFuture<T> : ICompletableFuture<T>, AsynTaskBasedFuture<T>
    {

        public static TaskFuture<P> RunAsync<P>(Func<IResult<P>> supplier)
        {
            return new TaskFuture<P>(Task.Factory.StartNew(supplier, TaskCreationOptions.DenyChildAttach));
        }

        private readonly Task<IResult<T>> task;

        private TaskFuture(Task<IResult<T>> task)
        {
            this.task = task;
        }

        public IResult<T> GetResult()
        {
            try
            {
                task.Wait();
                if (task.IsFaulted)
                {
                    return Results.InError<T>(task.Exception);
                }
                else
                {
                    return task.Result;
                }
            }
            catch (Exception e)
            {
                return Results.InError<T>(e);
            }
        }

        public IResult<T> GetResult(TimeSpan timeout)
        {
            try
            {
                if (task.Wait(timeout))
                {
                    if (task.IsFaulted)
                    {
                        return Results.InError<T>(task.Exception);
                    }
                    else
                    {
                        return task.Result;
                    }
                }
                else
                {
                    return Results.InError<T>(new Exception("Timeout"));
                }
            }
            catch (Exception e)
            {
                return Results.InError<T>(e);
            }

        }

        public bool IsDone()
        {
            return task.IsCompleted;
        }

        public ICompletableFuture<R> Select<R>(Func<T, R> transform)
        {
            return new TaskFuture<R>(task.Then(t => {
                try
                {
                    return t.Select(transform);
                }
                catch (Exception e)
                {
                    return Results.InError<R>(e);
                }
            }));
        }

        public ICompletableFuture<R> Zip<R, U>(ICompletableFuture<U> other, Func<T, U, R> combinator)
        {
            AsynTaskBasedFuture<U> asyncTask = other as AsynTaskBasedFuture<U>;

            if (asyncTask == null)
            {
                throw new Exception("Combined future is not asyncronous");
            }
            return new TaskFuture<R>(this.task.Then(t => asyncTask.GetTask().Then(u => t.Zip(u, combinator))));
        }

        public Task<IResult<T>> GetTask()
        {
            return task;
        }

        public T Wait()
        {
            return this.GetResult().OrThrow();
        }

        public T Wait(TimeSpan timeout)
        {
            return this.GetResult(timeout).OrThrow();
        }
    }
}
