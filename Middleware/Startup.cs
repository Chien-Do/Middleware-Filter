using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware.Attributes;
using Middleware.Filters;
using Middleware.Middlewares;
using Middleware.ModelBinding;
using Middleware.Models;
using System.Globalization;
using System.Net.Http;

namespace Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));
            services.AddScoped<LoggingFilter>();
            services.Configure<PositionOptions>(
         Configuration.GetSection("Position"));
            services.AddScoped<SampleActionFilterAttribute>();
            
            services.AddControllers(config =>
            {
               // config.Filters.Add(typeof(MyGlobalActionFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseAddresses();

            //TestMapMethod(app);
            //TestMiddlewareFlow(app);
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
            }
            
            app.ConfigureCustomExceptionMiddleware();   // Same with app.UseMiddleware<CustomExceptionMiddleware>();


            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<SessionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
            });
            

        }
 
        private void TestMapMethod(IApplicationBuilder app)
        {
            app.Map("/map1", config =>
            {
                config.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Hello Map 1");
                });
            });

            app.Map("/map2", config =>
            {
                config.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Hello Map 2");
                });
            });


            ////Nesting
            //app.Map("/level1", level1App =>
            //{
            //    level1App.Map("/level2a/level2a1", level2AApp =>
            //    {
            //        level2AApp.Run(async (context) =>
            //        { await context.Response.WriteAsync("Hello Level /level2a/level2a1"); });
            //    });
            //    // /level1/level2a
            //    //level1App.Map("/level2a", level2AApp =>
            //    //{
            //    //    level2AApp.Run(async (context) =>
            //    //    { await context.Response.WriteAsync("Hello Level 2A1"); });
            //    //});


            //    // /level1/level2b
            //    level1App.Map("/level2b", level2BApp =>
            //    {
            //        level2BApp.Run(async (context) =>
            //        { await context.Response.WriteAsync("Hello Level 2B"); });
            //    });
            //    // /level1
            //    level1App.Run(async (context) =>
            //    { await context.Response.WriteAsync("Hello Level 1"); });
            //});

            //Multi segment
            //app.Map("/level1/level2a", level1 =>
            //{
            //    level1.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("Hello Level 2A2");

            //    });
            //});

            // MapWhen

            app.MapWhen(r => r.Request.Query.ContainsKey("test"), app =>
            {
                app.Use(async (context, next) =>
                {
                    var val = context.Request.Query["test"];
                    await context.Response.WriteAsync($"test val = {val}");

                });
            });

            // UseWhen
            app.UseWhen(context => context.Request.Query.ContainsKey("test"), appBuilder => HandleUseWhen(appBuilder));

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("THE END\n");
            });
        }
        private void HandleUseWhen(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var val = context.Request.Query["test"];
                await context.Response.WriteAsync($"test val = {val} \n");
               await next();
            });
        }
   
      
        private void TestMiddlewareFlow(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                
                await context.Response.WriteAsync("1st - Before - App.Use()\n");
                await next();
                await context.Response.WriteAsync("1st - After - App.Use()\n");
            });
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("2nd - Before - App.Use()\n");
                await next();
                await context.Response.WriteAsync("2nd - After - App.Use()\n");
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("1st - App.Run()\n");
            });
            //this one will not excuted
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("2nd - App.Run()");
            //});
        }


        private void TestCustomMiddleware(IApplicationBuilder app)
        {
            app.UseMyMiddleware();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(
                    $"Hello {CultureInfo.CurrentCulture.DisplayName}");
            });
        }
    }
}
