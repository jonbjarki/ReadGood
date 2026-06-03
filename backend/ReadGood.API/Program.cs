using ReadGood.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ReadGood.API.Handlers;
using ReadGood.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using ReadGood.Infrastructure.Interfaces;
using ReadGood.Infrastructure.Implementations;
using ReadGood.Application.Features.Books.GetBookById;
using ReadGood.API.Errors;
using Microsoft.AspNetCore.Identity;
using ReadGood.API.Configuration;
using ReadGood.Domain.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adds timestamped console logging
builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.UseUtcTimestamp = true;
    options.IncludeScopes = true;
});

// Add services to the container.
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.Configure<GoogleConfiguration>(builder.Configuration.GetSection("Google"));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JWT"));


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContextPool<BooksDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("BooksDbConnectionString"),
        o => o
            .SetPostgresVersion(13, 0)
            .MigrationsAssembly("ReadGood.API"))
);


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<BooksDbContext>();

/* 
Sets up JWT Authentication and fetches required configuration variables
*/

// Ensures that the required configuration is provided
var jwtConfig = builder.Configuration.GetSection("JWT");
if (jwtConfig is null)
{
    throw new Exception("Missing required configuration section \"JWT\"");
}

var jwtKey = jwtConfig.GetValue<string>("Key");
var jwtAudience = jwtConfig.GetValue<string>("Audience");
var jwtIssuer = jwtConfig.GetValue<string>("Issuer");

if (jwtKey is null || jwtAudience is null || jwtIssuer is null)
{
    throw new Exception("JWT configuration is missing required values");
}

// Configures authentication middleware to use our JWT tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Makes every route authenticated by default following auth first approach
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
}
);

// Adds a typed http client for making http requests to the google books api
builder.Services.AddHttpClient<IGoogleBooksAPI, GoogleBooksAPI>(client =>
{
    client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
    client.Timeout = TimeSpan.FromSeconds(10);

    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
}).AddHttpMessageHandler<LoggingDelegatingHandler>();

// Register all MediatR services
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetBookByIdHandler).Assembly);
});

// Register exception handlers

// Handles specific exceptions like NotFoundException and GoogleBooksRateLimitExceededException, returning standardized ProblemDetails responses
// Returns 500 for any unknown errors
builder.Services.AddExceptionHandler<GoogleBooksExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Enable exception handling middleware
app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
