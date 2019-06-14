using System;
using System.Threading.Tasks;

namespace Impress.Futures
{
    internal interface IAsyncTaskBasedFuture<T>
    {
        Task<IResult<T>> GetTask();
    }
}
