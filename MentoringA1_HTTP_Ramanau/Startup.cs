using DAL;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentoringA1_HTTP_Ramanau
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddOData();

            services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new XmlOutputFormatter());
                options.OutputFormatters.Add(new ExcelOutputFormatter());
                options.RespectBrowserAcceptHeader = true;
            });


            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<UnitOfWork>();
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
                routeBuilder.Expand().Select().Filter().Count().OrderBy().MaxTop(1000);
            });
        }
    }
}
