﻿using System;
using Microsoft.AspNet.SignalR;
using Owin;
using RElmah.Host.SignalR.Hubs;
using RElmah.Middleware;
using RElmah.Services;

namespace RElmah.Host.SignalR
{
    public class Settings
    {
        public Action<IConfigurationUpdater> InitializeConfiguration { get; set; }
    }

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseRElmah(this IAppBuilder builder, Settings settings = null)
        {
            var registry = GlobalHost.DependencyResolver;

            //TODO: improve the way this part can be customized from outside

            var ctuip = new ClientTokenUserIdProvider();
            var ei    = new ErrorsInbox();
            var cs    = new InMemoryConfigurationStore();
            var ch    = new ConfigurationHolder(cs);
            var d     = new Dispatcher(ei, ch, ch);

            registry.Register(typeof(IErrorsInbox), () => ei);
            registry.Register(typeof(IDispatcher),  () => d);
            registry.Register(typeof(IConfigurationProvider), () => ch);
            registry.Register(typeof(IConfigurationUpdater), () => ch);
            registry.Register(typeof(IConfigurationStore), () => cs);
            registry.Register(typeof(IUserIdProvider), () => ctuip);

            registry.Register(typeof(ErrorsHub), () => new ErrorsHub(d, ctuip));

            if (settings != null && settings.InitializeConfiguration != null)
                settings.InitializeConfiguration(ch);

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
