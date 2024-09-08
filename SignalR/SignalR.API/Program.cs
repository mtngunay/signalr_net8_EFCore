using Microsoft.EntityFrameworkCore;
using SignalR.API.Data;
using SignalR.API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:7234") // Ýstemcinin kökenini ekleyin
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Çerezler ve kimlik doðrulama bilgilerini gönderme izni
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();

app.UseWebSockets();

app.MapHub<ChatHub>("/chathub"); 

app.MapControllers();


app.Run();
