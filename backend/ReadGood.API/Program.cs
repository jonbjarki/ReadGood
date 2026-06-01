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
builder.Services.Configure<GoogleConfiguration>(builder.Configuration.GetSection("Google"));

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
            .SetPostgresVersion(13, 0))
);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options=>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<BooksDbContext>();


builder.Services.AddAuthorization(options =>
{
    // options.FallbackPolicy = new AuthorizationPolicyBuilder()
    //     .RequireAuthenticatedUser()
    //     .Build();
}
);

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


// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
