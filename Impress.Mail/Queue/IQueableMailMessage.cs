using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail.Queue
{
    public interface IQueableMailMessage : IMailMessage
    {
        long? Id { get; }
        DateTime CreationDate { get; }
        DateTime NextRetryDate { get; }
        int Retries { get; }
        void IncrementRetry();
    }
}
