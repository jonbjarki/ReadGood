using ReadGood.Infrastructure.Interfaces;
using ReadGood.Infrastructure.Implementations;
using ReadGood.API.Errors;
using ReadGood.API.Handlers;
using ReadGood.Application.Features.Books.GetBookById;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.IncludeScopes = true;
});

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<LoggingDelegatingHandler>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
