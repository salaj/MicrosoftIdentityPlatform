using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApi2OAuthCallApp
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

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = $"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/v2.0";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = "00000003-0000-0000-c000-000000000000", //Application ClientId for Graph client
                        ValidIssuer = $"https://sts.windows.net/{Configuration["AzureAd:TenantId"]}/",
                        SignatureValidator = (string token, TokenValidationParameters parameters) =>
                            new JwtSecurityToken(token)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = ctx =>
                        {
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = ctx =>
                        {
                            Debug.WriteLine("token: " + ctx.SecurityToken.ToString());
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi2_OAuth_CallApp", Version = "v1" });
                c.EnableAnnotations();
                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "jwt_auth"
                    },
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {$"{Configuration["DownstreamApi:Scopes"]}", "Get scope required by Graph API"}
                            }
                        }
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi2_OAuth_CallApp v1");
                    c.OAuthClientId(Configuration["AzureAd:ClientId"]);
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
