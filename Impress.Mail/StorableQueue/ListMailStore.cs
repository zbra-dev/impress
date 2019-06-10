using System.Collections.Generic;

namespace Impress.Mail.StorableQueue
{
    public class ListMailStore : IMailStore
    {

        private List<StorableMailMessage> store = new List<StorableMailMessage>();
        private long nextId = 0;

        public System.Collections.Generic.IReadOnlyList<StorableMailMessage> RetriveMessages(int maxRetries, int pageSize)
        {
            return store.Where(s => s.Retries < maxRetries).Take(pageSize).ToList();
        }

        public StorableMailMessage Insert(StorableMailMessage msg)
        {
            lock (store)
            {
                msg.Id = ++nextId;
                store.Add(msg);
                return msg;
            }
        }

        public void Update(StorableMailMessage msg)
        {
            lock (store)
            {
                var pos = store.IndexOf(msg);
                if (pos >= 0)
                {
                    store.RemoveAt(pos);
                }
                store.Add(msg);
            }
        }

        public void Delete(StorableMailMessage msg)
        {
            lock (store)
            {
                var pos = store.IndexOf(msg);
                if (pos >= 0)
                {
                    store.RemoveAt(pos);
                }
            }
        }
    }
}
