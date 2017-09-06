using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace IoCStructureMapConfiguration.IoC
{
    /// <summary>
    /// Dependency resolver for StructureMap.
    /// </summary>
    public class StructureMapResolver : IDependencyResolver
    {
        private IContainer container;

        /// <summary>
        /// Constructs <see cref="StructureMapResolver"/> instance.
        /// </summary>
        /// <param name="container">IoC container.</param>
        public StructureMapResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        /// <summary>
        /// Starts resolution scope.
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new StructureMapResolver(child);
        }

        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">
        /// The service to be retrieved.
        /// </param>
        /// <returns>
        /// The retrieved service.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return container.TryGetInstance(serviceType);
            }
            else
            {
                return container.GetInstance(serviceType);
            }
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">
        /// The collection of services to be retrieved.
        /// </param>
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.GetAllInstances(serviceType).Cast<object>();
            }
            catch
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }
    }
}