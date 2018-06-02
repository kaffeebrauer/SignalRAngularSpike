using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRServer.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRServer.Providers;

namespace SignalRServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder =>
            //        {
            //            builder
            //                .AllowAnyOrigin()
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //        });
            //});

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder =>
            //        {
            //            builder
            //                .AllowAnyOrigin()
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //        });
            //});
            var sqlConnectionString = $"Data Source={Path.Combine(Environment.CurrentDirectory, "news.sqlite")}";
            
            services.AddDbContext<NewsContext>(options =>
                    options.UseSqlite(
                        sqlConnectionString
                    ), ServiceLifetime.Singleton
            );

            services.AddSingleton<NewsStore>();

            services.AddSignalR().AddMessagePackProtocol();

            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
            }
            app.UseHsts();

            app.UseCors(options => options
                //the below is screwed for some reason
                //builder
                //    .AllowAnyMethod()
                //    .AllowAnyHeader()
                //    .WithOrigins("http://localhost:4200");


                .AllowAnyMethod()
                .AllowAnyHeader()
                //.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
                .AllowCredentials()
            );

            //app.UseCors("AllowAllOrigins");
            
            app.UseSignalR(routes => {
                routes.MapHub<NewsHub>("/newsHub");
                routes.MapHub<MessageHub>("/messageHub");
            });

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=FileClient}/{action=Index}/{id?}");
            });
        }
    }
}
