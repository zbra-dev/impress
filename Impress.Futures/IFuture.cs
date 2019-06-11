using System;

namespace Impress.Futures
{
    public interface IFuture<out T>
    {
        IResult<T> GetResult();
        IResult<T> GetResult(TimeSpan timeout);
    }
}
