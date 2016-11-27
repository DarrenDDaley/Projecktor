﻿using Projecktor.Domain.Abstract;
using Projecktor.Domain.Concrete;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System.Diagnostics;
using System.Web.Mvc;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public abstract class ProjecktorViewPage : WebViewPage
    {
        protected IContext DataContext;
        public User CurrentUser { get; private set; }
        public IUserService Users { get; private set; }
        public ISecurityService Security { get; private set; }
        public string ProjecktorCDN { get; private set; }
        public bool IsCDNAvailable { get; private set; }

        public ProjecktorViewPage()
        {
            DataContext = new Context();
            Users = new UserService(DataContext);
            Security = new SecurityService(Users);
            CurrentUser = Security.GetCurrentUser();
            ProjecktorCDN = "http://projecktor.azureedge.net";
            IsCDNAvailable = Debugger.IsAttached ? false : true;
        }
    }

    public abstract class ProjecktorViewPage<TModel> : WebViewPage<TModel>
    {
        protected IContext DataContext;
        public User CurrentUser { get; private set; }
        public IUserService Users { get; private set; }
        public ISecurityService Security { get; private set; }
        public string ProjecktorCDN { get; private set; }
        public bool IsCDNAvailable { get; private set; }

        public ProjecktorViewPage()
        {
            DataContext = new Context();
            Users = new UserService(DataContext);
            Security = new SecurityService(Users);
            CurrentUser = Security.GetCurrentUser();
            ProjecktorCDN = "http://projecktor.azureedge.net";
            IsCDNAvailable = Debugger.IsAttached ? false : true;
        }
    }
}