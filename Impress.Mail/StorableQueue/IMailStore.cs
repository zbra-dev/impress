using System.Collections.Generic;

namespace Impress.Mail.StorableQueue
{
    public interface IMailStore
    {

        /// <summary>
        /// Retrives the first pageSize messages that have not exceed maxRetries
        /// </summary>
        /// <param name="maxRetries"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IReadOnlyList<StorableMailMessage> RetriveMessages(int maxRetries, int pageSize);

        /// <summary>
        /// Persist the Message fo the first time, for later retrival.
        /// This method show attribute an Id to the message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>The mail message with Id</returns>
        StorableMailMessage Insert(StorableMailMessage msg);
        /// <summary>
        /// Updates the message retry information
        /// </summary>
        /// <param name="msg"></param>
        void Update(StorableMailMessage msg);
        void Delete(StorableMailMessage msg);
    }
}
