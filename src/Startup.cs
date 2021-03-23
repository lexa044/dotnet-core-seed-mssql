using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

using log4net;

using DNSeed.Repositories;
using DNSeed.Security;
using DNSeed.Services;
using DNSeed.Filters;

namespace DNSeed
{
    public class Startup
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Startup));

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _logger.Debug($"Loading Settings for {env.EnvironmentName}");
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWebWorkContext, WebWorkContext>();
            services.AddScoped<IDalSession>(_ => new DalSession(Configuration["ConnectionStrings:ReadConnection"], Configuration["ConnectionStrings:WriteConnection"]));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddControllers();
            services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.0.0", new OpenApiInfo
                    {
                        Version = "1.0.0",
                        Title = "DNSeed",
                        Description = "DNSeed (ASP.NET Core 3.1)",
                        Contact = new OpenApiContact()
                        {
                            Name = "Swagger Contributors",
                            Url = new Uri("https://github.com/swagger-api/swagger-codegen"),
                            Email = "apiteam@swagger.io"
                        },
                        TermsOfService = new Uri("http://swagger.io/terms/")
                    });
                    c.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = ApiKeyAuthenticationHandler.ApiKeyHeaderName,
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.OperationFilter<ApiKeyOperationFilter>();
                });
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(_logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "DNSeed");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
