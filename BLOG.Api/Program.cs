using MediatR;
using BLOG.Application;
using BLOG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

try
{
    builder.Services.AddInfrastructureLayer();
    builder.Services.AddApplicationLayer();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        .WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
    });

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();

    app.UseCors("CorsPolicy");

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    //var mediator = app.Services.GetRequiredService<IMediator>();

    app.Run();

    return 0;
}
catch (Exception)
{

    return 1;
}