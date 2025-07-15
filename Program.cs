using FoodFlow;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.RegisterDependancies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();

app.Run();