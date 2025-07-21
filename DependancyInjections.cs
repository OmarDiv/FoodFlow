using FoodFlow.Contracts.Authentication;
using FoodFlow.Services.Implementations;
using FoodFlow.Settings;
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
            services.AddHybridCache();


            services.AddCors(options =>
                options.AddDefaultPolicy(builder =>
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                )
            );

            services
                .AddAuthConfig(configuration)
                    .AddSwagerConfig()
                    .AddMapsterConfig()
                    .AddFluentValidtionsConfig()
                    .DbContextConfig(configuration);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            //services.AddScoped<ICacheService, CacheService>(); //distributed cache

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddHttpContextAccessor();
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


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

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName)); //السطر ده هوه المسؤول عن عمليه ال DI 


            // لا يمكن استخدام IOptions<JwtOptions> في Program.cs أثناء تسجيل الـ Middleware(مثل AddJwtBearer)
            // لأننا في مرحلة بناء الـ services، ولسه الـ Dependency Injection ما اشتغلش
            // لذلك نحتاج إلى عملية Bind يدوية (Manual Bind) لإعداد JwtOptions مؤقتًا
            // حتى نستخدم القيم فورًا أثناء إعداد TokenValidationParameters
            var setting = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

            });
            return services;
        }

    }
}
