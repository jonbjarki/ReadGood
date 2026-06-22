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
builder.Services.AddScoped<IBookshelfRepository, BookshelfRepository>();

builder.Services.Configure<GoogleConfiguration>(builder.Configuration.GetSection("Google"));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<BookshelvesConfiguration>(builder.Configuration.GetSection("Bookshelves"));


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
        ValidateIssuerSigningKey = true,
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    // Extract JWT token from the HttpOnly cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Prefer Authorization header when present, fallback to auth cookie.
            var authHeader = context.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Token = authHeader["Bearer ".Length..].Trim();
                return Task.CompletedTask;
            }

            if (context.Request.Cookies.TryGetValue("X-Access-Token", out var token) && !string.IsNullOrWhiteSpace(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

// Makes every route authenticated by default following auth first approach
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("ProfileCompleted", policy =>
    {
        policy.RequireClaim("profileCompleted", "true");
    });
}
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("https://localhost:3000") // TODO: Change to actual frontend url in production
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


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

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
