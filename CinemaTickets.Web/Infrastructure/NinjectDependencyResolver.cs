using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTickets.Web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        IKernel kernel;
        public NinjectDependencyResolver(IKernel ikernel)
        {
            kernel = ikernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
           
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>();
        }
    }
}