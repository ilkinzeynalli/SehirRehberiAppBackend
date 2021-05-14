using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SehirRehberi.API.Modules;
using SehirRehberi.Core.Utilities.CloudMediaStorages.Cloudinary;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Seeders;
using SehirRehberi.WebApi.Modules;

namespace SehirRehberi.API
{
    public class Startup
    {
        private const string CorsPolicyName = "EnableCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable Cross Module  settings 
            EnableCrossModule.Load(services, CorsPolicyName);

            services.AddControllers()
                    //.AddFluentValidation()
                    .AddNewtonsoftJson(jsonOption =>
                    {
                        jsonOption.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });



            //DbContext settings
            services.AddDbContext<ApplicationIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"), x => x.MigrationsAssembly("SehirRehberi.DataAccess"));
            });

            //Cloudinary Settings
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            //Auto Mapper configuration
            //services.AddSingleton(AutoMapperConfig.CreateMapper());

            //Adding Identity Server
            IdentityModule.Load(services);

            //JWT configuration
            JwtModule.Load(services, Configuration);

            //Configure DI for app services
            //LogicModule.Load(services);

            ////Validator configure
            //ValidatorModule.Load(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Seed Datas
                SeederIdentityData.EnsurePopulated(app, Configuration).Wait();
                SeederData.EnsurePopulated(app).Wait();
            }

            app.UseRouting();

            app.UseCors(CorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
