using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignModule.BusinessLogic;
using CampaignModule.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CampaignModule.BusinessLogic;
using CampaignModule.Database;

namespace CampaignModule
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
            services.AddControllers(); //Imports Controllers
            services.AddTransient<ICampaignLogic, CampaignLogic>(); //Imports Campaign Logic
            services.AddSingleton<ICampaignTableDB, CampaignTableDB>(); //Imports DATABASE
            //Imports SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc
                (
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Campaigns API - DEV/QA",
                        Version ="v1"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaign");

            });
        }

    }
}
