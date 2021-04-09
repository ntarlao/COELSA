
using Web.Core.Models;
using Web.Core.Services;
using Web.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AgregarServicios(this IServiceCollection services, IConfiguration config)
        {
            //services.AddTransient<IUsuarios>(provider =>
            //{
            //    //var client = new ServiceSoapClient(new EndpointConfiguration());
            //    //client.Endpoint.Address = new System.ServiceModel.EndpointAddress(config["Services:Afip:Endpoint"]);
            //    //return new AfipFEVService(client);
            //});
            services.AddTransient<IContacts, ContactsService>();
            //services.AddTransient<IApiKeyService, DbApiKeyService>();

            return services;
        }

      
    }
}
