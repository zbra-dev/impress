namespace Impress.Globalization
{
    public class MessageTranslation
    {
        public string Translation { get; set; }
        public bool IsTranslated { get; set; }
        public string MessageKey { get; set; }
        public object[] MessageParams { get; set; }
    }
}
