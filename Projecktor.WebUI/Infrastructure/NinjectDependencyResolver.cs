﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Projecktor.Domain.Concrete;
using Projecktor.Domain.Abstract;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Infrastructure.Concrete;

namespace Projecktor.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel) {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType) {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IContext>().To<Context>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<ISecurityService>().To<SecurityService>();
        }
    }
}