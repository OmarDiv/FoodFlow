using FoodFlow;
using FoodFlow.Settings;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDependancies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var hangfireSettings = app.Services
    .GetRequiredService<IOptions<HangfireSettings>>()
    .Value;
app.UseHangfireDashboard("/Jobs",
    new DashboardOptions
    {
        Authorization = [
            new HangfireCustomBasicAuthenticationFilter
            {
                User = hangfireSettings.Username,
                Pass = hangfireSettings.Password
            }
        ]
    }
);

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

// هنا seeding
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await userManager.SeedAdminUserAsync();
}

app.MapControllers();
app.UseExceptionHandler();

app.Run();