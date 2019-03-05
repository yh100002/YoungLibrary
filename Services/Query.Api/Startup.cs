using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.Util;
using RabbitMQ.Client;
using Data.DependencyInjection;
using Events;
using Models;
using Query.Api.Messaging.Consumers;

namespace Query.Api
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            string dbConnString = Configuration.GetConnectionString("LibraryQuery");

            services.AddDbContext<BookQueryContext>(options =>
                options.UseSqlServer( dbConnString, builder1 => builder1.MigrationsAssembly(typeof(Startup).Assembly.FullName)
                )).AddUnitOfWork<BookQueryContext>();

            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<BookCreateEventConsumer>();
            builder.RegisterType<BookUpdateEventConsumer>();
            builder.RegisterType<BookDeleteEventConsumer>();

            builder.Register(context =>
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                        {
                            h.Username("rabbitmq");
                            h.Password("rabbitmq");
                        });
                        
                        cfg.ReceiveEndpoint(host, "YoungQueue" + Guid.NewGuid().ToString(), e =>
                        {
                            e.LoadFrom(context);
                            
                        });
                    });

                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {            
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
