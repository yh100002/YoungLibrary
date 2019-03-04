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
using Web.Configuration;
using Web.Helpers;
using Web.Auth;
using Web.Log;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.ExceptionHandler;
using PerformanceLogger;
using Models;
using Microsoft.EntityFrameworkCore;
using Data.DependencyInjection;

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
            services.AddCors();        
            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-gateway-version"));
            services.AddAutoMapper();

            string dbConnString = Configuration.GetConnectionString("Auth");

            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer( dbConnString, builder1 => builder1.MigrationsAssembly(typeof(Startup).Assembly.FullName)
                )).AddUnitOfWork<UserContext>();


            var builder = new ContainerBuilder();
            builder.RegisterInstance<IHttpClient>(new StandardHttpClient());

            services.Configure<Configuration.LibrarySettings>(Configuration.GetSection("LibrarySettings"));         
            services.Configure<ElasticConnectionSettings>(Configuration.GetSection("ElasticConnectionSettings"));    

            services.AddSingleton(typeof(ElasticClientProvider));      
            services.AddScoped<IAuthRepository, AuthRepository>(); 
            services.AddScoped<ILogViewRepository, LogViewRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options => {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };
                            });

            //SeriLog
            var url = Configuration.GetSection("ElasticConnectionSettings:ClusterUrl").Value;            
            Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() //levels can be overridden per logging source
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(url)) //Logging to Elasticsearch
            {
                AutoRegisterTemplate = true //auto index template like logstash as prefix           
            }).CreateLogger();


            builder.Populate(services);
            ApplicationContainer = builder.Build();

            //Create the IServiceProvider based on the ApplicationContainer.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<Configuration.LibrarySettings> pimsSettings, IApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {            
             // Handles non-success status codes with empty body
            app.UseExceptionHandler("/errors/500");            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            //To handle unexpected exception globally, register custome middleware
            app.UseMiddleware<CustomExceptionMiddleware>();   
             //Enable PerformanceLogger as middleware layer by using extention
            app.UsePerformanceLog(new LogOptions());            
            loggerFactory.AddSerilog();          
            

            //Enable CORS with CORS Middleware for convenience   
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //Using authentication
            app.UseAuthentication();       
            
            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            // You can only do this if you have a direct reference to the container,
            // so it won't work with the above ConfigureContainer mechanism.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
