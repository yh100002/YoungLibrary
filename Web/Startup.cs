using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Http;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider  ConfigureServices(IServiceCollection services)
        {
             services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });      

            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-gateway-version"));
            services.AddAutoMapper();

            var builder = new ContainerBuilder();
            builder.RegisterInstance<IHttpClient>(new StandardHttpClient());

            services.Configure<Configuration.LibrarySettings>(Configuration.GetSection("LibrarySettings"));             

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            //Create the IServiceProvider based on the ApplicationContainer.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<Configuration.LibrarySettings> pimsSettings, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());           
            
            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            // You can only do this if you have a direct reference to the container,
            // so it won't work with the above ConfigureContainer mechanism.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
