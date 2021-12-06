using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace WebApi4OAuthCallApp
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
            services.AddHttpClient<IGraphClient, GraphClient>();
            services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
            services.AddSingleton(provider => ConfidentialClientApplicationBuilder.Create(Configuration.GetValue<string>("SwaggerUIClientId"))
                .WithClientSecret(Configuration.GetValue<string>("SwaggerUISecret"))
                .WithAuthority(new Uri($"{Configuration.GetValue<string>("AzureAd:Instance")}{Configuration.GetValue<string>("AzureAd:TenantId")}"))
                .Build());
            services.AddSingleton(provider => ConfidentialClientApplicationBuilder.Create(Configuration.GetValue<string>("AzureAd:ClientId"))
                .WithClientSecret(Configuration.GetValue<string>("AzureAd:ClientSecret"))
                .WithAuthority(new Uri($"{Configuration.GetValue<string>("AzureAd:Instance")}{Configuration.GetValue<string>("AzureAd:TenantId")}"))
                .Build());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi4_OAuth_CallApp", Version = "v1" });
                c.EnableAnnotations();
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi4_OAuth_CallApp v1");
                    c.OAuthClientId(Configuration["SwaggerUIClientId"]);
                });
            }

            // turn on PII logging
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
