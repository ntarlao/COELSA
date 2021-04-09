using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Web.Core.Services;
using Web.Core.Services.Interfaces;
using Microsoft.AspNet.OData.Extensions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Web.Core.Models.Dto;
using System.Security.Cryptography.X509Certificates;


namespace Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public Autofac.IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("WebDataBase"));
           
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore
            };

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(180);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var key = Encoding.ASCII.GetBytes(Configuration["Services:apiLocalKey"]);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {

            //            return Task.CompletedTask;
            //        }
            //    };
            //    //x.Authority = Configuration["Authority"];
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});



            services.AddOData();
            services.AddODataQueryFilter();
            //manejo de session
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<SessionUserProvider>();


            //Injección
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<ContactsService>().As<IContacts>();

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);

        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=File}/{action}/{id?}");
            });


            //app.UseIdentityServer();

            //app.UseAuthentication();

            app.UseMvc();
        }
    }
}
