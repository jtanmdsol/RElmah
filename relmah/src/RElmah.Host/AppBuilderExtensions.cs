﻿using System;
using Microsoft.AspNet.SignalR;
using Owin;
using RElmah.Host.Hubs;
using RElmah.Middleware;
using RElmah.Services;

namespace RElmah.Host
{
    public class Settings
    {
        public Action<IConfigurationUpdater> Bootstrapper { get; set; }
        public Func<IConfigurationStore> BuildConfigurationStore { get; set; }
    }

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseRElmah(this IAppBuilder builder, Settings settings = null)
        {
            var registry = GlobalHost.DependencyResolver;

            //TODO: improve the way this part can be customized from outside

            var ei      = new ErrorsInbox();
            var cs      = settings.SafeCall(
                            s  => s.BuildConfigurationStore(), 
                            () => new InMemoryConfigurationStore(), 
                            s  => s != null && s.BuildConfigurationStore != null);

            var ch      = new ConfigurationHolder(cs);
            var c       = new Connector(ch);
            //var ctuip = new ClientTokenUserIdProvider();

            //Infrastructure
            registry.Register(typeof(IErrorsInbox),           () => ei);
            registry.Register(typeof(IConnection),            () => c);
            registry.Register(typeof(IConfigurationProvider), () => ch);
            registry.Register(typeof(IConfigurationUpdater),  () => ch);
            registry.Register(typeof(IConfigurationStore),    () => cs);
            //registry.Register(typeof(IUserIdProvider),      () => ctuip);

            Dispatcher.Wire(ei, ch);

            //Hubs
            registry.Register(typeof(ErrorsHub), () => new ErrorsHub(c, registry.Resolve<IUserIdProvider>()));

            if (settings != null && settings.Bootstrapper != null)
                settings.Bootstrapper(ch);

            return builder.UseRElmahMiddleware<RElmahMiddleware>(new Resolver());
        }

        static IAppBuilder UseRElmahMiddleware<T>(this IAppBuilder builder, params object[] args)
        {
            return builder.Use(typeof(T), args);
        }

        public static IAppBuilder RunSignalR(this IAppBuilder builder)
        {
            OwinExtensions.RunSignalR(builder);

            return builder;
        }
    }
}
