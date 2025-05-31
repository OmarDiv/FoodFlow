using FluentValidation;
namespace FoodFlow
{
    public static class DependancyInjection
    {

        public static IServiceCollection RegisterDependancies(this IServiceCollection services,WebApplicationBuilder builder)
        {

            services.AddControllers();

            services
                .AddSwagerServices()
                .AddMapsterConfig()
                .AddFluentValidtions()
                .DbContextConfig(builder);
            return services;
        }

        public static IServiceCollection DbContextConfig(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                );
            });

            return services;
        }

        public static IServiceCollection AddSwagerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           services.AddEndpointsApiExplorer();
           services.AddSwaggerGen();

            return services;
        }


        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            return services;
        }

        public static IServiceCollection AddFluentValidtions(this IServiceCollection services)
        {
           services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }



    }
}
