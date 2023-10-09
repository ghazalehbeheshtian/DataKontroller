var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();   // Add och Registrera SignalR i ConfigureServices:

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OpenArms",
                      policy =>
                      {
                          policy.WithOrigins(/*"http://localhost",*/ "https://localhost:7122")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("OpenArms");

app.UseAuthorization();

//app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<TemperatureHub>("/temperatureHub");
//});

app.MapHub<TemperatureHub>("/temperatureHub");   //Mapp SignalR hub i Configure:

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<TemperatureHub>("/temperatureHub");
//});


app.MapControllers();

app.Run();
