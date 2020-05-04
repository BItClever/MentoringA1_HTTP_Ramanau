using DAL;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentoringA1_HTTP_Ramanau
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration  configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddOData();

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new XmlOutputFormatter());
                options.OutputFormatters.Add(new ExcelOutputFormatter());
                options.OutputFormatters.Add(new SOAPOutputFormatter());
                options.InputFormatters.Add(new SOAPInputFormatter());
                options.RespectBrowserAcceptHeader = true;
            });


            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ILogger>(log => new Logger(_configuration.GetValue<string>("ApplicationInsights:InstrumentationKey")));
            services.AddTransient<UnitOfWork>();
            services.AddApplicationInsightsTelemetry();

            services.AddStackExchangeRedisCache(options => options.Configuration = _configuration.GetConnectionString("RedisCache"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Order>("Orders");
            app.UseMvc(routeBuilder =>
            {
                // and this line to enable OData query option, for example $filter
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", modelBuilder.GetEdmModel());
                routeBuilder.MapRoute(
                    name: "default",
                    template: "{controller=Orders}/{action=GetById}/{id?}");
                routeBuilder.Expand().Select().Filter().Count().OrderBy().MaxTop(1000);
            });
        }
    }
}
