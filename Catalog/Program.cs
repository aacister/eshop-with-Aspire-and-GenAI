
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductAIService>();
builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());

// Register Ollama-based chat & embedding
builder.AddOllamaSharpChatClient("ollama-llama3-2"); //param is connection string

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();
app.UseMigration();
app.MapProductEndpoints();
app.UseHttpsRedirection();

app.Run();
