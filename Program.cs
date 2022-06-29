using Blog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
