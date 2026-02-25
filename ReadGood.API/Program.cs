using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.Infrastructure.Interfaces;
using ReadGood.Infrastructure.Implementations;
using ReadGood.API.Errors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IOpenLibraryAPI, OpenLibraryAPI>(client =>
{
    client.BaseAddress = new Uri("https://openlibrary.org");
    client.Timeout = TimeSpan.FromSeconds(10);

    var agent = builder.Configuration.GetValue<string>("OpenLibrary:UserAgent") ?? throw new InvalidOperationException("UserAgent configuration is missing");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", agent);
});

// Register all MediatR services
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(SearchBooksHandler).Assembly);
});

// Register exception handlers

// Handles generic exceptions from OpenLibrary API calls, mapping them to appropriate HTTP status codes and logging details
builder.Services.AddExceptionHandler<OpenLibraryExceptionHandler>();

// Handles specific exceptions like NotFoundException and OpenLibraryRateLimitExceededException, returning standardized ProblemDetails responses
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
