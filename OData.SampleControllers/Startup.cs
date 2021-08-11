using ODataGeneric.BaseControllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ODataGeneric.SampleControllers.Entities;
using Serilog;

namespace ODataGeneric.SampleControllers
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
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, MyActionRoutingConvention>());

            var connectionString = Configuration.GetConnectionString("Default");

            services.AddHttpContextAccessor();
        
            var batchHandler = new DefaultODataBatchHandler
            {
                MessageQuotas =
                {
                    MaxNestingDepth = 3,
                    MaxOperationsPerChangeset = 10
                }
            };

            IMvcBuilder mvcBuilder = services.AddControllers();
            mvcBuilder.AddODataRouting(new[]{ typeof(Currency), typeof(Counterparty) }, batchHandler);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseODataBatching();

            app.UseRouting();//.UseODataRouteDebug(); //.UseMvcWithDefaultRoute();
            app.UseSerilogRequestLogging();

            #if DEBUG
            #else //if DEBUG
            app.UseAuthorization();
            #endif //DEBUG

            // adds http://localhost:7000/swagger/index.html 
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.AddSwaggerEndPoint(typeof(Counterparty));
                setupAction.AddSwaggerEndPoint(typeof(Currency));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
