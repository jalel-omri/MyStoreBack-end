using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;





using Swashbuckle.AspNetCore.SwaggerGen;

using Microsoft.IdentityModel.Tokens;
using MyStore.Classes;
using MyStore.Models;
using Products.API.Classes;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace MyStore
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });


            // var connection = "Server = ROG; Database = produits; User Id =jalel; Password =jalel123@omri;";
            //services.AddDbContext<produitsContext>(options => options.UseSqlServer(connection));
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<produitsContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();
            

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader =
                    new HeaderApiVersionReader("1-API-Version");
            });

            services.AddVersionedApiExplorer(
                options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
           
           app.UseMvc(routes =>
            {
                //Home
                routes.MapRoute(
                   name: "",
                   template: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Users", action = "Index" });
                routes.MapRoute(
                   name: "",
                   template: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Commands", action = "Index" });
                routes.MapRoute(
                   name: "",
                   template: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Produits", action = "Index" });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                                   $"/swagger/{description.GroupName}/swagger.json",
                                   description.GroupName.ToUpperInvariant());
                }
            });
            
        }
    }
}
