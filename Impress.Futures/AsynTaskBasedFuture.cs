using System;
using System.Threading.Tasks;

namespace Impress.Futures
{
    internal interface AsynTaskBasedFuture<T>
    {
        Task<IResult<T>> GetTask();
    }
}
