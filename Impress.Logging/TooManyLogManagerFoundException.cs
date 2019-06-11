using System.Collections.Generic;

namespace Impress.Logging
{
    public class TooManyLogManagerFoundException : LogManagerScanException
    {

        public TooManyLogManagerFoundException(List<string> namesOfAllFound)
            : base(string.Join("\n\t", namesOfAllFound))
        {
        }
    }
}
