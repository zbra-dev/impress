using System.Threading;

namespace Impress.Globalization
{
    public static class MessageTranslatorRegistry
    {
        internal static IMessageTranslatorRegistry cache = new FieldMessageTranslatorRegistry();


        private static ThreadLocal<IMessageTranslatorResolver> threadResolver = new ThreadLocal<IMessageTranslatorResolver>(() =>
        {
            return new StaticGlobalMessageTranslatorResolver();
        });

        public static void SetMessageTranslator(MessageTranslator translator)
        {
            cache.SetMessageTranslator(translator);
        }

        public static void SetMessageTranslatorResolver(IMessageTranslatorResolver resolver)
        {
            threadResolver.Value = resolver;
        }

        public static MessageTranslator GetMessageTranslator()
        {
            return threadResolver.Value.GetMessageTranslator();
        }

        public static void SetMessageTranslatorRegistry(IMessageTranslatorRegistry newCache)
        {
            cache = newCache;
        }
    }

    public class EditableMessageTranslatorResolver : IMessageTranslatorResolver
    {
        private MessageTranslator translator;

        public EditableMessageTranslatorResolver(MessageTranslator translator)
        {
            this.translator = translator;
        }

        public MessageTranslator GetMessageTranslator()
        {
            return translator;
        }
    }

    public class StaticGlobalMessageTranslatorResolver : IMessageTranslatorResolver
    {

        public MessageTranslator GetMessageTranslator()
        {
            return MessageTranslatorRegistry.cache.GetMessageTranslator();
        }
    }


    public interface IMessageTranslatorResolver
    {
        MessageTranslator GetMessageTranslator();
    }

    public interface IMessageTranslatorRegistry
    {
        void SetMessageTranslator(MessageTranslator translator);
        MessageTranslator GetMessageTranslator();
    }

    public class FieldMessageTranslatorRegistry : IMessageTranslatorRegistry
    {
        private MessageTranslator translator;

        public void SetMessageTranslator(MessageTranslator translator)
        {
            this.translator = translator;
        }

        public MessageTranslator GetMessageTranslator()
        {
            return this.translator;
        }
    }
}
