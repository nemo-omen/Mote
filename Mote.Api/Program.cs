using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mote.Api.Data;
using Mote.Api.Features.Identity.Extensions;
using Mote.Api.Features.Notes;
using Mote.Api.Models;
using Mote.Api.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
        {
            options.JsonSerializerOptions.WriteIndented = true;
        }
        else
        {
            options.JsonSerializerOptions.WriteIndented = false;
        }
});

// Add services to the container.

// Local services
builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserResolverService, UserResolverService>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

string connectionString;

if(builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DevConnection") ??
                       throw new Exception("Connection string 'DevConnection' not found");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("ProdConnection") ??
                       throw new Exception("Connection string 'ProdConnection' not found");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddAntiforgery();

const string onlyAllowLocalhostOrigins = "_onlyAllowLocalhostOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: onlyAllowLocalhostOrigins, policy =>
        {
            policy.WithOrigins("http://localhost");
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Mote API",
        Description = "A simple API for Mote",
    });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.CustomMapIdentityApi<ApplicationUser>()
    .RequireCors(onlyAllowLocalhostOrigins);
app.MapControllers();

app.UseHttpsRedirection();

app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager,
        [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .WithOpenApi()
    .RequireAuthorization()
    .RequireCors(onlyAllowLocalhostOrigins);

app.Run();