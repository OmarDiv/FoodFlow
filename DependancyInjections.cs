using FoodFlow.Contracts.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FoodFlow
{
    public static class DependancyInjection
    {

        public static IServiceCollection RegisterDependancies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();

            services
                .AddAuthConfig(configuration)
                .AddSwagerConfig()
                .AddMapsterConfig()
                .AddFluentValidtionsConfig()
                .DbContextConfig(configuration);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRestaurantService, RestaurantServicec>();

            return services;
        }

        private static IServiceCollection DbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                );
            });

            return services;
        }

        private static IServiceCollection AddSwagerConfig(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }


        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            return services;
        }

        private static IServiceCollection AddFluentValidtionsConfig(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                 .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName)); //السطر ده هوه المسؤول عن عمليه ال DI 

            var setting =configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>(); //بعمل bind بين ال appsettings و ال JwtOptions

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]!)), 
                    ValidIssuer = setting?.Issuer,
                    ValidAudience = setting?.Audience,
                };
            });

            return services;
        }

    }
}
