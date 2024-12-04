using KafkaTest.Api.Config;
using KafkaTest.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddTransient<VideoStreamService>();

#region Dependency injection
DependencyConfig.AddRegistration(builder.Services);
#endregion

#region Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
#endregion

builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<VideoStreamHub>("/videoStreamHub");

string wwwroot = builder.Environment.WebRootPath;

app.MapGet("/video", () =>
{
    string filePath = Path.Combine(wwwroot, "Pelea_de_Monos.mp4");
    return Results.Stream(new FileStream(filePath, FileMode.Open));
});

app.Run();