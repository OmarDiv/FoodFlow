using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FoodFlow
{
    public static class DependancyInjection
    {

        public static IServiceCollection RegisterDependancies(this IServiceCollection services, WebApplicationBuilder builder)
        {

            services.AddControllers();

            services
                .AddAuthConfig()
                .AddSwagerConfig()
                .AddMapsterConfig()
                .AddFluentValidtionsConfig()
                .DbContextConfig(builder);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IRestaurantService, RestaurantServicec>();

            return services;
        }

        private static IServiceCollection DbContextConfig(this IServiceCollection services, WebApplicationBuilder builder)
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

        private static IServiceCollection AddAuthConfig(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LlohKI6zKQojsQVBDUc6XVGPfiTga84R")), // Replace with your actual key
                    ValidIssuer = "FoodFlowApp",
                    ValidAudience = "FoodFlow Users"
                };
            });

            return services;
        }




    }
}
