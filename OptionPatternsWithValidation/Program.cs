using Microsoft.Extensions.Options;
using OptionPatternsWithValidation;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        # region default builder

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        builder.Services.AddOptions<AppOptions>().Bind(
            builder.Configuration.GetSection(nameof(AppOptions)))
            .ValidateDataAnnotations();

        #region default middleware

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        #endregion


        app.MapGet("/options", (IOptions<AppOptions> appOptions) =>
        {
            return Results.Text(
                JsonSerializer.Serialize(appOptions.Value),
                contentType: "application/json");
        })
        .WithName("Options")
        .WithOpenApi();

        app.MapGet("/optionsSnapshot", (IOptionsSnapshot<AppOptions> appOptions) =>
        {
            return Results.Text(
                JsonSerializer.Serialize(appOptions.Value),
                contentType: "application/json");
        })
        .WithName("OptionsSnapshot")
        .WithOpenApi();

        app.MapGet("/optionsMonitor", (IOptionsMonitor<AppOptions> appOptions) =>
        {
            return Results.Text(
                JsonSerializer.Serialize(appOptions.CurrentValue),
                contentType: "application/json");
        })
        .WithName("OptionsMonitor")
        .WithOpenApi();

        app.Run();
    }
}