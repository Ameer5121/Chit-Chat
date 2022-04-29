using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ChitChat.Services
{
    public static class IoCContainerService
    {
        public static IContainer _container;
        public static void Register()
        {
            var container = new ContainerBuilder();
            container.RegisterType<HttpService>().As<IHttpService>().SingleInstance().WithParameter("httpClient", new HttpClient());
            _container = container.Build();           
        }
    }
}
