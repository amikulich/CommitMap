using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace Analyzer.Api
{
    public static class ContainerProvider
    {
        private static IDependencyResolver _dependencyResolver;
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private static IDependencyResolver Current
        {
            get
            {
                if (_dependencyResolver == null)
                {
                    lock (typeof(ContainerProvider))
                    {
                        _dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
                    }
                    //throw new InvalidOperationException(
                    //    "IDependencyResolver is not registered. Please register IDependencyResolver instance using 'SetResolver' method before use");
                }
                return _dependencyResolver;
            }
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        public static IDependencyScope BeginScope()
        {
            return Current.BeginScope();
        }

        /// <summary>
        /// Sets the IoC provider during Application Start.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            lock (typeof(ContainerProvider))
            {
                _dependencyResolver = resolver;
            }
        }

        /// <summary>
        /// Resolves given type.
        /// </summary>
        /// <typeparam name="T">Type that is registered with IoC</typeparam>
        /// <returns>Instance of given type</returns>
        public static T Resolve<T>()
        {
            return (T)Current.GetService(typeof(T));
        }

        /// <summary>
        /// Gets the service or instance by resolving given type.
        /// </summary>
        /// <typeparam name="T">Type that is registered with IoC</typeparam>
        /// <returns>Instance of given type</returns>
        public static T GetService<T>()
        {
            return (T)Current.GetService(typeof(T));
        }

        /// <summary>
        /// Gets the list of services or instances that implement given type.
        /// </summary>
        /// <typeparam name="T">Type that is registered with IoC</typeparam>
        /// <returns>Collection of instances of given type</returns>
        public static IEnumerable<T> GetServices<T>()
        {
            return Current.GetServices(typeof(T)).Cast<T>();
        }
        /// <summary>
        /// Gets the list of services or instances that implement given type.
        /// </summary>
        /// <typeparam name="T">Type that is registered with IoC</typeparam>
        /// <param name="type">The type to be resolved</param>
        /// <returns>Collection of instances of given type</returns>
        public static IEnumerable<T> GetServices<T>(Type type)
        {
            return Current.GetServices(type).Cast<T>();
        }

    }
}
