using MediatR;
using BLOG.Application;
using BLOG.Infrastructure;
using BLOG.Domain.Model.ApplicationUser;
using BLOG.Infrastructure.Persistance;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Identity;
using BLOG.Api.Setups;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

try
{
    builder.Services.AddInfrastructureLayer(builder.Configuration);
    builder.Services.AddApplicationLayer();

    builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })
        .AddEntityFrameworkStores<ApplicationDbContext>();

    //builder.Services.AddCache(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        .WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
    });

    builder.Services.AddControllers();
        //.AddNewtonsoftJson(options =>
        //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.MapGroup("/acc").MapIdentityApi<ApplicationUser>();

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