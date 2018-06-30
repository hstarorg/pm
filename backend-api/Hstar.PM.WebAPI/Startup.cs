using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hstar.PM.Core.Extensions;
using Hstar.PM.Core.Models;
using Hstar.PM.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Hstar.PM.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "pm service",
                    Version = "v1",
                    Description = "The pm project backend services."
                });
                // System.Reflection.Assembly.GetExecutingAssembly().GetName().Name also OK.
                string xmlCommentName = $"{this.GetType().Assembly.GetName().Name}.xml";
                string xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlCommentName);
                c.IncludeXmlComments(xmlCommentPath, true);
            });
            return services.ToIocProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSimpleExceptionHandler(env, res =>
            {
                if (res.Error is BusinessException bizEx)
                {
                    return new ObjectResult(new
                    {
                        StatusCode = bizEx.StatusCode,
                        Message = bizEx.Message,
                        StackTrace = env.IsDevelopment() ? bizEx.StackTrace :null
                    });
                }
                return null;
            });
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            // Cors support
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger"; // 这个的作用的配置swagger UI的路由
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "pm service v1");
            });

            app.UseMvc();
        }
    }
}
