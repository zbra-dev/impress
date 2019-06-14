using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Impress.Logging
{
    public sealed class LogManagerRegistry
    {
        private static readonly LogManagerRegistry registry = new LogManagerRegistry();
        private ILogManager manager = new ConsoleLogManager();

        public static LogManagerRegistry GetInstance()
        {
            return registry;
        }

        /// <summary>
        /// Scans for all implementations of ILogManager available accros all loaded assemblies.
        /// If no ILogManager is found , the ConsoleLogManager will be used.
        /// If one ILogManager is found, that one will be used
        /// If more than one ILogManager is found, a TooManyLogManagerFoundException occurs.
        /// </summary>
        /// <returns>true is exactly one ILogManager is found, false if none is found</returns>
        public bool ScanAndRegister()
        {
            var type = typeof(ILogManager);
            var consoleManagerType = typeof(ConsoleLogManager);
            var types = GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !type.IsInterface && !type.IsAbstract && !consoleManagerType.IsAssignableFrom(type))
                .Where(p => type.IsAssignableFrom(p))
                .ToList();

            if (types.Count > 1)
            {
                throw new TooManyLogManagerFoundException(types.Select(t => t.FullName).ToList());
            }
            else if (types.Count == 1)
            {
                ILogManager mgr = (ILogManager)Activator.CreateInstance(types.First());
                RegisterLogManager(mgr);

                return true;
            }
            return false;
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                stack.Push(a);
                list.Add(a.FullName);
            }


            do
            {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);

        }

        public static void RegisterLogManager(ILogManager manager)
        {
            registry.manager = manager;
        }

        private LogManagerRegistry() { }


        public ILogger RetriveLoggerByType(Type type)
        {
            return manager.RetrieveLoggerByType(type);
        }

        public ILogger RetriveLoggerByName(string name)
        {
            return manager.RetrieveLoggerByName(name);
        }
    }
}
