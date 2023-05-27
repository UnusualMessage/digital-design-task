var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

var app = builder.Build();

// Middlewares
app.MapControllers();

app.Run();
