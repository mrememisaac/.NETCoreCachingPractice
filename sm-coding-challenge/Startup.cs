using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sm_coding_challenge.Persistence;
using sm_coding_challenge.Services.CacheProvider;
using sm_coding_challenge.Services.DataProvider;
using StackExchange.Redis;

namespace sm_coding_challenge
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
            
            services.AddControllersWithViews();
         
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")));
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();

            //We could use a relational database e.g. sql server as the primary store in a real world scenario
            //services.AddDbContext<SMContext>(item => item.UseSqlServer(Configuration.GetConnectionString("SMConnection")));
            //services.AddScoped<IPlayerRepository, PlayerRepository>();
            
            services.AddTransient<IDataProvider, DataProviderImpl>();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseResponseCaching();
        }
    }
}
