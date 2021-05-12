using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Concrete;
using SehirRehberi.Business.Mappings.AutoMapper;
using SehirRehberi.Core.Utilities.Interceptors;
using SehirRehberi.Core.Utilities.Security.Jwt;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories;

namespace SehirRehberi.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //SingleInstance -- Single
            //InstancePerLifetimeScope -- Scope
            //InstancePerDependency -- transient
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<ValueManager>().As<IValueService>();
            builder.RegisterType<EfValueRepository>().As<IValueDal>();
            builder.RegisterType<CityManager>().As<ICityService>();
            builder.RegisterType<EfCityRepository>().As<ICityDal>();
            builder.RegisterType<PhotoManager>().As<IPhotoService>();
            builder.RegisterType<EfPhotoRepository>().As<IPhotoDal>();
            builder.RegisterType<AspNetUserTokenManager>().As<IAspNetUserTokenService>();
            builder.RegisterType<EfAspNetUserToken>().As<IAspNetUserTokenDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            //Aspector selector implementation
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();


            //AutoMapper implementation
            builder.Register(c =>
            {
                return AutoMapperConfig.CreateMapper();
            })
            .As<IMapper>()
            .SingleInstance();
        }
    }
}
