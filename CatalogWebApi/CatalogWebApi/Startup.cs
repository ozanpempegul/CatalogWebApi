using CatalogWebApi.Base;
using CatalogWebApi.Dto;
using Hangfire;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CatalogWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static JwtConfig JwtConfig { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Email Service
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            // Configure JWT Bearer
            JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            // add Filter
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ResponseGuidAttribute));
            });

            // services
            services.AddJwtBearerAuthentication();
            services.AddServicesDependencyInjection();
            services.AddContextDependencyInjection(Configuration);
            services.AddCustomizeSwagger();

            //add hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1); // Remove Schema on Swagger UI
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patika");
                c.DocumentTitle = "Patika";
            });

            // error handling 
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetail()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. App Level."
                        }.ToString());
                    }
                });
            });

            // hangfire
            app.UseHangfireDashboard();

            app.UseHttpsRedirection();


            // middleware 
            app.UseMiddleware<HeartbeatMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();


            app.UseRouting();

            // add auth 
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            // add services
            app.AddApplicationServiceDependencyInjection();
        }
    }
}
