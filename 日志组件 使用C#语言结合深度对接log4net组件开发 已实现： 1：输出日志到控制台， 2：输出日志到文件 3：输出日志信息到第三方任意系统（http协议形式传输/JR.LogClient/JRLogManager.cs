using log4net.Core;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient
{
    public sealed class JRLogManager
    {
        // Fields
        private static readonly WrapperMap s_wrapperMap = new WrapperMap(new WrapperCreationHandler(JRLogManager.WrapperCreationHandler));

        // Methods
        private JRLogManager()
        {
        }

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository) =>
            LoggerManager.CreateRepository(repository);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Type repositoryType) =>
            CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Assembly repositoryAssembly, Type repositoryType) =>
            LoggerManager.CreateRepository(repositoryAssembly, repositoryType);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository, Type repositoryType) =>
            LoggerManager.CreateRepository(repository, repositoryType);

        public static ILoggerRepository CreateRepository(string repository) =>
            LoggerManager.CreateRepository(repository);

        public static ILoggerRepository CreateRepository(Type repositoryType) =>
            CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

        public static ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType) =>
            LoggerManager.CreateRepository(repositoryAssembly, repositoryType);

        public static ILoggerRepository CreateRepository(string repository, Type repositoryType) =>
            LoggerManager.CreateRepository(repository, repositoryType);

        public static IJRLog Exists(string name) =>
            Exists(Assembly.GetCallingAssembly(), name);

        public static IJRLog Exists(Assembly repositoryAssembly, string name) =>
            WrapLogger(LoggerManager.Exists(repositoryAssembly, name));

        public static IJRLog Exists(string repository, string name) =>
            WrapLogger(LoggerManager.Exists(repository, name));

        public static ILoggerRepository[] GetAllRepositories() =>
            LoggerManager.GetAllRepositories();

        public static IJRLog[] GetCurrentLoggers() =>
            GetCurrentLoggers(Assembly.GetCallingAssembly());

        public static IJRLog[] GetCurrentLoggers(Assembly repositoryAssembly) =>
            WrapLoggers(LoggerManager.GetCurrentLoggers(repositoryAssembly));

        public static IJRLog[] GetCurrentLoggers(string repository) =>
            WrapLoggers(LoggerManager.GetCurrentLoggers(repository));

        public static IJRLog GetLogger(string name) =>
            GetLogger(Assembly.GetCallingAssembly(), name);

        public static IJRLog GetLogger(Type type) =>
            GetLogger(Assembly.GetCallingAssembly(), type.FullName);

        public static IJRLog GetLogger(Assembly repositoryAssembly, string name) =>
            WrapLogger(LoggerManager.GetLogger(repositoryAssembly, name));

        public static IJRLog GetLogger(Assembly repositoryAssembly, Type type) =>
            WrapLogger(LoggerManager.GetLogger(repositoryAssembly, type));

        public static IJRLog GetLogger(string repository, string name) =>
            WrapLogger(LoggerManager.GetLogger(repository, name));

        public static IJRLog GetLogger(string repository, Type type) =>
            WrapLogger(LoggerManager.GetLogger(repository, type));

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository() =>
            GetRepository(Assembly.GetCallingAssembly());

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly) =>
            GetRepository(repositoryAssembly);

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(string repository) =>
            GetRepository(repository);

        public static ILoggerRepository GetRepository() =>
            GetRepository(Assembly.GetCallingAssembly());

        public static ILoggerRepository GetRepository(Assembly repositoryAssembly) =>
            LoggerManager.GetRepository(repositoryAssembly);

        public static ILoggerRepository GetRepository(string repository) =>
            LoggerManager.GetRepository(repository);

        public static void ResetConfiguration()
        {
            ResetConfiguration(Assembly.GetCallingAssembly());
        }

        public static void ResetConfiguration(Assembly repositoryAssembly)
        {
            LoggerManager.ResetConfiguration(repositoryAssembly);
        }

        public static void ResetConfiguration(string repository)
        {
            LoggerManager.ResetConfiguration(repository);
        }

        public static void Shutdown()
        {
            LoggerManager.Shutdown();
        }

        public static void ShutdownRepository()
        {
            ShutdownRepository(Assembly.GetCallingAssembly());
        }

        public static void ShutdownRepository(Assembly repositoryAssembly)
        {
            LoggerManager.ShutdownRepository(repositoryAssembly);
        }

        public static void ShutdownRepository(string repository)
        {
            LoggerManager.ShutdownRepository(repository);
        }

        private static IJRLog WrapLogger(ILogger logger) =>
            ((IJRLog)s_wrapperMap.GetWrapper(logger));

        private static IJRLog[] WrapLoggers(ILogger[] loggers)
        {
            IJRLog[] logArray = new IJRLog[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
            {
                logArray[i] = WrapLogger(loggers[i]);
            }
            return logArray;
        }

        private static ILoggerWrapper WrapperCreationHandler(ILogger logger) =>
            new JRLogImpl(logger);

    }
}
