using AutoMapper;
using BankingApi.Data;
using BankingApi.Data.Services;
using BankingApi.Models.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankingApi
{
    public class Startup
    {
        readonly string CorsPolicy = "_corsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BankingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BankingContext"), b => b.MigrationsAssembly("BankingApi.Data")));

            services.AddCors(o => o.AddPolicy(CorsPolicy, b =>
            {
                b.WithOrigins("http://localhost:5000", "https://localhost:5001", "https://localhost:44368");
                b.AllowAnyHeader();
                b.AllowAnyMethod();
                b.AllowCredentials();
            }));

            services.AddControllers();
            services.AddAutoMapper(typeof(InstitutionMappingProfile).Assembly);

            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IInstitutionService, InstitutionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicy);

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
