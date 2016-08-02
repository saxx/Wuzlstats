using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables("Wuzlstats.")
                .Build();
        }


        public IConfiguration Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new AppSettings(Configuration);
            services.AddSingleton(x => settings);
            services.AddTransient<ReloadPlayersViewModel>();

            services
                .AddDbContext<Db>(options => { options.UseSqlServer(settings.DatabaseConnectionString); });

            services.AddSignalR(options => { options.Hubs.EnableDetailedErrors = true; });
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.ApplicationServices
                .GetService<Db>()
                .Database
                .EnsureCreated();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSignalR();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
        }
    }
}
