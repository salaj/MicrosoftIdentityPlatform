using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace WebApp_OpenIDConnect_SignInUser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // <Configure_service_ref_for_docs_ms>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = $"{Configuration.GetValue<string>("AzureAd:Instance")}{Configuration.GetValue<string>("AzureAd:TenantId")}/v2.0";
                options.ClientId = Configuration.GetValue<string>("AzureAd:ClientId");
                options.CallbackPath = Configuration.GetValue<string>("AzureAd:CallbackPath");
                options.TokenValidationParameters.IssuerValidator = (issuer, token, parameters) =>
                {
                    // Accepts any issuer of the form "https://login.microsoftonline.com/{tenantid}/v2.0",
                    // where tenantid is the tid from the token.

                    if (token is JwtSecurityToken jwt)
                    {
                        Console.WriteLine($"Custom validator: {jwt}");

                        if (jwt.Payload.TryGetValue("tid", out var value) &&
                            value is string tokenTenantId)
                        {
                            var validIssuersTid = (parameters.ValidIssuers ?? Enumerable.Empty<string>())
                                .Append(parameters.ValidIssuer)
                                .Where(i => !string.IsNullOrEmpty(i));

                            if (validIssuersTid.Any(i => i.Replace("{tenantid}", tokenTenantId) == issuer))
                                return issuer;
                        }
                    }

                    // Recreate the exception that is thrown by default
                    // when issuer validation fails
                    var validIssuer = parameters.ValidIssuer ?? "null";
                    var validIssuers = parameters.ValidIssuers == null
                        ? "null"
                        : !parameters.ValidIssuers.Any()
                            ? "empty"
                            : string.Join(", ", parameters.ValidIssuers);
                    string errorMessage = FormattableString.Invariant(
                        $"IDX10205: Issuer validation failed. Issuer: '{issuer}'. Did not match: validationParameters.ValidIssuer: '{validIssuer}' or validationParameters.ValidIssuers: '{validIssuers}'.");

                    throw new SecurityTokenInvalidIssuerException(errorMessage)
                    {
                        InvalidIssuer = issuer
                    };
                };
            });

            //services.AddMicrosoftIdentityWebAppAuthentication(Configuration);

            services.AddControllersWithViews(options => { });
            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
        }

        

        // </ Configure_service_ref_for_docs_ms >

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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            // <endpoint_map_ref_for_docs_ms>
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            // </endpoint_map_ref_for_docs_ms>
        }
    }
}
